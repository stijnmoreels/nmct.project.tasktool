using nmct.project.tasktool.models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace nmct.project.tasktool.models
{
    public class Magnitude : ILabel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [DefaultValue(2)]
        public int Number { get; set; }

        public Magnitude()
        {

        }

        public Magnitude(string name)
        {
            this.Name = name;
        }

        public Magnitude(string id, string name)
        {
            this.Name = name;
            this.Id = id;
            this.Number = 2;
        }

        public Magnitude(string id, string name, int number)
        {
            this.Id = id;
            this.Name = name;
            this.Number = number;
        }
    }
}
