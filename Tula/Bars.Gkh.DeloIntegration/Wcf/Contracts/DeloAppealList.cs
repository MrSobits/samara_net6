using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.DeloIntegration.Wcf.Contracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeloAppealList: List<DeloAppeal>
    {
    }
}
