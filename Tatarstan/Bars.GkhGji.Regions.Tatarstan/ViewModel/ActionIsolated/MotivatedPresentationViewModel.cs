namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// View-модель для <see cref="MotivatedPresentation"/>
    /// </summary>
    public class MotivatedPresentationViewModel : BaseViewModel<MotivatedPresentation>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<MotivatedPresentation> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.Get(id);

            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();

            var documentGjiInspectorService = this.Container.Resolve<IDocumentGjiInspectorService>();

            using (this.Container.Using(documentGjiChildrenDomain, documentGjiInspectorService))
            {
                var dataInspectors = documentGjiInspectorService.GetInspectorsByDocumentId(id)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();

                return obj != null
                    ? new BaseDataResult(
                        new
                        {
                            obj.Id,
                            obj.DocumentDate,
                            obj.DocumentNum,
                            obj.DocumentSubNum,
                            obj.DocumentNumber,
                            obj.DocumentYear,
                            obj.State,
                            CreationPlace = obj.CreationPlace?.GetFiasProxy(this.Container),
                            obj.IssuedMotivatedPresentation,
                            obj.ResponsibleExecution,
                            Inspectors = string.Join(", ", dataInspectors.Select(x=>x.Fio)),
                            InspectorIds = string.Join(", ", dataInspectors.Select(x => x.InspectorId)),
                            ParentDocumentType = documentGjiChildrenDomain
                                .FirstOrDefault(x => x.Children.Id == obj.Id)?.Parent?.TypeDocumentGji
                        })
                    : new BaseDataResult();
            }
        }
    }
}