using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.Services
{
    class BookServices
    {
        private static List<string> books = new List<string>();

        static BookServices()
        {
            books.Add("Book qwe, 1562");
            books.Add("Book ertye, 1362");
            books.Add("Book efdye, 1562");
            books.Add("Book easdadfye, 1566");
            books.Add("Book easdasdadfye, 1572");
        }

        public List<string> GetBooks()
        {
            return books;
        }

        public void AddBook(string book)
        {
            books.Add(book);
        }
    }
}
