using System.Drawing;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace aspdotnet_crud.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase {
        private readonly ILogger<CatController> _logger;
         static readonly List<Models.CatModel> Cats = new();

        public CatController( ILogger<CatController> logger ) {
            _logger = logger;
        }

        /// <summary>
        /// POST /
        /// </summary>
        [HttpPost]
        public ActionResult Post([FromBody] CatRequest reqCat) {
            _logger.LogInformation("Request on: POST /");

            // Create the cat and add it to  cats list.
            int index = Cats.Count;
            CatModel newCat = new() {
                Index = index,
                Name = reqCat.Name,
                Color = reqCat.Color,
            };
            Cats.Add(newCat);

            // Return the cat.
            return Ok(newCat);
        }

        /// <summary>
        /// GET /{index}
        /// </summary>
        [HttpGet("{index}")]
        public ActionResult Get(int index) {
            _logger.LogInformation("Request on: GET /{index}", index);

            // Check if the provindexed index exists.
            if (index >= Cats.Count) {
                return BadRequest("index not exists.");
            }

            // Get and return the cat.
            CatModel cat = Cats[index];
            return Ok(cat);
        }

        /// <summary>
        /// PATCH /{index}
        /// </summary>
        [HttpPatch("{index}")]
        public ActionResult Patch(int index, [FromBody] CatRequest reqCat) {
            _logger.LogInformation("Request on: PATCH /{index}", index);

            // Check if the provindexed index exists.
            if (index >= Cats.Count) {
                return BadRequest("index not exists.");
            }

            // Update the cat.
            CatModel cat = new() {
                Index = index,
                Name = reqCat.Name,
                Color = reqCat.Color
            };
            Cats[index] = cat;

            // Get and return the cat.
            return Ok(cat);
        }

        /// <summary>
        /// DELETE /{index}
        /// </summary>
        [HttpDelete("{index}")]
        public ActionResult Delete(int index) {
            _logger.LogInformation("Request on: DELETE /");

            // Check if the provindexed index exists.
            if (index >= Cats.Count) {
                return BadRequest("index not exists.");
            }

            // Delete the cat.
            Cats[index] = null;

            // Get and return the cat.
            return Ok(Cats[index]);
        }
    }
}