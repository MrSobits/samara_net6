namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Вьюха на сметный расчет объекта КР"</summary>
    public class ViewSpecialObjectCrEstimateCalcDetailMap : PersistentObjectMap<ViewSpecialObjCrEstimateCalcDetail>
    {
        public ViewSpecialObjectCrEstimateCalcDetailMap() : 
                base("Вьюха на сметный расчет объекта КР", "VIEW_CR_SPECIAL_OBJ_EST_CALC_DET")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ObjectCrId, "Объект кап ремонта").Column("OBJECT_ID");
            this.Property(x => x.WorkName, "Наименование работы").Column("WORK_NAME");
            this.Property(x => x.FinSourceName, "Наименование источника финансировния").Column("FIN_SOURCE_NAME");
            this.Property(x => x.SumEstimate, "Сумма по сметам").Column("SUM_ESTIMATE");
            this.Property(x => x.SumResource, "Сумма по ведомостям ресурсов").Column("SUM_RESOURCE");
            this.Property(x => x.TotalEstimate, "Итоги по смете").Column("TOTAL_ESTIMATE");
            this.Property(x => x.EstimationType, "Тип сметы").Column("ESTIMATION_TYPE");

            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
