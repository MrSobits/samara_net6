namespace Bars.Gkh.RegOperator.Regions.Tatarstan.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Протокол голосования к уставу
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class UstavVotprotExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVVOTPROT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <summary>
        /// Репозиторий договоров управления
        /// </summary>
        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Протокол голосования",
                "Устав"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "USTAV",
                "PROTOCOLOSS"
            };
        }
    }
}