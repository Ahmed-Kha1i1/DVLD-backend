using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Countries;
using DVLD.Application.Features.Countries.Queries.GetCountryQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Countries")]
    [ApiController]
    public class CountriesController(ICountryRepository countryRepository) : AppControllerBase
    {
        [HttpGet("{Id:int}", Name = "GetCountryById")]//CountryID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountry([FromRoute()] IdRequest request)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(request.Id));
            return CreateResult(country);
        }

        [HttpGet("{CountryName:regex(^[[a-zA-Z ]]*$)}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountry(string CountryName)
        {
            var country = await _mediator.Send(new GetCountryByNameQuery(CountryName));
            return CreateResult(country);
        }

        [HttpGet("All", Name = "GetAllCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCountries()
        {
            var result = await countryRepository.ListAsync();
            return CreateResult(new Response<IReadOnlyList<CountryDTO>>(HttpStatusCode.OK, result));
        }
    }
}
