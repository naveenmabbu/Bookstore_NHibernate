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
    public class UserController : ApiController
    {
        //NHibernate Session  
        ISession session = OpenSessionNHibernate.OpenSession();
        //Get All USERS  
        public List<UserModel> GetListUser()
        {
            List<UserModel> user = session.Query<UserModel>().ToList();
            return user;
        }
        //Add New User
        [Route("registerUser")]
        [HttpPost]
        public HttpResponseMessage AddNewUser(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(user);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
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
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage UserLogin(UserLogin login)
        {
            try
            {
                string token = "";
                if (ModelState.IsValid)
                {
                    List<UserModel> Users = session.Query<UserModel>().ToList();
                    foreach (var user in Users)
                    {
                        if (user.Email == login.Email && user.Password == login.Password)
                        {
                            token = JWTToken.GenerateJWTToken(user.Email, user.UserId);
                            break;

                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, token);
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

        [Route("forgotPass")]
        [HttpPost]
        public HttpResponseMessage ForgotPass(ForgotPassModel forgotPass)
        {
            try
            {
                string token = "";
                if (ModelState.IsValid)
                {
                    List<UserModel> Users = session.Query<UserModel>().ToList();
                    foreach (var user in Users)
                    {
                        if (user.Email == forgotPass.Email)
                        {
                            token = JWTToken.GenerateJWTToken(user.Email, user.UserId);
                            new MSMQ().Sender(token);
                            break;

                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, "token sent to your email successfylly");
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

        [Route("resetPass")]
        [HttpPut]

        public HttpResponseMessage ResetPass(ResetPassModel resetPass)
        {
            try
            {
                //  var user = User.Claims.FirstOrDefault(e => e).Value;


                if (ModelState.IsValid)
                {
                    List<UserModel> Users = session.Query<UserModel>().ToList();

                    foreach (var user in Users)
                    {
                        if (user.Email == "naveenmabbu0207@gmail.com")
                        {

                            if (resetPass.NewPassword == resetPass.ConfrimPassword)
                            {
                                user.Password = resetPass.ConfrimPassword;
                                using (ITransaction transaction = session.BeginTransaction())
                                {
                                    session.SaveOrUpdate(user);
                                    transaction.Commit();
                                }
                                break;
                            }

                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "password reset successful");
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
    }
}
