using System;
using System.Collections.Generic;

using System.Text;

namespace Al.Security.CA
{
    public enum GNameType : int
    {
        OtherName = 0,
        Email = 1,
        DnsName = 2,
        DirectoryName = 4,
        UniformResourceIdentifier = 6,
        IPAddress = 7,
        RegisteredID = 8
    }
}
