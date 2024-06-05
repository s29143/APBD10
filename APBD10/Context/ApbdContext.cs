using APBD10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Context;

public class ApbdContext : DbContext
{
    protected ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prescription_Medicament>().HasKey(u => new
        {
            u.IdMedicament,
            u.IdPrescription
        });
        
    }
}