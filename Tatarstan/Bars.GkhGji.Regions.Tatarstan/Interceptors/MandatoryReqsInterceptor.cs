namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class MandatoryReqsInterceptor : EmptyDomainInterceptor<MandatoryReqs>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<MandatoryReqs> service, MandatoryReqs entity)
        {
            var controlListTypicalQuestionService = this.Container.Resolve<IDomainService<ControlListTypicalQuestion>>();
            var mandatoryReqsNormativeDocService = this.Container.Resolve<IDomainService<MandatoryReqsNormativeDoc>>();
            var mandatoryReqsControlTypeService = this.Container.Resolve<IDomainService<MandatoryReqsControlType>>();

            try
            {       
                var existEntity = controlListTypicalQuestionService.GetAll()                            
                    .Any(w => w.MandatoryRequirement.Id == entity.Id);

                if (!existEntity)
                {
                    mandatoryReqsNormativeDocService.GetAll()
                        .Where(w => w.MandatoryReqs.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => mandatoryReqsNormativeDocService.Delete(f));

                    mandatoryReqsControlTypeService.GetAll()
                        .Where(w => w.MandatoryReqs.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => mandatoryReqsControlTypeService.Delete(f));

                    return Success();
                }
                return Failure($"Невозможно удалить запись. Имеются связанные объекты");                
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                this.Container.Release(controlListTypicalQuestionService);
                this.Container.Release(mandatoryReqsNormativeDocService);
                this.Container.Release(mandatoryReqsControlTypeService);
            }
        }
    }
}
