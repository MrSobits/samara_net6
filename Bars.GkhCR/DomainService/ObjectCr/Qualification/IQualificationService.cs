namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;

    using B4;

    public interface IQualificationService
    {
        IEnumerable<string> GetActiveColumns(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);
    }
}
