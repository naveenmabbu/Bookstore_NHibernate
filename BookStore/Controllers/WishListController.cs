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
    public class WishListController : ApiController
    {
        ISession session = OpenSessionNHibernate.OpenSession();
        //Get all Notes


        [Route("AddWishList")]
        [HttpPost]
        public HttpResponseMessage AddToWishlist(WishListModel wish)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        session.Save(wish);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, wish);
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
        public HttpResponseMessage DeletefromWishlist(int id, int userId)
        {
            try
            {
                var wishlist = session.Get<WishListModel>(id);

                if (wishlist.UserId == userId)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(wishlist);
                        transaction.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, " deleted from wish list");
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
        [HttpGet]
        public List<WishListModel> getwishlist(int userId)
        {
            List<WishListModel> newwishlist = new List<WishListModel>();
            List<WishListModel> WishLists = session.Query<WishListModel>().ToList();
            foreach (var wishlist in WishLists)
            {
                if (wishlist.UserId == userId)
                {
                    newwishlist.Add(wishlist);
                }
            }

            return newwishlist;
        }
    }
}
