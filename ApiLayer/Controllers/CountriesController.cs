using AutoMapper;
using BusinessLayer.Country;
using DataLayerCore.Country;
using DVLDApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/Countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;

        public CountriesController(IMapper mapper)
        {
            this._mapper = mapper;
        }
        [HttpGet("{CountryId:int}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDTO>> GetCountry(int CountryId)
        {
            if(CountryId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid country id"));
            }

            clsCountry? Country = await clsCountry.Find(CountryId);

            if(Country is null)
            {
                return NotFound(CreateResponse(StatusFail, "Country not found"));
            }
            var CountryDTO = _mapper.Map<CountryDTO>(Country);
            return Ok(CreateResponse(StatusSuccess, CountryDTO) );
        }


        [HttpGet("{CountryName:regex(^[[a-zA-Z ]]*$)}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDTO>> GetCountry(string CountryName)
        {
            if (string.IsNullOrEmpty(CountryName))
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid country name"));
            }

            clsCountry? Country = await clsCountry.Find(CountryName.Trim());

            if (Country is null)
            {
                return NotFound(CreateResponse(StatusFail, "Country not found"));
            }

            var CountryDTO = _mapper.Map<CountryDTO>(Country);
            return Ok(CreateResponse(StatusSuccess, CountryDTO));
        }

        [HttpGet("All", Name = "GetAllCountries")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CountryDTO>>> GetAllCountries() 
        {
            var CountriesList = await clsCountry.GetAllCountries();

            if (CountriesList.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No countries found!"));
            }

            return Ok(CreateResponse(StatusSuccess, new { Length = CountriesList.Count,data = CountriesList }));
        }
    }
}
