namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseStatementService
    {
        IDataResult AddAppealCitizens(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListByAppealCits(BaseParams baseParams);

        IDataResult CreateWithAppealCits(BaseParams baseParams);

        IDataResult CreateWithBasis(BaseParams baseParams);

        IDataResult AddBasisDocs(BaseParams baseParams);

        /// <summary>
        /// Осуществляет проверку, есть ли тематики по данному обращению
        /// </summary>
        IDataResult AnyThematics(BaseParams baseParams);

        IDataResult CheckAppealCits(BaseParams baseParams);
    }
}