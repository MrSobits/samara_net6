namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;

    using B4;

    public interface ISpecialQualificationService
    {
        IEnumerable<string> GetActiveColumns(BaseParams baseParams);
    }
}
