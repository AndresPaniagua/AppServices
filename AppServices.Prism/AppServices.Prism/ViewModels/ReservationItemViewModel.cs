using AppServices.Common.Models;
using AppServices.Prism.Helpers;
using Prism.Commands;

namespace AppServices.Prism.ViewModels
{
    public class ReservationItemViewModel : ReservationResponse
    {
        private DelegateCommand _acceptReserveCommand;

        public ReservationItemViewModel()
        {
        }

        public DelegateCommand AcceptReserveCommand => _acceptReserveCommand ?? (_acceptReserveCommand = new DelegateCommand(AcceptAsync));

        private async void AcceptAsync()
        {
            await MyAgendaWaitingPageViewModel.GetInstance().AcceptReserveAsync(new ReservationModel
            {
                IdReservation = Id,
                CultureInfo = Languages.Culture
            });

            await MyAgendaPageViewModel.GetInstance().ReloadAgenda();
        }
    
    }
}
