namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    /// <summary>
    /// Базовый интерцептор для наследованного действия акта проверки
    /// </summary>
    public class InheritedActCheckActionBaseInterceptor<T> : EmptyDomainInterceptor<T>
        where T: ActCheckAction, new()
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            Utils.GetFiasAddressEntities(entity).ForEach(fiasAddress =>
            {
                Utils.SaveFiasAddress(this.Container, fiasAddress);
            });

            return this.Success();
        }
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            Utils.GetFiasAddressEntities(entity).ForEach(fiasAddress =>
            {
                Utils.SaveFiasAddress(this.Container, fiasAddress);
            });

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var actionCarriedOutEventService = this.Container.Resolve<IDomainService<ActCheckActionCarriedOutEvent>>();
            var actionFileService = this.Container.Resolve<IDomainService<ActCheckActionFile>>();
            var actionInspectorService = this.Container.Resolve<IDomainService<ActCheckActionInspector>>();
            var actionRemarkService = this.Container.Resolve<IDomainService<ActCheckActionRemark>>();
            var actionViolationService = this.Container.Resolve<IDomainService<ActCheckActionViolation>>();

            using (this.Container.Using(actionCarriedOutEventService, actionFileService,
                actionInspectorService, actionRemarkService, actionViolationService))
            {
                actionCarriedOutEventService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => actionCarriedOutEventService.Delete(x.Id));
                
                actionFileService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => actionFileService.Delete(x.Id));
                
                actionInspectorService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => actionInspectorService.Delete(x.Id));
                
                actionRemarkService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => actionRemarkService.Delete(x.Id));
                
                actionViolationService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => actionViolationService.Delete(x.Id));
            }
            
            this.DeleteInheritedActionAdditionalEntities(entity);
            
            return this.Success();
        }

        /// <summary>
        /// Удалить у наследованного действия дополнительные сущности
        /// </summary>
        protected virtual void DeleteInheritedActionAdditionalEntities(T entity)
        {
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            Utils.DeleteEntityFiasAddress(entity, this.Container);

            return this.Success();
        }
    }
}