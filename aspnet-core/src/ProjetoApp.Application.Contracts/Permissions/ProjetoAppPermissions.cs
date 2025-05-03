namespace ProjetoApp.Permissions;

public static class ProjetoAppPermissions
{
    public const string GroupName = "ProjetoApp";

    public static class Projects
    {
        public const string Default = GroupName + ".Projects";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string ViewReports = Default + ".ViewReports";
    }
    public class Customer
    {
        public const string Default = GroupName + ".Customer";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
