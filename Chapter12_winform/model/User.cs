using System;

namespace Chapter12_winform.model {
    public class User {
        public User(string uid, string uname, int count) {
            Uid = uid;
            Uname = uname;
            Count = count;
        }

        public User() { }
        public String Uid { set; get; }

        public String Uname { get; set; }
        
        public int Count { get; set; }

        public override string ToString() {
            return $"{nameof(Uid)}: {Uid}, {nameof(Uname)}: {Uname}, {nameof(Count)}: {Count}";
        }
    }
}