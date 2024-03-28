using System;
using System.ComponentModel;
using System.Globalization;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using NHibernate.Linq.Clauses;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities.Dict;

    [Binding]
    public class PrivilegedCategorySteps : BindingBase
    {

        private IDomainService DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<PrivilegedCategory>>();
            }
        }

        [Given(@"пользователь добавляет новую группу льготных категорий граждан")]
        public void ДопустимПользовательДобавляетНовуюГруппуЛьготныхКатегорийГраждан()
        {
            PrivilegedCategoryHelper.Current = new PrivilegedCategory();
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеКод(string code)
        {
            PrivilegedCategoryHelper.Current.Code = code;
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеНаименование(string name)
        {
            PrivilegedCategoryHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеПроцентЛьготы(decimal percent)
        {
            PrivilegedCategoryHelper.Current.Percent = percent;
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеПредельноеЗначениеПлощади(decimal limitArea)
        {
            PrivilegedCategoryHelper.Current.LimitArea = limitArea;
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Действует с ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеДействуетС(string dateFrom)
        {
            PrivilegedCategoryHelper.Current.DateFrom = DateTime.Parse(dateFrom);
        }
        
        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Действует по ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеДействуетПо(string dateTo)
        {
            PrivilegedCategoryHelper.Current.DateTo = DateTime.Parse(dateTo);
        }
        
        [When(@"пользователь сохраняет эту группу льготных категорий граждан")]
        public void ЕслиПользовательСохраняетЭтуГруппуЛьготныхКатегорийГраждан()
        {
            var id = PrivilegedCategoryHelper.Current.Id;
            
            try
            {
                if (id == 0)
                {
                    Container.Resolve<IDomainService<PrivilegedCategory>>().Get((long)0);

                    this.DomainService.Save(PrivilegedCategoryHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PrivilegedCategoryHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту группу льготных категорий граждан")]
        public void ЕслиПользовательУдаляетЭтуГруппуЛьготныхКатегорийГраждан()
        {
            var id = PrivilegedCategoryHelper.Current.Id;

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой группе льготных категорий граждан присутствует в справочнике групп льготных категорий граждан")]
        public void ТоЗаписьПоЭтойГруппеЛьготныхКатегорийГражданПрисутствуетВСправочникеГруппЛьготныхКатегорийГраждан()
        {
            var id = PrivilegedCategoryHelper.Current.Id;

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этой группе льготных категорий граждан должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой группе льготных категорий граждан отсутствует в справочнике групп льготных категорий граждан")]
        public void ТоЗаписьПоЭтойГруппеЛьготныхКатегорийГражданОтсутствуетВСправочникеГруппЛьготныхКатегорийГраждан()
        {
            var id = PrivilegedCategoryHelper.Current.Id;

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этой группе льготных категорий граждан должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеКодСимволов(int count, char ch)
        {
            PrivilegedCategoryHelper.Current.Code = new string(ch, count);
        }

        [Given(@"пользователь у этой группы льготных категорий граждан заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыЛьготныхКатегорийГражданЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            PrivilegedCategoryHelper.Current.Name = new string(ch, count);
        }

    }
}
