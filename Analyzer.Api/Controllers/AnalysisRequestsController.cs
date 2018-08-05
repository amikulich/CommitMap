using System;
using System.Threading.Tasks;
using System.Web.Http;
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

        public async Task Post()
        {
            try
            {
                await Task.Factory.StartNew(async () =>
                {
                    await _engine.Run("", "");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}