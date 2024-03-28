namespace Bars.GkhGji.Regions.Saha.Permissions
{
    using B4;

    public class GkhGjiActSurveyOwnerPermissionMap : PermissionMap
    {

        public GkhGjiActSurveyOwnerPermissionMap()
        {
            Namespace("GkhGji.DocumentsGji.ActSurvey.Register.Owner", "Лица, присутствующие при обследовании");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Edit", "Изменение записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Create", "Создание записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Delete", "Удаление записей");
        }
    }
}