namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерцептор для сущности <see cref="Inspector"/>
    /// </summary>
    public class InspectorInterceptor : EmptyDomainInterceptor<Inspector>
    {
        /// <summary>
        /// Метод вызываемый до добавления сущности
        /// </summary>
        /// <param name="service">Доменный сервис</param>
        /// <param name="entity">Добавляемая сущность</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeCreateAction(IDomainService<Inspector> service, Inspector entity)
        {
            if (entity.InspectorPosition != null)
            {
                entity.Position = entity.InspectorPosition.Name;
            }
            
            return CheckForm(entity);
        }

        /// <summary>
        /// Метод вызываемый до обновления сущности
        /// </summary>
        /// <param name="service">Доменный сервис</param>
        /// <param name="entity">Обновляемая сущность</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<Inspector> service, Inspector entity)
        {
            if (entity.InspectorPosition != null)
            {
                entity.Position = entity.InspectorPosition.Name;
            }
            
            return CheckForm(entity);
        }


        /// <summary>
        /// Метод вызываемый до удаления сущности
        /// </summary>
        /// <param name="service">Доменный сервис</param>
        /// <param name="entity">Удаляемая сущность</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<Inspector> service, Inspector entity)
        {
            var operatorInspector = Container.Resolve<IDomainService<Operator>>().GetAll().Count(x => x.Inspector.Id == entity.Id);

            if (operatorInspector >= 1)
            {
                return Failure("Существуют связанные записи в таблице: Операторы");
            }
            
            return Success();
        }

        /// <summary>
        /// Метод выполняющий валидацию сущности <see cref="Inspector"/>
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат выполнения запроса</returns>
        private IDataResult CheckForm(Inspector entity)
        {
            if (entity.Fio.IsEmpty())
            {
                return Failure("Поле ФИО должно быть заполнено");
            }

            if (entity.Fio.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО не должно превышать 300 символов");
            }

            if (entity.Code.IsEmpty())
            {
                return Failure("Поле Код должно быть заполнено");
            }

            if (entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            if (entity.ShortFio.IsNotEmpty() && entity.ShortFio.Length > 100)
            {
                return Failure("Количество знаков в поле Фамилия и инициалы не должно превышать 100 символов");
            }

            if (entity.Position.IsNotEmpty() && entity.Position.Length > 300)
            {
                return Failure("Количество знаков в поле Должность не должно превышать 300 символов");
            }

            if (entity.PositionAccusative.IsNotEmpty() && entity.PositionAccusative.Length > 300)
            {
                return Failure("Количество знаков в поле Должность в винительном падеже не должно превышать 300 символов");
            }

            if (entity.PositionDative.IsNotEmpty() && entity.PositionDative.Length > 300)
            {
                return Failure("Количество знаков в поле Должность в дательном падеже не должно превышать 300 символов");
            }

            if (entity.PositionPrepositional.IsNotEmpty() && entity.PositionPrepositional.Length > 300)
            {
                return Failure("Количество знаков в поле Должность в предложном падеже не должно превышать 300 символов");
            }

            if (entity.PositionGenitive.IsNotEmpty() && entity.PositionGenitive.Length > 300)
            {
                return Failure("Количество знаков в поле Должность в родительном падеже не должно превышать 300 символов");
            }

            if (entity.PositionAblative.IsNotEmpty() && entity.PositionAblative.Length > 300)
            {
                return Failure("Количество знаков в поле Должность в творительном падеже не должно превышать 300 символов");
            }

            if (entity.FioAccusative.IsNotEmpty() && entity.FioAccusative.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО в винительном падеже не должно превышать 300 символов");
            }

            if (entity.FioDative.IsNotEmpty() && entity.FioDative.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО в дательном падеже не должно превышать 300 символов");
            }

            if (entity.FioPrepositional.IsNotEmpty() && entity.FioPrepositional.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО в предложном падеже не должно превышать 300 символов");
            }

            if (entity.FioGenitive.IsNotEmpty() && entity.FioGenitive.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО в родительном падеже не должно превышать 300 символов");
            }

            if (entity.FioAblative.IsNotEmpty() && entity.FioAblative.Length > 300)
            {
                return Failure("Количество знаков в поле ФИО в творительном падеже не должно превышать 300 символов");
            }

            if (entity.Phone.IsNotEmpty() && entity.Phone.Length > 300)
            {
                return Failure("Количество знаков в поле Телефон не должно превышать 300 символов");
            }

            if (entity.Email.IsNotEmpty())
            {
                if (entity.Email.Length > 100)
                {
                    return Failure("Количество знаков в поле Электронная почта не должно превышать 100 символов");
                }

                if (!Utils.VerifyMail(entity.Email))
                {
                    return Failure("Поле Электронная почта не удовлетворяет формату");
                }
            }

            return Success();
        }
    }
}
