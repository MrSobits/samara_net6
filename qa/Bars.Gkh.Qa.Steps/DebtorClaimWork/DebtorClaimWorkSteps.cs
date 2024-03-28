namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using NHibernate;

    using TechTalk.SpecFlow;

    [Binding]
    internal class DebtorClaimWorkSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = DebtorClaimWorkHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"добавлена запись в разделе Реестр неплательщиков")]
        public void ДопустимДобавленаЗаписьВРазделеРеестрНеплательщиков(Table table)
        {
            var debtorClaimWork = Activator
                .CreateInstance("Bars.Gkh.RegOperator", "Bars.Gkh.RegOperator.Entities.DebtorClaimWork").Unwrap();

            DebtorClaimWorkHelper.Current = debtorClaimWork;

            ReflectionHelper.SetPropertyValue(debtorClaimWork, "PersonalAccount", BasePersonalAccountHelper.Current);

            this.DomainService.Save(debtorClaimWork);

            Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();

            ReflectionHelper.ChangeState(debtorClaimWork, table.Rows.First()["state"], false);
        }

        [Given(@"в карточке основания в Реестре неплательщиков пользователь формирует документ - ""(.*)""")]
        public void ДопустимВКарточкеОснованияВРеестреНеплательщиковПользовательФормируетДокумент(string document)
        {
            string ruleId;

            switch (document)
            {
                case "Уведомление":
                    {
                        ruleId = "NotificationCreateRule";

                        break;
                    }

                case "Претенезия":
                    {
                        ruleId = "PretensionCreateRule";

                        break;
                    }

                case "Исковое заявление":
                    {
                        ruleId = "LawSuitCreateRule";

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("нет типа документа {0}", document));
                    }
            }

            var claimWorkDocProvider = Container.Resolve<IClaimWorkDocumentProvider>();

            var providerParams = new DynamicDictionary();

            providerParams.WriteInstance(
                new
                    {
                        ruleId, 
                        claimWorkId = ReflectionHelper.GetPropertyValue(DebtorClaimWorkHelper.Current, "Id")
                    });

            var result  = claimWorkDocProvider.CreateDocument(new BaseParams { Params = providerParams });

            switch (document)
            {
                case "Уведомление":
                    {
                        NotificationClwHelper.Current = result.Data as NotificationClw;

                        break;
                    }

                case "Претенезия":
                    {
                        PretensionClwHelper.Current = result.Data as PretensionClw;

                        break;
                    }

                case "Исковое заявление":
                    {
                        PetitionHelper.Current = result.Data as Petition;

                        break;
                    }
            }
        }

        [When(@"пользователь из карточки основания в Реестре неплательщиков удаляет этот документ - ""(.*)""")]
        public void ЕслиПользовательИзКарточкиОснованияВРеестреНеплательщиковУдаляетЭтотДокумент(string document)
        {
            try
            {
                switch (document)
                {
                    case "Уведомление":
                        {
                            var ds = Container.Resolve<IDomainService<NotificationClw>>();

                            ds.Delete(NotificationClwHelper.Current.Id);

                            break;
                        }

                    case "Претенезия":
                        {
                            var ds = Container.Resolve<IDomainService<PretensionClw>>();

                            ds.Delete(PretensionClwHelper.Current.Id);

                            break;
                        }

                    case "Исковое заявление":
                        {
                            var ds = Container.Resolve<IDomainService<Petition>>();

                            ds.Delete(PetitionHelper.Current.Id);

                            break;
                        }

                    default:
                        {
                            throw new SpecFlowException(string.Format("нет типа документа {0}", document));
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"этот документ - ""(.*)"" отсутствует в карточке основания в Реестре неплательщиков")]
        public void ТоЭтотДокументОтсутствуетВКарточкеОснованияВРеестреНеплательщиков(string document)
        {
            DocumentClw clwDocument;

            switch (document)
            {
                case "Уведомление":
                    {
                        var ds = Container.Resolve<IDomainService<NotificationClw>>();

                        clwDocument = ds.Get(NotificationClwHelper.Current.Id);

                        break;
                    }

                case "Претенезия":
                    {
                        var ds = Container.Resolve<IDomainService<PretensionClw>>();

                        clwDocument = ds.Get(PretensionClwHelper.Current.Id);

                        break;
                    }

                case "Исковое заявление":
                    {
                        var ds = Container.Resolve<IDomainService<Petition>>();

                        clwDocument = ds.Get(PetitionHelper.Current.Id);

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("нет типа документа {0}", document));
                    }
            }

            clwDocument.Should()
                .BeNull(
                    string.Format(
                        "этот документ  - {0}, должен отсутствовать в карточке основания в Реестре неплательщиков. {1}",
                        document,
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"этот документ - ""(.*)"" присутствует в карточке основания в Реестре неплательщиков")]
        public void ТоЭтотДокументПрисутствуетВКарточкеОснованияВРеестреНеплательщиков(string document)
        {
            var clwDocument = new DocumentClw();

            switch (document)
            {
                case "Уведомление":
                    {
                        var ds = Container.Resolve<IDomainService<NotificationClw>>();

                        clwDocument = ds.Get(NotificationClwHelper.Current.Id);

                        break;
                    }

                case "Претенезия":
                    {
                        var ds = Container.Resolve<IDomainService<PretensionClw>>();

                        clwDocument = ds.Get(PretensionClwHelper.Current.Id);

                        break;
                    }

                case "Акт выявления нарушений":
                    {
                        var ds = Container.Resolve<IDomainService<ActViolIdentificationClw>>();

                        clwDocument = ds.Get(ActViolIdentificationClwHelper.Current.Id);

                        break;
                    }

                case "Исковое заявление":
                    {
                        var ds = Container.Resolve<IDomainService<Petition>>();

                        clwDocument = ds.Get(PetitionHelper.Current.Id);

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("нет типа документа {0}", document));
                    }
            }

            clwDocument.Should()
                .NotBeNull(
                    string.Format(
                        "этот документ  - {0}, должен присутствовать в карточке основания в реестре Подрядчики, нарушившие условия договора. {1}",
                        document,
                        ExceptionHelper.GetExceptions()));
        }
    }
}
