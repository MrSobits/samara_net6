namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class PreventiveActionTaskRegulationService : IPreventiveActionTaskRegulationService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddNormativeDocs(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var normativeDocsIds = baseParams.Params.GetAs<long[]>("ids");
            var regulationDomain = this.Container.ResolveDomain<PreventiveActionTaskRegulation>();

            using (this.Container.Using(regulationDomain))
            {
                var regulationsToSave = normativeDocsIds.Select(id => new PreventiveActionTaskRegulation()
                {
                    Task = new PreventiveActionTask() { Id = documentId },
                    NormativeDoc = new NormativeDoc() { Id = id }
                });

                TransactionHelper.InsertInManyTransactions(this.Container, regulationsToSave);
            }

            return new BaseDataResult();
        }
    }
}