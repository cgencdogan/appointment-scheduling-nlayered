using AppointmentSchedulingNLayered.Common.DataTransferObjects.Authentication;
using AppointmentSchedulingNLayered.Common.Results;
using AppointmentSchedulingNLayered.Common.Jwt;

namespace AppointmentSchedulingNLayered.Business.Abstract;
public interface IAuthService {
    Task<IResult> RegisterAsync(RegisterDto registerVM);
    Task<IDataResult<TokenInfo>> LoginAsync(LoginDto loginVM);
}
