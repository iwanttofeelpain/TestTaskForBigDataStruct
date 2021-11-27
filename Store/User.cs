using System.Collections.Generic;

namespace Store
{
    public class User : IEqualityComparer<User>
    {
        
        public string UserName { get; set; }
        public string City { get;  set;}
        public string FullName { get;  set;}

        public bool Equals(User x, User y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.UserName == y.UserName;
        }

        public int GetHashCode(User obj)
        {
            return (obj.UserName != null ? obj.UserName.GetHashCode() : 0);
        }
    }
}