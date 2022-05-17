using BookStore.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStore.Controllers
{
    public class OrderController : ApiController
    {
        ISession session = OpenSessionNHibernate.OpenSession();

        [Route("order")]
        [HttpPost]
        public HttpResponseMessage Order(OrderModel order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(order);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, order);
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
        public List<OrderModel> GetOrderListByUser(int userId)
        {
            List<OrderModel> newOrderList = new List<OrderModel>();
            List<OrderModel> Orders = session.Query<OrderModel>().ToList();
            foreach (var order in Orders)
            {
                if (order.UserId == userId)
                {
                    newOrderList.Add(order);
                }
            }

            return newOrderList;
        }
    }
}
