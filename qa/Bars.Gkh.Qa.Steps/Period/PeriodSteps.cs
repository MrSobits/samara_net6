namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow.Assist;
    using System;
    using System.ComponentModel;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class PeriodSteps : BindingBase
    {
        private IDomainService<Period> ds = Container.Resolve<IDomainService<Period>>();
        
        [Given(@"пользователь добавляет новый период программы")]
        public void ДопустимПользовательДобавляетНовыйПериодПрограммы()
        {
            PeriodHelper.Current = new Period();
        }
        
        [Given(@"пользователь у этого периода программы заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоПериодаПрограммыЗаполняетПолеНаименование(string name)
        {
            PeriodHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого периода программы заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоПериодаПрограммыЗаполняетПолеДатаНачала(string dateStart)
        {
            PeriodHelper.Current.DateStart = DateTime.Parse(dateStart);
        }
        
        [Given(@"пользователь у этого периода программы заполняет поле Дата окончания ""(.*)""")]
        public void ДопустимПользовательУЭтогоПериодаПрограммыЗаполняетПолеДатаОкончания(string dateEnd)
        {
            PeriodHelper.Current.DateEnd = DateTime.Parse(dateEnd);
        }

        [Given(@"добавлен период программ")]
        public void ДопустимДобавленПериодПрограмм(Table table)
        {
            var period = table.CreateInstance<Period>();

            this.ds.Save(period);

            PeriodHelper.Current = period;
        }
        
        [When(@"пользователь сохраняет этот период программы")]
        public void ЕслиПользовательСохраняетЭтотПериодПрограммы()
        {
            try
            {
                ds.SaveOrUpdate(PeriodHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот период программы")]
        public void ЕслиПользовательУдаляетЭтотПериодПрограммы()
        {
            try
            {
                ds.Delete(PeriodHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому периоду программы присутствует в справочнике периодов программ")]
        public void ТоЗаписьПоЭтомуПериодуПрограммыПрисутствуетВСправочникеПериодовПрограмм()
        {
            ds.Get(PeriodHelper.Current.Id).Should().NotBeNull(string.Format("период должен присутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому периоду программы отсутствует в справочнике периодов программ")]
        public void ТоЗаписьПоЭтомуПериодуПрограммыОтсутствуетВСправочникеПериодовПрограмм()
        {
            ds.Get(PeriodHelper.Current.Id).Should().BeNull(string.Format("период должен отсутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого периода программы заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПериодаПрограммыЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            PeriodHelper.Current.Name = new string(ch, count);
        }

    }
}
