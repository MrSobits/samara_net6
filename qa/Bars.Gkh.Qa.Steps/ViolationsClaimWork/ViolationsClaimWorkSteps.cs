namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class ViolationsClaimWorkSteps : BindingBase
    {

        private IDomainService<ViolClaimWork> ds = Container.Resolve<IDomainService<ViolClaimWork>>();

        [Given(@"пользователь добавляет новый вид нарушения договора подряда")]
        public void ДопустимПользовательДобавляетНовыйВидНарушенияДоговораПодряда()
        {
            ViolationsClaimWorkHelper.Current = new ViolClaimWork();
        }
        
        [Given(@"пользователь у этого вида нарушения договора подряда заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаНарушенияДоговораПодрядаЗаполняетПолеНаименование(string name)
        {
            ViolationsClaimWorkHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого вида нарушения договора подряда заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаНарушенияДоговораПодрядаЗаполняетПолеКод(string code)
        {
            ViolationsClaimWorkHelper.Current.Code = code;
        }
        
        [When(@"пользователь сохраняет этот вид нарушения договора подряда")]
        public void ЕслиПользовательСохраняетЭтотВидНарушенияДоговораПодряда()
        {
            try
            {
                ds.SaveOrUpdate(ViolationsClaimWorkHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот вид нарушения договора подряда")]
        public void ЕслиПользовательУдаляетЭтотВидНарушенияДоговораПодряда()
        {
            try
            {
                ds.Delete(ViolationsClaimWorkHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому виду нарушения договора подряда присутствует в справочнике видов нарушений договора подряда")]
        public void ТоЗаписьПоЭтомуВидуНарушенияДоговораПодрядаПрисутствуетВСправочникеВидовНарушенийДоговораПодряда()
        {
            ds.Get(ViolationsClaimWorkHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому виду нарушения договора подряда отсутствует в справочнике видов нарушений договора подряда")]
        public void ТоЗаписьПоЭтомуВидуНарушенияДоговораПодрядаОтсутствуетВСправочникеВидовНарушенийДоговораПодряда()
        {
            ds.Get(ViolationsClaimWorkHelper.Current.Id).Should().BeNull();
        }

        [Given(@"пользователь у этого вида нарушения договора подряда заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаНарушенияДоговораПодрядаЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            ViolationsClaimWorkHelper.Current.Name = new string(ch, count);
        }

        [Given(@"пользователь у этого вида нарушения договора подряда заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаНарушенияДоговораПодрядаЗаполняетПолеКодСимволов(int count, char ch)
        {
            ViolationsClaimWorkHelper.Current.Code = new string(ch, count);
        }
    }
}
