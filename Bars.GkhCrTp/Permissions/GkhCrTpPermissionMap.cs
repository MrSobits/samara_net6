namespace Bars.GkhCrTp.Permissions
{
    using Bars.B4;

    public class GkhCrTpPermissionMap : PermissionMap
    {
        public GkhCrTpPermissionMap()
        {
            #region Отчеты

            Permission("Reports.CR.SetTwoOutlineBoiler", "Установка 2-х контур.котла");
            Permission("Reports.CR.ProgramCr", "Отчет по программе КР");
            Permission("Reports.CR.ExcerptFromTechPassportMkd", "Выписка из технического паспорта многоквартирного дома");
            #endregion
        }
    }
}
