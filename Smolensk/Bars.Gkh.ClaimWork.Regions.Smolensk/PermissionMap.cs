namespace Bars.Gkh.ClaimWork.Regions.Smolensk
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Clw.ClaimWork.Debtor.Lawsuit.Field", "Поля");
            this.Permission("Clw.ClaimWork.Debtor.Lawsuit.Field.DateDirectionForSsp_View", "Дата направления на исполнение в ССП - Просмотр");
        }
    }
}