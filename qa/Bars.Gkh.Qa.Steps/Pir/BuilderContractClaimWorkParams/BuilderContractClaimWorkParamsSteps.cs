namespace Bars.Gkh.Qa.Steps.Pir.BuilderContractClaimWorkParams
{
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class BuilderContractClaimWorkParamsSteps : BindingBase
    {
        private IGkhParamService service = Container.Resolve<IGkhParamService>();

        [Given(@"пользователь указывает в настройках реестра подрядчиков в поле Акт выявления нарушений значение ""(.*)""")]
        public void ДопустимПользовательУказываетВНастройкахРеестраПодрядчиковВПолеАктВыявленияНарушенийЗначение(
            string notifFormationKindExternalValue)
        {
            int notifFormationKindInternalValue;

            switch (notifFormationKindExternalValue)
            {
                case "Формировать":
                    {
                        notifFormationKindInternalValue = 1;
                        break;
                    }

                case "Не формировать":
                    {
                        notifFormationKindInternalValue = 0;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format(
                            "отсутствует тип Акта выявления нарушений с наименованием {0}",
                            notifFormationKindExternalValue));
                    }
            }

            this.service.SaveParams(
                "BuilderContractClaimWork.ViolationNotification.ViolActFormationKind", 
                notifFormationKindInternalValue);
        }

        [Given(@"пользователь указывает в настройках реестра подрядчиков в поле Документ уведомления значение ""(.*)""")]
        public void ДопустимПользовательУказываетВНастройкахРеестраПодрядчиковВПолеДокументУведомленияЗначение(
            string notifFormationKindExternalValue)
        {
            int notifFormationKindInternalValue;

            switch (notifFormationKindExternalValue)
            {
                case "Формировать":
                    {
                        notifFormationKindInternalValue = 1;
                        break;
                    }

                case "Не формировать":
                    {
                        notifFormationKindInternalValue = 0;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format(
                            "отсутствует тип Документа уведомления с наименованием {0}",
                            notifFormationKindExternalValue));
                    }
            }

            this.service.SaveParams(
                "BuilderContractClaimWork.ViolationNotification.NotifFormationKind",
                notifFormationKindInternalValue);
        }
    }
}
