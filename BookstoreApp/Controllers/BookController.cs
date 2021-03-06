using Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BookController : Controller
    {
        private readonly IBookManager manager;

        public BookController(IBookManager manager)
        {
            this.manager = manager;

        }

        [HttpPost]
        [Route("AddBook")]
        public IActionResult AddBook([FromBody] BookModel bookmodel)
        {
            try
            {
                var result = this.manager.AddBook(bookmodel);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added New Book Successfully !" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add new book" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }
        [HttpGet]
        [Route("GetBook")]
        public IActionResult GetBook(int bookId)
        {
            var result = this.manager.GetBook(bookId);
            try
            {
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Book is retrived", data = result });

                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Try again" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
            }
        }

        [HttpPut]
        [Route("UpdateBook")]
        public IActionResult EditBook(BookModel Bookmodel)
        {
            try
            {
                var result = this.manager.EditBook(Bookmodel);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Book updated Successfully !" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to updated Book" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }
        [HttpDelete]
        [Route("RemoveBooks")]
        public IActionResult RemoveBook(int BookId)
        {
            try
            {
                var result = this.manager.RemoveBook(BookId);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Removed Book Successfully !" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to Remove Book, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }

        [HttpPost]
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var result = this.manager.GetAllBooks();
            try
            {
                if (result.Count > 0)
                {
                    return this.Ok(new { Status = true, Message = "All Notes", data = result });

                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Try again" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
            }
        }

        [HttpPost]
        [Route("AddCustomerFeedBack")]
        public IActionResult AddCustomerFeedBack([FromBody] FeedBackModel feedbackModel)
        {
            try
            {
                var result = this.manager.AddCustomerFeedBack(feedbackModel);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added FeedBack Successfully !" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add feedback, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }

        [HttpPost]
        [Route("GetFeedback")]
        public IActionResult GetCustomerFeedBack(int bookid)
        {
            try
            {
                var result = this.manager.GetCustomerFeedBack(bookid);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Feedbackertrived", Data = result });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add to wish list, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }
    }
}
