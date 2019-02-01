
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
            _window = new MainWindow();
            _window.DataContext = _viewModel;
            _window.Show();
        }
    }
}
