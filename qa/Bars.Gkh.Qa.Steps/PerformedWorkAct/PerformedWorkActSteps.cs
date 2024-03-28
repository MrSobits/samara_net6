namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PerformedWorkActSteps : BindingBase
    {
        [Given(@"пользователь к этому объекту КР добавляет акт выполненных работ")]
        public void ДопустимПользовательКЭтомуОбъектуКРДобавляетАктВыполненныхРабот()
        {
            PerformedWorkActHelper.Current = new PerformedWorkAct
                                                 {
                                                     ObjectCr = ObjectCrHelper.Current
                                                 };
        }


        [Given(@"пользователь у этого акта выполненных работ заполняет поле Работа ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаВыполненныхРаботЗаполняетПолеРабота(string typeWorkName)
        {
            var dsTypeWork = Container.Resolve<IDomainService<TypeWorkCr>>();

            try
            {
                PerformedWorkActHelper.Current.TypeWorkCr =
                    dsTypeWork.GetAll().FirstOrDefault(x => x.Work.Name == typeWorkName);
            }
            finally
            {
                Container.Release(dsTypeWork);
            }
        }

        [Given(@"пользователь у этого акта выполненных работ заполняет поле от ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаВыполненныхРаботЗаполняетПолеОт(string dateFrom)
        {
            PerformedWorkActHelper.Current.DateFrom = dateFrom.DateParse();
        }

        [Given(@"пользователь у этого акта выполненных работ заполняет поле Объем ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаВыполненныхРаботЗаполняетПолеОбъем(string volume)
        {
            PerformedWorkActHelper.Current.Volume = volume.DecimalParse();
        }

        [Given(@"пользователь у этого акта выполненных работ заполняет поле Сумма ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаВыполненныхРаботЗаполняетПолеСумма(string sum)
        {
            PerformedWorkActHelper.Current.Sum = sum.DecimalParse();
        }

        [When(@"пользователь сохраняет этот акт выполненных работ")]
        public void ЕслиПользовательСохраняетЭтотАктВыполненныхРабот()
        {
            var dsPerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();

            try
            {
                dsPerformedWorkAct.Save(PerformedWorkActHelper.Current);
            }
            catch (Exception exception)
            {
                ExceptionHelper.AddException(
                    "IDomainService<PerformedWorkAct>.Save",
                    string.Format("При сохранении акта выполненных работ выпала ошибка: {0}", exception.Message));
            }
            finally
            {
                Container.Release(dsPerformedWorkAct);
            }
        }

        [Then(@"у этого объекта КР присутствует запись по этому акту выполненных работ")]
        public void ТоУЭтогоОбъектаКРПрисутствуетЗаписьПоЭтомуАктуВыполненныхРабот()
        {
            var dsPerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();

            try
            {
                var performedWorks = dsPerformedWorkAct.GetAll().Where(x => x.ObjectCr.Id == ObjectCrHelper.Current.Id);

                performedWorks.Any(x => x.Id == PerformedWorkActHelper.Current.Id)
                    .Should()
                    .BeTrue("у этого объекта КР должна присутствовать запись по этому акту выполненных работ");
            }
            finally
            {
                Container.Release(dsPerformedWorkAct);
            }
        }

    }
}
