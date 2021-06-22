namespace BuildingMaterialRent.Models
{
    public class LoginInfo
    {
        private LoginInfo(bool isSignedIn, UserInfo userInfo)
        {
            IsSignedIn = isSignedIn;
            UserInfo = userInfo;
        }
        public bool IsSignedIn { get; }

        public UserInfo UserInfo { get; }

        public static LoginInfo NotSignedIn => new(false, null);

        public static LoginInfo SignedIn(string userName, string role) => new(true, new UserInfo
        {
            UserName = userName,
            Role = role
        });
    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
