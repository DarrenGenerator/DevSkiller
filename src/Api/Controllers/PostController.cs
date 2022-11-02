using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;

using Repository;

using static Api.Validation.PostValidation;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly BlogContext _blogContext;
        public PostController(ILogger<PostController> logger, BlogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            try
            {
                PostRepository repository = new(_blogContext);

                var result = repository.GetAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Post> Get([FromRoute] Guid id)
        {
            try
            {
                PostRepository repository = new(_blogContext);

                Post result = repository.Get(id);

                return Ok(result);
            }
            catch (MissingPostException ex)
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] Post post)
        {
            try
            {
                PostRepository repository = new(_blogContext);

                //Validate with this extension method
                post.Validate();

                Post result = repository.Create(post);

                return Ok(result);
            }
            catch (PostAlreadyExists ex) //Post with that Id exists already
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (BadDataException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Post> Put([FromRoute] Guid id, [FromBody] Post post)
        {
            try
            {
                PostRepository repository = new(_blogContext);

                //Validate with this extension method
                post.Validate();

                Post result = repository.Update(id, post);

                return Ok(result);
            }
            catch (BadDataException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (MissingPostException ex)
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                PostRepository repository = new(_blogContext);

                bool deletionResult = repository.Delete(id);

                return Ok(deletionResult);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        [HttpGet("{id:guid}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments([FromRoute] Guid id)
        {
            try
            {
                PostRepository repositoryPosts = new(_blogContext);

                Post result = repositoryPosts.Get(id);

                CommentRepository repositoryComments = new(_blogContext);

                var comments = repositoryComments.GetByPostId(id);

                return Ok(comments);
            }
            catch(MissingPostException ex)
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}