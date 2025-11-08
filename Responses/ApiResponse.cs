namespace MiniInventoryManagementAPI.Responses
{
    public class ApiResponse : ApiResponseBase
    {
        public ApiResponse(bool isSucceeded = true)
        {
            succeeded = isSucceeded;
        }

        public ApiResponse(string errorMessage = null, string successMessage = null, bool isSucceeded = false)
        {
            succeeded = isSucceeded;
            if (errorMessage != null)
                errorMessages = new List<string> { errorMessage };
            if (successMessage != null)
                successMessages = new List<string> { successMessage };
        }

        public ApiResponse(IEnumerable<string> errorMessages = null, IEnumerable<string> successMessages = null, bool isSucceeded = false)
        {
            succeeded = isSucceeded;
            if (errorMessages != null)
                this.errorMessages = errorMessages;
            if (successMessages != null)
                this.successMessages = successMessages;
        }

        public ApiResponse(object payloads, bool isSucceeded = true, string errorMessage = null, string successMessage = null)
        {
            succeeded = isSucceeded;
            payload = payloads;
            if (errorMessage != null)
                errorMessages = new List<string> { errorMessage };
            if (successMessage != null)
                successMessages = new List<string> { successMessage };
        }

    }


    public abstract class ApiResponseBase
    {
        public bool succeeded { get; set; }

        private IEnumerable<string> _errorMessages;

        public IEnumerable<string> errorMessages
        {
            get { return _errorMessages ?? new List<string>(); }
            set { _errorMessages = value; }
        }

        private IEnumerable<string> _successMessages;

        public IEnumerable<string> successMessages
        {
            get { return _successMessages ?? new List<string>(); }
            set { _successMessages = value; }
        }

        private object _payload;

        public object payload
        {
            get { return _payload ?? new List<object>(); }
            set { _payload = value; }
        }
    }
}
