namespace Bars.Gkh.Interceptors.Dict
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Перехватчик операций с БД для объекта Работы и услуги
    /// </summary>
    public class WorkInterceptor : EmptyDomainInterceptor<Work>
    {
        /// <summary>
        /// Метод до создания
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeCreateAction(IDomainService<Work> service, Work entity)
        {
            return this.ValidateEntity(entity);
        }

        /// <summary>
        /// Метод до изменения
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<Work> service, Work entity)
        {
            return this.ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(Work entity)
        {
            if (entity.Normative < 0)
            {
                return this.Failure("Значение поля Норматив должно быть неотрицательным");
            }

            if (entity.Description.IsNotEmpty() && entity.Description.Length > 500)
            {
                return this.Failure("Количество знаков в поле Описание не должно превышать 500 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 10)
            {
                return this.Failure("Количество знаков в поле Код не должно превышать 10 символов");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return this.Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            var emptyFields = this.GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return this.Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return this.Success();
        }

        private string GetEmptyFields(Work entity)
        {
            List<string> fieldList = new List<string>();
            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.Code.IsEmpty())
            {
                fieldList.Add("Код");
            }

            if (entity.UnitMeasure == null)
            {
                fieldList.Add("Единица измерения");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}