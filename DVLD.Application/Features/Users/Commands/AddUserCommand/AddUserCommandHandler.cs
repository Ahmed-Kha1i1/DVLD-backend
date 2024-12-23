﻿using AutoMapper;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.Users.Commands.AddUserCommand
{
    public class AddUserCommandhandler(IUserRepository userRepository, IMapper mapper) : ResponseHandler, IRequestHandler<AddUserCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request);

            if (await userRepository.SaveAsync(user))
            {
                return Created<int?>(user.Id);
            }
            else
            {
                return Fail<int?>(null, "Error adding user");
            }
        }
    }
}
