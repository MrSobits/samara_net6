namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System.Linq;
    using System;
    using Bars.GkhGji.Interceptors;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.SurveyPlan;
    using Bars.GkhGji.Utils;

    public class BaseOMSUServiceInterceptor : BaseOMSUServiceInterceptor<BaseOMSU>
    {
        //Внимание! данный клласс мог служит ьродительским в других регионах
    }

    // Делаю Generic чтобы в других регнионах лучше расширять функционал
    public class BaseOMSUServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseOMSU
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            entity.TypeJurPerson = Gkh.Enums.TypeJurPerson.LocalGovernment;
            entity.ObjectCreateDate = DateTime.Now;
            entity.ObjectEditDate = DateTime.Now;
            entity.ObjectVersion = 1;
            entity.PhysicalPerson = entity.OmsuPerson;
            Utils.CreateInspectionNumber(Container, entity, entity.DateStart.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
          
            entity.ObjectEditDate = DateTime.Now;
        

            return Success(); 
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var serviceInspectionInspectors = Container.Resolve<IDomainService<InspectionGjiInspector>>();

            var serviceInspectionZonalInspection = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();

            try
            {
                if (!string.IsNullOrEmpty(entity.JurPersonInspectors))
                {
                    var inspectorIds = entity.JurPersonInspectors.Split(',');

                    foreach (var id in inspectorIds)
                    {
                        var newId = id.ToLong();

                        var newInspectionInspector = new InspectionGjiInspector
                        {
                            Inspector = new Inspector { Id = newId },
                            Inspection = new InspectionGji { Id = entity.Id }
                        };

                        serviceInspectionInspectors.Save(newInspectionInspector);
                    }
                }

                if (!string.IsNullOrEmpty(entity.JurPersonZonalInspections))
                {
                    var zonalInspectionIds = entity.JurPersonZonalInspections.Split(',');

                    foreach (var id in zonalInspectionIds)
                    {
                        var newId = id.ToLong();

                        var newZonalInspection = new InspectionGjiZonalInspection
                        {
                            ZonalInspection = new ZonalInspection { Id = newId },
                            Inspection = new InspectionGji { Id = entity.Id }
                        };

                        serviceInspectionZonalInspection.Save(newZonalInspection);
                    }
                }

                return base.AfterCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceInspectionInspectors);
                Container.Release(serviceInspectionZonalInspection);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            var surveyPlanContragentDomain = Container.ResolveDomain<SurveyPlanContragent>();
            try
            {
                var contragents = surveyPlanContragentDomain.GetAll().Where(x => x.Inspection.Id == entity.Id);
                foreach (var contragent in contragents)
                {
                    contragent.Inspection = null;
                }
            }
            finally
            {
                Container.Release(surveyPlanContragentDomain);
            }

            return result;
        }
    }
}
