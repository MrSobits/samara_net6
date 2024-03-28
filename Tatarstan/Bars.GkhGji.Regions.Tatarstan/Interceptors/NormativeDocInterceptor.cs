namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class NormativeDocInterceptor : Bars.GkhGji.Interceptors.NormativeDocInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            var mandatoryReqsNormativeDocService = this.Container.Resolve<IDomainService<MandatoryReqsNormativeDoc>>();
            var сontrolListTypicalQuestionDocService = this.Container.Resolve<IDomainService<ControlListTypicalQuestion>>();

            using (this.Container.Using(mandatoryReqsNormativeDocService, сontrolListTypicalQuestionDocService))
            {
                if (!base.BeforeDeleteAction(service, entity).Success)
                    return this.Failure(base.BeforeDeleteAction(service, entity).Message);

                if (сontrolListTypicalQuestionDocService.GetAll().Any(x => x.NormativeDoc.Id == entity.Id))
                    return this.Failure($"Нормативно-правовой документ <b>{entity.FullName ?? entity.Name}</b> используется в типовых вопросах проверочного листа");
                
                mandatoryReqsNormativeDocService.GetAll()
                        .Where(w => w.Npa.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => mandatoryReqsNormativeDocService.Delete(f));

                return this.Success();
            }
        }
    }
}
