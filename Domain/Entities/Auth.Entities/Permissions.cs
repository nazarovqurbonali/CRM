using System.Reflection;

namespace Domain
{
    public static class Permissions
    {
        public static void GetPermissions(this List<GetAllPermissionsDto> allPermissions, Type policy)
        {
            var nestedTypes = policy.GetNestedTypes(BindingFlags.Public);
            if (nestedTypes.Length > 0)
            {
                foreach (var nested in nestedTypes)
                {
                    FieldInfo[] fields = nested.GetFields(BindingFlags.Static | BindingFlags.Public);

                    foreach (FieldInfo fi in fields)
                    {
                        allPermissions.Add(new GetAllPermissionsDto { PermissionType = "Permission", PermissionValue = fi.GetValue(null).ToString()});
                    }
                }
            }
            else
            {
                allPermissions.Add(new GetAllPermissionsDto());
                return;
            }

        }
        public static List<string> PermissionsForRoleByModule(string module)
        {
            return new List<string>()
        {
            $"{module}.Create",
            $"{module}.View",
            $"{module}.Edit",
            $"{module}.Delete",
        };
        }
        public class Group
        {
            public const string View = "Group.View";
            public const string Create = "Group.Create";
            public const string Edit = "Group.Edit";
            public const string Delete = "Group.Delete";
        }
        public class Mentor
        {
            public const string View = "Mentor.View";
            public const string Create = "Mentor.Create";
            public const string Edit = "Mentor.Edit";
            public const string Delete = "Mentor.Delete";
        }
        public class Course
        {
            public const string View = "Course.View";
            public const string Create = "Course.Create";
            public const string Edit = "Course.Edit";
            public const string Delete = "Course.Delete";
        }
        public class User
        {
            public const string View = "User.View";
            public const string Create = "User.Create";
            public const string Edit = "User.Edit";
            public const string Delete = "User.Delete";
            public const string Blocked = "User.Blocked";
            public const string UnBlocked = "User.UnBlocked";
        }
        public class Role
        {
            public const string View = "Role.View";
            public const string Create = "Role.Create";
            public const string Edit = "Role.Edit";
            public const string Delete = "Role.Delete";
            //public const string PermissionView = "Role.View";
            //public const string PermissionEdit = "Permission.Edit.for.Role";
        }
        public class Question
        {
            public const string View = "Question.View";
            public const string CreateOrEdit = "Question.CreateOrEdit";
            public const string Create = "Question.Create";
            public const string Edit = "Question.Edit";
            public const string Delete = "Question.Delete";
        }
        public class Test
        {
            public const string View = "Test.View";
            public const string CreateOrEdit = "Test.CreateOrEdit";
            public const string Create = "Test.Create";
            public const string Edit = "Test.Edit";
            public const string Delete = "Test.Delete";
        }
    }
}

