namespace Bars.Gkh.Gasu
{
    using System;
    using Bars.B4.Utils;
    using Bars.Gkh.Gasu.Enums;

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            Namespace("Administration.ExportData", "Экспорт данных");
            Namespace("Administration.ExportData.GasuIndicator", "Показатели ГАСУ");
           
            Namespace("Administration.ExportData.ModuleEbir", "Модуль ЕБИР");

            foreach (EbirModule ebirModule in Enum.GetValues(typeof(EbirModule)))
            {
                Permission("Administration.ExportData.ModuleEbir.{0}".FormatUsing(ebirModule.GetEnumMeta().Description), ebirModule.GetEnumMeta().Display);
            }

            Namespace("Administration.ExportManagSys", "Данные для 'ГАС Управление'");
            Permission("Administration.ExportManagSys.View", "Просмотр раздела");

            Namespace("Administration.ExportOverhaulToGasu", "Данные для 'ГАС Управление' (КР)");
            Permission("Administration.ExportOverhaulToGasu.View", "Просмотр раздела");

            Namespace("Export", "Экспорт из ЖКХ");
            Permission("Export.GasuService_Edit", "Редактирование адреса сервиса ГАСУ");

            CRUDandViewPermissions("Administration.ExportData.GasuIndicator");

            Namespace("Administration.ExportData.GasuIndicatorValue", "Сведения для отправки в ГАСУ");
            Permission("Administration.ExportData.GasuIndicatorValue.View", "Просмотр");
            Permission("Administration.ExportData.GasuIndicatorValue.Edit", "Редактирование");
            Permission("Administration.ExportData.GasuIndicatorValue.CreateValues", "Заполнить перечень показателей");
            Permission("Administration.ExportData.GasuIndicatorValue.SendService", "Отправить сведения в ГАСУ");
        }
    }
}