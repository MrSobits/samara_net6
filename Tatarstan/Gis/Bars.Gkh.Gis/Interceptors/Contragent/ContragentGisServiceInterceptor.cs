namespace Bars.Gkh.Gis.Interceptors.Contragent
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gis.Entities.Dict;

    public class ContragentGisServiceInterceptor : EmptyDomainInterceptor<Contragent>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<Contragent> service, Contragent entity)
        {
            var gisTarrifDictServ = this.Container.Resolve<IDomainService<GisTariffDict>>();

            using (this.Container.Using(gisTarrifDictServ))
            {
                if (gisTarrifDictServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    return this.Failure("Удаление невозможно: у контрагента существует связь с справочником тарифов ГИС ЖКХ");
                }
            }

            return this.Success();
        }
    }
}