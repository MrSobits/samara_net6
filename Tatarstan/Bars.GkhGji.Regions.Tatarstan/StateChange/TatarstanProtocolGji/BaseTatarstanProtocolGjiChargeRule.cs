namespace Bars.GkhGji.Regions.Tatarstan.StateChange.TatarstanProtocolGji
{
    using System.Globalization;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Базовое правило для формирования файла начисления для ГИС ГМП
    /// </summary>
    public abstract class BaseTatarstanProtocolGjiChargeRule : TatarstanProtocolGjiValidationRule
    {
        /// <summary>
        /// Тип отправки начисления
        /// </summary>
        public abstract ChargeStatus ChargeStatus { get; }

        /// <inheritdoc/>
        protected override ValidateResult AfterValidationAction(TatarstanProtocolGjiContragent protocol)
        {
            if (protocol.Pattern == null)
            {
                return ValidateResult.No("Не заполнено поле \"Шаблон ГИС ГМП\"");
            }

            var gisChargeToSendDomain = this.Container.ResolveDomain<GisChargeToSend>();

            using (this.Container.Using(gisChargeToSendDomain))
            {
                var gisChargeToSend = new GisChargeToSend
                {
                    Document = protocol,
                    JsonObject = new GisChargeJson
                    {
                        PatternCode = protocol.Pattern.PatternCode,
                        BillDate = protocol.DocumentDate?.ToString("dd.MM.yyyy HH:mm:ss"),
                        TotalAmount = protocol.PenaltyAmount?.ToString("0.##", new NumberFormatInfo { NumberDecimalSeparator = "." }) ?? "0",
                        BillFor = protocol.Pattern.PatternName,
                        Details = "Оплата административного штрафа по протоколу \"{0}\" \"{1}\""
                            .FormatUsing(protocol.DocumentNumber, protocol.DocumentDate.ToDateString()),
                        Payer = this.CreateChargePayer(protocol),
                        Oktmo = protocol.Municipality.Return(x => x.Oktmo).ToStr()
                    }
                };

                this.SetDataByChargeStatus(gisChargeToSend, protocol);

                gisChargeToSendDomain.Save(gisChargeToSend);
            }

            return ValidateResult.Yes();
        }

        private GisChargeJsonPayer CreateChargePayer(TatarstanProtocolGjiContragent protocol)
        {
            var executant = protocol.Executant;
            var chargePayer = new GisChargeJsonPayer();

            switch (executant)
            {
                case TypeDocObject.Individual:
                case TypeDocObject.Official:
                    chargePayer.PayerType = "1";
                    chargePayer.PayerSurname = protocol.SurName;
                    chargePayer.PayerName = protocol.Name;
                    chargePayer.PayerPatronymic = protocol.Patronymic;
                    chargePayer.PayerBirthday = protocol.BirthDate?.ToShortDateString();
                    chargePayer.PayerAddress = protocol.Address;
                    chargePayer.PayerDoctype = protocol.IdentityDocumentType?.Code;
                    chargePayer.PayerDocNumber = protocol.SerialAndNumberDocument;
                    chargePayer.PayerDocnat = protocol.CitizenshipType == CitizenshipType.Other
                        ? protocol.Citizenship?.OksmCode.ToString()
                        : "643";
                    break;
                case TypeDocObject.Legal:
                    chargePayer.PayerType = "2";
                    chargePayer.PayerCode = protocol.Contragent?.ShortName;
                    chargePayer.PayerCaption = protocol.Contragent?.Name;
                    chargePayer.PayerDocNumber = protocol.Contragent?.Inn;
                    chargePayer.PayerKpp = protocol.Contragent?.Kpp;
                    break;
                case TypeDocObject.Entrepreneur:
                    chargePayer.PayerType = "4";
                    chargePayer.PayerSurname = protocol.SurName;
                    chargePayer.PayerName = protocol.Name;
                    chargePayer.PayerPatronymic = protocol.Patronymic;
                    chargePayer.PayerDocNumber = protocol.Contragent?.Inn;
                    break;
            }

            return chargePayer;
        }

        private void SetDataByChargeStatus(GisChargeToSend gisCharge, TatarstanProtocolGjiContragent protocol)
        {
            var jsonObject = gisCharge.JsonObject;

            jsonObject.ChargeStatus = this.ChargeStatus.ToString("d");

            switch (this.ChargeStatus)
            {
                case ChargeStatus.Change:
                    jsonObject.SupplierBillId = protocol.GisUin;
                    jsonObject.OperationName = protocol.UpdateReason;
                    break;
                case ChargeStatus.Annul:
                    jsonObject.SupplierBillId = protocol.GisUin;
                    jsonObject.OperationName = protocol.AnnulReason;
                    break;
            }
        }
    }
}