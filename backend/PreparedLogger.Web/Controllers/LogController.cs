using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PreparedLogger.DataAccess;
using PreparedLogger.Models;

namespace PreparedLogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly PreparedLoggerContext context;

        public LogController(PreparedLoggerContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<Log[]> GetAll()
        {
            return context.Logs.ToArray();
        }

        [HttpGet("{logID}", Name = "GetLog")]
        public ActionResult<Log> GetById(int logID)
        {
            var result = context.Logs.SingleOrDefault(l => l.LogID == logID);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost]
        public ActionResult<Log> Create([FromBody]Log log)
        {
            context.Logs.Add(log);
            context.SaveChanges();
            return log;
        }
    }
}