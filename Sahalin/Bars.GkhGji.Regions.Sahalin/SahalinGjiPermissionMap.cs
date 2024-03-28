namespace Bars.GkhGji.Regions.Sahalin
{
    using Bars.B4;

    public class SahalinGjiPermissionMap: PermissionMap
    {
        public SahalinGjiPermissionMap()
        {
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonFields", "Реквизиты физ. лица");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonAddress_Edit", "Адрес (место жительства, телефон) - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonJob_Edit", "Место работы - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonPosition_Edit", "Должность - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonBirthDatePlace_Edit", "Дата, место рождения - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonDoc_Edit", "Документ, удостоверяющий личность - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonSalary_Edit", "Заработная плата - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PersonRelationship_Edit", "Семейное положение, кол-во иждивенцев - Редактирование");
        }
    }

}