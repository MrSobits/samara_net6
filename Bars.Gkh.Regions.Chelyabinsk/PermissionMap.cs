namespace Bars.Gkh.Regions.Chelyabinsk
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Permission("Reports.GKH.UnderstandingHomeReport", "Общие сведения по домам");

            this.Permission("Gkh.RealityObject.Field.Edit.AreaCleaning_Edit", "Уборочная площадь (кв.м.)");

            
        }
    }
}