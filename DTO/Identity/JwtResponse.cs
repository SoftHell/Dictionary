namespace DTO.Identity
{
    public class JwtResponse
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}