namespace Bars.Gkh.Regions.Tatarstan.Permissions
{
    using Bars.Gkh.DomainService;

    public class GkhFieldRequirementMap : FieldRequirementMap
    {
        public GkhFieldRequirementMap()
        {
            this.Namespace("Gkh.RealityObjectOutdoor.Image", "Фото-архив");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.DateImage_Rqrd", "Дата изображения");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.Name_Rqrd", "Наименование");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.ImagesGroup_Rqrd", "Группа");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.Period_Rqrd", "Период");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.WorkCr_Rqrd", "Вид работы");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.File_Rqrd", "Файл");
            this.Requirement("Gkh.RealityObjectOutdoor.Image.Description_Rqrd", "Описание");
        }
    }
}