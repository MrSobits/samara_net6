namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using Authentification;
    using B4.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Entities;
    using Entities.PersonalAccount;
    using Enums;

    public class PersonalAccountHistoryCreator : IPersonalAccountHistoryCreator
    {
        private readonly IGkhUserManager _userManager;
        private readonly IChargePeriodRepository periodRepository;
      

        public PersonalAccountHistoryCreator(IGkhUserManager userManager, IChargePeriodRepository periodRepository)
        {
            _userManager = userManager;
            this.periodRepository = periodRepository;
        }

        public PersonalAccountChange CreateChange(
            BasePersonalAccount account,
            PersonalAccountChangeType type,
            string description,
            string newValue,
            string oldValue,
            DateTime? actualFrom = null,
            FileInfo baseDoc = null,
            string reason = null)
        {
            var userName = _userManager.GetActiveUser().Return(x => x.Login);

            return new PersonalAccountChange(
                account,
                description,
                type,
                DateTime.UtcNow,
                actualFrom.GetValueOrDefault(DateTime.UtcNow),
                userName,
                newValue,
                oldValue,
                this.periodRepository.GetCurrentPeriod())
            {
                Document = baseDoc,
                Reason = reason
            };
        }
    }
}