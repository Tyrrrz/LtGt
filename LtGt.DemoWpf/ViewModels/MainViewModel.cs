using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace LtGt.DemoWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private string? _documentUrl;
        private HtmlDocument? _document;
        private IReadOnlyList<HtmlNode>? _topLevelNodes;
        private string? _selector;
        private bool _isBusy;

        public string? DocumentUrl
        {
            get => _documentUrl;
            set
            {
                Set(ref _documentUrl, value);
                GetDocumentCommand.RaiseCanExecuteChanged();
            }
        }

        public HtmlDocument? Document
        {
            get => _document;
            private set
            {
                Set(ref _document, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public bool IsDataAvailable => Document != null;

        public IReadOnlyList<HtmlNode>? TopLevelNodes
        {
            get => _topLevelNodes;
            private set
            {
                Set(ref _topLevelNodes, value);
                RaisePropertyChanged(() => IsFiltered);
            }
        }

        public string? Selector
        {
            get => _selector;
            set
            {
                Set(ref _selector, value);
                ApplySelectorCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsFiltered => Document != null && !ReferenceEquals(TopLevelNodes, Document.Children);

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                GetDocumentCommand.RaiseCanExecuteChanged();
                ApplySelectorCommand.RaiseCanExecuteChanged();
            }
        }

        // Commands
        public RelayCommand GetDocumentCommand { get; }
        public RelayCommand ApplySelectorCommand { get; }

        public MainViewModel()
        {
            // Commands
            GetDocumentCommand = new RelayCommand(GetDocument, () => !IsBusy && !string.IsNullOrWhiteSpace(DocumentUrl));
            ApplySelectorCommand = new RelayCommand(ApplySelector, () => !IsBusy);
        }

        private async void GetDocument()
        {
            IsBusy = true;

            try
            {
                DocumentUrl = new UriBuilder(DocumentUrl!).Uri.ToString();

                var raw = await _httpClient.GetStringAsync(DocumentUrl);
                Document = Html.ParseDocument(raw);
                TopLevelNodes = Document.Children;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplySelector()
        {
            if (Document == null)
                return;

            TopLevelNodes = string.IsNullOrWhiteSpace(Selector)
                ? Document.Children
                : Document.QueryElements(Selector).ToArray();
        }
    }
}