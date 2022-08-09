using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class UserRoleWithClaim: Entity
    {
        public int OrgId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public string ClaimName { get; set; }
    }
}
