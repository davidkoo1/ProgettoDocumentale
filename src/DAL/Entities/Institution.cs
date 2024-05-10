﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Institution
    {
        public int Id { get; set; }
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }

        // Коллекция пользователей, связанных с учреждением
        public virtual List<User> Users { get; set; } // Используйте virtual для ленивой загрузки

    }
}
