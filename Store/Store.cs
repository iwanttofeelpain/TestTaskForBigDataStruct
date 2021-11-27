using System;
using System.Collections.Generic;

namespace Store
{
    public class StoreData
    {
        private readonly Dictionary<string, User> _users;
        private readonly Dictionary<string, HashSet<User>> _linksCitiesForUsers;

        public StoreData(Dictionary<string, User> users, Dictionary<string, HashSet<User>> linksCitiesForUsers)
        {
            _users = users;
            _linksCitiesForUsers = linksCitiesForUsers;
        }
        
        public bool AddUser(string userName, string city, string fullName)
        {
            User user = new User
            {
                City = city,
                FullName = fullName,
                UserName = userName
            };
            if (_users.TryAdd(userName, user))
            {
                HashSet<User> linkCitiesForUsers = _linksCitiesForUsers.GetValueOrDefault(city);
                if (linkCitiesForUsers != null)
                {
                    linkCitiesForUsers.Add(user);
                }
                else
                {
                    _linksCitiesForUsers.Add(city, new HashSet<User>()
                    {
                        user
                    });
                }

                return true;
            }

            return false;
        }

        public User GetUserByUserName(string userName)
        {
            _users.TryGetValue(userName, out User user);
            return user;
        }

        public HashSet<User> GetUsersByCity(string city)
        {
            _linksCitiesForUsers.TryGetValue(city, out HashSet<User> users);
            return users;
        }

        public bool DeleteUser(string userName)
        {
            if (_users.TryGetValue(userName, out User user))
            {
                _users.Remove(userName);
                if(_linksCitiesForUsers.TryGetValue(user.City, out HashSet<User> linkCitiesForUsers))
                    linkCitiesForUsers.Remove(user);

                return true;
            }

            return false;
        }
        
    }
}