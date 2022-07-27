using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMPJSON_Task
{
    public sealed class Employee
    {
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("SalaryPerHour")]
        public decimal SalaryPerHour { get; set; }
    }
}
