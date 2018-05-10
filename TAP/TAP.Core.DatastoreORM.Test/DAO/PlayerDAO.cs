using System;
using System.Collections.Generic;
using TAP.Core.DatastoreORM.DAO;

namespace TAP.Core.DatastoreORM.Test.DAO
{
    public enum PlayerStatus
    {
        Active = 1,
        NotActive
    }

    [Kind("TestORM-Player")]
    public class PlayerDAO: IdentityBaseDAO
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public List<string> Hobbies { get; set; }
        public DateTime RegisterTime { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public List<PlayerSolider> PlayerSoliders { get; set; }

        
        [ExcludeIndex]
        [DbFieldName("description")]
        public string Description { get; set; }
    }

    public class AddressInfo
    {
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }

        [ExcludeIndex]
        public string Description { get; set; }
    }

    public class PlayerSolider
    {
        public string Name { get; set; }
        public int Power { get; set; }
    }
}
