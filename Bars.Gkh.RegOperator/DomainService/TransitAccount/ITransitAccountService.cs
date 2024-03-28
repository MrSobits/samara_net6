namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections;

    using B4;

    public interface ITransitAccountService
    {
        // Дополнительная информация
       IDataResult GetInfo(BaseParams baseParams);

        // Лист по дебету
        IList DebetList(BaseParams baseParams, bool paging, ref int totalCount);

        // Лист по кредиту
        IList CreditList(BaseParams baseParams, bool paging, ref int totalCount);

        void MakeDebetList();

        void MakeCreditList();

        IDataResult ExportToTxt(BaseParams baseParams);
    }
}