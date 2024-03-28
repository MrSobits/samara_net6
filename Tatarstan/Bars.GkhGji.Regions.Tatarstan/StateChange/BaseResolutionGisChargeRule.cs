namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public abstract class BaseResolutionGisChargeRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        protected abstract ChargeStatus ChargeStatus { get; } 

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var resolution = statefulEntity as TatarstanResolution;
            TatarstanProtocolMvd protocolMvd = null;

            var protocolMvdDomain = this.Container.Resolve<IDomainService<TatarstanProtocolMvd>>();

            try
            {
                protocolMvd = protocolMvdDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == resolution.Inspection.Id);
            }
            finally
            {
                this.Container.Release(protocolMvdDomain);
            }

            if (resolution == null)
            {
                return ValidateResult.No("Объект не является постановлением");
            }

            if (string.IsNullOrEmpty(resolution.Executant?.Name) && protocolMvd?.TypeExecutant == null)
            {
                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Тип исполнителя»");
            }

            if (resolution.Municipality == null)
            {
                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Местонахождение»");
            }

            var payerType = this.GetPayerType(protocolMvd != null?((int)protocolMvd.TypeExecutant).ToString():resolution.Executant.Code);

            ValidateResult payerValidationResult;
            switch (this.ChargeStatus)
            {
                case ChargeStatus.Send:
                    payerValidationResult = this.PayerValidation(resolution, protocolMvd, payerType);
                    if (!payerValidationResult.Success) return payerValidationResult;
                    break;

                case ChargeStatus.Change:
                    payerValidationResult = this.PayerValidation(resolution, protocolMvd, payerType);
                    if (!payerValidationResult.Success) return payerValidationResult;

                    if (string.IsNullOrWhiteSpace(resolution.ChangeReason))
                    {
                        return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Причина изменения»");
                    }

                    if (string.IsNullOrWhiteSpace(resolution.GisUin))
                    {
                        return ValidateResult.No("Для изменения начисления неободимо отправить начисление в ГИС ГМП");
                    }
                    break;

                case ChargeStatus.Annul:
                    if (string.IsNullOrWhiteSpace(resolution.AbandonReason))
                    {
                        return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Причина аннулирования»");
                    }
                    
                    if (string.IsNullOrWhiteSpace(resolution.GisUin))
                    {
                        return ValidateResult.No("Для аннулирования начисления неободимо отправить начисление в ГИС ГМП");
                    }
                    break;

            }

            var paramService = this.Container.Resolve<IGjiTatParamService>();
            var gisChargeToSendDomain = this.Container.ResolveDomain<GisChargeToSend>();
            using (this.Container.Using(paramService, gisChargeToSendDomain))
            {
                var config = paramService.GetConfig();
                var gisCharge = this.GenerateGisCharge(resolution, protocolMvd, payerType, config);
                gisChargeToSendDomain.Save(gisCharge);
            }
            
            return ValidateResult.Yes();
        }

        private PayerType GetPayerType(string executantCode)
        {
            switch (executantCode)
            {
                case "0":
                case "2":
                case "4":
                case "8":
                case "9":
                case "11":
                case "15":
                case "17":
                case "18":
                case "21":
                    return PayerType.Legal;
                case "1":
                case "3":
                case "6":
                case "5":
                case "7":
                case "16":
                case "10":
                case "12":
                case "13":
                case "14":
                case "19":
                case "20":
                case "22":
                    return PayerType.Individual;

                default:
                    throw new ArgumentException("Неизвестный код исполнителя документа");
            }
        }

        protected abstract GisChargeToSend GenerateGisCharge(TatarstanResolution resolution, TatarstanProtocolMvd protocolMvd, PayerType payerType, DynamicDictionary config);

        protected virtual ValidateResult PayerValidation(TatarstanResolution resolution, TatarstanProtocolMvd protocolMvd, PayerType payerType)
        {
            switch (payerType)
            {
                    case PayerType.Individual:
                        if (protocolMvd != null)
                        {
                            if (string.IsNullOrWhiteSpace(protocolMvd.SurName))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Фамилия»");
                            }

                            if (string.IsNullOrWhiteSpace(protocolMvd.Name))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Имя»");
                            }

                            if (!protocolMvd.BirthDate.HasValue)
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Дата рождения»");
                            }

                            if (string.IsNullOrWhiteSpace(protocolMvd.PhysicalPersonInfo))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Фактический адрес проживания»");
                            }

                            if (string.IsNullOrWhiteSpace(protocolMvd.SerialAndNumber))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Серия и номер паспорта»");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(resolution.SurName))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Фамилия»");
                            }

                            if (string.IsNullOrWhiteSpace(resolution.Name))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Имя»");
                            }

                            if (!resolution.BirthDate.HasValue)
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Дата рождения»");
                            }

                            if (string.IsNullOrWhiteSpace(resolution.Address))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Фактический адрес проживания»");
                            }

                            if (string.IsNullOrWhiteSpace(resolution.SerialAndNumber))
                            {
                                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Серия и номер паспорта»");
                            }
                        }
                        break;

                    case PayerType.Legal:
                        if (resolution.Contragent == null)
                        {
                            return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Контрагент»");
                        }
                        break;
            }

            return ValidateResult.Yes();
        }
    }
}