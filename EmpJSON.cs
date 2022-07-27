using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EMPJSON_Task
{
    public sealed class EmpJSON
    {
        private List<Employee> _employees { get; set; }
        private JsonSerializer _serializer = new JsonSerializer();
        public EmpJSON()
        {
            _employees = new List<Employee>();
            if (!File.Exists("employee.json"))
            using (File.Create("employee.json")) { }
        }

        public int FindLastID()
        {
            List<Employee> temp = new List<Employee>();
            temp = ReadJson(temp, "employee.json");
            if (temp == null) return 0;
            return temp.Max(emp => emp.ID);          
        }
        private bool WriteJson(List<Employee> empList, string path)
        {
            if (empList is null) return false;
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                _serializer.Serialize(jw, empList);
            }
            return true;
        }
        private List<Employee> ReadJson(List<Employee> empList, string path)
        {
            using (StreamReader sr = new StreamReader(path))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                empList = _serializer.Deserialize<List<Employee>>(jr);
            }
            return empList;
        }

        public bool Add(Employee employee)
        {
            if (employee == null) return false;
            _employees.Add(employee);
            WriteJson(_employees, "employee.json");
            return true;
        }

        public string Delete(int ID)
        {
            try
            {
                _employees.RemoveAt(_employees.FindIndex(emp => emp.ID == ID));
                WriteJson(_employees, "employee.json");
                return new string("Успешно");
            }
            catch { throw new ArgumentOutOfRangeException("Неверно указан ID"); }
        }

        private Employee Get(int ID)
        {
            try
            {
                return GetAll().Where(emp => emp.ID == ID).Single();
            }
            catch { throw new ArgumentOutOfRangeException("Неверно указан ID"); }
        }

        public string GetToString(int ID)
        {
            Employee employee = Get(ID);
            return new string($"ID = {employee.ID}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
        }

        public List<Employee> GetAll()
        {
            List<Employee> temp = new List<Employee>();
            temp = ReadJson(temp, "employee.json");
            if (temp != null) _employees = temp;
            return _employees;
        }


        public bool Update(int ID, Dictionary<string,string> valuePairs)
        {
            string FirstName, LastName, SalaryPerHour;
            Employee tempEmp = Get(ID);
            if (valuePairs.TryGetValue("FirstName", out FirstName)) tempEmp.FirstName = FirstName;
            if (valuePairs.TryGetValue("LastName", out LastName)) tempEmp.LastName = LastName;
            if (valuePairs.TryGetValue("SalaryPerHour", out SalaryPerHour)) tempEmp.SalaryPerHour = Convert.ToDecimal(SalaryPerHour);
            _employees[_employees.FindIndex(s => s.ID == tempEmp.ID)] = tempEmp;
            WriteJson(_employees, "employee.json");
            return true;
        }

    }



}
