namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using System.Globalization;
    using B4.Utils;
    
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Entities;
    using Gkh.Utils;

    public class ResolutionGisChargeChangeRule : BaseResolutionGisChargeRule
    {
        public override string Id => "gji_resolution_gis_charge_change_rule";

        public override string Name => "Постановление - Проверка заполненности поля \"Причина изменения\"";

        public override string TypeId => "gji_document_resol";

        public override string Description => "Проверяет заполненность поля \"Причина изменения\" и формирует файл с начислением для ГИС ГМП";

        protected override ChargeStatus ChargeStatus => ChargeStatus.Change;

        /// <inheritdoc />
        protected override GisChargeToSend GenerateGisCharge(TatarstanResolution resolution, TatarstanProtocolMvd protocolMvd, PayerType payerType, DynamicDictionary config)
        {
            return new GisChargeToSend
            {
                Document = resolution,
                JsonObject = new GisChargeJson
                    {
                        PatternCode = resolution.PatternDict?.PatternCode ?? config.GetAs<string>("GisGmpPatternCode"),
                        BillDate = resolution.DocumentDate.ToDateString() + resolution.DocumentDate.ToTimeString(" HH:mm:ss"),
                        TotalAmount = resolution.PenaltyAmount.ToDecimal().ToString(
                            "##.00",
                            new NumberFormatInfo
                            {
                                NumberDecimalSeparator = "."
                            }),
                        SupplierBillId = resolution.GisUin,
                        BillFor = resolution.PatternDict?.PatternName,
                        ChargeStatus = this.ChargeStatus.ToString("D"),
                        Details =
                            "Оплата административного штрафа по постановлению \"{0}\" \"{1}\""
                                .FormatUsing(resolution.DocumentNumber, resolution.DocumentDate.ToDateString()),
                        Payer = payerType == PayerType.Individual
                            ?new GisChargeJsonPayer
                            {
                                PayerType = payerType.ToString("D"),
                                PayerSurname = protocolMvd?.SurName ?? resolution.SurName,
                                PayerName = protocolMvd?.Name ?? resolution.Name,
                                PayerPatronymic = protocolMvd?.Patronymic ?? resolution.Patronymic ?? "",
                                PayerBirthday = protocolMvd?.BirthDate?.ToShortDateString() ?? resolution.BirthDate?.ToShortDateString(),
                                PayerAddress = protocolMvd?.PhysicalPersonInfo ?? resolution.Address,
                                PayerDocNumber = protocolMvd?.SerialAndNumber ?? resolution.SerialAndNumber,
                                PayerDocnat = protocolMvd?.Citizenship?.OksmCode.ToString() ?? resolution.Citizenship?.OksmCode.ToString() ?? "643",
                                PayerDoctype = "01"
                            }
                            :new GisChargeJsonPayer
                            {
                                PayerType = payerType.ToString("D"),
                                PayerDocNumber = resolution.Contragent.Return(x => x.Inn),
                                PayerKpp = resolution.Contragent.Return(x => x.Kpp),
                                PayerCode = resolution.Contragent.Return(x => x.ShortName),
                                PayerCaption = resolution.Contragent.Return(x => x.Name)

                            },
                        BudgetIndex = new GisChargeBudgetIndex
                        {
                            Status = "01"
                        },
                        OperationName = resolution.ChangeReason,
                        Oktmo = resolution.Municipality.Return(x => x.Oktmo).ToStr()
                    }
            };
        }
    }
}