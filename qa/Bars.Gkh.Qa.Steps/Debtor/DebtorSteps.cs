namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using TechTalk.SpecFlow;

    [Binding]
    public class DebtorSteps : BindingBase
    {
        private IDebtorService DebtorService = Container.Resolve<IDebtorService>();

        [When(@"пользователь очищает реестр неплательщиков")]
        public void ЕслиПользовательОчищаетРеестрНеплательщиков()
        {
            try
            {
                var result = DebtorService.Clear(new BaseParams());

                if (!result.Success)
                {
                    ExceptionHelper.TestExceptions.Add("DebtorService.Clear", "возвратил ошибку");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь формирует реестр неплательщиков")]
        public void ЕслиПользовательФормируетРеестрНеплательщиков()
        {
            var result = DebtorService.Create(new BaseParams());

            var debtorResult = (CreateTasksResult)result.Data;

            if (!result.Success)
            {
                ExceptionHelper.TestExceptions.Add("DebtorService.Clear", "возвратил ошибку");
            }

            ScenarioContext.Current["TaskEntryParentTaskId"] = debtorResult.ParentTaskId;
        }

        [Then(@"в течении (.*) мин в реестре неплательщиков обновились записи по неплательщикам")]
        public void ТоВТеченииМинВРеестреНеплательщиковОбновилисьЗаписиПоНеплательщикам(int minutes)
        {
            var i = new TimeSpan(0, minutes, 0);

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            do
            {
                Container.Resolve<ISessionProvider>().CreateNewSession();

                //обновляем текущее состояние
                var debtorsList = Container.Resolve<IDomainService<RegOperator.Entities.PersonalAccount.Debtor>>().GetAll().ToList();

                if (debtorsList.IsNotEmpty())
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            while (stopWatch.Elapsed <= i);
        }
    }
}
