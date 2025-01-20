using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Interfaces.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [EnableRateLimiting("user-ip")]
public class CuttingDownController(ICuttingDownAService cuttingDownAService, ICuttingDownBService cuttingDownBService) : ControllerBase
{
    /// <summary>
    /// Generate test data for CuttingDownA
    /// </summary>
    /// <returns>Success message</returns>
    [HttpGet("generate-cabin-cuttings")]
    public async Task<IActionResult> GenerateCabinCuttingsAsync()
    {
        try
        {
            var result = await cuttingDownAService.GenerateCabinCuttingsAsync();
            if (!result)
                return BadRequest("Data already exists in CuttingDownA table.");
            
            return Ok("Cabin cuttings test data generated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while generating cabin cuttings data.");
        }
    }

    /// <summary>
    /// Generate test data for CuttingDownB
    /// </summary>
    /// <returns>Success message</returns>
    [HttpGet("generate-cable-cuttings")]
    public async Task<IActionResult> GenerateCableCuttingsAsync()
    {
        try
        {
            var result = await cuttingDownBService.GenerateCableCuttingsAsync();
            if (!result)
                return BadRequest("Data already exists in CuttingDownB table.");
            
            return Ok("Cable cuttings test data generated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while generating cable cuttings data.");
        }
    }
}
