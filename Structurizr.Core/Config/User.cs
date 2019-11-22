using System;
using System.Runtime.Serialization;

namespace Structurizr.Config
{
    
    [DataContract]
    public sealed class User : IEquatable<User>
    {
        
        [DataMember(Name = "username", EmitDefaultValue = false)]
        public string Username { get; internal set; }
        
        [DataMember(Name = "role", EmitDefaultValue = true)]
        public Role Role { get; internal set; }

        internal User()
        {
        }

        internal User(string username, Role role)
        {
            Username = username;
            Role = role;
        }
        
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Username, other.Username);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

    }
    
}