namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlTypeInterceptor : EmptyDomainInterceptor<ControlType>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ControlType> service, ControlType entity)
        {
            var tatarstanDisposalService = this.Container.Resolve<IDomainService<TatarstanDisposal>>();
            var controlOrganizationControlTypeRelationService = this.Container.Resolve<IDomainService<ControlOrganizationControlTypeRelation>>();
            var mandatoryReqsControlTypeService = this.Container.Resolve<IDomainService<MandatoryReqsControlType>>();

            try
            {                  
                var existEntity = tatarstanDisposalService.GetAll()
                        .Any(w => w.ControlType.Id == entity.Id);

                if(!existEntity)
                {
                    controlOrganizationControlTypeRelationService.GetAll()
                        .Where(w => w.ControlType.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => controlOrganizationControlTypeRelationService.Delete(f));

                    mandatoryReqsControlTypeService.GetAll()
                        .Where(w => w.ControlType.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => mandatoryReqsControlTypeService.Delete(f));

                    return Success();
                }

                return Failure($"Вид контроля <b>{entity.Name}</b> используется в распоряжениях");                                             
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                this.Container.Release(tatarstanDisposalService);
                this.Container.Release(controlOrganizationControlTypeRelationService);
                this.Container.Release(mandatoryReqsControlTypeService);
            }
        }
    }
}