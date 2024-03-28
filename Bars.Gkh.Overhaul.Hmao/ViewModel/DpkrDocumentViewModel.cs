using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    public class DpkrDocumentViewModel : BaseViewModel<DpkrDocument>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<DpkrDocument> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    DocumentKindName = x.DocumentKind.Name,
                    x.DocumentName,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.DocumentDepartment,
                    x.State
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
