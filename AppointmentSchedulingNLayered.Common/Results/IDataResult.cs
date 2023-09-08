namespace AppointmentSchedulingNLayered.Common.Results;

public interface IDataResult<out T> : IResult {
    T Value { get; }
}