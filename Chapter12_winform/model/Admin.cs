using System;

namespace Chapter12_winform.model {
    public class Admin: Models {
        private string _name;
        public String Name {
            get { return _name; }
            set {
                _name = value;
                id = value;
            }
        }

        public String pwd { get; set; }

        public int role { get; set; }

        public Admin(string name, string pwd, int role) {
            Name = name;
            this.pwd = pwd;
            this.role = role;
        }

        public Admin(string name, string pwd) { 
            Name = name;
            this.pwd = pwd;
        }

        public override string ToString() {
            return $"{nameof(_name)}: {_name}, {nameof(pwd)}: {pwd}, {nameof(role)}: {role}";
        }
    }
}