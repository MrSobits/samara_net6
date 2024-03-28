namespace Bars.Gkh.Overhaul.Nso.Permissions
{
    using Bars.Gkh.Overhaul.Nso.Entities;

    public sealed class OverhaulNsoPermissionMap : PermissionMap
    {
        public OverhaulNsoPermissionMap()
        {
            Permission("Reports.GkhRegOp.RealtiesOutOfDpkr", "Дома, не включенные в ДПКР (%)");

            Namespace("Ovrhl.ProgramVersion", "Версия программы");
            Permission("Ovrhl.ProgramVersion.Copy", "Копирование");

            Namespace("Ovrhl.PublicationProgs", "Опубликованная программа");

            Namespace<PublishedProgram>("Ovrhl.PublicationProgs.PublishDate", "Поле дата опубликования");
            Permission("Ovrhl.PublicationProgs.PublishDate.View", "Просмотр");

            Namespace("Import.KpkrForNsk", "Импорт КПКР для Новосибирска");
            Permission("Import.KpkrForNsk.View", "Просмотр");
        }
    }
}