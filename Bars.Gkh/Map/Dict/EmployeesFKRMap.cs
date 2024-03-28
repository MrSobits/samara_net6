
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class EmployeesFKRMap : BaseEntityMap<EmployeesFKR>
    {
        
        public EmployeesFKRMap() : 
                base("Сотрудники ФКР", "GKH_DICT_EMPLOYESS_FKR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "ФИО").Column("NAME").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(500);
            Property(x => x.Departament, "Отдел").Column("DEPARTAMENT").Length(500);
        }
    }
}
