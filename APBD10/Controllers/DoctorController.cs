using APBD10.Context;
using APBD10.DTOs;
using APBD10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    protected readonly ApbdContext _context;

    public DoctorController(ApbdContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = await _context.Doctors
            .Include(d => d.Prescriptions)
            .Select(d => new DoctorDTO()
            {
                IdDoctor = d.IdDoctor,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Prescriptions = d.Prescriptions
            }).ToListAsync();
        return Ok(doctors);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDTO dto)
    {
        var doctor = new Doctor()
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDoctor(int idDoctor)
    {
        var doctor = await _context.Doctors.FindAsync(idDoctor);
        if (doctor is null) 
            return NotFound("Doctor not found");
        
        await _context.Doctors
            .Where(s => s.IdDoctor == idDoctor)
            .ExecuteDeleteAsync();
        return NoContent();
    }
}