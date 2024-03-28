namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <inheritdoc />
    public class MotivationConclusionViewModel : BaseViewModel<MotivationConclusion>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<MotivationConclusion> domainService, BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAsId();
            var inspectorsDomain = this.Container.ResolveDomain<InspectionGjiInspector>();
            var inspectionDocumentsDomain = this.Container.ResolveDomain<InspectionGjiDocumentGji>();
            var disposalDomain = this.Container.ResolveRepository<Disposal>();

            using (this.Container.Using(inspectorsDomain, inspectionDocumentsDomain, disposalDomain))
            {
                // получаем 'проверку по обращению граждан' с этим мотивирочным заключением и распоряжение этой проверки
                var disposalNumber = inspectionDocumentsDomain.GetAll()
                    .Join(disposalDomain.GetAll(), x => x.Inspection.Id, y => y.Inspection.Id, (x, y) => new {inspectionDocument = x, disposal = y})
                    .Where(x => x.disposal.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => x.inspectionDocument.Document.Id == docId)
                    .Select(y => y.disposal.DocumentNumber)
                    .FirstOrDefault();

                var doc = domainService.GetAll()
                    .Where(x => x.Id == docId)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeDocumentGji,
                        BaseDocumentId = (long?) x.BaseDocument.Id,
                        Autor = x.Autor != null ? new { x.Autor.Id, x.Autor.Fio } : null,
                        Executant = x.Executant != null ? new { x.Executant.Id, x.Executant.Fio } : null,
                        InspectionId = x.Inspection.Id,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentYear,
                        x.DocumentNum,
                        x.DocumentSubNum,
                        x.State
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeDocumentGji,
                        x.BaseDocumentId,
                        x.Autor,
                        x.Executant,
                        Inspectors = inspectorsDomain.GetAll()
                            .Where(y => y.Inspection.Id == x.InspectionId)
                            .Select(y => y.Inspector.Fio)
                            .AggregateWithSeparator(", "),
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentYear,
                        x.DocumentNum,
                        x.DocumentSubNum,
                        x.State,
                        DisposalNumber = disposalNumber
                    })
                    .FirstOrDefault();

                return new BaseDataResult(doc);
            }
        }
    }
}