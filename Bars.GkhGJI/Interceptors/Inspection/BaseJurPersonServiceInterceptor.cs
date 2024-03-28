namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.SurveyPlan;
    using Bars.GkhGji.Utils;

    public class BaseJurPersonServiceInterceptor : BaseJurPersonServiceInterceptor<BaseJurPerson>
    {
        //Внимание! данный клласс мог служит ьродительским в других регионах
    }

    // Делаю Generic чтобы в других регнионах лучше расширять функционал
    public class BaseJurPersonServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseJurPerson
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            Utils.CreateInspectionNumber(this.Container, entity, entity.DateStart.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var serviceInspectionInspectors = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();

            var serviceInspectionZonalInspection = this.Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();

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
                this.Container.Release(serviceInspectionInspectors);
                this.Container.Release(serviceInspectionZonalInspection);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            var surveyPlanContragentDomain = this.Container.ResolveDomain<SurveyPlanContragent>();
            var jurPersonContragentDomain = this.Container.ResolveDomain<BaseJurPersonContragent>();

            using (this.Container.Using(surveyPlanContragentDomain, jurPersonContragentDomain))
            { 
                var contragents = surveyPlanContragentDomain.GetAll().Where(x => x.Inspection.Id == entity.Id);
                foreach (var contragent in contragents)
                {
                    contragent.Inspection = null;
                }

                jurPersonContragentDomain.GetAll().Where(x => x.BaseJurPerson.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => jurPersonContragentDomain.Delete(x));
            }

            return result;
        }
    }
}
