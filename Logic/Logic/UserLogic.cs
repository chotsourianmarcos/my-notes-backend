﻿using Data;
using Entities.Entities;
using Entities.Enums;
using Logic.Exceptions;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace Logic.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly ServiceContext _serviceContext;
        public UserLogic(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public async Task InsertUserAsync(UserItem userItem)
        {
            if (!userItem.ValidateValues(true, true))
            {
                throw new BadRequestException(BadRequestExceptionType.InvalidData);
            }

            var repeated = await ValidateRepetedUser(userItem);
            if (repeated)
            {
                throw new BadRequestException(BadRequestExceptionType.RepeatedOperation);
            }

            if (userItem.IdRol == (int)UserRolEnum.Administrador)
            {
                throw new AuthenticationException(AuthenticationExceptionType.RolNotAuthorized);
            }

            userItem.HashedAccessToken = "NOT GENERATED";
            userItem.HashedRefreshToken = "NOT GENERATED";
            
            await _serviceContext.Users.AddAsync(userItem);

            await _serviceContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateRepetedUser(UserItem userItem)
        {
            var repeatedUser = await _serviceContext.Users
                .Where(u => u.Name == userItem.Name || u.Email == userItem.Email)
                .FirstOrDefaultAsync();

            if (repeatedUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}