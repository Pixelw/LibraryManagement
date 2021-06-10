using System;

namespace Chapter12_winform.model {
    public class Book {
        public String Bid { set; get; }

        public String Bname { set; get; }

        public String Bpress { set; get; }

        public String Author { set; get; }

        public int Quantity{get; set;}

        public override string ToString() {
            return $"{nameof(Bid)}: {Bid}, {nameof(Bname)}: {Bname}, {nameof(Bpress)}: {Bpress}, {nameof(Author)}: {Author}, {nameof(Quantity)}: {Quantity}";
        }
    }
}