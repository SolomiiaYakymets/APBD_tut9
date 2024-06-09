using Microsoft.EntityFrameworkCore;
using tutorial9.Models;

namespace tutorial9.Context;

public partial class ApbdContext : DbContext
{
    public ApbdContext()
    {
    }
    
    public ApbdContext(DbContextOptions<ApbdContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
          base.OnConfiguring(optionsBuilder);
          optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=APBD_tut9;User Id=sa;Password=Sy24091976!;Trust Server Certificate=True;Encrypt=True");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient);
            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Birthdate)
                  .IsRequired();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor);
            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(100);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(e => e.IdMedicament);
            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Description)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Type)
                  .IsRequired()
                  .HasMaxLength(100);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription);
            entity.Property(e => e.Date)
                  .IsRequired();
            entity.Property(e => e.DueDate)
                  .IsRequired();
            entity.HasOne(e => e.Patient)
                  .WithMany(p => p.Prescriptions)
                  .HasForeignKey(e => e.IdPatient);
            entity.HasOne(e => e.Doctor)
                  .WithMany(d => d.Prescriptions)
                  .HasForeignKey(e => e.IdDoctor);
        });

        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });
            entity.Property(pm => pm.Dose);
            entity.Property(pm => pm.Details)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.HasOne(pm => pm.Prescription)
                  .WithMany(p => p.PrescriptionMedicaments)
                  .HasForeignKey(pm => pm.IdPrescription);
            entity.HasOne(pm => pm.Medicament)
                  .WithMany(m => m.PrescriptionMedicaments)
                  .HasForeignKey(pm => pm.IdMedicament);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
              entity.HasKey(e => e.Id );
              entity.Property(e => e.FirstName);
              entity.Property(e => e.LastName)
                    .IsRequired();
              entity.Property(e => e.Password)
                    .IsRequired();
              entity.Property(e => e.Salt)
                    .IsRequired();
              entity.Property(e => e.RefreshToken);
              entity.Property(e => e.RefreshTokenExp);
        });
    }

}