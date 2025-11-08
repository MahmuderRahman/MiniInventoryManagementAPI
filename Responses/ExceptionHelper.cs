using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MiniInventoryManagementAPI.Responses
{
    public class ExceptionHelper : ApiResponseBase
    {
     
        public ExceptionHelper(Exception e)
        {
            SetResponseData(new ApiResponse(e.InnerMessage()));
        }

        public static int ThrowSqlException(DbUpdateException e)
        {
            var ex = e.GetBaseException() as SqlException;
            if (ex != null)
                return ex.Number;

            return -1;
        }

        public void SetResponseData(ApiResponse apiResponse)
        {
            succeeded = apiResponse.succeeded;
            errorMessages = apiResponse.errorMessages;
            successMessages = apiResponse.successMessages;
            payload = apiResponse.payload;
        }

    }
    public static class ExceptionExtensions
    {
        public static string InnerMessage(this Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception.Message;
        }
    }

    public class ErrorException : Exception
    {
        public ErrorException(string errorMessage) : base(errorMessage)
        {
        }
    }


}
