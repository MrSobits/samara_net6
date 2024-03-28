namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;

    using Bars.Gkh.RegOperator.DomainModelServices.MassUpdater;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис абонентов
    /// </summary>
    public class PersonalAccountOwnerService : DefaultMassOperationExecutor<PersonalAccountOwner>, IPersonalAccountOwnerService
    {
        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomainService { get; set; }

        /// <summary>
        /// При обновлении абонента
        /// </summary>
        /// <param name="owner">абонент</param>
        /// <returns>Изменился ли абонент</returns>
        public bool OnUpdateOwner(PersonalAccountOwner owner)
        {
            if (MassUpdateContext.CurrentContext.IsNotNull())
            {
                this.AddEntity(owner);
                return false;
            }
            else
            {
                return this.OnOwnerUpdateIternal(owner);
            }
        }

        /// <summary>
        /// Обновление наименования абонента
        /// </summary>
        /// <param name="owner">Абонент</param>
        public bool UpdateName(PersonalAccountOwner owner)
        {
            var result = false;
            if (owner is IndividualAccountOwner)
            {
                var individualOwner = (IndividualAccountOwner) owner;
                var ownerName = $"{individualOwner.Surname} {individualOwner.FirstName} {individualOwner.SecondName}";
                if (individualOwner.Name != ownerName)
                {
                    individualOwner.UpdateOwnerName(ownerName);
                    result = true;
                }
            }
            else if (owner is LegalAccountOwner)
            {
                var legalOwner = (LegalAccountOwner) owner;
                var ownerName = legalOwner.Contragent.Return(x => x.Name);
                if (legalOwner.Name != ownerName)
                {
                    legalOwner.UpdateOwnerName(ownerName);
                    result = true;
                }
            }

            return result;
        }

        protected override IEnumerable<PersonalAccountOwner> ProcessChangesInternal(IEnumerable<PersonalAccountOwner> entities, bool useStateless)
        {
            return entities.Where(this.OnOwnerUpdateIternal);
        }

        private bool OnOwnerUpdateIternal(PersonalAccountOwner owner)
        {
            var result = this.UpdateName(owner);

            var states = this.BasePersonalAccountDomainService.GetAll()
                .Where(x => x.AccountOwner.Id == owner.Id)
                .Select(x => x.State.StartState)
                .ToArray();

            var totalAccountCount = states.Length;
            var activeAccountCount = states.Count(x => x);

            if (owner.TotalAccountsCount != totalAccountCount || owner.ActiveAccountsCount != activeAccountCount)
            {
                owner.TotalAccountsCount = totalAccountCount;
                owner.ActiveAccountsCount = activeAccountCount;
                result = true;
            }

            return result;
        }
    }
}
