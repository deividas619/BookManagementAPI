namespace BookManagementAPI.Models
{
    public class User : CommonProperties
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        //public virtual ICollection<Book> UserCreatedBooks { get; set; } = null;
    }
}
