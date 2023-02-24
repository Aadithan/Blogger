using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlog.Host.Controllers.Common;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}