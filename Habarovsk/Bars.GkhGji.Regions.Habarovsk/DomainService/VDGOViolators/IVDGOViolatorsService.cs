using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;
using System.Collections;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы со СНИЛС
    /// </summary>
    public interface IVDGOViolatorsService
    {
        IDataResult GetListRO(BaseParams baseParams);

        IDataResult GetListMinOrgContragent(BaseParams baseParams);

        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);
    }
}
