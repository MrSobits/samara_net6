namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface ITypeSurveyGjiService
    {
        IDataResult AddKindInsp(BaseParams baseParams);

        IDataResult AddProvidedDocuments(BaseParams baseParams);

        IDataResult AddAdministrativeRegulations(BaseParams baseParams);

        IDataResult AddGoals(BaseParams baseParams);

        IDataResult AddTaskInsp(BaseParams baseParams);

        IDataResult AddInspFoundation(BaseParams baseParams);

		IDataResult AddInspFoundationChecks(BaseParams baseParams);
	}
}