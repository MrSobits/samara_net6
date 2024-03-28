/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class SpecialAccountDecisionNoticeMap : BaseImportableEntityMap<SpecialAccountDecisionNotice>
///     {
///         public SpecialAccountDecisionNoticeMap()
///             : base("OVRHL_DEC_SPEC_ACC_NOTICE")
///         {
///             this.Map(x => x.NoticeNumber, "NOTICE_NUMBER");
///             this.Map(x => x.NoticeNum, "NOTICE_NUM");
///             this.Map(x => x.NoticeDate, "NOTICE_DATE");
///             this.Map(x => x.RegDate, "REG_DATE");
///             this.Map(x => x.GjiNumber, "GJI_NUMBER");
///             this.Map(x => x.HasOriginal, "HAS_ORIG", true);
///             this.Map(x => x.HasCopyProtocol, "HAS_COPY_PROT", true);
///             this.Map(x => x.HasCopyCertificate, "HAS_COPY_CERT", true);
/// 
///             this.References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             this.References(x => x.SpecialAccountDecision, "SPEC_ACC_DEC_ID", ReferenceMapConfig.Fetch);
///             this.References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Уведомления решения собственников помещений МКД (при формирования фонда КР на спец.счете)"</summary>
    public class SpecialAccountDecisionNoticeMap : BaseImportableEntityMap<SpecialAccountDecisionNotice>
    {
        
        public SpecialAccountDecisionNoticeMap() : 
                base("Уведомления решения собственников помещений МКД (при формирования фонда КР на спе" +
                        "ц.счете)", "OVRHL_DEC_SPEC_ACC_NOTICE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.SpecialAccountDecision, "Решение собственников помещений МКД").Column("SPEC_ACC_DEC_ID").Fetch();
            Property(x => x.NoticeNum, "Номер уведомления (числ)").Column("NOTICE_NUM");
            Property(x => x.NoticeNumber, "Номер уведомления").Column("NOTICE_NUMBER").Length(250);
            Property(x => x.NoticeDate, "Дата уведомления").Column("NOTICE_DATE");
            Reference(x => x.File, "Документ уведомления").Column("FILE_ID").Fetch();
            Property(x => x.RegDate, "Дата регистрации уведомления в ГЖИ").Column("REG_DATE");
            Property(x => x.GjiNumber, "Входящий номер в ГЖИ").Column("GJI_NUMBER").Length(250);
            Property(x => x.HasOriginal, "Оригинал уведомления поступил").Column("HAS_ORIG").NotNull();
            Property(x => x.HasCopyProtocol, "Копия протокола поступила").Column("HAS_COPY_PROT").NotNull();
            Property(x => x.HasCopyCertificate, "Копия справки поступила").Column("HAS_COPY_CERT").NotNull();
        }
    }
}
