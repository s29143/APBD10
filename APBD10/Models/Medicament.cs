using System.ComponentModel.DataAnnotations;

namespace APBD10.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";
    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = "";
    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = "";

    public virtual ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; } =
        new List<Prescription_Medicament>();
}