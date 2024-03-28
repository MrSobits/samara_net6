
using Bars.B4.Modules.DataExport.Domain;

namespace Bars.Gkh.Modules.ClaimWork.Export
{
    using System.Collections;
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    public interface IBaseClaimWorkExport<T> : IDataExportService
         where T : BaseClaimWork
    {
    }

}