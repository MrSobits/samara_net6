using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections;

using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ConfigSections
{
    public class GjiValidityDocPeriodInterceptor : EmptyDomainInterceptor<GjiValidityDocPeriod>
    {
        public override IDataResult BeforeCreateAction(IDomainService<GjiValidityDocPeriod> service, GjiValidityDocPeriod entity)
            => CheckDocument(service, entity);

        public override IDataResult BeforeUpdateAction(IDomainService<GjiValidityDocPeriod> service, GjiValidityDocPeriod entity)
            => CheckDocument(service, entity);

        private IDataResult CheckDocument(IDomainService<GjiValidityDocPeriod> service, GjiValidityDocPeriod entity)
        {
            if (entity.EndDate == null
                && service.GetAll().Any(x =>
                    x.Id != entity.Id
                    && x.TypeDocument == entity.TypeDocument
                    && x.EndDate == null))
            {
                return Failure($"Имеется запись по документу \"{entity.TypeDocument.GetDisplayName()}\", у которой не закрыт период действия.<br/>" +
                    " Просьба закрыть период действия старого документа и повторить сохранение.");
            }

            if (service.GetAll().Any(x =>
                x.Id != entity.Id
                && x.TypeDocument == entity.TypeDocument
                && (entity.StartDate >= x.StartDate && entity.StartDate <= x.EndDate
                    || entity.EndDate >= x.StartDate && entity.EndDate <= x.EndDate
                    || entity.StartDate < x.StartDate && entity.EndDate > x.EndDate)))
            {
                return Failure($"Имеется запись по документу \"{entity.TypeDocument.GetDisplayName()}\" с пересекающимся периодом действия.");
            }

            return this.Success();
        }
    }
}
