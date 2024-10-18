using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Domain.Entities;
using MediatR;

namespace DVLD.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserQueryHandler(IUserRepository userRepository, IPersonRepository personRepository, ICountryRepository countryRepository, IMapper mapper) : ResponseHandler,
        IRequestHandler<GetUserQuery, Response<UserDTO>>,
        IRequestHandler<GetUserOverviewQuery, Response<UserOverviewDTO>>
    {
        public async Task<Response<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var User = await userRepository.GetByIdAsync(request.UserId);
            if (User == null)
            {
                return NotFound<UserDTO>("User not found");
            }
            User.PersonInfo = await personRepository.GetByIdAsync(User.PersonID);
            User.PersonInfo.CountryInfo = await countryRepository.GetByIdAsync(User.PersonInfo.NationalityCountryID);
            return Success(mapper.Map<UserDTO>(User));
        }

        public async Task<Response<UserOverviewDTO>> Handle(GetUserOverviewQuery request, CancellationToken cancellationToken)
        {
            var User = await userRepository.GetByIdAsync(request.UserId);
            if (User == null)
            {
                return NotFound<UserOverviewDTO>("User not found");
            }
            User.PersonInfo = await personRepository.GetByIdAsync(User.PersonID);

            return Success(mapper.Map<UserOverviewDTO>(User));

        }


    }
}
