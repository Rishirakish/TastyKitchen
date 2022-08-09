using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Services.Authorization
{
   public class ResourceAuthorizationCrudHandler
    {
    }

    public class CustomClaimTypes
    {
        public const string Permission = "Application.Permission";
    }

    public static class Permissions
    {
        public static class UsersPermissions
        {
            public const string Add = "users.add";
            public const string Edit = "users.edit";
            public const string Read = "users.read";
            public const string Delete = "users.delete";
            public const string EditRole = "users.edit.role";
        }

        public static class TeamsPermissions
        {
            public const string AddRemove = "teams.addremove";
            public const string EditManagers = "teams.edit.managers";
            public const string Delete = "teams.delete";
        }
    }
    

    public static class PolicyTypes
    {
        public static class Users
        {
            public const string Manage = "users.manage.policy";
            public const string EditRole = "users.edit.role.policy";
        }
    }
}
