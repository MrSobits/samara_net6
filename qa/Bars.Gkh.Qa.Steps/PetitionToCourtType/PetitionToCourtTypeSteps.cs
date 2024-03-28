namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.ComponentModel;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class PetitionToCourtTypeSteps : BindingBase
    {
        public IDomainService<PetitionToCourtType> ds = Container.Resolve <IDomainService<PetitionToCourtType>>();
        
        [Given(@"пользователь добавляет новое заявление в суд")]
        public void ДопустимПользовательДобавляетНовоеЗаявлениеВСуд()
        {
            PetitionToCourtTypeHelper.Current = new PetitionToCourtType();
        }
        
        [Given(@"пользователь у этого заявления в суд заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеКод(string code)
        {
            PetitionToCourtTypeHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого заявления в суд заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеКодСимволов(int count, char ch)
        {
            PetitionToCourtTypeHelper.Current.Code = new string(ch, count);
        }

        [Given(@"пользователь у этого заявления в суд заполняет поле Краткое наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеКраткоеНаименование(string shortName)
        {
            PetitionToCourtTypeHelper.Current.ShortName = shortName;
        }

        [Given(@"пользователь у этого заявления в суд заполняет поле Краткое наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеКраткоеНаименованиеСимволов(int count, char ch)
        {
            PetitionToCourtTypeHelper.Current.ShortName = new string(ch, count);
        }
        
        [Given(@"пользователь у этого заявления в суд заполняет поле Полное наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеПолноеНаименование(string fullName)
        {
            PetitionToCourtTypeHelper.Current.FullName = fullName;
        }

        [Given(@"пользователь у этого заявления в суд заполняет поле Полное наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоЗаявленияВСудЗаполняетПолеПолноеНаименованиеСимволов(int count, char ch)
        {
            PetitionToCourtTypeHelper.Current.FullName = new string(ch, count);
        }
        
        [When(@"пользователь сохраняет этот заявление в суд")]
        public void ЕслиПользовательСохраняетЭтотЗаявлениеВСуд()
        {
            try
            {
                ds.SaveOrUpdate(PetitionToCourtTypeHelper.Current);            
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет это заявление в суд")]
        public void ЕслиПользовательУдаляетЭтоЗаявлениеВСуд()
        {
            try
            {
                ds.Delete(PetitionToCourtTypeHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому заявлению в суд присутствует в справочнике заявлений в суд")]
        public void ТоЗаписьПоЭтомуЗаявлениюВСудПрисутствуетВСправочникеЗаявленийВСуд()
        {
            ds.Get(PetitionToCourtTypeHelper.Current.Id).Should().NotBeNull(string.Format("Заявление в суд должно присутствовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому заявлению в суд отсутствует в справочнике заявлений в суд")]
        public void ТоЗаписьПоЭтомуЗаявлениюВСудОтсутствуетВСправочникеЗаявленийВСуд()
        {
            ds.Get(PetitionToCourtTypeHelper.Current.Id).Should().BeNull(string.Format("Заявление в суд должно отсутствовать в справочник.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
