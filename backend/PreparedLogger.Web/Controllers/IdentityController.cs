using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PreparedLogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IdentityResult[]> Get()
        {
            return User.Claims.Select(c => new IdentityResult
            {
                Type = c.Type,
                Value = c.Value
            }).ToArray();
        }
    }

    public class IdentityResult
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}