using System.Text.Json.Serialization;

namespace BookManagementAPI.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Role { get; set; }

        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public ResponseDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public ResponseDto(bool isSuccess, string message, string role)
        {
            IsSuccess = isSuccess;
            Message = message;
            Role = role;
        }
    }
}
