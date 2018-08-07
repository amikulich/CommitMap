using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Analyzer.Api.Models;
using CommitMap.Services.Facade;

namespace Analyzer.Api.Controllers
{
    public class AnalysisRequestsController : ApiController
    {
        private readonly ICommitMapEngine _engine;

        public AnalysisRequestsController(ICommitMapEngine engine)
        {
            _engine = engine;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] AnalysisRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Analysis request model is missing");
            }

            await Task.Factory.StartNew(async () =>
            {
                await _engine.Run(model.From, model.To);
            });

            return Ok();
        }
    }
}