using System.ComponentModel.DataAnnotations;
using APBD10.Models;

namespace APBD10.DTOs;

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}

public class CreateDoctorDTO
{
    [Required]
    [MaxLength(200)]
    public string FirstName { get; set; } = "";
    [Required]
    [MaxLength(200)]
    public string LastName { get; set; } = "";
    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = "";
}