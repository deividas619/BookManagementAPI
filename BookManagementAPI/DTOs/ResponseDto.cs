using BookManagementAPI.Models;
using System.Text.Json.Serialization;

namespace BookManagementAPI.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UserRole? Role { get; set; }

        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public ResponseDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public ResponseDto(bool isSuccess, string message, UserRole role)
        {
            IsSuccess = isSuccess;
            Message = message;
            Role = role;
        }
    }
}
