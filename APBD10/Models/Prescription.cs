using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD10.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public virtual Doctor IdDoctorNavigation { get; set; } = null!;
    [ForeignKey(nameof(IdPatient))]
    public virtual Patient IdPatientNavigation { get; set; } = null!;
}