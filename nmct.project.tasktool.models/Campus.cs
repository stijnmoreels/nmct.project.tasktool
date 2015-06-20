using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models
{
    public class Campus
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        [NotMapped]
        public string BoardId { get; set; }
        
        public string Name { get; set; }

        public Campus()
        {

        }

        public Campus(string name)
        {
            this.Name = name;
        }

        public Campus(int id, string name, string boardId)
        {
            this.Id = id;
            this.Name = name;
            this.BoardId = boardId;
        }

        public Campus(string name, string boardId)
        {
            this.Name = name;
            this.BoardId = boardId;
        }
    }
}
