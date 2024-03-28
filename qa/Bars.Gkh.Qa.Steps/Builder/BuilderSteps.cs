namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class BuilderSteps : BindingBase
    {
        private IDomainService<Builder> ds = Container.Resolve<IDomainService<Builder>>();

        [Given(@"пользователь добавляет новую подрядную организацию")]
        public void ДопустимПользовательДобавляетНовуюПодряднуюОрганизацию()
        {
            BuilderHelper.Current = new Builder();
        }
        
        [Given(@"пользователь у этой подрядной организации заполняет поле Контрагент")]
        public void ДопустимПользовательУЭтойПодряднойОрганизацииЗаполняетПолеКонтрагент()
        {
            BuilderHelper.Current.Contragent = ContragentHelper.CurrentContragent;
        }
        
        [Given(@"пользователь у этой подрядной организации заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтойПодряднойОрганизацииЗаполняетПолеОписание(string description)
        {
            BuilderHelper.Current.Description = description;
        }

        [Given(@"пользователь у этой подрядной организации заполняет поле Описание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПодряднойОрганизацииЗаполняетПолеОписаниеСимволов(int count, char ch)
        {
            BuilderHelper.Current.Description = new string(ch, count);
        }

        [Given(@"добавлена подрядная организация c текущим контрагентом")]
        public void ДопустимДобавленаПодряднаяОрганизацияСТекущимКонтрагентом()
        {
            var builder = new Builder
            {
                Contragent = ContragentHelper.CurrentContragent
            };

            this.ds.Save(builder);

            BuilderHelper.Current = builder;
        }

        [When(@"пользователь сохраняет эту подрядную организацию")]
        public void ЕслиПользовательСохраняетЭтуПодряднуюОрганизацию()
        {
            try
            {
                this.ds.SaveOrUpdate(BuilderHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту подрядную организацию")]
        public void ЕслиПользовательУдаляетЭтуПодряднуюОрганизацию()
        {
            try
            {
                this.ds.Delete(BuilderHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой подрядной организации присутствует в разделе подрядных организаций")]
        public void ТоЗаписьПоЭтойПодряднойОрганизацииПрисутствуетВРазделеПодрядныхОрганизаций()
        {
            this.ds.Get(BuilderHelper.Current.Id).Should().NotBeNull(string.Format("подрядная организация должна присутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой подрядной организации отсутствует в разделе подрядных организаций")]
        public void ТоЗаписьПоЭтойПодряднойОрганизацииОтсутствуетВРазделеПодрядныхОрганизаций()
        {
            this.ds.Get(BuilderHelper.Current.Id).Should().BeNull(string.Format("подрядная организация должна отсутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
