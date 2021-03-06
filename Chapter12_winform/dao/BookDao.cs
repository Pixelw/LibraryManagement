using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chapter12_winform.model;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    public class BookDao : BaseDao {

        public BookDao(SqlHelper sqlHelper) : base(sqlHelper) {

        }

        public bool AddBook(Book book) {
            int i = sqlHelper.ExecuteNonQuery(
                "insert into T_book values (@BID, @BNAME, @PRESS, @AUTHOR, @Q)",
                new SqlParameter("@BID", book.Bid), new SqlParameter("@BNAME", book.Bname),
                new SqlParameter("@AUTHOR", book.Author), new SqlParameter("@PRESS", book.Bpress),
                 new SqlParameter("@Q", book.Quantity)
            );
            return i > 0;
        }

        public List<Book> GetAllBooks() {
            List<Book> books = new List<Book>();
            DataTable dataTable = sqlHelper.ExecuteTable("select * from T_book");
            for (int i = 0; i < dataTable.Rows.Count; i++) {
                Book book = new Book();
                book.Bid = dataTable.Rows[i][0].ToString().Trim();
                book.Bname = dataTable.Rows[i][1].ToString().Trim();
                book.Bpress = dataTable.Rows[i][2].ToString().Trim();
                book.Author = dataTable.Rows[i][3].ToString().Trim();
                book.Quantity = int.Parse(dataTable.Rows[i][4].ToString().Trim());
                books.Add(book);
            }

            return books;
        }

        public Book GetBook(String bid) {
            var dataTable = sqlHelper.ExecuteTable("select * from T_book where Bid=@BID",
                new SqlParameter("@BID", bid));
            if (dataTable.Rows.Count < 1) {
                return null;
            }
            var book = new Book();
            book.Bid = dataTable.Rows[0][0].ToString().Trim();
            book.Bname = dataTable.Rows[0][1].ToString().Trim();
            book.Bpress = dataTable.Rows[0][2].ToString().Trim();
            book.Author = dataTable.Rows[0][3].ToString().Trim();
            book.Quantity = int.Parse(dataTable.Rows[0][4].ToString().Trim());
            return book;
        }

        public bool UpdateBook(Book book) {
            int i = sqlHelper.ExecuteNonQuery(
                "update T_book set Bname=@BNAME, Bpress=@PRESS, Author=@AUTHOR, Quantity=@Q where Bid=@BID",
                new SqlParameter("@BNAME", book.Bname), new SqlParameter("@PRESS", book.Bpress),
                new SqlParameter("@AUTHOR", book.Author), new SqlParameter("@BID", book.Bid),
                new SqlParameter("@Q", book.Quantity));
            return i > 0;
        }

        public bool DeleteBook(String bid) {
            int i = sqlHelper.ExecuteNonQuery("delete from T_book where Bid=@BID",
                new SqlParameter("@BID", bid));
            return i > 0;
        }

        
    }
}