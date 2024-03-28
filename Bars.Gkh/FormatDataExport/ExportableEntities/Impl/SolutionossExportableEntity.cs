namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Повестка общего собрания  собственников
    /// </summary>
    public class SolutionossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "SOLUTIONOSS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return new List<ExportableRow>();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Протокол голосования",
                "Номер вопроса",
                "Вопрос",
                "Тип решения общего собрания собственников",
                "Результаты голосования «За»",
                "Результаты голосования «Против»",
                "Результаты голосования «Воздержался»",
                "Способ формирования фонда капитального ремонта",
                "Итог голосования"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                //"PROTOCOLOSS"
            };
        }
    }
}