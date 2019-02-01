using unbox.contracts;
using unbox.frontend.addConsultation;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand ShowAddConsultationCommand { get; set; }

        private AddConsultationUi _addConsultationUi;

        public MainViewModel(IBackendRequestHandler backendRequestHandler)
        {
            _addConsultationUi = new AddConsultationUi(backendRequestHandler);
            ShowAddConsultationCommand = new RelayCommand(OnShowAddConsultationWindowRequest);
        }

        private void OnShowAddConsultationWindowRequest()
        {
            _addConsultationUi.ShowConsultationUi();
        }

    }
}
