using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSchedulingNLayered.Common.Redis;
public class RedisSettings {
    public string Connection { get; set; }
    public int ExpiryTimeInSeconds { get; set; }
}
