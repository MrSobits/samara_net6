using System;

namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using B4;

    public interface ICalcAccountMoneyService
    {
        ListDataResult List(BaseParams baseParams);
        DateTime GetLastOperationDate(BaseParams baseParams);
    }
}