namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations
{
    using System.Collections.Generic;
    using B4;

    public interface IAccountOperationProvider
    {
        List<AccOperationProxy> GetAllOperations();

        IDataResult Execute(BaseParams baseParams);
    }
}