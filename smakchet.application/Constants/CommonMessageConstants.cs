namespace smakchet.application.Constants
{
    public static class CommonMessageConstants { }

    public static class ErrorMessageConstants
    {
        public const string GeneralUnexpectedError = "An unexpected error occurred.";
        public const string ValidationFailed = "One or more validation errors occurred.";
        public const string ResourceNotFound = "The requested resource was not found.";
        public const string UnauthorizedAccess = "You are not authorized to perform this action.";
        public const string ForbiddenAccess = "You do not have permission to access this resource.";
        public const string BadRequest = "The request is invalid.";
        public const string Conflict = "A conflict occurred with the current state of the resource.";
        public const string ResourceNotFoundById = "{0} with Id {1} was not found.";
        public const string AlreadyExists = "{0} with {1} already exists.";
        public const string OperationFailed = "{0} operation failed. Please try again.";
    }

    // ✅ SUCCESS MESSAGES
    public static class SuccessMessageConstants
    {
        public const string Created = "{0} was created successfully.";
        public const string Updated = "{0} was updated successfully.";
        public const string Deleted = "{0} was deleted successfully.";
        public const string Retrieved = "{0} retrieved successfully.";
    }

    // ⚠️ WARNING / INFORMATIONAL MESSAGES
    public static class WarningMessageConstants
    {
        public const string PartialSuccess = "Operation completed with some issues.";
        public const string DeprecatedApi = "This API endpoint is deprecated and may be removed in future.";
        public const string LimitedAccess = "You have limited access to this resource.";
        public const string DataIncomplete = "Some required data is missing.";
    }

    // ℹ️ INFO / LOGGING MESSAGES (OPTIONAL)
    public static class InfoMessageConstants
    {
        public const string OperationStarted = "{0} operation started.";
        public const string OperationInProgress = "{0} operation in progress.";
        public const string OperationQueued = "{0} operation queued for processing.";
        public const string UserLoggedIn = "User {0} logged in.";
        public const string UserLoggedOut = "User {0} logged out.";
        public const string OperationCompleted = "{0} operation completed successfully.";

    }
}
