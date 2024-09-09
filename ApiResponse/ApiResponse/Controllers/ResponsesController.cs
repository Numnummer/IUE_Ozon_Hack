using ApiResponse.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiResponse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly IResponsesDbService _responsesService;

        public ResponsesController(IResponsesDbService responsesService)
        {
            _responsesService = responsesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetResponse(int queryId)
        {
            var res = await _responsesService.GetByQueryId("products", "responses", queryId);
            Console.WriteLine("--> Got result in res");
            foreach (var response in res)
            {
                Console.Write(response.Id + " ");
            }
            Console.WriteLine();
            if (res == null)
            {
                throw new Exception("Can't handle operation");
            }
            if (res.Count == 0)
            {
                return Accepted(102);
            }

            return Ok(res);
        }
    }
}
