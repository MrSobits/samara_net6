namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;

    public class InstrExamActionInterceptor : InheritedActCheckActionBaseInterceptor<InstrExamAction>
    {
        /// <inheritdoc />
        protected override void DeleteInheritedActionAdditionalEntities(InstrExamAction entity)
        {
            var instrExamActionNormativeDocService = this.Container.Resolve<IDomainService<InstrExamActionNormativeDoc>>();

            using (this.Container.Using(instrExamActionNormativeDocService))
            {
                instrExamActionNormativeDocService
                    .GetAll()
                    .Where(x => x.InstrExamAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => instrExamActionNormativeDocService.Delete(x.Id));
            }
        }
    }
}