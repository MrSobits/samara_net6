namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    internal class TaskEntrySteps : BindingBase
    {
        [Then(@"в реестре задач появилась задача с Наименованием ""(.*)""")]
        public void ТоВРеестреЗадачПоявиласьЗадачаСНаименованием(string name)
        {
            var parentTaskId = ScenarioContext.Current.Get<long>("TaskEntryParentTaskId");

            var dsTask = Container.Resolve<IDomainService<TaskEntry>>();

            try
            {
                TaskEntry task =
                    dsTask.GetAll()
                        .OrderByDescending(x => x.ObjectCreateDate)
                        .FirstOrDefault(x => x.Parent.Id == parentTaskId);

                if (task == null)
                {
                    throw new SpecFlowException(
                        string.Format(
                            "Задача с наименованием {0} не была создана. {1}",
                            name,
                            ExceptionHelper.GetExceptions()));
                }

                task.Name.Should()
                    .Be(
                        name,
                        string.Format("Наименование задачи должно быть {0}. {1}", name, ExceptionHelper.GetExceptions()));

                CommonHelper.IsNow(task.ObjectCreateDate).Should().BeTrue("Должна появится новая задача");

                TaskEntryHelper.Current = task;
            }
            finally
            {
                Container.Release(dsTask);
            }
        }

        [Then(@"в реестре задач не появилась задача")]
        public void ТоВРеестреЗадачНеПоявиласьЗадача()
        {
            var parentTaskId = ScenarioContext.Current.Get<long>("TaskEntryParentTaskId");

            var dsTask = Container.Resolve<IDomainService<TaskEntry>>();

            try
            {
                TaskEntry task = dsTask.GetAll().FirstOrDefault(x => x.Parent.Id == parentTaskId);

                task.Should().BeNull("Задача не должна появляться");
            }
            finally
            {
                Container.Release(dsTask);
            }
        }

        [Then(@"у этой задачи заполнено поле Дата запуска ""(.*)""")]
        public void ТоУЭтойЗадачиЗаполненоПолеДатаЗапуска(string date)
        {
            if (TaskEntryHelper.Current.ObjectCreateDate.Date != date.DateParse())
            {
                Assert.Fail("Дата задачи не равна" + date);
            }
        }

        [Then(@"в течении (.*) мин статус задачи стал ""(.*)""")]
        public void ТоВТеченииМинСтатусЗадачиСтал(int minutes, string expectedState)
        {
            var taskEntryDomain = Container.Resolve<IDomainService<TaskEntry>>();
            var sessionProvider = Container.Resolve<ISessionProvider>();

            try
            {
                var i = new TimeSpan(0, minutes, 0);

                string currentState = string.Empty;

                var stopWatch = new Stopwatch();

                stopWatch.Start();

                do
                {
                    //обновляем текущее состояние
                    sessionProvider.CreateNewSession();

                    TaskEntryHelper.Current = taskEntryDomain.Get(TaskEntryHelper.Current.Id);

                    currentState = EnumHelper.GetDisplayValue(TaskEntryHelper.Current.Status);

                    if (currentState == expectedState)
                    {
                        break;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                } while (stopWatch.Elapsed <= i);

                currentState.Should()
                    .Be(
                        expectedState,
                        string.Format(
                            "текущий статус задачи {0} должен быть {1}",
                            TaskEntryHelper.Current.Name,
                            expectedState));
            }
            finally
            {
                Container.Release(taskEntryDomain);
                Container.Release(sessionProvider);
            }
            
        }

        [Then(@"в течении (.*) мин процент выполнения задачи стал ""(.*)""")]
        public void ТоВТеченииМинПроцентВыполненияЗадачиСтал(int minutes, int expectedPercent)
        {
            var i = new TimeSpan(0, minutes, 0);

            int currentPercent;

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            do
            {
                Container.Resolve<ISessionProvider>().CreateNewSession();

                //обновляем текущее состояние
                TaskEntryHelper.Current = Container.Resolve<IDomainService<TaskEntry>>().Get(TaskEntryHelper.Current.Id);

                currentPercent = TaskEntryHelper.Current.Percentage;

                if (currentPercent == expectedPercent)
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            while (stopWatch.Elapsed <= i);

            currentPercent.Should()
                .Be(expectedPercent, string.Format("текущий процент выполнения задачи должен быть {0}", expectedPercent));
        }

        [Then(@"в течении (.*) мин ход выполнения задачи стал ""(.*)""")]
        public void ТоВТеченииМинХодВыполненияЗадачиСтал(int minutes, string expectedProgress)
        {
            var i = new TimeSpan(0, minutes, 0);

            string currentProgress;

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            do
            {
                Container.Resolve<ISessionProvider>().CreateNewSession();

                //обновляем текущее состояние
                TaskEntryHelper.Current = Container.Resolve<IDomainService<TaskEntry>>().Get(TaskEntryHelper.Current.Id);

                currentProgress = TaskEntryHelper.Current.Progress;

                if (currentProgress == expectedProgress)
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            while (stopWatch.Elapsed <= i);

            currentProgress.Should()
                .Be(expectedProgress, string.Format("текущий ход выполнения задачи должен быть {0}", expectedProgress));
        }
    }
}
