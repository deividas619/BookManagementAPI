namespace BookManagementAPI.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }

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

        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
