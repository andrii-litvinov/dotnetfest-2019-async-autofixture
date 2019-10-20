using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Service.Middleware.ExceptionHandling
{
    public class DomainExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Conflict;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(e.Message));
            }
        }
    }
}