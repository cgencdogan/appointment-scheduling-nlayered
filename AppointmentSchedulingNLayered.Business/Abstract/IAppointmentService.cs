using AppointmentSchedulingNLayered.Common.DataTransferObjects;
using AppointmentSchedulingNLayered.Common.Results;

namespace AppointmentSchedulingNLayered.Business.Abstract;
public interface IAppointmentService {
    Task<IDataResult<AppointmentDto>> GetByIdAsync(Guid appointmentId);
    Task<IDataResult<List<AppointmentDto>>> GetAllAsync();
    Task<IDataResult<List<AppointmentDto>>> GetAllByPersonelIdAsync(Guid personnelId);
    Task<IDataResult<AppointmentDto>> UpdateAsync(UpdateAppointmentDto appointmentDto);
    Task<IDataResult<AppointmentDto>> AddAsync(AddAppointmentDto appointmentDto);
    Task<IResult> DeleteAsync(Guid appointmentId);
}
