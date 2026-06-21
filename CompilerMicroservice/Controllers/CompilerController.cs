using System.Threading.Tasks;
using System.Web.Http;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.Controllers
{
    [RoutePrefix("api/compiler")]
    public class CompilerController : ApiController
    {
        private readonly ICodeExecutionService _executionService;

        public CompilerController(ICodeExecutionService executionService)
        {
            _executionService = executionService;
        }

        [HttpPost]
        [Route("run")]
        public async Task<IHttpActionResult> RunCode([FromBody] CompileRequest request)
        {
            if (request == null || request.Files == null || request.Files.Count == 0)
                return BadRequest("Invalid request: At least one code file is required.");

            if (string.IsNullOrEmpty(request.Language))
                return BadRequest("Language must be specified (python or csharp).");

            if (string.IsNullOrEmpty(request.EntryPoint))
                request.EntryPoint = request.Files[0].Name; // fallback

            var result = await _executionService.ExecuteAsync(request);
            return Ok(result);
        }
    }
}