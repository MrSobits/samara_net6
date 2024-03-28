/// <mapping-converter-backup>
/// namespace Bars.Gkh.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class ActViolIdentificationClwMap : BaseJoinedSubclassMap<ActViolIdentificationClw>
///     {
///         public ActViolIdentificationClwMap()
///             : base("CLW_ACT_VIOL_IDENTIF", "ID")
///         {
///             Map(x => x.SendDate, "SEND_DATE");
///             Map(x => x.SignDate, "SIGN_DATE");
///             Map(x => x.FormDate, "FORM_DATE");
///             Map(x => x.SignTime, "SIGN_TIME", false, 50);
///             Map(x => x.FormTime, "FORM_TIME", false, 50);
///             Map(x => x.SignPlace, "SIGN_PLACE", false, 500);
///             Map(x => x.FormPlace, "FORM_PLACE", false, 500);
///             Map(x => x.Persons, "PERSONS", false, 500);
///             Map(x => x.ActType, "ACT_TYPE", true, (object)0);
///             Map(x => x.FactOfReceiving, "FACT_OF_RECEIVE", true, (object)0);
///             Map(x => x.FactOfSigning, "FACT_OF_SIGN", true, (object)0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Акт выявления нарушений"</summary>
    public class ActViolIdentificationClwMap : JoinedSubClassMap<ActViolIdentificationClw>
    {
        
        public ActViolIdentificationClwMap() : 
                base("Акт выявления нарушений", "CLW_ACT_VIOL_IDENTIF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ActType, "Тип акта").Column("ACT_TYPE").DefaultValue(ActViolIdentificationType.NotSet).NotNull();
            Property(x => x.SendDate, "Дата отправки").Column("SEND_DATE");
            Property(x => x.FactOfReceiving, "Факт получения").Column("FACT_OF_RECEIVE").DefaultValue(FactOfReceiving.NotConfirmed).NotNull();
            Property(x => x.FactOfSigning, "Факт подписания").Column("FACT_OF_SIGN").DefaultValue(FactOfSigning.NotSet).NotNull();
            Property(x => x.SignDate, "Дата подписания").Column("SIGN_DATE");
            Property(x => x.SignTime, "Время подписания").Column("SIGN_TIME").Length(50);
            Property(x => x.SignPlace, "Место подписания").Column("SIGN_PLACE").Length(500);
            Property(x => x.FormDate, "Дата составления").Column("FORM_DATE");
            Property(x => x.FormTime, "Время составления").Column("FORM_TIME").Length(50);
            Property(x => x.FormPlace, "Место составления").Column("FORM_PLACE").Length(500);
            Property(x => x.Persons, "Лица, присутствующий при составлении").Column("PERSONS").Length(500);
        }
    }
}
