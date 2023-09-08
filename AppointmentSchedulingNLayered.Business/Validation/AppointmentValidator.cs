using FluentValidation;
using AppointmentSchedulingNLayered.Common.DataTransferObjects;

namespace AppointmentSchedulingNLayered.Business.Validation;
public class AppointmentValidator : AbstractValidator<IAppointmentDto> {
    public AppointmentValidator() {
        RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow).WithMessage("Başlangıç zamanı geçmişte olamaz.");
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("Bitiş zamanı, başlangıç zamanından önce olamaz.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Bitiş zamanı geçmişte olamaz.");
        RuleFor(x => x.PersonnelId).NotNull().WithMessage("PersonnelId alanı boş olamaz.").NotEmpty().WithMessage("PersonnelId alanı boş olamaz.");
        RuleFor(x => x.Details).Must(x => x.Length > 5).WithMessage("Detay alanı 5 karakterden az olamaz");
    }
}
