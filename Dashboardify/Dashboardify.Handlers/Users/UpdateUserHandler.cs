﻿using System;
using System.Collections.Generic;
using Dashboardify.Contracts;
using Dashboardify.Contracts.Users;
using Dashboardify.Models;
using Dashboardify.Repositories;

namespace Dashboardify.Handlers.Users
{
    public class UpdateUserHandler
    {
        private UsersRepository _usersRepository;

        public UpdateUserHandler(string connectionString)
        {
            _usersRepository = new UsersRepository(connectionString);
        }

        public UpdateUserResponse Handle(UpdateUserRequest request)
        {
            var response = new UpdateUserResponse();

            response.Errors = Validate(request);
                        
            if (response.HasErrors)
            {
                return response;
            }

            var user = _usersRepository.Get(request.User.Id);

            if (user == null)
            {
                throw new Exception("User does not exist!");
            }

            UpdateUserObject(user, request.User);

            user.Name = request.User.Name;
            
            _usersRepository.Update(user);

            return response;
        }

        private void UpdateUserObject(User origin, User updated)
        {
            if (!string.IsNullOrEmpty(updated.Name)) { 
                origin.Name = updated.Name;
            }

            if (!string.IsNullOrEmpty(updated.Email))
            {
                origin.Email = updated.Email;
            }

            if (!string.IsNullOrEmpty(updated.Password))
            {
                origin.Password = updated.Password;
            }
        } 

        private IList<ErrorStatus> Validate(UpdateUserRequest request)
        {
            var errors = new List<ErrorStatus>();

            if (request.User == null) 
            {
                errors.Add(new ErrorStatus("BAD_REQUEST"));

                return errors;
            }

            if (!string.IsNullOrEmpty(request.User.Email) && request.User.Email == "one@one.lt")
            {
                errors.Add(new ErrorStatus("EMAIL_WRONG_FORMAT"));
            }

            return errors;
        }
    }
}