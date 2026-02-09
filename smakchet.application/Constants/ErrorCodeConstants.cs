namespace smakchet.application.Constants
{
    public static class ErrorCodeConstants
    {
        // 🔹 General
        public const string UnexpectedError = "UNEXPECTED_ERROR";
        public const string ValidationFailed = "VALIDATION_FAILED";
        public const string BadRequest = "BAD_REQUEST";

        // 🔹 Authentication / Authorization
        public const string Unauthorized = "UNAUTHORIZED";
        public const string Forbidden = "FORBIDDEN";
        public const string TokenExpired = "TOKEN_EXPIRED";
        public const string InvalidToken = "INVALID_TOKEN";

        // 🔹 Resource / Data
        public const string NotFound = "NOT_FOUND";
        public const string AlreadyExists = "ALREADY_EXISTS";
        public const string Conflict = "CONFLICT";

        // 🔹 Business rules
        public const string OperationNotAllowed = "OPERATION_NOT_ALLOWED";
        public const string InvalidState = "INVALID_STATE";

        // 🔹 External / Infrastructure
        public const string ExternalServiceFailed = "EXTERNAL_SERVICE_FAILED";
        public const string DatabaseError = "DATABASE_ERROR";
        public const string Timeout = "TIMEOUT";
    }
}