namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class WarningInspectionDataExport : BaseDataExportService
    {
        /// <summary>
        /// Получить данные для экспорта
        /// </summary>
        /// <param name="baseParams">
        /// dateStart - период с
        /// dateEnd - период по
        /// realityObjectId - жилой дом
        /// </param>
        public override IList GetExportData(BaseParams baseParams)
        {
            var domainService = this.Container.Resolve<IDomainService<WarningInspection>>();
            var viewModel = this.Container.Resolve<IViewModel<WarningInspection>>();

            using (this.Container.Using(domainService, viewModel))
            {
                var result = viewModel.List(domainService, baseParams);

                var viewResult = result.Data as List<ViewWarningInspection>;

                if (viewResult == null)
                {
                    return result.Data as IList;
                }

                return viewResult.Select(x => new
                {
                    x.Id,
                    x.Municipality,
                    x.ContragentName,
                    PersonInspection = x.PersonInspection?.GetDisplayName(),
                    TypeJurPerson = x.TypeJurPerson?.GetDisplayName(),
                    x.InspectionDate,
                    x.RealityObjectCount,
                    x.DocumentNumber,
                    x.InspectionNumber,
                    x.State
                }).ToList();
            }
        }
    }
}