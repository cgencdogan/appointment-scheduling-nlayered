using AutoMapper;
using AppointmentSchedulingNLayered.Entities.Concrete;
using AppointmentSchedulingNLayered.Common.DataTransferObjects;

namespace AppointmentSchedulingNLayered.Business.Helpers;

public class ApplicationMapper : Profile {
    public ApplicationMapper() {
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<Appointment, AddAppointmentDto>().ReverseMap();
        CreateMap<Appointment, UpdateAppointmentDto>().ReverseMap();
    }
}
