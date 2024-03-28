namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;
    using Domain;
    using Modules.ClaimWork.Entities;
    using Regions.Voronezh.Entities;
    using RegOperator.Entities.PersonalAccount;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Интерцептор для Выписок
    /// </summary>
    /// <typeparam name="RosRegExtractDesc">ПИР</typeparam>
    public class RosRegExtractDescInterceptor : EmptyDomainInterceptor<RosRegExtractDesc>
    { 

        public override IDataResult BeforeUpdateAction(IDomainService<RosRegExtractDesc> service, RosRegExtractDesc entity)
        {
            if (entity.Room_id > 0)
            {
                entity.YesNoNotSet = Gkh.Enums.YesNoNotSet.Yes;

                var debtorDomain = this.Container.ResolveDomain<Debtor>();
                try
                {
                    var debtors = debtorDomain.GetAll().Where(x => x.PersonalAccount.Room.Id == entity.Room_id);
                    foreach (var debtor in debtors)
                    {
                        debtor.ExtractExists = Gkh.Enums.YesNo.Yes;
                        debtorDomain.Save(debtor);
                    }
                }
                finally
                {
                    Container.Release(debtorDomain);
                }

            }
            return this.Success();
        }
    }
}