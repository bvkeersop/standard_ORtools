using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standard_ORtools.Model
{
    class Consultant
    {
        public int ConsultantId { get; set; }
        public List<ClientTime> ClientAndTravelTime { get; set; }

        public Consultant(int consultantId, List<ClientTime> traveltimesInMinutes)
        {
            ConsultantId = consultantId;
            ClientAndTravelTime = traveltimesInMinutes;
        }
    }
}
