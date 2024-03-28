using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.Map
{
    public class PSDWorksMap : BaseEntityMap<PSDWorks>
    {
        public PSDWorksMap() :  base("Связка работ и их псд работ", "CR_OBJ_PSD_WORK")
        {            
        }

        protected override void Map()
        {
            Reference(x => x.PSDWork, "ПСД работа").Column("PSDWORK_ID").Fetch();
            Property(x => x.Cost, "Сумма").Column("COST");
            Reference(x => x.Work, "Работа").Column("WORK_ID").Fetch();
        }
    }
}
