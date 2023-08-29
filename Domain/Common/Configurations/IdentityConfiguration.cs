namespace Domain.Common.Configurations;

public class IdentityConfiguration
{
    public const string IdClaim = "id";
    public const string UserStateClaim = "userstate";

    public class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
    }

    public class Policy
    {
        public const string Admin = "AdminPolicy";
        public const string Manager = "ManagerPolicy";
        public const string User = "UserPolicy";
    }
}