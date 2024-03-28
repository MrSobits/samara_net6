using Bars.B4;
using FluentAssertions;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    [Binding]
    internal class JobSteps : BindingBase
    {

        private IDomainService ds
        {
            get
            {
                var type = JobHelper.Current.GetType();
                var dsgen = typeof (IDomainService<>).MakeGenericType(type);
                return (IDomainService)Container.Resolve(dsgen);
            }
        }

        [Given(@"пользователь добавляет новую работу")]
        public void ДопустимПользовательДобавляетНовуюРаботу()
        {
            JobHelper.Current = Activator.CreateInstance("Bars.Gkh.Overhaul", "Bars.Gkh.Overhaul.Entities.Job").Unwrap();
        }

        [Given(@"пользователь у этого вида работы заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеНаименование(string name)
        {
            JobHelper.ChangeCurrent("Name", name);
        }

        [Given(@"пользователь у этого вида работы заполняет поле Вид работы")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеВидРаботы()
        {
            JobHelper.ChangeCurrent("Work", WorkHelper.Current);
        }

        [Given(@"пользователь у этого вида работы заполняет поле Ед\. измерения")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеЕд_Измерения()
        {
            JobHelper.ChangeCurrent("UnitMeasure", UnitMeasureHelper.Current);
        }

        [When(@"пользователь сохраняет эту работу")]
        public void ЕслиПользовательСохраняетЭтуРаботу()
        {
            try
            {
                if ( (long)JobHelper.GetPropertyValue("Id") == 0 )
                {
                    ds.Save(JobHelper.Current);
                }
                else
                {
                    ds.Update(JobHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту работу")]
        public void ЕслиПользовательУдаляетЭтуРаботу()
        {
            try
            {
                ds.Delete(JobHelper.GetPropertyValue("Id"));
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой работе присутствует в реестре работ")]
        public void ТоЗаписьПоЭтойРаботеПрисутствуетВРеестреРабот()
        {
            ds.Get(JobHelper.GetPropertyValue("Id"))
                .Should()
                .NotBeNull(string.Format("работа должна присутствовать в справочнике работ.{0}",
                    ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой работе отсутствует в реестре работ")]
        public void ТоЗаписьПоЭтойРаботеОтсутствуетВРеестреРабот()
        {
            ds.Get(JobHelper.GetPropertyValue("Id"))
                .Should()
                .BeNull(string.Format("работа должна отсутствовать в справочнике работ.{0}",
                    ExceptionHelper.GetExceptions()));
        }
    }
}
