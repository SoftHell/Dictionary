namespace DTO.Identity
{
    public class Register
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public string FirstName { get; set; }= default!;
        
        public string LastName { get; set; }= default!;
        
        public string IdNumber { get; set; }= default!;
    }
}