using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Features.Post.Queries;
using BLOG.Domain.DTO;
using BLOG.Domain.ReadModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ApiControllerBase
    {
        /// <summary>
        /// Utwóż nowy post
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePostDTO dto)
        {
            return HandleAppResult(await Mediator.Send(new PostCreateCommand { PostDTO = dto }));
        }

        /// <summary>
        /// Edytuj post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] int id ,[FromBody] CreatePostDTO dto)
        {
            return HandleAppResult(await Mediator.Send(new PostUpdateCommand { Id = id ,PostDTO = dto }));
        }

        /// <summary>
        /// Usuń post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return HandleAppResult(await Mediator.Send(new PostDeleteCommand { Id = id }));
        }

        /// <summary>
        /// Pobierz post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PostSearchResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return HandleAppResult(await Mediator.Send(new PostGetQuery { Id = id }));
        }

    }
}
