using tutorial9.DTOs;

namespace tutorial9.Services;

public interface IPrescriptionsService
{
    Task AddPrescription(PrescriptionRequestDto prescriptionDto);
    Task<object?> GetPatientWithPrescriptions(int id);
}