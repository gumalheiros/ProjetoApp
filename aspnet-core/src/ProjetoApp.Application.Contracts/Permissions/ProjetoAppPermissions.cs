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
}
