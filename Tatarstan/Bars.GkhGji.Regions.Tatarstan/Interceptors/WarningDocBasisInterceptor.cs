namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class WarningDocBasisInterceptor : EmptyDomainInterceptor<WarningDocBasis>
    {
        public override IDataResult BeforeCreateAction(IDomainService<WarningDocBasis> service, WarningDocBasis entity)
        {
            if (this.CheckExists(service, entity))
            {
                return this.Failure($"Основание {entity.WarningBasis.Name} уже добавлено");
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<WarningDocBasis> service, WarningDocBasis entity)
        {
            if (this.CheckExists(service, entity))
            {
                return this.Failure($"Основание {entity.WarningBasis.Name} уже добавлено");
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private bool CheckExists(IDomainService<WarningDocBasis> service, WarningDocBasis entity)
        {
            return service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Where(x => x.WarningDoc.Id == entity.WarningDoc.Id)
                .Any(x => x.WarningBasis.Id == entity.WarningBasis.Id);
        }
    }
}