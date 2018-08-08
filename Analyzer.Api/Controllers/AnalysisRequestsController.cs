using System;
using System.Threading.Tasks;
using System.Web.Http;
using Analyzer.Api.Models;
using CommitMap.Services.Facade;
using CommitMap.Services.Integration;

namespace Analyzer.Api.Controllers
{
    public class AnalysisRequestsController : ApiController
    {
        [HttpPost]
        [Usage("/v1/campaigns/{id}", "GET")]
        public IHttpActionResult Post([FromBody] AnalysisRequestModel model)
        {
            if (model == null) 
            {
                return BadRequest("Analysis request model is missing");
            }

            if (!Uri.IsWellFormedUriString(model.CallbackUrl, UriKind.Absolute))
            {
                return BadRequest("Callback Uri is invalid. Provide a valid absolute Uri");
            }

            //var context = HttpContext.Current;

            string from = model.From;
            string to = model.To;
            Uri callbackUrl = new Uri(model.CallbackUrl, UriKind.Absolute);

            Task.Factory.StartNew(async () =>
            {
                using (var scope = ContainerProvider.BeginScope())
                {
                    //SetContextForThread(context, requestedBy, "ReserveThread");
                    var engine = (ICommitMapEngine)scope.GetService(typeof(ICommitMapEngine));

                    var client = (IAnalysisResultsApiClient)scope.GetService(typeof(IAnalysisResultsApiClient));

                    try
                    {
                        var result = await engine.Run(from, to);

                        await client.PostAsync(callbackUrl, result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            });

            return Ok();
        }
    }
}