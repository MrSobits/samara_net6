namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// 2.22.8 Документы ПКР (pkrdoc.csv)
    /// Общий для регионов
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
            return this.ProxySelectorFactory.GetSelector<PkrProxy>()
                .ExtProxyListCache
                .Where(x => x.DocId.HasValue)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.DocId.ToStr(),
                        x.Id.ToStr(),
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