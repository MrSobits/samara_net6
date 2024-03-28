namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Документы ПКР (pkrdoc.csv)
    /// Для Ставропольского края
    /// </summary>
    public class PkrDocExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PKRDOC";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PkrDocProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PkrId.ToStr(),
                        x.DocType,
                        x.DocName,
                        x.DocNum,
                        this.GetDate(x.DocDate),
                        x.AcceptedGoverment,
                        x.DocState.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код документа КПР",
                "Идентификатор программы",
                "Вид документа",
                "Наименование документа",
                "Номер документа",
                "Дата документа",
                "Орган, принявший документ",
                "Статус документа"
            };
        }
    }
}