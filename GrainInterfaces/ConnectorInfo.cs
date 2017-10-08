using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    [Serializable]
    public class ConnectorInfo : IEquatable<ConnectorInfo>
    {
        /// <summary>Unique Id for this user</summary>
        public long UserId { get; private set; }

        /// <summary>Alias / username for this user</summary>
        public string UserAlias { get; private set; }

        public static ConnectorInfo GetUserInfo(long userId, string userAlias)
        {
            return new ConnectorInfo { UserId = userId, UserAlias = userAlias };
        }

        public override string ToString()
        {
            return "ConnectionInfo:Alias=" + UserAlias + ",Id=" + UserId;
        }

        public bool Equals(ConnectorInfo other)
        {
            return this.UserId == other.UserId;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }

    }
}
