using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Repository;

using static Api.Validation.CommentValidation;

namespace Api.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly BlogContext _blogContext;

        public CommentController(ILogger<CommentController> logger, BlogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            try
            {
                CommentRepository repository = new(_blogContext);

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
        public ActionResult<Comment> Get([FromRoute] Guid id)
        {
            try
            {
                CommentRepository repository = new(_blogContext);

                Comment result = repository.Get(id);

                return Ok(result);
            }
            catch (MissingCommentException ex) //Comment not found in repository
            {
                string message = string.Format("Target Comment not found: {0}", id.ToString());
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
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            try
            {
                CommentRepository repository = new(_blogContext);

                //Validate with this extension method
                comment.Validate(_blogContext);

                Comment result = repository.Create(comment);

                return Ok(result);
            }
            catch (CommentAlreadyExists ex) //Comment with that Id exists already
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (BadDataException ex) //Validation Issue
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (MissingPostException ex) //PostId supplied doesn't actually match any existing post
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

        [HttpPut("{id:guid}")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Comment comment)
        {
            try
            {
                CommentRepository repository = new(_blogContext);

                comment.Validate(_blogContext);

                Comment result = repository.Update(id, comment);

                return Ok(result);
            }
            catch (BadDataException ex) //Validation Issue
            {
                _logger.Log(LogLevel.Warning, ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (MissingPostException ex)  //PostId supplied doesn't actually match any existing post
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
                CommentRepository repository = new(_blogContext);

                bool deletionResult = repository.Delete(id);

                return Ok(deletionResult);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}