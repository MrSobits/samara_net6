/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount;
/// 
///     public class DefferedUnactivationMap: BaseImportableEntityMap<DefferedUnactivation>
///     {
///         public DefferedUnactivationMap() : base("REGOP_DEFFER_UNACTIV")
///         {
///             Map(x => x.UnactivationDate, "UNACT_DATE");
///             Map(x => x.Processed, "PROCESSED");
/// 
///             References(x => x.PersonalAccount, "ACCOUNT_ID");
///             References(x => x.GovDecision, "GOV_DECISION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Отложенный перевод статуса счета в "Неактивно". Необходимость вознкла в связи с добавлением "Протокола решений госорганов""</summary>
    public class DefferedUnactivationMap : BaseImportableEntityMap<DefferedUnactivation>
    {
        
        public DefferedUnactivationMap() : 
                base("Отложенный перевод статуса счета в \"Неактивно\". Необходимость вознкла в связи с д" +
                        "обавлением \"Протокола решений госорганов\"", "REGOP_DEFFER_UNACTIV")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.UnactivationDate, "Дата, когда счета дома будут переведены в статус неактивно").Column("UNACT_DATE");
            Property(x => x.Processed, "Дом был обработан").Column("PROCESSED");
            Reference(x => x.PersonalAccount, "Дом, счета которого будут переведены в статус некативно").Column("ACCOUNT_ID");
            Reference(x => x.GovDecision, "Решение, по которому лицевые счета будут переведены в статус неактивно").Column("GOV_DECISION_ID");
        }
    }
}
