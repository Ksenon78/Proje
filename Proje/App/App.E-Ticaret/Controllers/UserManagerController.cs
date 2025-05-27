using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class UserAccount
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<Claim> Claims { get; set; }
}

public static class UserManager
{
    private static List<UserAccount> _accounts;

    static UserManager()
    {
        _accounts = new List<UserAccount>
        {
            new UserAccount
            {
                UserName = "admin",
                Password = "testing123",
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"), // Identity.Name olarak alınacak değer
                    new Claim(ClaimTypes.Role, "Admin")  // Role-based yetkilendirme
                }
            },
            new UserAccount
            {
                UserName = "jdoe",
                Password = "testing123",
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "jdoe") // Sadece kullanıcı adı claim'i var
                }
            }
        };
    }

    public static UserAccount Login(string username, string password)
    {
        return _accounts.FirstOrDefault(x =>
            x.UserName == username &&
            x.Password == password);
    }


public class UserAccount
    {
        public string UserName { get; set; } 
        public string Password { get; set; }
        public List<Claim> Claims { get; set; }
    }
    }


