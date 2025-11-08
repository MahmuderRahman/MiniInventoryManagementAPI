using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MiniInventoryManagementAPI.Responses.Lib
{
    public class ExceptionController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int ThrowSqlException(SqlException e)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return e.Number;
        }

        public string ThrowException(Exception e)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return e.Message;
        }
    }

}
