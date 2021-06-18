using System;

namespace Chapter12_winform.model {
    public class User : Models {
        public User(string uid, string uname, int count) {
            Uid = uid;
            Uname = uname;
            Count = count;
        }

        public User() { }
        private string _uid;

        public String Uid {
            set {
                _uid = value;
                id = value;
            }
            get { return _uid; }
        }

        public String Uname { get; set; }

        public int Count { get; set; }

        public override string ToString() {
            return $"{nameof(Uid)}: {Uid}, {nameof(Uname)}: {Uname}, {nameof(Count)}: {Count}";
        }
    }
}