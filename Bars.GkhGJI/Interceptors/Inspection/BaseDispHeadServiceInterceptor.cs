namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Utils;

    public class BaseDispHeadServiceInterceptor : BaseDispHeadServiceInterceptor<BaseDispHead>
    {
    }

    public class BaseDispHeadServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseDispHead
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            Utils.CreateInspectionNumber(Container, entity, entity.DispHeadDate.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }

        // Изменения вносить с осторожностью, часть полей нехранимые и заполняются только во View-модели
        // То есть нужно учитывать, что поля будут заполнены только если сущность прилетела от клиента,
        // а не получена через Get или GetAll домен-сервиса 
    }
}
