namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
{
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.B4.Modules.Mapping.Mappers;

    /// <summary>
    /// Маппинг для "Настройки источников для документов на оплату"
    /// </summary>
    public class BuilderConfigMap : BaseEntityMap<BuilderConfig>
    {
        public BuilderConfigMap() : 
                base("Настройки источников для документов на оплату", "REGOP_BUILDER_CONFIG")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Path, "Код настройки через точку").Column("PATH").Length(500).NotNull();
            this.Property(x => x.PaymentDocumentType, "Тип платежного документа").Column("OWNER_TYPE").NotNull();
            this.Property(x => x.Enabled, "Включен ли источник").Column("ENABLED").NotNull();            
        }
    }
}