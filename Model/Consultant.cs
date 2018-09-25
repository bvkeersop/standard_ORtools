using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standard_ORtools.Model
{
    class Consultant
    {
        public int ConsultantId { get; set; }
        public int[] TraveltimesInMinutes { get; set; }

        public Consultant(int consultantId, int[] traveltimesInMinutes)
        {
            ConsultantId = consultantId;
            TraveltimesInMinutes = traveltimesInMinutes;
        }
    }
}
