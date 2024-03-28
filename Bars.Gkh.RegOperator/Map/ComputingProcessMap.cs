/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class ComputingProcessMap : BaseEntityMap<ComputingProcess>
///     {
///         public ComputingProcessMap()
///             : base("REGOP_COMP_PROC")
///         {
///             Map(x => x.Type, "TYPE");
///             Map(x => x.Status, "STATUS");
///             Map(x => x.TaskId, "TASK_ID");
///             Map(x => x.Name, "NAME");
///             References(x => x.Issuer, "ISSUER");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Вычислительный процесс, запущенный пользователем"</summary>
    public class ComputingProcessMap : BaseEntityMap<ComputingProcess>
    {
        
        public ComputingProcessMap() : 
                base("Вычислительный процесс, запущенный пользователем", "REGOP_COMP_PROC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Type, "Тип процесса").Column("TYPE");
            Property(x => x.Status, "Статус процесса").Column("STATUS");
            Property(x => x.TaskId, "Идентификатор Task, который выполняет процесс").Column("TASK_ID");
            Property(x => x.Name, "Название процесса").Column("NAME");
            Reference(x => x.Issuer, "Пользователь, запустивший процесс").Column("ISSUER");
        }
    }
}
