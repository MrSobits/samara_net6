namespace Bars.Gkh.Import.Organization
{
    using Bars.B4;
    using Bars.GkhExcel;

    using NHibernate;

    public interface IOrganizationImportHelper
    {
        ILogImport LogImport { get; }

        string OrganizationType { get; }

        IDataResult ProcessLine(GkhExcelCell[] data, Record record);

        IDataResult CreateContract(Record record, bool updatePeriodsManOrgs);

        void SaveData(IStatelessSession session);
    }
}