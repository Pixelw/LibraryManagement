using System;

namespace Chapter12_winform.model {
    public class Admin {
        public String name { get; set; }

        public String pwd { get; set; }

        public int role { get; set; }

        public Admin(string name, string pwd, int role) {
            this.name = name;
            this.pwd = pwd;
            this.role = role;
        }

        public Admin(string name, string pwd) {
            this.name = name;
            this.pwd = pwd;
        }

        public override string ToString() {
            return $"{nameof(name)}: {name}, {nameof(pwd)}: {pwd}, {nameof(role)}: {role}";
        }
    }
}