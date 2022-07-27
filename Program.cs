using System;
using System.Collections.Generic;
using System.Linq;

namespace EMPJSON_Task
{
    class Program
    {
        private static int _ID;
        private static EmpJSON _empJSON = new EmpJSON();
        private static Dictionary<string, string> _propDictionary = new Dictionary<string, string>();
        private static void Main(string[] args)
        {
            //-add FirstName: John LastName:McCarty SalaryPerHour:12.5
            _ID = _empJSON.FindLastID() + 1;
            while (true)
            {
                ConsoleCommand(Console.ReadLine());
            }
        }



        private static void ConsoleCommand(string param)
        {
            string[] sArray = param.Split(new char[] { ' ', '.', ':', ';', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            #region -add
            try
            {
                if (sArray[0] == "-add")
                {
                    _propDictionary = SearchProp(sArray);
                    if (_propDictionary != null)
                    {
                        Employee newEmp = new Employee()
                        {
                            ID = _ID++,
                            FirstName = _propDictionary["FirstName"],
                            LastName = _propDictionary["LastName"],
                            SalaryPerHour = Convert.ToDecimal(_propDictionary["SalaryPerHour"])
                        };
                        _empJSON.Add(newEmp);
                        _propDictionary.Clear();
                    }
                }
            }
            catch { Console.WriteLine("Ошибка, некорректный ввод значений"); }
            #endregion
            #region -update
            try
            {
                if (sArray[0] == "-update")
                {
                    _propDictionary = SearchProp(sArray);
                    if (_propDictionary != null)
                    {
                        _empJSON.Update(int.Parse(_propDictionary["ID"]), _propDictionary);
                        Console.WriteLine("Успешно");
                        _propDictionary.Clear();
                    }
                }
            }
            catch { Console.WriteLine("Ошибка."); }
            #endregion
            #region -get
            try
            {
                if (sArray[0] == "-get")
                {
                    int id;
                    if (int.TryParse(sArray[1], out id)) Console.WriteLine(_empJSON.GetToString(id));
                    else { Console.WriteLine("Указан неверный ID"); return; }
                }
            }
            catch { Console.WriteLine("Укажите верный ID"); }
            #endregion
            #region -getall
            if (sArray[0] == "-getall")
            {
                OutAll();
            }
            #endregion
            #region -delete
            try
            {
                if (sArray[0] == "-delete")
                {
                    int id;
                    if (int.TryParse(sArray[1], out id)) Console.WriteLine(_empJSON.Delete(id));
                    else { Console.WriteLine("Указан неверный ID"); return; }
                }
            }
            catch { Console.WriteLine("Укажите верный ID"); }
            #endregion
        }

        private static Dictionary<string, string> SearchProp(string[] array)
        {
            string[] property = { "ID", "FirstName", "LastName", "SalaryPerHour" };
            int index;
            for (int i = 0; i < array.Length - 1; i++)
            {
                index = Array.IndexOf(property, array[i]);
                if (index != -1) _propDictionary.Add(array[i], array[i + 1]);
            }
            return _propDictionary;
        }

        private static void OutAll()
        {
            List<Employee> employees = _empJSON.GetAll();
            if (employees.Count == 0) { Console.WriteLine("Список пуст"); return; }
            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"" +
                    $"ID = {employees[i].ID}, FirstName = {employees[i].FirstName}, " +
                    $"LastName = {employees[i].LastName}, " +
                    $"SalaryPerHour = {employees[i].SalaryPerHour}");
            }
        }

    }
}

