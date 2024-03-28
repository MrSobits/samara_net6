namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Ресурсоснабжающие организации
    /// </summary>
    public class RsoExportableEntity : BaseExportableEntity<PublicServiceOrg>
    {
        /// <inheritdoc />
        public override string Code => "RSO";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.Contragent)
                .Select(x => new ExportableRow(x.Contragent,
                    new List<string>
                    {
                        this.GetStrId(x.Contragent),
                        string.Empty, // 2. Номер документа о включении
                        string.Empty, // 3. Дата включения в Реестр субъектов естественных монополий
                        string.Empty, // 4. Номер документа о включении
                        string.Empty // 5. Дата включения в Федеральный информационный реестр гарантирующих поставщиков и зон их деятельности
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Номер документа о включении (Реестр субъектов естественных монополий)",
                "Дата включения в Реестр субъектов естественных монополий",
                "Номер документа о включении (Федеральный информационный реестр гарантирующих поставщиков и зон их деятельности)",
                "Дата включения в Федеральный информационный реестр гарантирующих поставщиков и зон их деятельности"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENT"
            };
        }
    }
}