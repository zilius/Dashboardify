﻿using System;
using System.Collections.Generic;
using Dashboardify.Contracts;
using Dashboardify.Contracts.Dashboards;
using Dashboardify.Models;
using Dashboardify.Repositories;

namespace Dashboardify.Handlers.Dashboards
{
    public class CreateDashboardHandler:BaseHandler
    {
        private DashRepository _dashRepository;

        private UserSessionRepository _userSessionRepository;

        public CreateDashboardHandler(string connectionString) : base(connectionString)
        {
            _dashRepository = new DashRepository(connectionString);

            _userSessionRepository = new UserSessionRepository(connectionString);
        }

        public CreateDashboardResponse Handle(CreateDashboardRequest request)
        {
            var response = new CreateDashboardResponse();

            response.Errors = Validate(request);

            if (response.HasErrors)
            {
                return response;
            }
            try
            {
                _dashRepository.Create(new DashBoard
                {
                    Name = request.DashName,
                    UserId = _userSessionRepository.GetUserBySessionId(request.Ticket).Id,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true
                });

                int userId = _userSessionRepository.GetUserBySessionId(request.Ticket).Id;

                var responseDash = _dashRepository.GetByNameAndUserId(request.DashName, userId);

                response.Dashboard = responseDash;


            }
            catch (Exception)
            {
                response.Errors.Add(new ErrorStatus("BAD_REQUEST"));

                return response;
            }

            
            return response;
        }

        private IList<ErrorStatus> Validate(CreateDashboardRequest request)
        {
            var errors = new List<ErrorStatus>();

            if (IsRequestNull(request))
            {
                errors.Add(new ErrorStatus("BAD_REQUEST"));
                return errors;
            }
            if (string.IsNullOrEmpty(request.Ticket))
            {
                errors.Add(new ErrorStatus("TICKET_NOT_DEFINED"));
                return errors;
            }

            if (string.IsNullOrEmpty(request.DashName))
            {
                errors.Add(new ErrorStatus("DASHBOARD_NOT_DEFINED"));
                return errors;
            }
            var user = _userSessionRepository.GetUserBySessionId(request.Ticket);

            if (user == null)
            {
                errors.Add(new ErrorStatus("USER_NOT_DEFINED"));
                return errors;
            }

            if (!_dashRepository.CheckIfNameAvailable(user.Id, request.DashName))
            {
                errors.Add(new ErrorStatus("NAME_ALREADY_EXISTS"));
            }

            //TODO method who checks ir name exsists
            return errors;
        }
    }
}
