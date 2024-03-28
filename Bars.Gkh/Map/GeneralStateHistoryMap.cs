namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    public class GeneralStateHistoryMap : GkhBaseEntityMap<GeneralStateHistory>
    {

        public GeneralStateHistoryMap() : base("GKH_GENERAL_STATE_HISTORY")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ChangeDate, "Дата изменения").Column("CHANGE_DATE").NotNull();
            this.Property(x => x.EntityId, "Id сущности").Column("ENTITY_ID").NotNull();
            this.Property(x => x.EntityType, "Тип сущности").Column("ENTITY_TYPE").Length(200).NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").Length(200).NotNull();
            this.Property(x => x.StartState, "Исходное состояние").Column("START_STATE").Length(200);
            this.Property(x => x.FinalState, "Финальное состояние").Column("FINAL_STATE").Length(200).NotNull();
            this.Property(x => x.UserName, "Имя пользователя").Column("USER_NAME").Length(100);
            this.Property(x => x.UserLogin, "Логин пользователя").Column("USER_LOGIN").Length(100);
        }
    }
}