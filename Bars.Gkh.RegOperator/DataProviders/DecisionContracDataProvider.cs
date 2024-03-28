namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для печати договора протокола решений
    /// </summary>
    public class DecisionContracDataProvider : BaseCollectionDataProvider<DecisionContract>
    {
        public DecisionContracDataProvider(IWindsorContainer container) : base(container)
        {
        }

        public override string Name
        {
            get { return "Договор протокола решений"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public long DecisionProtocolId { get; set; }

        protected override IQueryable<DecisionContract> GetDataInternal(BaseParams baseParams)
        {
            var realObjDecProtDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var regOperatorDomain = Container.ResolveDomain<RegOperator>();
            var regOpCalcAccDomain = Container.ResolveDomain<RegopCalcAccount>();
            var regOpCalcAccRoDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            try
            {
                var protocol = realObjDecProtDomain.FirstOrDefault(x => x.Id == DecisionProtocolId);
                if (protocol == null)
                {
                    throw new Exception("Не найден протокол решений");
                }

                var realObj = protocol.RealityObject;

                var records = new List<DecisionContract>();

                var record = new DecisionContract
                {
                    HomeAddress = realObj.Address
                };

                var regOperator =
                    regOperatorDomain.GetAll()
                        .Where(x => x.Contragent != null)
                        .FirstOrDefault(x => x.Contragent.ContragentState == ContragentState.Active);

                if (regOperator != null)
                {
                    record.Contragent = regOperator.Contragent.Name;
                    record.JuridicalAddress = regOperator.Contragent.FiasJuridicalAddress.AddressName;
                    record.FactAddress = regOperator.Contragent.FactAddress;
                    record.Email = regOperator.Contragent.Email;
                    record.InnKpp = string.Format(
                        "{0}/{1}",
                        regOperator.Contragent.Inn,
                        regOperator.Contragent.Kpp);
                    record.Ogrn = regOperator.Contragent.Ogrn;

                    var regOpCalcAccRo = regOpCalcAccRoDomain.GetAll()
                        .Where(x => x.Account.AccountOwner.Id == regOperator.Contragent.Id)
                        .FirstOrDefault(x => x.RealityObject.Id == realObj.Id);

                    if (regOpCalcAccRo != null)
                    {
                        var regopacc = regOpCalcAccDomain.GetAll().FirstOrDefault(x => x.Id == regOpCalcAccRo.Account.Id);

                        if (regopacc != null)
                        {
                            record.AccNumber = regopacc.ContragentCreditOrg.SettlementAccount;
                            if (regopacc.CreditOrg != null)
                            {
                                record.CreditOrg = regopacc.CreditOrg.Name;
                            }
                            record.Bik = regopacc.ContragentCreditOrg.Bik;
                            record.CorAcc = regopacc.ContragentCreditOrg.CorrAccount;
                        }
                    }
                }

                records.Add(record);

                return records.AsQueryable();
            }
            finally
            {
                Container.Release(realObjDecProtDomain);
                Container.Release(regOperatorDomain);
                Container.Release(regOpCalcAccDomain);
                Container.Release(regOpCalcAccRoDomain);
            }
        }
    }
}