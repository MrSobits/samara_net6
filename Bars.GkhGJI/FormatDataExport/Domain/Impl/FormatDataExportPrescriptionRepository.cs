namespace Bars.GkhGji.FormatDataExport.Domain.Impl
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class FormatDataExportPrescriptionRepository : BaseFormatDataExportRepository<Prescription>
    {
        public override IQueryable<Prescription> GetQuery(IFormatDataExportFilterService filterService)
        {
            var viewRepository = this.Container.Resolve<IFormatDataExportRepository<ViewFormatDataExportInspection>>();
            var repository = this.Container.ResolveRepository<DocumentGjiChildren>();
            using (this.Container.Using(viewRepository, repository))
            {
                var filterQuery = viewRepository.GetQuery(filterService);

                return repository.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Where(x => filterQuery.Any(y => x.Parent == y.ActCheck))
                    .Select(x => x.Children as Prescription);
            }
        }
    }
}