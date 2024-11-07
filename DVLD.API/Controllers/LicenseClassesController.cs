using DVLD.API.Base;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/LicenseClasses")]
    [ApiController]
    public class LicenseClassesController(ILicenseClassRepository licenseClassRepository) : AppControllerBase
    {
        [HttpGet("All", Name = "GetAllLicnseClasses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLicnseClasses()
        {

            var result = await licenseClassRepository.ListAllAsync();
            return CreateResult(new Response<IReadOnlyList<LicenseClass>>(HttpStatusCode.OK, result));
        }
    }
}
