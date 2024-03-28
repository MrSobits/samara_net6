namespace Bars.GkhGji.DomainService
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IHeatSeasonService
    {
        IDataResult ListView(BaseParams baseParams);

        IQueryable<ViewHeatingSeason> GetViewList();

        IList GetListForViewList(BaseParams baseParams, bool paging, ref int totalCount);
    }
}