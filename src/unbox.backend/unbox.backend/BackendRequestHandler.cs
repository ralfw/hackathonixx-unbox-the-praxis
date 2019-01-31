using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unbox.contracts;

namespace unbox.backend
{
    public class BackendRequestHandler : IBackendRequestHandler
    {
        public void Handle(ConfigureCommand cmd)
        {
            throw new NotImplementedException();
        }

        public bool Handle(RegisterConsultationCommand cmd)
        {
            throw new NotImplementedException();
        }

        public void Handle(RegisterRealTimeslotCommand cmd)
        {
            throw new NotImplementedException();
        }

        public CurrentPlanResult Handle(CurrentPlanQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
