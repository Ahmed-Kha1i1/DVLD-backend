﻿using AutoMapper;
using BusinessLayer.InternationalLicense;
using DataLayerCore.InternationalLicense;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/InternationalLicenses")]
    [ApiController]
    public class InternationalLicenseController : ControllerBase
    {
        private readonly IMapper _mapper;

        public InternationalLicenseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{InternationalLicenseID}", Name = "GetInternationalLicenseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInternationalLicense(int InternationalLicenseID)
        {
            if (InternationalLicenseID < 0)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid international license ID"));
            }

            var internationalLicense = await clsInternationalLicense.FindInternationalLicense(InternationalLicenseID);

            if (internationalLicense == null)
            {
                return NotFound(CreateResponse(StatusFail, "International license not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<InternationalLicenseDTO>(internationalLicense)));
        }

        [HttpPost(Name = "AddInternationalLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddInternationalLicense(InternationalLicenseForCreateDTO internationalLicenseDTO)
        {
            if (internationalLicenseDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "International license object cannot be null"));
            }

            var internationalLicense = _mapper.Map<clsInternationalLicense>(internationalLicenseDTO);

            if (await internationalLicense.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<InternationalLicenseDTO>(internationalLicense));
                return CreatedAtRoute("GetInternationalLicenseById", new { InternationalLicenseID = internationalLicense.InternationalLicenseID }, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding international license"));
            }
        }

        [HttpPut("{id}", Name = "UpdateInternationalLicense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInternationalLicense(int id, InternationalLicenseForUpdateDTO internationalLicenseDTO)
        {
            if (internationalLicenseDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "International license object cannot be null"));
            }

            if (id < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid international license ID"));
            }

            var internationalLicense = await clsInternationalLicense.FindInternationalLicense(id);

            if (internationalLicense == null)
            {
                return NotFound(CreateResponse(StatusFail, $"International license with ID {id} not found."));
            }

            _mapper.Map(internationalLicenseDTO, internationalLicense);

            if (await internationalLicense.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<InternationalLicenseDTO>(internationalLicense));
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating international license"));
            }
        }

        [HttpGet("All", Name = "GetAllInternationalLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllInternationalLicenses()
        {
            var licensesList = await clsInternationalLicense.GetInternationalLicenses();

            if (licensesList.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No international licenses found!"));
            }

            var result = CreateResponse(StatusSuccess, new { length = licensesList.Count, data = licensesList });
            return Ok(result);
        }

        [HttpGet("ByDriver/{DriverID}", Name = "GetInternationalLicensesByDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInternationalLicensesByDriver(int DriverID)
        {
            if (DriverID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid Driver ID"));
            }
            var licenses = await clsInternationalLicense.GetInternationalLicenses(DriverID);

            if (licenses.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No international licenses found for driver!"));
            }

            return Ok(CreateResponse(StatusSuccess, licenses));
        }

        [HttpGet("ActiveByDriver/{DriverID}", Name = "GetActiveInternationalLicenseByDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiveInternationalLicenseByDriver(int DriverID)
        {
            if (DriverID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid Driver ID"));
            }

            var activeLicenseID = await clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(DriverID);

            if (activeLicenseID == null)
            {
                return NotFound(CreateResponse(StatusFail, "No active international license found for driver!"));
            }

            return Ok(CreateResponse(StatusSuccess, new { ActiveInternationalLicenseID = activeLicenseID }));
        }
    }

}