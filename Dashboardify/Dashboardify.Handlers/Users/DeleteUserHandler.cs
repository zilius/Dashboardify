﻿using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Dashboardify.Contracts;
using Dashboardify.Contracts.Users;
using Dashboardify.Handlers.Items;
using Dashboardify.Models;
using Dashboardify.Repositories;

namespace Dashboardify.Handlers.Users
{
    public class DeleteUserHandler
    {
        private UsersRepository _userRepository;
        public DeleteUserHandler(string connectionString)
        {
            _userRepository = new UsersRepository(connectionString);
        }

        public DeleteUserResponse Handle(UpdateUserRequest request)
        {
            var response = new DeleteUserResponse();
            response.Errors = Validate(request);

            if (response.HasErrors)
            {
                return response;
            }
            _userRepository.DeleteUser(request.User.Id);

            return response;

        }

        public IList<ErrorStatus> Validate(UpdateUserRequest request)
        {
            var errors = new List<ErrorStatus>();
            if (request.User.Id < 1)
            {
                errors.Add(new ErrorStatus("INVALID_ID"));
                return errors;
            }
            if (request.User.Id.ToString() == "")
            {
                errors.Add(new ErrorStatus("ID_NOT_RECIEVED"));
            }
            return errors;
        }
    }
}
