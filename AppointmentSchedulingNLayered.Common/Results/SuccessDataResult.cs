namespace AppointmentSchedulingNLayered.Common.Results;

public class SuccessDataResult<T> : DataResult<T> {
    public SuccessDataResult(T value, string message) : base(value, true, message) {
    }

    public SuccessDataResult(T value) : base(value, true) {
    }

    public SuccessDataResult(string message) : base(default, true, message) {

    }

    public SuccessDataResult() : base(default, true) {

    }
}
