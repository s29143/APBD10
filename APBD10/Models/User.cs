using System.ComponentModel.DataAnnotations;

namespace APBD10.Models;

public class User
{
    [Key]
    public int IdUser { get; set; }
    [MaxLength(255)]
    [Required]
    public string Login { get; set; } = "";
    [MaxLength(255)]
    [Required]
    public string Password { get; set; } = "";

    public string Salt { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpr { get; set; }
}