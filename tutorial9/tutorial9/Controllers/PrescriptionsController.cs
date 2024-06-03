using Microsoft.AspNetCore.Mvc;
using tutorial9.DTOs;
using tutorial9.Services;

namespace tutorial9.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionsService _prescriptionsService;

    public PrescriptionsController(IPrescriptionsService prescriptionsService)
    {
        _prescriptionsService = prescriptionsService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto prescriptionDto)
    {
        if (prescriptionDto.Medications.Count > 10)
        {
            return BadRequest("A prescription can include a maximum of 10 medications.");
        }
        
        try
        {
            await _prescriptionsService.AddPrescription(prescriptionDto);
            return Ok("Client assigned to trip.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        try
        {
            var patient = await _prescriptionsService.GetPatientWithPrescriptions(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}