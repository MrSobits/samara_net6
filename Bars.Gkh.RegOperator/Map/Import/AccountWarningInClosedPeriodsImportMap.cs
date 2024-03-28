namespace Bars.Gkh.RegOperator.Map.Import
{
    using System;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Предупреждение про ЛС при импорте в закрытый период"
    /// </summary>
    public class AccountWarningInClosedPeriodsImportMap : JoinedSubClassMap<AccountWarningInClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AccountWarningInClosedPeriodsImportMap() :
            base("Предупреждение про ЛС при импорте в закрытый период", "REGOP_ACCOUNT_WARNING_IN_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalNumber, "Внешний номер ЛС").Column("EXTERNAL_NUMBER");
            this.Property(x => x.ExternalRkcId, "Внешний идентификатор РКЦ").Column("EXTERNAL_RKC_ID");
            this.Property(x => x.Name, "ФИО").Column("NAME");
            this.Property(x => x.Address, "Адрес").Column("Address");
            this.Property(x => x.IsProcessed, "Обработана").Column("IS_PROCESSED").DefaultValue(YesNo.No).NotNull();
            this.Property(x => x.IsCanAutoCompared, "Может быть сопоставлена автоматически").Column("IS_CAN_AUTO_COMPARED").DefaultValue(YesNo.No).NotNull();
            this.Property(x => x.ComparingAccountId, "Идентификатор ЛС, заданный при сопоставлении (автоматическом или ручном)").Column("COMPARING_ACCOUNT_ID");
            this.Property(x => x.ComparingInfo, "Информация по сопоставлению (автоматическом или ручном): номер ЛС - адрес - ФИО").Column("COMPARING_INFO");
        }
    }
}
