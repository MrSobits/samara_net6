namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class PersonalAccountOpenCloseDatesMap : VersionedEntity<BasePersonalAccount>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountOpenCloseDatesMap()
            : base(null)
        {
            this.Map(x => x.OpenDate, VersionedParameters.PersonalAccountOpenDate, "Дата открытия ЛС");
            this.Map(
                x => x.CloseDate,
                VersionedParameters.PersonalAccountCloseDate,
                "Дата закрытия ЛС",
                x => x.CloseDate <= DateTime.MinValue);
        }

        /// <summary>
        /// Интерфейс сервиса лицевых счетов
        /// </summary>
        public IPersonalAccountOperationService OperationService { get; set; }

        /// <summary>
        /// Интерфейс создания отсечек перерасчета для ЛС
        /// </summary>
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }
        
        /// <inheritdoc />
        protected override bool CanValidate(string parameterName, object entity)
        {
            if (entity is BasePersonalAccount)
            {
                return true;
            }

            return base.CanValidate(parameterName, entity);
        }

        /// <inheritdoc />
        protected override IDataResult ValidateInternal(object entity, object value, DateTime factDate, string parameterName)
        {
            var acc = entity as BasePersonalAccount;

            if (acc != null && parameterName == VersionedParameters.PersonalAccountCloseDate && acc.State.Code == "1")
            {
                // Вызвать закрытие лс

                var baseParams = new BaseParams
                {
                    Params = new DynamicDictionary {{"accId", acc.Id}, {"closeDate", value}}
                };

                var result = this.OperationService.ClosePersonalAccount(baseParams);

                if (!result.Success)
                {
                    return new BaseDataResult
                    {
                        Success = result.Success,
                        Message = result.Message
                    };
                }
            }

            if (acc != null && parameterName == VersionedParameters.PersonalAccountOpenDate && value.ToDateTime() > acc.OpenDate)
            {
                this.RecalcEventManager.CreateChargeEvent(acc, acc.OpenDate, RecalcEventType.ChangeOpenDate, "Смена даты открытия");
                this.RecalcEventManager.SaveEvents();
            }

            return base.ValidateInternal(entity, value, factDate, parameterName);
        }
    }
}