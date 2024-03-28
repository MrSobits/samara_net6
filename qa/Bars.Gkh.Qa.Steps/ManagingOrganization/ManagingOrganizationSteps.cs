namespace Bars.Gkh.Qa.Steps.ManagingOrganization
{
    using System;
    using System.Reflection;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ManagingOrganizationSteps : BindingBase
    {
        public IDomainService<Entities.ManagingOrganization> ds = Container.Resolve<IDomainService<Entities.ManagingOrganization>>();
        
        [Given(@"пользователь добавляет новую управляющую организацию")]
        public void ДопустимПользовательДобавляетНовуюУправляющуюОрганизацию()
        {
            ManagingOrganizationHelper.Current = new Entities.ManagingOrganization();
        }
        
        [Given(@"пользователь у этой управляющей организации заполняет поле Контрагент")]
        public void ДопустимПользовательУЭтойУправляющейОрганизацииЗаполняетПолеКонтрагент()
        {
            ManagingOrganizationHelper.Current.Contragent = ContragentHelper.CurrentContragent;
        }
        
        [Given(@"пользователь у этой управляющей организации заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтойУправляющейОрганизацииЗаполняетПолеОписание(string description)
        {
            ManagingOrganizationHelper.Current.Description = description;
        }
        
        [When(@"пользователь сохраняет эту управляющую организацию")]
        public void ЕслиПользовательСохраняетЭтуУправляющуюОрганизацию()
        {
            try
            {
                ds.SaveOrUpdate(ManagingOrganizationHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту управляющую организацию")]
        public void ЕслиПользовательУдаляетЭтуУправляющуюОрганизацию()
        {
            try
            {
                ds.Delete(ManagingOrganizationHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"добавлена управляющая организация для этого контрагента")]
        public void ДопустимДобавленаУправляющаяОрганизацияДляЭтогоКонтрагента(Table table)
        {
            ManagingOrganizationHelper.Current = table.CreateInstance<Entities.ManagingOrganization>();
            ManagingOrganizationHelper.Current.Contragent = ContragentHelper.CurrentContragent;
            
            try
            {
                ds.SaveOrUpdate(ManagingOrganizationHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой управляющей организации присутствует в справочнике управляющих организаций")]
        public void ТоЗаписьПоЭтойУправляющейОрганизацииПрисутствуетВСправочникеУправляющихОрганизаций()
        {
            ds.Get(ManagingOrganizationHelper.Current.Id)
                .Should()
                .NotBeNull(string.Format("управляющая организация должна присутствовать в справочнике.{0}",
                    ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой управляющей организации отсутствует в справочнике управляющих организаций")]
        public void ТоЗаписьПоЭтойУправляющейОрганизацииОтсутствуетВСправочникеУправляющихОрганизаций()
        {
            ds.Get(ManagingOrganizationHelper.Current.Id)
                .Should()
                .BeNull(string.Format("управляющая организация должна отсутствовать в справочнике.{0}",
                    ExceptionHelper.GetExceptions()));
        }
    }
}
