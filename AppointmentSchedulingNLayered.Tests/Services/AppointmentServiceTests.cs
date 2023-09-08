using AppointmentSchedulingNLayered.Business.Concrete;
using AppointmentSchedulingNLayered.Business.Helpers;
using AppointmentSchedulingNLayered.Common.DataTransferObjects;
using AppointmentSchedulingNLayered.Common.Exceptions;
using AppointmentSchedulingNLayered.Common.Session;
using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.Entities.Concrete;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Linq.Expressions;

namespace AppointmentSchedulingNLayered.Tests.Services;

public class AppointmentServiceTests {
    private readonly AppointmentService _sut;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<SessionProvider> _sessionProviderMock;
    private readonly Mock<Session> _sessionMock;

    public AppointmentServiceTests() {
        var mapperConfig = new MapperConfiguration(c => { c.AddProfile<ApplicationMapper>(); });
        _mapper = mapperConfig.CreateMapper();
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _sessionMock = new();
        _sessionMock.Setup(s => s.UserId).Returns(Guid.NewGuid().ToString());
        _sessionProviderMock = new();
        _sessionProviderMock.Setup(s => s.Session).Returns(_sessionMock.Object);
        _sut = new AppointmentService(_appointmentRepoMock.Object, _mapper, _sessionProviderMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAppointment_WhenAppointmentExists() {
        var appointmentId = Guid.NewGuid();

        var appointment = new Appointment {
            Id = appointmentId,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details",
            CreatedById = Guid.NewGuid()
        };

        _appointmentRepoMock.Setup(a => a.GetAsync(x => x.Id == appointmentId)).ReturnsAsync(appointment);

        var result = await _sut.GetByIdAsync(appointmentId);
        var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(appointmentDto);
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowAppointmentNotFoundException_WhenAppointmentNotExists() {
        var appointmentId = Guid.NewGuid();

        _appointmentRepoMock.Setup(a => a.GetAsync(x => x.Id == appointmentId)).ReturnsAsync((Appointment)null);

        Func<Task> act = () => _sut.GetByIdAsync(appointmentId);

        await act.Should().ThrowAsync<AppointmentNotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ShouldThrowValidationException_WhenAppointmentNotValid() {
        var addAppointmentDto = new AddAppointmentDto {
            StartDate = DateTime.UtcNow.AddHours(2),
            EndDate = DateTime.UtcNow.AddHours(1),
            PersonnelId = Guid.NewGuid(),
            Details = "Details Test"
        };

        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(true);

        Func<Task> act = () => _sut.AddAsync(addAppointmentDto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AddAsync_ShouldThrowAppointmentTimeNotAvailableException_WhenAppointmentTimeNotAvailable() {
        var addAppointmentDto = new AddAppointmentDto {
            StartDate = DateTime.UtcNow.AddHours(1),
            EndDate = DateTime.UtcNow.AddHours(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details"
        };

        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(false);

        Func<Task> act = () => _sut.AddAsync(addAppointmentDto);

        await act.Should().ThrowAsync<AppointmentTimeNotAvailableException>();
    }

    [Fact]
    public async Task AddAsync_ShouldReturnAddedAppointment_WhenAppointmentAddedSuccessfully() {
        var addAppointmentDto = new AddAppointmentDto {
            StartDate = DateTime.UtcNow.AddHours(1),
            EndDate = DateTime.UtcNow.AddHours(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details"
        };

        var appointment = _mapper.Map<Appointment>(addAppointmentDto);
        appointment.CreatedById = Guid.NewGuid();
        appointment.Id = Guid.NewGuid();

        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(true);
        _appointmentRepoMock.Setup(a => a.AddAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);

        var result = await _sut.AddAsync(addAppointmentDto);
        result.Should().NotBeNull();
        result.Value.Should().BeOfType(typeof(AppointmentDto));
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenAppointmentNotValid() {
        var updateAppointmentDto = new UpdateAppointmentDto {
            StartDate = DateTime.UtcNow.AddHours(2),
            EndDate = DateTime.UtcNow.AddHours(1),
            PersonnelId = Guid.NewGuid(),
            Details = "Details Test"
        };

        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(true);

        Func<Task> act = () => _sut.UpdateAsync(updateAppointmentDto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAppointmentTimeNotAvailableException_WhenAppointmentTimeNotAvailable() {
        var appointmentId = Guid.NewGuid();
        var updateAppointmentDto = new UpdateAppointmentDto {
            Id = appointmentId,
            StartDate = DateTime.UtcNow.AddHours(1),
            EndDate = DateTime.UtcNow.AddHours(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details"
        };

        _appointmentRepoMock.Setup(a => a.AppointmentExistsAsync(appointmentId)).ReturnsAsync(true);
        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(false);

        Func<Task> act = () => _sut.UpdateAsync(updateAppointmentDto);

        await act.Should().ThrowAsync<AppointmentTimeNotAvailableException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedAppointment_WhenAppointmentUpdatedSuccessfully() {
        var appointmentId = Guid.NewGuid();
        var updateAppointmentDto = new UpdateAppointmentDto {
            Id = appointmentId,
            StartDate = DateTime.UtcNow.AddHours(1),
            EndDate = DateTime.UtcNow.AddHours(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details"
        };

        var appointment = _mapper.Map<Appointment>(updateAppointmentDto);

        _appointmentRepoMock.Setup(a => a.AppointmentExistsAsync(appointmentId)).ReturnsAsync(true);
        _appointmentRepoMock.Setup(a => a.AppointmentTimeAvailableAsync(It.IsAny<Appointment>())).ReturnsAsync(true);
        _appointmentRepoMock.Setup(a => a.UpdateAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);

        var result = await _sut.UpdateAsync(updateAppointmentDto);
        result.Should().NotBeNull();
        result.Value.Should().BeOfType(typeof(AppointmentDto));
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAppointmentNotFoundException_WhenAppointmentNotExists() {
        var appointmentId = Guid.NewGuid();
        var updateAppointmentDto = new UpdateAppointmentDto {
            Id = appointmentId,
            StartDate = DateTime.UtcNow.AddHours(1),
            EndDate = DateTime.UtcNow.AddHours(2),
            PersonnelId = Guid.NewGuid(),
            Details = "Test details"
        };

        _appointmentRepoMock.Setup(a => a.GetAsync(x => x.Id == appointmentId)).ReturnsAsync((Appointment)null);

        Func<Task> act = () => _sut.UpdateAsync(updateAppointmentDto);

        await act.Should().ThrowAsync<AppointmentNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNullAppointmentIdException_WhenAppointmentIdNull() {
        Guid appointmentId = Guid.Empty;

        Func<Task> act = () => _sut.DeleteAsync(appointmentId);

        await act.Should().ThrowAsync<NullAppointmentIdException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowAppointmentNotFoundException_WhenAppointmentNotExists() {
        var appointmentId = Guid.NewGuid();
        _appointmentRepoMock.Setup(a => a.GetAsync(x => x.Id == appointmentId)).ReturnsAsync((Appointment)null);

        Func<Task> act = () => _sut.DeleteAsync(appointmentId);

        await act.Should().ThrowAsync<AppointmentNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenAppointmentDeletedSuccessfully() {
        var appointmentId = Guid.NewGuid();
        _appointmentRepoMock.Setup(a => a.GetAsync(x => x.Id == appointmentId)).ReturnsAsync(new Appointment());

        var result = await _sut.DeleteAsync(appointmentId);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowAppointmentNotFoundException_WhenAppointmentNotExists() {
        _appointmentRepoMock.Setup(a => a.GetListAsync(null)).ReturnsAsync(new List<Appointment>());

        Func<Task> act = () => _sut.GetAllAsync();

        await act.Should().ThrowAsync<AppointmentNotFoundException>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAppointmentList_WhenAppointmentsExists() {
        var appointments = new List<Appointment>() {
            new Appointment() { Id = Guid.NewGuid()}
        };
        _appointmentRepoMock.Setup(a => a.GetListAsync(null)).ReturnsAsync(appointments);

        var result = await _sut.GetAllAsync();
        result.Should().NotBeNull();
        result.Value.Count.Should().BeGreaterThan(0);
        result.Value.Should().BeOfType(typeof(List<AppointmentDto>));
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllByPersonelIdAsync_ShouldThrowAppointmentNotFoundException_WhenAppointmentNotExists() {
        var personnelId = Guid.NewGuid();

        _appointmentRepoMock.Setup(a => a.GetListAsync(x => x.PersonnelId == personnelId)).ReturnsAsync(new List<Appointment>());

        Func<Task> act = () => _sut.GetAllByPersonelIdAsync(personnelId);

        await act.Should().ThrowAsync<AppointmentNotFoundException>();
    }

    [Fact]
    public async Task GetAllByPersonelIdAsync_ShouldReturnAppointmentList_WhenAppointmentsExists() {
        var personnelId = Guid.NewGuid();
        var appointments = new List<Appointment>() {
            new Appointment() { Id = Guid.NewGuid(), PersonnelId=personnelId}
        };

        _appointmentRepoMock.Setup(a => a.GetListAsync(It.IsAny<Expression<Func<Appointment, bool>>>())).ReturnsAsync(appointments);

        var result = await _sut.GetAllByPersonelIdAsync(personnelId);
        result.Should().NotBeNull();
        result.Value.Count.Should().BeGreaterThan(0);
        result.Value.Should().BeOfType(typeof(List<AppointmentDto>));
        result.Success.Should().BeTrue();
    }
}