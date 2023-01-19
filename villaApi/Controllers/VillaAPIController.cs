using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using villaApi.Data;
using villaApi.Models;
using villaApi.Models.Dto;

namespace villaApi.Controllers
{

  //   [Route("api/VillaAPI")]
  [Route("api/[controller]")]
  [ApiController]
  public class VillaAPIController : ControllerBase
  {
    private readonly ILogger<VillaAPIController> _logger;

    public VillaAPIController(ILogger<VillaAPIController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
      _logger.LogInformation("Get All Villa called");
      return Ok(VillaStore.villaList);
    }

    [HttpGet("id:int")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(200, Type = typeof(VillaDTO))]
    // [ProducesResponseType(400)]
    // [ProducesResponseType(404)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
      _logger.LogInformation("Get Villa called");
      if (id == 0)
      {
        return BadRequest();
      }
      var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
      if (villa == null)
      {
        return NotFound();
      }

      return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
    {
      _logger.LogInformation("Create Villa called");
      if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
      {
        ModelState.AddModelError("Error", "Villa name already exists");
        return BadRequest(ModelState);
      }
      if (villa == null)
      {
        return BadRequest(villa);
      }

      if (villa.Id > 0)
      {
        return BadRequest(StatusCodes.Status500InternalServerError);
      }

      villa.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
      VillaStore.villaList.Add(villa);
      return CreatedAtAction(nameof(GetVilla), new { id = villa.Id }, villa);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVilla(int id)
    {
      _logger.LogInformation("Delete Villa called");
      if (id == 0)
      {
        return BadRequest();
      }
      var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
      if (villa == null)
      {
        return NotFound();
      }

      VillaStore.villaList.Remove(villa);
      return NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villa)
    {
      _logger.LogInformation("Update Villa called");
      if (villa == null || villa.Id != id)
      {
        return BadRequest();
      }
      if (id == 0)
      {
        return BadRequest();
      }
      var villaToUpdate = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
      if (villaToUpdate == null)
      {
        return NotFound();
      }

      villaToUpdate.Name = villa.Name;
      villaToUpdate.Ocupancy = villa.Ocupancy;
      villaToUpdate.Sqft = villa.Sqft;
      return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PartialUpdateVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDoc)
    {
      _logger.LogInformation("Partial Update Villa called");
      if (patchDoc == null || id == 0)
      {
        return BadRequest();
      }
      var villaToUpdate = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
      if (villaToUpdate == null)
      {
        return NotFound();
      }

      patchDoc.ApplyTo(villaToUpdate, ModelState);
      if (!TryValidateModel(villaToUpdate))
      {
        return ValidationProblem(ModelState);
      }

      return NoContent();
    }
  }
}
