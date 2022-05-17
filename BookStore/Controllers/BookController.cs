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
    public class BookController : ApiController
    {
        ISession session = OpenSessionNHibernate.OpenSession();
        [Route("getBooks")]
        public List<BookModel> GetBookList()
        {
            List<BookModel> Books = session.Query<BookModel>().ToList();
            return Books;
        }

        [Route("AddBook")]
        [HttpPost]
        public HttpResponseMessage AddNewBook(BookModel book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(book);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, book);
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

        [Route("get")]
        [HttpGet]
        public BookModel getBook(int id)
        {
            var book = session.Get<BookModel>(id);
            return book;
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Updatebook(BookModel bookModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = session.Get<BookModel>(bookModel.BookId);
                    book.BookName = bookModel.BookName;
                    book.AuthorName = bookModel.AuthorName;
                    book.ActualPrice = bookModel.ActualPrice;
                    book.Rating = bookModel.Rating;
                    book.RatingCount = bookModel.RatingCount;
                    book.Description = bookModel.Description;
                    book.ActualPrice = bookModel.ActualPrice;
                    book.DiscountPrice = bookModel.DiscountPrice;
                    book.BookImage = bookModel.BookImage;
                    book.BookQuantity = bookModel.BookQuantity;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(book);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, book);
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

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteBook(int id)
        {
            try
            {
                var book = session.Get<BookModel>(id);
                if (book != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(book);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Book deleted Successfully");
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
