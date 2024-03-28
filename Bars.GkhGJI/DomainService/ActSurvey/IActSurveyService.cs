namespace Bars.GkhGji.DomainService
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IActSurveyService
    {
        IDataResult GetInfo(long? documentId);

        IList ListView(BaseParams baseParams, bool pagging, ref int totalCount);

        IQueryable<ViewActSurvey> GetViewList();
    }
}