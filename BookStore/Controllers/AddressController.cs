﻿using BookStore.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStore.Controllers
{
    public class AddressController : ApiController
    {
        ISession session = OpenSessionNHibernate.OpenSession();
        //Get all Notes

        [Route("getAddress")]

        public List<AddressModel> GetAddressList()
        {
            List<AddressModel> Address = session.Query<AddressModel>().ToList();
            return Address;
        }
        [Route("AddAddress")]
        [HttpPost]
        public HttpResponseMessage AddAddress(AddressModel address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(address);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, address);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public AddressModel getAddress(int id)
        {
            var address = session.Get<AddressModel>(id);
            return address;
        }


        [HttpPut]
        public HttpResponseMessage UpdateAddress(int AddressId, AddressModel addressModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var address = session.Get<AddressModel>(AddressId);
                    address.Address = addressModel.Address;
                    address.City = addressModel.City;
                    address.State = addressModel.State;
                    address.TypeId = addressModel.TypeId;
                    address.UserId = addressModel.UserId;



                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(address);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, address);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpDelete]
        public HttpResponseMessage DeleteAddress(int id, int userId)
        {
            try
            {
                var address = session.Get<AddressModel>(id);

                if (address.UserId == userId)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(address);
                        transaction.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, "Address deleted Successfully");
                    }

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error !");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
