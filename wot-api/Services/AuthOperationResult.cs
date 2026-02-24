namespace wot_api.Services
{
    public enum AuthErrorType
    {
        None,
        Validation,
        Unauthorized
    }

    public class AuthOperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public AuthErrorType ErrorType { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Value { get; private set; }

        public static AuthOperationResult<T> Success(T value)
        {
            return new AuthOperationResult<T>
            {
                IsSuccess = true,
                ErrorType = AuthErrorType.None,
                Value = value
            };
        }

        public static AuthOperationResult<T> ValidationFailure(string errorMessage)
        {
            return new AuthOperationResult<T>
            {
                IsSuccess = false,
                ErrorType = AuthErrorType.Validation,
                ErrorMessage = errorMessage
            };
        }

        public static AuthOperationResult<T> UnauthorizedFailure(string errorMessage)
        {
            return new AuthOperationResult<T>
            {
                IsSuccess = false,
                ErrorType = AuthErrorType.Unauthorized,
                ErrorMessage = errorMessage
            };
        }
    }
}
