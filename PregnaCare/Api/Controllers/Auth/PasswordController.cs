using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PregnaCare.Api.Controllers.Auth
{
    /// <summary>
    /// PasswordController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class PasswordController : ControllerBase
    {

    }
}
