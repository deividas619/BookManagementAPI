namespace BookManagementAPI.Models
{
    public class Admin : CommonProperties
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}