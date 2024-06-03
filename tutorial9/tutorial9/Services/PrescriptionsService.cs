using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tutorial9.Context;
using tutorial9.DTOs;
using tutorial9.Models;

namespace tutorial9.Services;

public class PrescriptionsService : IPrescriptionsService
{
    private readonly ApbdContext _context;

    public PrescriptionsService(ApbdContext context)
    {
        _context = context;
    }

    public async Task AddPrescription(PrescriptionRequestDto prescriptionDto)
    {
        var patient = await _context.Patients.FindAsync(prescriptionDto.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                IdPatient = prescriptionDto.IdPatient,
                FirstName = prescriptionDto.PatientFirstName,
                LastName = prescriptionDto.PatientLastName,
                Birthdate = prescriptionDto.PatientBirthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var doctor = await _context.Doctors.FindAsync(prescriptionDto.IdDoctor);
        if (doctor == null)
        {
            throw new Exception("Doctor does not exist.");
        }

        foreach (var med in prescriptionDto.Medications)
        {
            var medicament = await _context.Medicaments.FindAsync(med.IdMedication);
            if (medicament == null)
            {
                throw new Exception($"Medication with Id {med.IdMedication} does not exist.");
            }
        }

        var prescription = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            Patient = patient,
            Doctor = doctor,
            PrescriptionMedicaments = prescriptionDto.Medications.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedication,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<object?> GetPatientWithPrescriptions(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return null;
        }

        var result = new
        {
            patient.IdPatient,
            patient.FirstName,
            patient.LastName,
            Prescriptions = patient.Prescriptions.Select(pr => new
            {
                pr.IdPrescription,
                pr.Date,
                pr.DueDate,
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new
                {
                    pm.IdMedicament,
                    pm.Medicament.Name,
                    pm.Dose,
                    pm.Medicament.Description
                }),
                Doctor = new
                {
                    pr.Doctor.IdDoctor,
                    pr.Doctor.FirstName,
                    pr.Doctor.LastName
                }
            }).OrderBy(pr => pr.DueDate)
        };
        return result;
    }

}