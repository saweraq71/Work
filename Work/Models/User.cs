using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Work.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string POS { get; set; } // имя пользователя
        public string TEL { get; set; } // возраст пользователя
       public string PATH{ get; set; } // путь к фото пользователя 
        public string Imagename { get; set; }
    }
}
