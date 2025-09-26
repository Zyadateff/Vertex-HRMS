namespace VertexHRMS.BLL.Common
{
    /// <summary>
    /// Generic API Response Wrapper
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }          
        public string Message { get; set; }        
        public T Data { get; set; }                
        public List<string> Errors { get; set; } = new List<string>(); 
        public int StatusCode { get; set; }        
        public Dictionary<string, object> Meta { get; set; } = new(); 

        public static ApiResponse<T> SuccessResult(
            T data,
            string message = "Operation completed successfully",
            int statusCode = 200,
            Dictionary<string, object> meta = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                Meta = meta ?? new Dictionary<string, object>()
            };
        }

        public static ApiResponse<T> FailureResult(
            string message,
            List<string> errors = null,
            int statusCode = 400,
            Dictionary<string, object> meta = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode,
                Meta = meta ?? new Dictionary<string, object>()
            };
        }
    }

    /// <summary>
    /// Non-generic ApiResponse
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse SuccessResult(
            string message = "Operation completed successfully",
            int statusCode = 200,
            Dictionary<string, object> meta = null)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Meta = meta ?? new Dictionary<string, object>()
            };
        }

        public static ApiResponse FailureResult(
            string message,
            List<string> errors = null,
            int statusCode = 400,
            Dictionary<string, object> meta = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode,
                Meta = meta ?? new Dictionary<string, object>()
            };
        }
    }
}
