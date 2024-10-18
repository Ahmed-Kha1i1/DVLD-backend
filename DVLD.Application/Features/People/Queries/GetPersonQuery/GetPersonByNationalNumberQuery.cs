﻿using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery.ByNationalNumber
{
    public class GetPersonByNationalNumberQuery : IRequest<Response<PersonDTO>>
    {
        public string NationalNumber { get; set; }

        public GetPersonByNationalNumberQuery(string nationalNumber)
        {
            NationalNumber = nationalNumber;
        }
    }
}
