using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string  department { get; set; }
        public string dateofjoining { get; set; }
        public string photofilename { get; set; }
    }
}