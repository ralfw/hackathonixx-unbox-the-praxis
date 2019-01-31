namespace unbox.contracts
{
    public interface IBackendRequestHandler
    {
        void Handle(ConfigureCommand cmd);
        bool Handle(RegisterConsultationCommand cmd);
        void Handle(RegisterRealTimeslotCommand cmd);
        CurrentPlanResult Handle(CurrentPlanQuery query);
    }
}