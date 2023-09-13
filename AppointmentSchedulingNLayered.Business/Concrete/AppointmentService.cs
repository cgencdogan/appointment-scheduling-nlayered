using AutoMapper;
using FluentValidation;
using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Business.Validation;
using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.Common.DataTransferObjects;
using AppointmentSchedulingNLayered.Common.Results;
using AppointmentSchedulingNLayered.Common.Exceptions;
using AppointmentSchedulingNLayered.Entities.Concrete;
using AppointmentSchedulingNLayered.Common.Session;

namespace AppointmentSchedulingNLayered.Business.Concrete;
public class AppointmentService : IAppointmentService {
    private IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private Session _session;
    private readonly ICacheService _cacheService;

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, SessionProvider sessionProvider, ICacheService cacheService) {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
        _session = sessionProvider.Session;
        _cacheService = cacheService;
    }

    public async Task<IDataResult<AppointmentDto>> AddAsync(AddAppointmentDto appointmentDto) {
        AppointmentValidator validator = new AppointmentValidator();
        await validator.ValidateAndThrowAsync(appointmentDto);

        var available = await _appointmentRepository.AppointmentTimeAvailableAsync(_mapper.Map<Appointment>(appointmentDto));
        if (!available)
            throw new AppointmentTimeNotAvailableException();

        var appointmentToBeAdded = _mapper.Map<Appointment>(appointmentDto);
        appointmentToBeAdded.CreatedById = Guid.Parse(_session.UserId);

        var addedAppointment = await _appointmentRepository.AddAsync(appointmentToBeAdded);

        _cacheService.SetData($"appointment:{addedAppointment.Id}", addedAppointment);

        return new SuccessDataResult<AppointmentDto>(_mapper.Map<AppointmentDto>(addedAppointment), "Randevu oluşturuldu");
    }

    public async Task<IResult> DeleteAsync(Guid appointmentId) {
        if (appointmentId == Guid.Empty)
            throw new NullAppointmentIdException();

        var appointment = await _appointmentRepository.GetAsync(a => a.Id == appointmentId);

        if (appointment is null)
            throw new AppointmentNotFoundException();

        await _appointmentRepository.DeleteAsync(appointment);

        _cacheService.RemoveData($"appointment:{appointmentId}");

        return new SuccessResult("Randevu silindi");
    }

    public async Task<IDataResult<List<AppointmentDto>>> GetAllAsync() {
        var cacheData = _cacheService.GetData<IEnumerable<Appointment>>("appointments");

        if (cacheData is not null && cacheData.Count() > 0)
            return new SuccessDataResult<List<AppointmentDto>>(_mapper.Map<List<AppointmentDto>>(cacheData), "Tüm randevular getirildi.");

        var appointments = (await _appointmentRepository.GetListAsync()).ToList();

        if (appointments.Count <= 0 || appointments is null)
            throw new AppointmentNotFoundException();

        _cacheService.SetData<IEnumerable<Appointment>>("appointments", appointments);

        return new SuccessDataResult<List<AppointmentDto>>(_mapper.Map<List<AppointmentDto>>(appointments), "Tüm randevular getirildi.");
    }

    public async Task<IDataResult<List<AppointmentDto>>> GetAllByPersonelIdAsync(Guid personnelId) {
        var cacheData = _cacheService.GetData<IEnumerable<Appointment>>($"{personnelId}:appointments");
        if (cacheData is not null && cacheData.Count() > 0)
            return new SuccessDataResult<List<AppointmentDto>>(_mapper.Map<List<AppointmentDto>>(cacheData), "Belirtilen personele ait randevular getirildi.");

        var appointments = (await _appointmentRepository.GetListAsync(a => a.Personnel.Id == personnelId)).ToList();

        if (appointments.Count <= 0 || appointments is null)
            throw new AppointmentNotFoundException();

        _cacheService.SetData<IEnumerable<Appointment>>($"{personnelId}:appointments", appointments);

        return new SuccessDataResult<List<AppointmentDto>>(_mapper.Map<List<AppointmentDto>>(appointments), "Belirtilen personele ait randevular getirildi.");
    }

    public async Task<IDataResult<AppointmentDto>> GetByIdAsync(Guid appointmentId) {
        var cacheData = _cacheService.GetData<Appointment>($"appointment:{appointmentId}");
        if (cacheData is not null)
            return new SuccessDataResult<AppointmentDto>(_mapper.Map<AppointmentDto>(cacheData), "Randevu getirildi.");

        var appointment = await _appointmentRepository.GetAsync(a => a.Id == appointmentId);

        if (appointment is null)
            throw new AppointmentNotFoundException();

        _cacheService.SetData($"appointment:{appointmentId}", appointment);

        return new SuccessDataResult<AppointmentDto>(_mapper.Map<AppointmentDto>(appointment), "Randevu getirildi.");
    }

    public async Task<IDataResult<AppointmentDto>> UpdateAsync(UpdateAppointmentDto appointmentDto) {
        AppointmentValidator validator = new AppointmentValidator();
        await validator.ValidateAndThrowAsync(appointmentDto);

        if (!(await _appointmentRepository.AppointmentExistsAsync(appointmentDto.Id)))
            throw new AppointmentNotFoundException();

        bool timespanAvailable = await _appointmentRepository.AppointmentTimeAvailableAsync(_mapper.Map<Appointment>(appointmentDto));
        if (!timespanAvailable)
            throw new AppointmentTimeNotAvailableException();

        var updated = await _appointmentRepository.UpdateAsync(_mapper.Map<Appointment>(appointmentDto));

        _cacheService.SetData($"appointment:{updated.Id}", updated);

        return new SuccessDataResult<AppointmentDto>(_mapper.Map<AppointmentDto>(updated), "Randevu bilgileri güncellendi.");
    }
}
