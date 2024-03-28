namespace Bars.Gkh.DomainService
{
    using System.Collections;
    using Bars.B4;

    public interface IPersonService
    {
        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);

        IDataResult GetContactDetails(BaseParams baseParams);

        IDataResult AddWorkPlace(BaseParams baseParams);

        IDataResult ChangeOfCertificateStatus();
    }
}