using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace App
{
    public abstract class BaseController : Controller
    {
        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }


        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }


        protected IActionResult Error(string error)
        {
            return BadRequest(Envelope.Error(error));
        }


        protected IActionResult FromResult(Result result)
        {
            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
