using System;

namespace Chapter12_winform.model {
    public class Book: Models {
        private string _bid;

        public String Bid {
            set {
                _bid = value;
                id = value;
            }
            get {
                return _bid;
            }
        }

        public String Bname { set; get; }

        public String Bpress { set; get; }

        public String Author { set; get; }

        public int Quantity{get; set;}

        public Book(string bid, string bname, string bpress, string author, int quantity) {
            Bid = bid;
            Bname = bname;
            Bpress = bpress;
            Author = author;
            Quantity = quantity;
        }

        public Book() { }

        public override string ToString() {
            return $"{nameof(Bid)}: {Bid}, {nameof(Bname)}: {Bname}, {nameof(Bpress)}: {Bpress}, {nameof(Author)}: {Author}, {nameof(Quantity)}: {Quantity}";
        }
    }
}