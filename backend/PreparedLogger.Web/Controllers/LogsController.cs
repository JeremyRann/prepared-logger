using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreparedLogger.Data;
using PreparedLogger.Data.Models;

namespace PreparedLogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly PreparedLoggerContext context;
        public LogsController(PreparedLoggerContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<Log[]> GetAll()
        {
            return context.Logs.ToArray();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Log> Get(int id)
        {
            var log = context.Logs.FirstOrDefault(l => l.LogID == id);
            if (log == null)
                return new NotFoundResult();

            return log;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Log log)
        {
            context.Logs.Add(log);
            await context.SaveChangesAsync();
            return new NoContentResult();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Log log)
        {
            if (log.LogID != id)
                return new BadRequestObjectResult("LogID mismatch");
            var originalLog = context.Logs.FirstOrDefault(l => l.LogID == id);
            if (originalLog == null)
                return new NotFoundResult();
            originalLog.Name = log.Name;
            await context.SaveChangesAsync();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var log = context.Logs.FirstOrDefault(l => l.LogID == id);
            if (log == null)
                return new NotFoundResult();
            context.Logs.Remove(log);
            await context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}