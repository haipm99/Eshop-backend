using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Demo
{
    class Student
    {
        public string Name { get; set; }
        public Classes Class { get; set; }
        
    }

    class Classes
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var classA = new Classes()
            {
                Name = "A",
                Students = new List<Student>()
            };
            var student1 = new Student()
            {
                Name = "David",
                Class = classA
            };
            classA.Students.Add(student1);

            var stu = new Dictionary<string, object>();
            stu["Name"] = student1.Name;
            var cla = new Dictionary<string, object>();
            stu["Class"] = cla;
            cla["Name"] = classA.Name;
            Console.WriteLine(JsonConvert.SerializeObject(stu));

            dynamic stu2 = new ExpandoObject();
            stu2.Name = student1.Name;
            stu2.Class = new ExpandoObject();
            stu2.Class.Name = classA.Name;
            Console.WriteLine(JsonConvert.SerializeObject(stu2));

        }
    }
}
