namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public partial class Service
    {
        public IDomainService<BasePersonalAccount> PersAccDomain { get; set; }
        public IDomainService<PersonalAccountOwner> PersAccOwnerDomain { get; set; }
        public IDomainService<IndividualAccountOwner> PersAccIndOwnerDomain { get; set; }

        public bool GetOwnerInfo(List<string> accNum, string email, bool isSelected)
        {
            try
            {
                if (isSelected)
                {
                    foreach (var acc in accNum)
                    {
                        var owner = PersAccDomain.GetAll()
                            .Where(x => x.PersonalAccountNum == acc)
                            .Select(x => x.AccountOwner)
                            .FirstOrDefault();

                        var indOwn = PersAccIndOwnerDomain.GetAll()
                            .Where(x => x.Id == owner.Id)
                            .FirstOrDefault();

                        owner.BillingAddressType = AddressType.Email;
                        indOwn.Email = email;

                        PersAccOwnerDomain.Update(owner);
                        PersAccIndOwnerDomain.Update(indOwn);
                    }

                    return true;
                }
                else
                {
                    foreach (var acc in accNum)
                    {
                        var owner = PersAccDomain.GetAll()
                            .Where(x => x.PersonalAccountNum == acc)
                            .Select(x => x.AccountOwner)
                            .FirstOrDefault();

                        owner.BillingAddressType = AddressType.NotSet;

                        PersAccOwnerDomain.Update(owner);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}