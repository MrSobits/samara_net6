namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class UnitMeasureInterceptor : EmptyDomainInterceptor<UnitMeasure>
    {
        public override IDataResult BeforeCreateAction(IDomainService<UnitMeasure> service, UnitMeasure entity)
        {
            return CheckUnitMeasure(entity, ServiceOperationType.Save);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<UnitMeasure> service, UnitMeasure entity)
        {
            return CheckUnitMeasure(entity, ServiceOperationType.Update);
        }

        private IDataResult CheckUnitMeasure(UnitMeasure entity, ServiceOperationType operationType)
        {
            if (entity.ShortName.IsNull())
            {
                return Failure("Поле 'Краткое наименование' не должно быть пустым");
            }

            if (entity.ShortName.Length > 20)
            {
                return Failure("Количество знаков в поле 'Краткое наименование' не должно превышать 20 символов");
            }

            if (entity.Name.IsNull())
            {
                return Failure("Поле 'Наименование' не должно быть пустым");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле 'Наименование' не должно превышать 300 символов");
            }

            if (entity.Description.IsNull())
            {
                return Failure("Поле 'Описание' не должно быть пустым");
            }

            if (entity.Description.Length > 500)
            {
                return Failure("Количество знаков в поле 'Описание' не должно превышать 500 символов");
            }

            if (entity.OkeiCode?.Length > 10)
            {
                return Failure("Количество знаков в поле 'Код ОКЕИ' не должно превышать 10 символов");
            }

            return Success();
        }
    }
}
