﻿using System;
using System.Collections.Generic;
using Dashboardify.Repositories;
using Dashboardify.Contracts;
using Dashboardify.Contracts.Items;

namespace Dashboardify.Handlers.Items
{
    public class UpdateItemHandler : BaseHandler
    {
        private ItemsRepository _itemRepository;

        public UpdateItemHandler(string connectionString) : base(connectionString)
        {
            _itemRepository = new ItemsRepository(connectionString);

           }

        public UpdateItemResponse Handle(UpdateItemRequest request)
        {
            var response = new UpdateItemResponse();

            response.Errors = Validate(request);

            if (response.HasErrors)
            {
                return response;
            }
            try
            {
                var item = _itemRepository.Get(request.Item.Id);

                if (item == null)
                {
                    throw new Exception("ITEM_NOT_FOUND");
                }

                UpdateItemObject(item, request.Item);

                _itemRepository.Update(item);

                return response;
            }
            catch (Exception)
            {

                response.Errors.Add(new ErrorStatus("BAD_REQUEST"));

                return response;
            }


        }

        private void UpdateItemObject(Item origin, Item Updated)
        {

            if (!(Updated.CheckInterval < 30000 && Updated.CheckInterval > 86400000))
            {
                origin.CheckInterval = Updated.CheckInterval;
            }

            if (!string.IsNullOrEmpty(Updated.Name))
            {
                origin.Name = Updated.Name;
            }

            if (Updated.IsActive != origin.IsActive)
            {
                origin.IsActive = Updated.IsActive;
            }
            if (Updated.NotifyByEmail != origin.NotifyByEmail)
            {
                origin.NotifyByEmail = Updated.NotifyByEmail;
            }

            
        }

        private IList<ErrorStatus> Validate(UpdateItemRequest request)
        {
            var errors = new List<ErrorStatus>();

            if (request.Item == null)
            {
                errors.Add(new ErrorStatus("BAD_REQUEST"));
                return errors;

            }

            if (!string.IsNullOrEmpty(request.Item.Name))
            {
                if (request.Item.Name.Length > 254)
                {
                    errors.Add(new ErrorStatus("NAME_TOO_LONG"));
                    return errors;
                }
            }

            if (request.Item.Id < 1)
            {
                errors.Add(new ErrorStatus("BAD_REQUEST"));
                return errors;
            }

            
            if (request.Item.CheckInterval < 30000 && request.Item.CheckInterval > 86400000)
            {
                errors.Add(new ErrorStatus("INVALID_CHECK_INTERVAL"));
                return errors;
            }

            var ownerUser = _itemRepository.GetUserByItemId(request.Item.Id);

            if (ownerUser == null)
            {
                errors.Add(new ErrorStatus("ITEM_NOT_FOUND"));
                return errors;
            }

            if (request.UserId != ownerUser.Id)
            {
                errors.Add(new ErrorStatus("UNAUTHORIZED_ACCESS"));
            }
            

            return errors;
        }

    }
}
