
using unbox.contracts;

namespace unbox.frontend
{
    internal class MainUi
    {
        private MainViewModel _viewModel;
        private MainWindow _window;

        private IBackendRequestHandler _backendRequestHandler;

        internal void Init(IBackendRequestHandler backendRequestHandler)
        {
            _backendRequestHandler = backendRequestHandler;
        }

        internal void Show()
        {
            _viewModel = new MainViewModel(_backendRequestHandler);
            _window = new MainWindow(_viewModel);
            _viewModel.OnShowAddConsultationWindowRequest = () => { _viewModel.AddConsultationUi.ShowConsultationUi(_window); };

            var preview = new Preview();
            preview.DataContext = _viewModel;
            preview.Show();
            _window.Preview = preview;
            //preview.ShowToast = (dateTime) => _backendRequestHandler.Handle()
            _window.Show();
        }
    }
}
