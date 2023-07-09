// Libs
using Microsoft.AspNetCore.Mvc;
using Models;

// Classes
namespace aspdotnet_crud.Controllers {
  [ApiController]
  [Route("/")]
  public class CatController : ControllerBase {
    private readonly ILogger<CatController> _logger;

    public CatController( ILogger<CatController> logger ) {
      _logger = logger;
    }

    /// <summary>
    /// POST /
    /// </summary>
    [HttpPost]
    public ActionResult Create([FromBody] CatRequest reqCat) {
      _logger.LogInformation("Request on: POST /");

      // Try to add the cat to the database.
      _logger.LogInformation("Trying to add the cat to the database...");
      try {
        CatModel createdCat = CatModel.Create(reqCat);
        _logger.LogInformation("The cat#{id} was successfully created. Returning...", createdCat.Id);
        return Created($"/{createdCat.Id}", createdCat);
      } catch (Exception err) {
        _logger.LogWarning("Cat not created. {err}", err.Message);
        _logger.LogInformation("Returning...");
        return BadRequest();
      }
    }

    /// <summary>
    /// GET /{id}
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult Get(string id) {
      _logger.LogInformation("Request on: GET /{id}", id);
      
      // Check if the provided id is a valid uuid.
      if (!Guid.TryParse(id, out _)) {
        _logger.LogWarning("The provided id is not valid. Returning...");
        return BadRequest("Id not valid.");
      }

      // Try to get the cat using the provided id.
      _logger.LogInformation("Trying to get the cat#{id} ...", id);
      try {
        CatModel cat = CatModel.Get(id);
        _logger.LogInformation("The cat was found. Returning...");
        return Ok(cat);
      } catch (Exception err) {
        _logger.LogWarning("The cat was not found. {err}", err);
        _logger.LogInformation("Returning...");
        return BadRequest();
      }
    }

    /// <summary>
    /// PATCH /{id}
    /// </summary>
    [HttpPatch("{id}")]
    public ActionResult Update(string id, [FromBody] CatRequest reqCat) {
      _logger.LogInformation("Request on: PATCH /{id}", id);

      // Check if the provided id is a valid uuid.
      if (!Guid.TryParse(id, out _)) {
        _logger.LogWarning("The provided id is not valid. Returning...");
        return BadRequest("Id not valid.");
      }

      // Try to update the cat in the database.
      _logger.LogInformation("Trying to update the cat#{id} to the database...", id);
      try {
        CatModel updatedCat = CatModel.Update(id, reqCat);
        _logger.LogInformation("The cat#{id} was successfully updated. Returning...", id);
        return Ok(updatedCat);
      } catch (Exception err) {
        _logger.LogWarning("Cat not updated. {err}", err.Message);
        _logger.LogInformation("Returning...");
        return BadRequest();
      }
    }

    /// <summary>
    /// DELETE /{id}
    /// </summary>
    [HttpDelete("{id}")]
    public ActionResult Delete(string id) {
      _logger.LogInformation("Request on: DELETE /{id}", id);
      
      // Check if the provided id is a valid uuid.
      if (!Guid.TryParse(id, out _)) {
        _logger.LogWarning("The provided id is not valid. Returning...");
        return BadRequest("Id not valid.");
      }

      // Try to delete the cat.
      _logger.LogInformation("Trying to delete the cat#{id} ...", id);
      try {
        CatModel.Delete(id);
        _logger.LogInformation("The cat was deleted. Returning...");
        return NoContent();
      } catch (Exception err) {
        _logger.LogWarning("The cat was not delete. {err}", err);
        _logger.LogInformation("Returning...");
        return BadRequest();
      }
    }
  }
}