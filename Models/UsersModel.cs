
using Oracle.ManagedDataAccess.Types;
using System;

namespace AccioInventory.Models
{
    /// <summary>
    /// Each model class represents a datatype usually used to create lists.
    /// </summary>
    public class UsersModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Password { get; set; } 
        public int TelNo { get; set; } 
        public DateTime LastSeen { get; set; } 
        public string UserInSession { get; set; } 
        public string UserAuthLevel { get; set; }
        public int DepartmentId { get; set; }

    }
}
