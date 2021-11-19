using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interface
{
   public interface IBookManager
    {
        public bool AddBook(BookModel bookmodel);
        public BookModel GetBook(int bookId);

        public bool EditBook(BookModel bookmodel);

        public bool RemoveBook(int bookId);
        public List<BookModel> GetAllBooks();

        public bool AddCustomerFeedBack(FeedBackModel feedbackmodel);

        public List<FeedBackModel> GetCustomerFeedBack(int bookid);

    }
}
