namespace Bars.GkhGji.Regions.Tatarstan.StateChange.TatarstanProtocolGji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило для проверки заполненности карточки протокола
    /// </summary>
    public class TatarstanProtocolGjiValidationRule : IRuleChangeStatus
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор правила
        /// </summary>
        public virtual string Id => "gji_document_protocol_gji_rt_validation_rule";

        /// <summary>
        /// Тип статусной сущности     
        /// </summary>
        public virtual string TypeId => "gji_document_protocol_gji_rt";

        /// <summary>
        /// Название правила
        /// </summary>
        public virtual string Name => "Проверка заполненности карточки протокола";

        /// <summary>
        /// Описание правила
        /// </summary>
        public virtual string Description => "Данное правило проверяет заполненность обязательных полей протокола";

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="statefulEntity">Сущность</param>
        /// <param name="oldState">Старый статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns>Результат валидации</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var protocol = statefulEntity as TatarstanProtocolGjiContragent;

            if (protocol != null)
            {
                var protocolValidationResult = this.ValidateProtocol(protocol);
                if (!protocolValidationResult.Success)
                {
                    return protocolValidationResult;
                }

                var afterValidationActionResult = this.AfterValidationAction(protocol);
                if (!afterValidationActionResult.Success)
                {
                    return afterValidationActionResult;
                }
            }

            return ValidateResult.Yes();
        }

        protected virtual ValidateResult AfterValidationAction(TatarstanProtocolGjiContragent protocol)
        {
            return ValidateResult.Yes();
        }

        private ValidateResult ValidateProtocol(TatarstanProtocolGjiContragent protocol)
        {
            var inspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();

            using (this.Container.Using(inspectorDomain))
            {
                var emptyFields = new List<string>();

                emptyFields.AddRange(this.GetEmptyFields(protocol, new Dictionary<string, string>
                {
                    ["DocumentDate"] = "Дата",
                    ["DocumentNumber"] = "Номер документа",
                    ["Municipality"] = "Муниципальное образование",
                    ["ZonalInspection"] = "Орган ГЖИ, оформивший протокол",
                    ["DateSupply"] = "Дата поступления в ГЖИ",
                    ["DateOffense"] = "Дата правонарушения",
                    ["TimeOffense"] = "Время правонарушения"
                }));

                var inspectorsCount = inspectorDomain.GetAll().Count(x => x.DocumentGji.Id == protocol.Id);
                if (inspectorsCount == 0)
                {
                    emptyFields.Add("Инспекторы");
                }

                if (protocol.Executant == default(TypeDocObject))
                {
                    emptyFields.Add("Тип исполнителя");
                }
                else if (protocol.Executant == TypeDocObject.Legal || protocol.Executant == TypeDocObject.Entrepreneur)
                {
                    if (protocol.Contragent == null)
                    {
                        emptyFields.Add("Контрагент");
                    }
                    else
                    {
                        emptyFields.AddRange(this.GetEmptyFields(protocol.Contragent, new Dictionary<string, string>
                        {
                            ["Ogrn"] = "ОГРН",
                            ["Inn"] = "ИНН"
                        }));
                    }
                }

                if (emptyFields.Count > 0)
                {
                    return ValidateResult.No($"Не заполнены обязательные поля:<br><b>{emptyFields.AggregateWithSeparator("<br>")}</b>");
                }
            }

            return ValidateResult.Yes();
        }

        private List<string> GetEmptyFields<T>(T entity, Dictionary<string, string> propertyDict)
        {
            var objType = typeof(T);
            var objProperties = objType.GetProperties()
                .Where(x => propertyDict.Keys.Contains(x.Name));

            var emptyProperties = new List<string>();

            foreach (var property in objProperties)
            {
                var value = property.GetValue(entity);
                if (this.IsEmpty(value, property.PropertyType))
                {
                    emptyProperties.Add(propertyDict.Get(property.Name));
                }
            }

            return emptyProperties;
        }

        private bool IsEmpty(object value, Type type)
        {
            var isEmpty = false;

            if (value == null)
            {
                isEmpty = true;
            }
            else if (type == typeof(DateTime))
            {
                isEmpty = !((DateTime)value).IsValid();
            }
            else if (type == typeof(string))
            {
                isEmpty = string.IsNullOrWhiteSpace((string)value);
            }

            return isEmpty;
        }
    }
}