using BookManagementAPI.Models;
using System.Text.Json.Serialization;

namespace BookManagementAPI.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] //Augustas: Ensures the enum is serialized as a string
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

        public UserRole Role { get; set; } //Augustas: string to UserRole?

        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public ResponseDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public ResponseDto(bool isSuccess, string message, UserRole role) //Augustas: string to UserRole
        {
            IsSuccess = isSuccess;
            Message = message;
            Role = role;
        }
    }
}
