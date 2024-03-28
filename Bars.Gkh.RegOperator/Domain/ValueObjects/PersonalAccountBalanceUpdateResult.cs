namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Entities;
    using Gkh.Entities;

    public class PersonalAccountBalanceUpdateResult
    {
        public PersonalAccountBalanceUpdateResult()
        {
            RealityObjects = new Dictionary<RealityObject, List<BasePersonalAccount>>();
        }

        public Dictionary<RealityObject, List<BasePersonalAccount>> RealityObjects { get; set; }
    }
}