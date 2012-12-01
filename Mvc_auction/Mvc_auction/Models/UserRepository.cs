using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Data.Objects;


namespace Mvc_auction.Models
{
    public class UserRepository
    {
       // private CustomRoleMembershipDB db = new CustomRoleMembershipDB();

        public MembershipUser CreateUser(string username, string password, string email)
        {
            using (CustomMembershipDB db = new CustomMembershipDB())
            {
                User user = new User();
                user.userName = username;
                user.mail = email;
                user.passwordSalt = CreateSalt();
                user.password = CreatePasswordHash(password, user.passwordSalt);                
                user.createdDate = DateTime.Now;
                user.isActivated = false;
                user.isLockedOut = false;
                user.lastLockedOutDate = DateTime.Now;
                user.lastLoginDate = DateTime.Now;
                db.AddToUsers(user); 
                db.SaveChanges();

                return GetUser(username);
            }
        }
        public string GetUserNameByEmail(string email)
        {
            using (CustomMembershipDB db = new CustomMembershipDB())
            {
               IQueryable<User>  result = from u in db.Users where (u.mail == email) select u;
                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();
                    return dbuser.userName;
                }
                else
                {
                    return "";
                }
            }
        }

        public MembershipUser GetUser(string username)
        {
            using (CustomMembershipDB db = new CustomMembershipDB())
            {
                var result = from u in db.Users where (u.userName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    string _username = dbuser.userName;
                    int _providerUserKey = dbuser.user_id;
                    string _email = dbuser.mail;
                    string _passwordQuestion = "";
                    string _comment = dbuser.comments;
                    bool _isApproved =(bool) dbuser.isActivated;
                    bool _isLockedOut =(bool) dbuser.isLockedOut;
                    DateTime _creationDate =(DateTime) dbuser.createdDate;
                    DateTime _lastLoginDate =(DateTime) dbuser.lastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate =(DateTime) dbuser.lastLockedOutDate;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate);

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }
        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPwd, "sha1");
            return hashedPwd;
        }
        
        public bool ValidateUser(string username, string password)
        {
            using (CustomMembershipDB db = new CustomMembershipDB())
            {
                var result = from u in db.Users where (u.userName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.password == CreatePasswordHash(password, dbuser.passwordSalt))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}