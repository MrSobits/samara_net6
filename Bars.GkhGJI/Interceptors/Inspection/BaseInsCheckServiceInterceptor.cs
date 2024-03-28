namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Utils;

    public class BaseInsCheckServiceInterceptor : BaseInsCheckServiceInterceptor<BaseInsCheck>
    {
    }

    public class BaseInsCheckServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseInsCheck
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            Utils.CreateInspectionNumber(Container, entity, entity.InsCheckDate.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {

#warning Данное говно работает только при сохранении с клиент. Тоест ьдобавили в Entity не хранимое поле и в Интерцепторе предполагают что оно должно прийти
#warning Данная херня не сработает когда будет например Перевод Статуса которая не склиента запускается а ссервера

            // Специально для этого типа Проверки делаем при Обновлении создание Инспектора и Дома проверки
            // Поскольку Инспектора и проверяемые дома - это субтаблицы но мы сделали обман зрения для пользователей
            // подсунули выбор одного инчпектора и одного дома и они приходят именно при сохранении все сущности проверки
            // Поэтому для верности сначала удаляем инспекторов и дома по этой проверки а затем добавляем переданные InspectorId и RealityObjectId

            var domainServiceObject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var domainServiceInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();

            try
            {
                // теперь то что пришло из модели мы добавляем 
                if (entity.RealityObjectId > 0)
                {
                    // Удаляем все дочерние Проверяемые дома
                    var objectIds = domainServiceObject.GetAll().Where(x => x.Inspection.Id == entity.Id)
                        .Select(x => x.Id).ToList();

                    foreach (var objId in objectIds)
                    {
                        domainServiceObject.Delete(objId);
                    }

                    var newRealObj = new InspectionGjiRealityObject
                    {
                        Inspection = entity,
                        RealityObject = new RealityObject { Id = entity.RealityObjectId.ToLong() }
                    };

                    domainServiceObject.Save(newRealObj);
                }

                if (entity.InspectorId > 0)
                {
                    // Удаляем всех дочерних инспекторов
                    var inspectorIds = domainServiceInspector.GetAll().Where(x => x.Inspection.Id == entity.Id)
                        .Select(x => x.Id).ToList();

                    foreach (var insId in inspectorIds)
                    {
                        domainServiceInspector.Delete(insId);
                    }

                    var newInspector = new InspectionGjiInspector
                    {
                        Inspection = entity,
                        Inspector = new Inspector { Id = entity.InspectorId.ToLong() }
                    };

                    domainServiceInspector.Save(newInspector);
                }

                return this.Success();
            }
            finally
            {
                Container.Release(domainServiceObject);
                Container.Release(domainServiceInspector);
            }
        }
        
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
#warning Данное говно работает только при сохранении с клиент. Тоест ьдобавили в Entity не хранимое поле и в Интерцепторе предполагают что оно должно прийти
#warning Данная херня не сработает когда будет например Перевод Статуса которая не склиента запускается а ссервера

            var serviceRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            try
            {
                if (entity.RealityObjectId > 0)
                {
                    var newInspectionReaityObj = new InspectionGjiRealityObject
                    {
                        RealityObject = new RealityObject { Id = entity.RealityObjectId.ToLong() },
                        Inspection = new InspectionGji { Id = entity.Id }
                    };

                    serviceRo.Save(newInspectionReaityObj);
                }

                return base.AfterCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceRo);
            }
        }
    }
}
