namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    public class BaseActivityTsjInterceptor : BaseActivityTsjInterceptor<BaseActivityTsj>
    {
    }

    public class BaseActivityTsjInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseActivityTsj
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // из деятельности ТСЖ берем контрагента и сохраняем его в основание проверки
            if (entity.ActivityTsj.ManagingOrganization == null)
            {
                return this.Failure("Не выбрана ТСЖ");
            }

            entity.TypeJurPerson = TypeJurPerson.ManagingOrganization;
            entity.Contragent = entity.ActivityTsj.ManagingOrganization.Contragent;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
#warning Данный код работает только при сохранении с клиент. Тоесть добавили в Entity не хранимое поле и в Интерцепторе предполагают что оно должно прийти
#warning Данный код не сработает когда будет например Перевод Статуса которая не склиента запускается а ссервера

            // получаем сущности домов из списка id домов 
            var serviceRobjects = Container.Resolve<IDomainService<RealityObject>>();
            var serviceObj = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var ServiceProvider = Container.Resolve<IInspectionGjiProvider>();

            try
            {
                if (entity.RealityObjects.Any())
                {
                    var realityObjectList = serviceRobjects.GetAll().Where(x => entity.RealityObjects.Contains(x.Id)).Select(x => x.Id).ToList();

                    // берем дом из деятельности тсж и переносим в проверяемые дома
                    foreach (var objId in realityObjectList)
                    {
                        var newObj = new InspectionGjiRealityObject
                        {
                            Inspection = entity,
                            RealityObject = serviceRobjects.Load(objId)
                        };

                        serviceObj.Save(newObj);
                    }     
                }
               
                // Необходио для праверки создать дкоумент Распоряжение, это делаем через правило
                ServiceProvider.CreateDocument(entity, TypeDocumentGji.Disposal);

                return base.AfterCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceRobjects);
                Container.Release(serviceObj);
                Container.Release(ServiceProvider);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {

            if (service.GetAll().Any(x => x.ActivityTsj.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Документы в деятельности ТСЖ;");
            }

            return base.BeforeDeleteAction(service, entity);
        }

    }
}