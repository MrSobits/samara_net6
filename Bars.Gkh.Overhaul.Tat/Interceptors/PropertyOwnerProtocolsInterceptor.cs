namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System;
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class PropertyOwnerProtocolsInterceptor : EmptyDomainInterceptor<PropertyOwnerProtocols>
    {
        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<PropertyOwnerProtocols> service, PropertyOwnerProtocols entity)
        {
            return BeforeSaveAction(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PropertyOwnerProtocols> service, PropertyOwnerProtocols entity)
        {
            return BeforeSaveAction(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PropertyOwnerProtocols> service, PropertyOwnerProtocols entity)
        {
            BasePropertyOwnerDecisionDomain.GetAll().Where(x => x.PropertyOwnerProtocol.Id == entity.Id)
                            .Select(x => x.Id).ForEach(x => BasePropertyOwnerDecisionDomain.Delete(x));

            return Success();
        }

        private IDataResult BeforeSaveAction(PropertyOwnerProtocols entity)
        {
            if (entity.DocumentDate != null && entity.DocumentDate.Value.Date > DateTime.Now.Date)
            {
                return Failure("Дата протокола не может быть больше текущей даты");
            }

            return Success();
        }
    }
}