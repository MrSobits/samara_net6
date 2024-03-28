namespace Bars.Gkh.Regions.Yanao
{
    using Bars.B4;

    public class GkhPermissionMapYanao : PermissionMap
    {
        public GkhPermissionMapYanao()
        {
            this.Permission("Gkh.RealityObject.Field.View.TechPassportScanFile_View", "Технический паспорт объекта");
            this.Permission("Gkh.RealityObject.Field.Edit.TechPassportScanFile_Edit", "Технический паспорт объекта");
            this.Permission("Gkh.RealityObject.Field.View.MethodFormFundCr_View", "Предполагаемый способ формирования фонда КР");
            this.Permission("Gkh.RealityObject.Field.Edit.MethodFormFundCr_Edit", "Предполагаемый способ формирования фонда КР");
        }
    }
}