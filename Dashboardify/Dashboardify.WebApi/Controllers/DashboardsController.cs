﻿using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Net;
using Dashboardify.Contracts.Dashboards;
using Dashboardify.Handlers.Dashboards;
using Dashboardify.Models;
using Dashboardify.Security;


namespace Dashboardify.WebApi.Controllers
{
    public class DashboardsController : BaseController
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GCP"].ConnectionString;
        
        
        [HttpPost]
        public HttpResponseMessage GetList(string ticket)
        {
            var securityProvider = new SecurityProvider(_connectionString);

            var sessionInfo = securityProvider.GetSessionInfo(ticket);

            if (sessionInfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var GetListRequest = new GetDashboardsRequest
            {
                Ticket = ticket,
                Id = sessionInfo.User.Id
            };

            var handler = new GetDashboardsHandler(_connectionString);

            var response = handler.Handle(GetListRequest);

            var statusCode = ResolveStatusCode(response);

            return Request.CreateResponse(statusCode, response);

        }

        [HttpPost]
        public HttpResponseMessage Update(string ticket, DashBoard dash)
        {
            var securityProvider = new SecurityProvider(_connectionString);

            var sessionInfo = securityProvider.GetSessionInfo(ticket);

            if (sessionInfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var UpdateRequest = new UpdateDashboardRequest
            {
                DashBoard = dash,
                UserId = sessionInfo.User.Id
            };

            var handler = new UpdateDashBoardHandler(_connectionString);

            var response = handler.Handle(UpdateRequest);

            var statusCode = ResolveStatusCode(response);

            return Request.CreateResponse(statusCode, response);
        }

        [HttpPost]
        public HttpResponseMessage Create(string ticket, string dashName)
        {
            var securityProvider = new Security.SecurityProvider(_connectionString);

            var sessionInfo = securityProvider.GetSessionInfo(ticket);

            if (sessionInfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var deleteRequest = new CreateDashboardRequest
            {
                DashName = dashName,
                UserId = sessionInfo.User.Id
            };

            var handler = new CreateDashboardHandler(_connectionString);

            var response = handler.Handle(deleteRequest);

            var statusCode = ResolveStatusCode(response);

            return Request.CreateResponse(statusCode, response);

        }

        [HttpPost]
        public HttpResponseMessage Delete(string ticket, int id)
        {
            var securityProvider = new Security.SecurityProvider(_connectionString);

            var sessionInfo = securityProvider.GetSessionInfo(ticket);

            if (sessionInfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var deleteRequest = new DeleteDashRequest
            {
                DashboardId = id,
                UserId = sessionInfo.User.Id
            };

            var handler = new DeleteDashHandler(_connectionString);

            var response = handler.Handle(deleteRequest);

            var statusCode = ResolveStatusCode(response);

            return Request.CreateResponse(statusCode, response);

        }
    }
}
