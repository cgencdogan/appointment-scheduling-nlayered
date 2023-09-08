namespace AppointmentSchedulingNLayered.Common.Results;

public class DataResult<T> : Result, IDataResult<T> {
    public DataResult(T value, bool success, string message) : base(success, message) {
        Value = value;
    }

    public DataResult(T value, bool success) : base(success) {
        Value = value;
    }

    public T Value { get; }
}
