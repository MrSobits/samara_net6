using System.Collections;
using System.ComponentModel;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Qa.Utils;
using Bars.GkhCr.DomainService;
using Bars.GkhCr.Enums;
using FluentAssertions;

namespace Bars.Gkh.Qa.Steps
{
    
    using System;
    using Bars.GkhCr.Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class FinanceSourceSteps : BindingBase
    {
        private IDomainService<FinanceSource> ds = Container.Resolve<IDomainService<FinanceSource>>();

        [Given(@"пользователь добавляет новый разрез финансирования")]
        public void ДопустимПользовательДобавляетНовыйРазрезФинансирования()
        {
            FinanceSourceHelper.Current = new FinanceSource();
        }
        
        [Given(@"пользователь у этого разреза финансирования заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазрезаФинансированияЗаполняетПолеНаименование(string name)
        {
            FinanceSourceHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого разреза финансирования заполняет поле Группа финансирования ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазрезаФинансированияЗаполняетПолеГруппаФинансирования(string typeFinanceGroup)
        {
            switch (typeFinanceGroup)
            {
                case "Другие":
                    FinanceSourceHelper.Current.TypeFinanceGroup = TypeFinanceGroup.Other;
                    break;
                case "Программа КР":
                    FinanceSourceHelper.Current.TypeFinanceGroup = TypeFinanceGroup.ProgramCr;
                    break;
                default:
                    throw new Exception("Нет такой группы финансирования");
            }
        }
        
        [Given(@"пользователь у этого разреза финансирования заполняет поле Тип разреза ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазрезаФинансированияЗаполняетПолеТипРазреза(string typeFinance)
        {
            switch (typeFinance)
            {
                case "Другие":
                    FinanceSourceHelper.Current.TypeFinance = TypeFinance.Other;
                    break;
                case "Лизинг":
                    FinanceSourceHelper.Current.TypeFinance = TypeFinance.Leasing;
                    break;
                case "Не указано":
                    FinanceSourceHelper.Current.TypeFinance = TypeFinance.NotDefined;
                    break;
                case "Федеральный закон":
                    FinanceSourceHelper.Current.TypeFinance = TypeFinance.FederalLaw;
                    break;
                default:
                    throw new Exception("Нет такого типа разреза");
            }
        }

        [Given(@"добавлены разрезы финансирования")]
        public void ДопустимДобавленыРазрезыФинансирования(Table table)
        {
            foreach (var row in table.Rows)
            {
                var financeSource = new FinanceSource();

                financeSource.Name = row["Name"];
                financeSource.TypeFinanceGroup =
                    EnumHelper.GetFromDisplayValue<TypeFinanceGroup>(row["TypeFinanceGroup"]);

                financeSource.TypeFinance =
                    EnumHelper.GetFromDisplayValue<TypeFinance>(row["TypeFinance"]);

                financeSource.Code = row["Code"];

                ds.Save(financeSource);
            }
        }
        
        [When(@"пользователь сохраняет этот разрез финансирования")]
        public void ЕслиПользовательСохраняетЭтотРазрезФинансирования()
        {
            try
            {
                ds.SaveOrUpdate(FinanceSourceHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот разрез финансирования")]
        public void ЕслиПользовательУдаляетЭтотРазрезФинансирования()
        {
            try
            {
                ds.Delete(FinanceSourceHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"пользователь у этого рзареза финансирования заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоРзарезаФинансированияЗаполняетПолеКод(string code)
        {
            FinanceSourceHelper.Current.Code = code;
        }
        
        [Then(@"запись по этому разрезу финансирования присутствует в справочнике")]
        public void ТоЗаписьПоЭтомуРазрезуФинансированияПрисутствуетВСправочнике()
        {
            ds.Get(FinanceSourceHelper.Current.Id).Should().NotBeNull(string.Format("Разрез финансирования должен присутстовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому разрезу финансирования отсутствует в справочнике")]
        public void ТоЗаписьПоЭтомуРазрезуФинансированияОтсутствуетВСправочнике()
        {
            ds.Get(FinanceSourceHelper.Current.Id).Should().BeNull(string.Format("Разрез финансирования должен отсутствовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого разреза финансирования заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазрезаФинансированияЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            FinanceSourceHelper.Current.Name = new string(ch, count);
        }

        [Given(@"пользователь у этого разреза финансирования заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазрезаФинансированияЗаполняетПолеКодСимволов(int count, char ch)
        {
            FinanceSourceHelper.Current.Code = new string(ch, count);
        }

        [When(@"пользователь разрезу финансирования добавляет вид работ")]
        public void ЕслиПользовательРазрезуФинансированияДобавляетВидРабот()
        {
            var baseParams = new BaseParams()
                                 {
                                     Params =
                                         new DynamicDictionary()
                                             {
                                                 {
                                                     "financeSourceId",
                                                     FinanceSourceHelper.Current.Id
                                                 },
                                                 { "objectIds", WorkHelper.Current.Id }
                                             }
                                 };

            Container.Resolve<IFinanceSourceWorkService>().AddWorks(baseParams);

            //id только что созданной FinanceSourceWork
            ScenarioContext.Current["financeSourceWorkId"] =
                Container.Resolve<IDomainService<FinanceSourceWork>>()
                    .GetAll()
                    .First(x => x.FinanceSource.Id == FinanceSourceHelper.Current.Id)
                    .Id;
        }

        [Then(@"запись по этому виду работ присутствует в разрезе финансирования")]
        public void ТоЗаписьПоЭтомуВидуРаботПрисутствуетВРазрезеФинансирования()
        {
            Container.Resolve<IDomainService<FinanceSourceWork>>().GetAll()
                .Where(x => x.FinanceSource.Id == FinanceSourceHelper.Current.Id).Should().NotBeEmpty(string.Format("вид работы должен присутствовать в разрезе финансирования.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому виду работ отсутствует в разрезе финансирования")]
        public void ТоЗаписьПоЭтомуВидуРаботОтсутствуетВРазрезеФинансирования()
        {
            Container.Resolve<IDomainService<FinanceSourceWork>>().GetAll()
               .Where(x => x.FinanceSource.Id == FinanceSourceHelper.Current.Id).Should().BeEmpty(string.Format("вид работы должен присутствовать в разрезе финансирования.{0}", ExceptionHelper.GetExceptions()));
        }

        [When(@"пользователь удаляет вид работ из разреза финансирования")]
        public void ЕслиПользовательУдаляетВидРаботИзРазрезаФинансирования()
        {
            Container.Resolve<IDomainService<FinanceSourceWork>>().Delete((long)ScenarioContext.Current["financeSourceWorkId"]);
        }

        [Then(@"дубли записей по этим видам работ отсутствуют в этом разрезе финансирования")]
        public void ТоДублиЗаписейПоЭтимВидамРаботОтсутствуютВЭтомРазрезеФинансирования()
        {
            var financeSourceWork =
                Container.Resolve<IDomainService<FinanceSourceWork>>()
                    .GetAll()
                    .AsEnumerable()
                    .Where(x => x.FinanceSource.Id == FinanceSourceHelper.Current.Id && x.Work.Id == WorkHelper.Current.Id)
                    .ToList();

            (financeSourceWork.Count == 1).Should()
                .BeTrue(string.Format("дубли вида работы должны отсутствовать в разрезе финансирования.{0}",
                    ExceptionHelper.GetExceptions()));
        }   

    }
}
