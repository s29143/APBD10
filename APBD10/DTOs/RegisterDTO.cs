namespace APBD10.DTOs;

public class RegisterDTO
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginDTO
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RefreshTokenDTO
{
    public string RefreshToken { get; set; } = "";
}