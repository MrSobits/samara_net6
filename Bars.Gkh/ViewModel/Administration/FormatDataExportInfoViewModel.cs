using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.ViewModel.Administration
{
    using Bars.B4;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Utils;

    public class FormatDataExportInfoViewModel : BaseViewModel<FormatDataExportInfo>
    {
        public override IDataResult List(IDomainService<FormatDataExportInfo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.LoadDate,
                    x.ObjectType
                }).ToListDataResult(loadParams, this.Container);
        }
    }
}
