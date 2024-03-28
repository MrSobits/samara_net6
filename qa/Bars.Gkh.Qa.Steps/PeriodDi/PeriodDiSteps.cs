using System.Collections;
using Bars.B4;
using FluentAssertions;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    [Binding]
    public class PeriodDiSteps : BindingBase
    {

        private IDomainService DomainService
        {
            get
            {
                Type entityType = PeriodDiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый отчетный период")]
        public void ДопустимПользовательДобавляетНовыйОтчетныйПериод()
        {
            PeriodDiHelper.Current = Activator
                .CreateInstance("Bars.GkhDi", "Bars.GkhDi.Entities.PeriodDi").Unwrap();
        }

        [Given(@"пользователь у этого отчетного периода заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоОтчетногоПериодаЗаполняетПолеНаименование(string name)
        {
            PeriodDiHelper.ChangeCurrent("Name", name);
        }

        [Given(@"пользователь у этого отчетного периода заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоОтчетногоПериодаЗаполняетПолеДатаНачала(string startDate)
        {
            PeriodDiHelper.ChangeCurrent("DateStart", DateTime.Parse(startDate));
        }

        [Given(@"пользователь у этого отчетного периода заполняет поле Дата конца ""(.*)""")]
        public void ДопустимПользовательУЭтогоОтчетногоПериодаЗаполняетПолеДатаКонца(string dataEnd)
        {
            PeriodDiHelper.ChangeCurrent("DateEnd", DateTime.Parse(dataEnd));
        }

        [Given(@"пользователь у этого отчетного периода заполняет поле Не учитывать дома, введенные в эксплуатацию позже следующей даты ""(.*)""")]
        public void ДопустимПользовательУЭтогоОтчетногоПериодаЗаполняетПолеНеУчитыватьДомаВведенныеВЭксплуатациюПозжеСледующейДаты(string dataAccounting)
        {
            PeriodDiHelper.ChangeCurrent("DateAccounting", DateTime.Parse(dataAccounting));
        }

        [When(@"пользователь удаляет этот отчетный период")]
        public void ЕслиПользовательУдаляетЭтотОтчетныйПериод()
        {
            var id = (long)PeriodDiHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"пользователь у этого отчетного периода заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоОтчетногоПериодаЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            PeriodDiHelper.ChangeCurrent("Name", new string(ch, count));
        }
        
        [When(@"пользователь сохраняет этот отчетный период")]
        public void ЕслиПользовательСохраняетЭтотОтчетныйПериод()
        {
            var id = (long)PeriodDiHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PeriodDiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PeriodDiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому отчетному периоду присутствует в разделе отчетных периодов")]
        public void ТоЗаписьПоЭтомуОтчетномуПериодуПрисутствуетВРазделеОтчетныхПериодов()
        {
            var id = (long)PeriodDiHelper.GetPropertyValue("Id");

            var periodDi = this.DomainService.Get(id);

            periodDi.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому отчетному периоду должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому отчетному периоду отсутствует в разделе отчетных периодов")]
        public void ТоЗаписьПоЭтомуОтчетномуПериодуОтсутствуетВРазделеОтчетныхПериодов()
        {
            var id = (long)PeriodDiHelper.GetPropertyValue("Id");

            var periodDi = this.DomainService.Get(id);

            periodDi.Should()
                .BeNull(
                    string.Format(
                        "запись по этому отчетному периоду должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}
