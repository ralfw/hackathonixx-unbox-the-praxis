using System;
using unbox.frontend.addConsultation.view;
using unbox.frontend.addConsultation.viewModels;

namespace unbox.frontend.addConsultation
{
    internal class AddConsultationUi
    {
        private AddConsultationViewModel _viewModel;
        private AddConsultationWindow _window;

        private TimeSlotSelectionViewModel _timeSlotSelectionViewModel;
        private TimeSlotSelectionWindow _timeSlotSelectionWindow;

        internal void ShowConsultationUi()
        {
            if (_viewModel == null)
            {
                _viewModel = new AddConsultationViewModel(OnAddConsultationRequest, OnHasToShowSelectionTimeSlots);
            }

            _window = new AddConsultationWindow();
            _window.DataContext = _viewModel;
            _window.Show();
        }

      
        private void OnAddConsultationRequest()
        {
            throw new NotImplementedException();
        }
        private void OnHasToShowSelectionTimeSlots()
        {
            _timeSlotSelectionViewModel = new TimeSlotSelectionViewModel();
            _timeSlotSelectionWindow = new TimeSlotSelectionWindow {DataContext = _timeSlotSelectionViewModel};
            _timeSlotSelectionWindow.ShowDialog();
        }

    }
}
