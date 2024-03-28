using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.Period;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class FieldDetailsSteps : BindingBase
    {
        [Then(@"у поля Начислено пени всего есть детальная информация")]
        public void ТоУПоляНачисленоПениВсегоЕстьДетальнаяИнформацияПоНачисленнымПени()
        {
            var fieldDetail =
                Container.Resolve<IPersonalAccountDetailService>()
                    .GetFieldDetail(BasePersonalAccountHelper.Current, "ChargedPenalty")
                    .ToList();

            fieldDetail.Any().Should().BeTrue("у поля Начислено пени всего должна быть детальная информация");

            ScenarioContext.Current.Add("ChargedPenaltyFieldDetail", fieldDetail);
        }

        [Then(@"у поля Задолженность пени всего есть детальная информация")]
        public void ТоУПоляЗадолженностьПениВсегоЕстьДетальнаяИнформацияПоЗадолженностиПениВсего()
        {
            var fieldDetail =
                Container.Resolve<IPersonalAccountDetailService>()
                    .GetFieldDetail(BasePersonalAccountHelper.Current, "DebtPenalty")
                    .ToList();

            fieldDetail.Any().Should().BeTrue("у поля Задолженность пени всего должна быть детальная информация");

            ScenarioContext.Current.Add("DebtPenaltyFieldDetail", fieldDetail);
        }


        [Then(@"у этой детальной информации по начисленным пени всего есть запись по текущему периоду")]
        public void ТоУЭтойДетальнойИнформацииПоНачисленнымПениВсегоЕстьЗаписьПоТекущемуПериоду()
        {
            var fieldDetails = ScenarioContext.Current.Get<IEnumerable<FieldDetail>>("ChargedPenaltyFieldDetail");

            var currentPeriod =
                Container.Resolve<IDomainService<PeriodCloseCheckResult>>().GetAll().FirstOrDefault(x => x.CheckState != PeriodCloseCheckStateType.Pending);

            if (currentPeriod == null)
            {
                throw new NullReferenceException("Отсутствует открытый период");
            }

            fieldDetails.Any(x => x.Period == currentPeriod.Name)
                .Should().BeTrue("у этой детальной информации по начисленным пени всего должна быть запись по текущему периоду");
        }

        [Then(@"у этой детальной информации по задолженности пени всего есть запись по текущему периоду")]
        public void ТоУЭтойДетальнойИнформацииПоЗадолженностиПениВсегоЕстьЗаписьПоТекущемуПериоду()
        {
             var fieldDetails = ScenarioContext.Current.Get<IEnumerable<FieldDetail>>("DebtPenaltyFieldDetail");

            var currentPeriod =
                Container.Resolve<IDomainService<PeriodCloseCheckResult>>().GetAll().FirstOrDefault(x => x.CheckState != PeriodCloseCheckStateType.Pending);

            if (currentPeriod == null)
            {
                throw new NullReferenceException("Отсутствует открытый период");
            }

            fieldDetails.Any(x => x.Period == currentPeriod.Name)
                .Should().BeTrue("у этой детальной информации по задолженности пени всего должна быть запись по текущему периоду");
        }
    }
}
