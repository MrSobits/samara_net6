namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    public class BaseHeatSeasonServiceInterceptor : BaseHeatSeasonServiceInterceptor<BaseHeatSeason>
    {
    }

    public class BaseHeatSeasonServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T : BaseHeatSeason
    {
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            // берем дом из отапливаемого сезона и переносим в проверяемые дома
            var serviceInspObj = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceObj = Container.Resolve<IDomainService<RealityObject>>();
            var ServiceProvider = Container.Resolve<IInspectionGjiProvider>();

            try
            {
                if (entity.HeatingSeason != null && entity.HeatingSeason.RealityObject != null)
                {
                    var newObj = new InspectionGjiRealityObject
                    {
                        Inspection = entity,
                        RealityObject = serviceObj.Load(entity.HeatingSeason.RealityObject.Id)
                    };

                    serviceInspObj.Save(newObj);    
                }
                
                // Необходио для праверки создать дкоумент Распоряжение, это делаем через правило
                ServiceProvider.CreateDocument(entity, TypeDocumentGji.Disposal);

                return base.AfterCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceInspObj);
                Container.Release(serviceObj);
            }
        }

    }
}