namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class MeteringDeviceServiceInterceptor : EmptyDomainInterceptor<MeteringDevice>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MeteringDevice> service, MeteringDevice entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MeteringDevice> service, MeteringDevice entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MeteringDevice> service, MeteringDevice entity)
        {
            if (Container.Resolve<IDomainService<RealityObjectMeteringDevice>>().GetAll().Any(x => x.MeteringDevice.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Приборы учета жилого дома;");
            }

            return Success();
        }

        private IDataResult CheckForm(MeteringDevice entity)
        {
            if (entity.AccuracyClass.IsNotEmpty() && entity.AccuracyClass.Length > 30)
            {
                return Failure("Количество знаков в поле Класс точности не должно превышать 30 символов");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Description.IsNotEmpty() && entity.Description.Length > 1000)
            {
                return Failure("Количество знаков в поле Описание не должно превышать 1000 символов");
            }

            var emptyFields = GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(MeteringDevice entity)
        {
            List<string> fieldList = new List<string>();
            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.AccuracyClass.IsEmpty())
            {
                fieldList.Add("Класс точности");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}
