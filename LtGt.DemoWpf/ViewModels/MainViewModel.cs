using System.Net.Http;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LtGt.Models;
using Tyrrrz.Extensions;

namespace LtGt.DemoWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private string _documentUrl;
        private HtmlDocument _document;
        private bool _isBusy;

        public string DocumentUrl
        {
            get => _documentUrl;
            set
            {
                Set(ref _documentUrl, value);
                GetDocumentCommand.RaiseCanExecuteChanged();
            }
        }

        public HtmlDocument Document
        {
            get => _document;
            private set
            {
                Set(ref _document, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public bool IsDataAvailable => Document != null;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                GetDocumentCommand.RaiseCanExecuteChanged();
            }
        }

        // Commands
        public RelayCommand GetDocumentCommand { get; }

        public MainViewModel()
        {
            // Commands
            GetDocumentCommand = new RelayCommand(GetDocument, () => !IsBusy && !DocumentUrl.IsNullOrWhiteSpace());
        }

        private async void GetDocument()
        {
            IsBusy = true;

            try
            {
                DocumentUrl = DocumentUrl.ToUri().ToString();

                // Download and parse document
                var raw = await _httpClient.GetStringAsync(DocumentUrl);
                Document = HtmlParser.Default.ParseDocument(raw);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}