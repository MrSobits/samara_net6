namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Castle.Core.Internal;

    public class SubsidyRecordInterceptor : EmptyDomainInterceptor<SubsidyRecord>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SubsidyRecord> service, SubsidyRecord entity)
        {

            //Поскольку хотитм удалить субсидирвоание то над оудалить и все версии которые к этому субсидирваонию относятся
            var subsidyRecVersDomain = Container.Resolve<IDomainService<SubsidyRecordVersion>>();

            subsidyRecVersDomain.GetAll().Where(x => x.SubsidyRecord.Id == entity.Id).Select(x => x.Id).ForEach(x => subsidyRecVersDomain.Delete(x));

            Container.Release(subsidyRecVersDomain);

            return base.BeforeDeleteAction(service, entity);
        }

    }
}