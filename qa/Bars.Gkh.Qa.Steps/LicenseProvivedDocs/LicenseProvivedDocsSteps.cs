using System;
using System.ComponentModel;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class LicenseProvivedDocsSteps : BindingBase
    {

        private IDomainService<LicenseProvidedDoc> ds = Container.Resolve<IDomainService<LicenseProvidedDoc>>();

        [Given(@"пользователь добавляет новый документ для выдачи лицензии")]
        public void ДопустимПользовательДобавляетНовыйДокументДляВыдачиЛицензии()
        {
            LicenseProvidedDocsHelper.Current = new LicenseProvidedDoc();
        }
        
        [Given(@"пользователь у этого документа для выдачи лицензии заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоДокументаДляВыдачиЛицензииЗаполняетПолеНаименование(string name)
        {
            LicenseProvidedDocsHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого документа для выдачи лицензии заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоДокументаДляВыдачиЛицензииЗаполняетПолеКод(string code)
        {
            LicenseProvidedDocsHelper.Current.Code = code;
        }
        
        [When(@"пользователь сохраняет этот документ для выдачи лицензии")]
        public void ДопустимПользовательСохраняетЭтотДокументДляВыдачиЛицензии()
        {
            try
            {
                ds.SaveOrUpdate(LicenseProvidedDocsHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот документ для выдачи лицензии")]
        public void ЕслиПользовательУдаляетЭтотДокументДляВыдачиЛицензии()
        {
            try
            {
                ds.Delete(LicenseProvidedDocsHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому документу для выдачи лицензии присутствует в справочнике документов для выдачи лицензии")]
        public void ТоЗаписьПоЭтомуДокументуДляВыдачиЛицензииПрисутствуетВСправочникеДокументовДляВыдачиЛицензии()
        {
            ds.Get(LicenseProvidedDocsHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому документу для выдачи лицензии отсутствует в справочнике документов для выдачи лицензии")]
        public void ТоЗаписьПоЭтомуДокументуДляВыдачиЛицензииОтсутствуетВСправочникеДокументовДляВыдачиЛицензии()
        {
            ds.Get(LicenseProvidedDocsHelper.Current.Id).Should().BeNull();
        }

        [Given(@"пользователь у этого документа для выдачи лицензии заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоДокументаДляВыдачиЛицензииЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            LicenseProvidedDocsHelper.Current.Name = new string(ch, count);
        }

        [Given(@"пользователь у этого документа для выдачи лицензии заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоДокументаДляВыдачиЛицензииЗаполняетПолеКодСимволов(int count, char ch)
        {
            LicenseProvidedDocsHelper.Current.Code = new string(ch, count);
        }


    }
}
