using System;
using System.Windows;
using System.Windows.Input;

namespace LtGt.DemoWpf.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                SelectorPopup.IsOpen = !SelectorPopup.IsOpen;

                if (SelectorPopup.IsOpen)
                    SelectorTextBox.Focus();
            }
        }

        private void SelectorPopup_OnOpened(object sender, EventArgs e)
        {
            SelectorPopup.PlacementRectangle = new Rect(
                NodesTreeView.ActualWidth - SelectorPopup.ActualWidth - 20,
                NodesTreeView.ActualHeight - SelectorPopup.ActualHeight - 10,
                SelectorPopup.ActualWidth,
                SelectorPopup.ActualHeight);
        }
    }
}