using BLOG.Application.Features.File.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiControllerBase
    {
        /// <summary>
        /// Pobierz zdjęcie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("image/{name}")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImage([FromRoute] string name)
        {
            var result = await Mediator.Send(new ImageGetQuery { FileName = name });
            if (!result.IsSuccess)
                return NotFound();
            return File(result, "image/*");
        }
    }
}
