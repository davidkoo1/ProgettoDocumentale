using System.Collections.Generic;
using System;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public Nullable<int> IdInstitution { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Document> Documents { get; set; } //Optional
    }
}
