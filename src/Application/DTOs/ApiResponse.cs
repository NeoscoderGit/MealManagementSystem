
namespace Application.DTOs
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }
        public ApiResponse(bool success = true, string message = "")
        {
            Success = success;
            Message = message;
        }
        public ApiResponse(T data, bool success = true, string message = "")
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
