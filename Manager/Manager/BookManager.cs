using Manager.Interface;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Manager
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository repository;
        public BookManager(IBookRepository repository)
        {
            this.repository = repository;
        }
        public bool AddBook(BookModel bookmodel)
        {
            try
            {
                return this.repository.AddBook(bookmodel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EditBook(BookModel bookmodel)
        {
            try
            {
                return this.repository.EditBook(bookmodel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<BookModel> GetAllBooks()
        {
            try
            {
                return this.repository.GetAllBooks();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public BookModel GetBook(int bookId)
        {
            try
            {
                return this.repository.GetBook(bookId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RemoveBook(int bookId)
        {
            try
            {
                return this.repository.RemoveBook(bookId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddCustomerFeedBack(FeedBackModel feedbackmodel)
        {
            try
            {
                return this.repository.AddCustomerFeedBack(feedbackmodel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<FeedBackModel> GetCustomerFeedBack(int bookid)
        {
            try
            {
                return this.repository.GetCustomerFeedBack(bookid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
