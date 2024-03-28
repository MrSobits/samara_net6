namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class DebtorClaimWorkParamsSteps : BindingBase
    {
        private IGkhParamService service = Container.Resolve<IGkhParamService>();

        [Given(@"пользователь указывает в настройках Настройки реестра неплательщиков - (.*) в поле Документ уведомления значение ""(.*)""")]
        public void ДопустимПользовательУказываетВНастройкахНастройкиРеестраНеплательщиковВПолеДокументУведомленияЗначение(
            string externalOwnerType,
            string notifFormationKindExternalValue)
        {
            int notifFormationKindInternalValue;

            string internalOwnerType;

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

            switch (externalOwnerType)
            {
                case "Физ. лицо":
                    {
                        internalOwnerType = "Individual";
                        break;
                    }

                case "Юр. лицо":
                    {
                        internalOwnerType = "Legal";
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format(
                            "В настройках реестра неплательщиков отсутствует тип подпункт {0}",
                            externalOwnerType));
                    }
            }

            this.service.SaveParams(
                string.Format("DebtorClaimWork.{0}.DebtNotification.NotifFormationKind", internalOwnerType),
                notifFormationKindInternalValue);
        }

        [Given(@"пользователь указывает в настройках Настройки реестра неплательщиков - (.*) в поле Документ претензии значение ""(.*)""")]
        public void ДопустимПользовательУказываетВНастройкахНастройкиРеестраНеплательщиковВПолеДокументПретензииЗначение(
            string externalOwnerType,
            string pretensionFormationKindExternalValue)
        {
            int pretensionFormationKindInternalValue;

            string internalOwnerType;

            switch (pretensionFormationKindExternalValue)
            {
                case "Формировать":
                    {
                        pretensionFormationKindInternalValue = 1;
                        break;
                    }

                case "Не формировать":
                    {
                        pretensionFormationKindInternalValue = 0;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format(
                            "отсутствует тип Документа претензии с наименованием {0}",
                            pretensionFormationKindExternalValue));
                    }
            }

            switch (externalOwnerType)
            {
                case "Физ. лицо":
                    {
                        internalOwnerType = "Individual";
                        break;
                    }

                case "Юр. лицо":
                    {
                        internalOwnerType = "Legal";
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format(
                            "В настройках реестра неплательщиков отсутствует тип подпункт {0}",
                            externalOwnerType));
                    }
            }

            this.service.SaveParams(
                string.Format("DebtorClaimWork.{0}.Pretension.PretensionFormationKind", internalOwnerType),
                pretensionFormationKindInternalValue);
        }
    }
}
