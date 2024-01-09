using BLOG.Application.Features.Comment.Commands;
using BLOG.Application.Features.Comment.Queries;
using BLOG.Application.Wrappers;
using BLOG.Domain.DTO;
using BLOG.Domain.ReadModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ApiControllerBase
    {
        /// <summary>
        /// Dodaj komentarz
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateCommentDTO dto)
        {
            return HandleAppResult(await Mediator.Send(new CommentCreateCommand { CommentDTO = dto }));
        }

        /// <summary>
        /// Edytuj komentarz
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("udpate/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateCommentDTO dto)
        {
            return HandleAppResult(await Mediator.Send(new CommentUpdateCommand { Id = id, CommentDTO = dto }));
        }

        /// <summary>
        /// Usuń komentarz
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return HandleAppResult(await Mediator.Send(new CommentDeleteCommand { Id = id }));
        }

        /// <summary>
        /// Pobierz komentarze
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(PagedList<CommentSearchResult>), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Search([FromQuery] CommentSearchQuery query)
        {
            return HandleAppResult(await Mediator.Send(query));
        }
    }
}
