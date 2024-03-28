namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Общая информация
    /// </summary>
    public class InfoExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "_INFO";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var inn = this.Contragent.Inn;
            var kpp = this.Contragent.Kpp;
            var ogrn = this.Contragent.Ogrn;
            var name = this.Contragent.Name;
            var dataId = string.Empty;
            var date = this.SelectParams.GetAs("Info.Date", DateTime.Now);
            var year = this.FilterService.ExportDate.Year;
            var month = this.FilterService.ExportDate.Month;
            var senderName = this.SelectParams.GetAs<string>("Info.SenderName");
            var useIncremental = this.SelectParams.GetAs<bool>("UseIncremental");
            var phone = this.Contragent.Phone;
            var type = (int)this.Provider;

            var info = new ExportableRow(1,
                new List<string>
                {
                    this.FormatVersion,
                    inn,
                    kpp,
                    ogrn,
                    name,
                    dataId,
                    year.ToStr(),
                    month.ToStr(),
                    this.GetDateTime(date),
                    senderName,
                    phone,
                    type.ToStr(),
                    "ЖКХ Комплекс",
                    useIncremental ? "2" : "1"
                });

            return new List<ExportableRow>(new[] { info });
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Версия формата",
                "ИНН",
                "КПП",
                "ОГРН (ОГРНИП)",
                "Наименование организации (ФИО ИП)",
                "Ключ банка данных (Наименование банка данных)",
                "Год",
                "Месяц",
                "Дата и время формирования",
                "ФИО отправителя",
                "Телефон отправителя",
                "Тип поставщика информации",
                "Система, из которой выгружены данные",
                "Тип выгрузки"
            };
        }
    }
}