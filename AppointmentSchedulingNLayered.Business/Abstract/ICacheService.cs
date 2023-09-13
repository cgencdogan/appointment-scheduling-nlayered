using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSchedulingNLayered.Business.Abstract;
public interface ICacheService {
    T GetData<T>(string key);
    bool SetData<T>(string key, T value);
    object RemoveData(string key);
}
