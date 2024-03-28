namespace Bars.Gkh.Qa.Steps
{
    using System;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.GkhCr.Entities;
    using Castle.Windsor.Diagnostics.Extensions;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Domain;

    [Binding]
    public class PersonalAccountPrivilegedCategorySteps : BindingBase
    {
        private IDomainService<PersonalAccountPrivilegedCategory> domainService = Container.Resolve<IDomainService<PersonalAccountPrivilegedCategory>>();
        
        [Given(@"пользователь добавляет новую категорию льготы к лс")]
        public void ДопустимПользовательДобавляетНовуюКатегориюЛьготыКЛс()
        {
            PersonalAccountPrivilegedCategoryHelper.Current = new PersonalAccountPrivilegedCategory();

            //заполняем лицевой счет к которой дообавляется категория льготы
            PersonalAccountPrivilegedCategoryHelper.Current.PersonalAccount =
                Container.Resolve<IDomainService<BasePersonalAccount>>().FirstOrDefault(x => x != null);
        }
        
        [Given(@"пользователь у этой категории льготы заполняет поле Льготная категория ""(.*)""")]
        public void ДопустимПользовательУЭтойКатегорииЛьготыЗаполняетПолеЛьготнаяКатегория(string p0)
        {
            PersonalAccountPrivilegedCategoryHelper.Current.PrivilegedCategory =
                Container.Resolve<IDomainService<PrivilegedCategory>>().FirstOrDefault(x => x != null);
        }
        
        [Given(@"пользователь у этой категории льготы заполняет поле Действует с ""(.*)""")]
        public void ДопустимПользовательУЭтойКатегорииЛьготыЗаполняетПолеДействуетС(string dateFrom)
        {
            if (dateFrom == "текущая дата")
            {
                PersonalAccountPrivilegedCategoryHelper.Current.DateFrom = DateTime.Today;
                
            }
            else
            {
                PersonalAccountPrivilegedCategoryHelper.Current.DateFrom = DateTime.Parse(dateFrom);
            }
        }
        
        [Given(@"пользователь у этой категории льготы заполняет поле Действует по ""(.*)""")]
        public void ДопустимПользовательУЭтойКатегорииЛьготыЗаполняетПолеДействуетПо(string dateTo)
        {
            if (dateTo == "текущая дата")
            {
                PersonalAccountPrivilegedCategoryHelper.Current.DateTo = DateTime.Today;

            }
            else
            {
                PersonalAccountPrivilegedCategoryHelper.Current.DateTo = DateTime.Parse(dateTo);
            }
        }
        
        [When(@"пользователь сохраняет эту категорию льготы")]
        public void ЕслиПользовательСохраняетЭтуКатегориюЛьготы()
        {
            try
            {
                domainService.SaveOrUpdate(PersonalAccountPrivilegedCategoryHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту категорию льготы")]
        public void ЕслиПользовательУдаляетЭтуКатегориюЛьготы()
        {
            try
            {
                domainService.Delete(PersonalAccountPrivilegedCategoryHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой категории льготы присутствует в списке категорий льгот у этого лицевого счета")]
        public void ТоЗаписьПоЭтойКатегорииЛьготыПрисутствуетВСпискеКатегорийЛьготУЭтогоЛицевогоСчета()
        {
            domainService.Get(PersonalAccountPrivilegedCategoryHelper.Current.Id)
                .Should()
                .NotBeNull(
                    string.Format(
                        "Запись категории льготы должна присутствовать в списке категорий льгот у этого лицевого счета. {0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой категории льготы отсутствует в списке категорий льгот у этого лицевого счета")]
        public void ТоЗаписьПоЭтойКатегорииЛьготыОтсутствуетВСпискеКатегорийЛьготУЭтогоЛицевогоСчета()
        {
            domainService.Get(PersonalAccountPrivilegedCategoryHelper.Current.Id).Should().BeNull("Запись категории льготы должна отсутсвовать в списке категорий льгот у этого лицевого счета");

        }
    }
}
