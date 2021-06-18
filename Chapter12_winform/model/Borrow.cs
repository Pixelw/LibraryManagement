using System;

namespace Chapter12_winform.model {
    public class Borrow: Models {
        private string _bid;

        public string Bid {
            get {
                return _bid;
            }
            set {
                _bid = value;
                id = value;
            }
        }

        public string Uid { get; set; }

        public long Date { get; set; }

        public bool OnBorrow{get; set;}

         public long ReturnDate { get; set; }

         public Borrow(string bid, string uid, long date, long returnDate) {
             Bid = bid;
             Uid = uid;
             Date = date;
             OnBorrow = true;
             ReturnDate = returnDate;
         }

         public override string ToString() {
             return $"{nameof(Bid)}: {Bid}, {nameof(Uid)}: {Uid}";
         }

         public Borrow() { }
    }

}