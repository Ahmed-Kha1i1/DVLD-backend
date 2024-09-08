﻿using AutoMapper;
using BusinessLayer;
using DataLayerCore.DetainedLicense;
using Microsoft.AspNetCore.Mvc;

using static DVLDApi.Helpers.ApiResponse;
namespace DVLDApi.Controllers
{
    [ApiController]
    [Route("api/DetainedLicenses")]
    public class DetainedLicensesController : ControllerBase
    {
        private readonly IMapper _mapper;

        public DetainedLicensesController(IMapper mapper)
        {
            this._mapper = mapper;
        }

        [HttpGet("{DetainID}", Name = "GetDetainedLicenseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetainedLicense(int DetainID)
        {
            if (DetainID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid detain id"));
            }

            clsDetainedLicense? detainedLicense = await clsDetainedLicense.Find(DetainID);

            if (detainedLicense == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Detained license with ID {DetainID} not found."));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DetainedLicenseDTO>(detainedLicense)));
        }

        [HttpGet("ByLicenseID/{LicenseID}", Name = "GetDetainedLicenseByLicenseId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetainedLicenseByLicenseID(int LicenseID)
        {
            if (LicenseID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
            }

            clsDetainedLicense? detainedLicense = await clsDetainedLicense.FindByLicenseID(LicenseID);

            if (detainedLicense == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Detained license with License ID {LicenseID} not found."));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DetainedLicenseDTO>(detainedLicense)));
        }

        [HttpPost(Name = "AddDetainedLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDetainedLicense(DetainedLicenseForCreateDTO detainedLicenseDTO)
        {
            if (detainedLicenseDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Detained license object cannot be null"));
            }

            var detainedLicense = _mapper.Map<clsDetainedLicense>(detainedLicenseDTO);

            if (await detainedLicense.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<DetainedLicenseDTO>(detainedLicense));
                return CreatedAtRoute("GetDetainedLicenseById", new { DetainID = detainedLicense.DetainID }, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding detained license"));
            }
        }

        [HttpPut("{id}", Name = "UpdateDetainedLicense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDetainedLicense(int id, DetainedLicenseForUpdateDTO detainedLicenseDTO)
        {
            if (detainedLicenseDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Detained license object cannot be null"));
            }

            if (id < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid detain id"));
            }

            clsDetainedLicense? detainedLicense = await clsDetainedLicense.Find(id);

            if (detainedLicense == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Detained license with ID {id} not found."));
            }

            _mapper.Map(detainedLicenseDTO, detainedLicense);

            if (await detainedLicense.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<DetainedLicenseDTO>(detainedLicense));
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating detained license"));
            }
        }


        [HttpGet("All", Name = "GetAllDetainedLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDetainedLicenses()
        {
            var detainedLicensesList = await clsDetainedLicense.GetAllDetainedLicenses();

            if (detainedLicensesList.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No detained licenses found!"));
            }

            var result = CreateResponse(StatusSuccess, new { length = detainedLicensesList.Count, data = detainedLicensesList });
            return Ok(result);
        }

        [HttpGet("IsDetained/{LicenseID}", Name = "IsLicenseDetained")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IsLicenseDetained(int LicenseID)
        {
            if (LicenseID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
            }

            bool isDetained = await clsDetainedLicense.IsLicenseDetained(LicenseID);

            return Ok(CreateResponse(StatusSuccess, isDetained));
        }

        [HttpPost("Release", Name = "ReleaseDetainedLicense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReleaseDetainedLicense(ReleaseDetainedLicenseDTO releaseDetainedLicenseDTO)
        {
            if (releaseDetainedLicenseDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Release details object cannot be null"));
            }

            if (await clsDetainedLicense.ReleaseDetainedicense(releaseDetainedLicenseDTO))
            {
                return Ok(CreateResponse(StatusSuccess, "Detained license released successfully"));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error releasing detained license"));
            }
        }
    }

}