using DVLD.API.Base;
using DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [Route("api/Statistics")]
    [ApiController]
    public class StatisticsController : AppControllerBase
    {
        [HttpGet("All", Name = "GetAllStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStatistics([FromQuery] GetAllStatisticsQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }
    }
}
