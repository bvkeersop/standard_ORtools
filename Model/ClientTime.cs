using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standard_ORtools.Model
{
    class ClientTime
    {
        public int ClientId { get; set; }
        public int TimeInMinutes { get; set; }

        public ClientTime(int clientId, int timeInMinutes)
        {
            ClientId = clientId;
            TimeInMinutes = timeInMinutes;
        }
    }
}
