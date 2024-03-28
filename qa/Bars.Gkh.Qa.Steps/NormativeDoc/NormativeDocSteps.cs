using System;
using System.ComponentModel;
using System.Web.Configuration;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Enums;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class NormativeDocSteps : BindingBase
    {

        private IDomainService<NormativeDoc> ds = Container.Resolve<IDomainService<NormativeDoc>>();
        private IDomainService<NormativeDocItem> dsItem = Container.Resolve<IDomainService<NormativeDocItem>>();

        [Given(@"пользователь добавляет новый нормативно-правовой документ")]
        public void ДопустимПользовательДобавляетНовыйНормативно_ПравовойДокумент()
        {
            NormativeDocHelper.Current = new NormativeDoc();
        }
        
        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Полное наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеПолноеНаименование(string fullName)
        {
            NormativeDocHelper.Current.FullName = fullName;
        }
        
        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеНаименование(string name)
        {
            NormativeDocHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеКод(int code)
        {
            NormativeDocHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Категория ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеКатегория(string normativeDocCategory)
        {
            switch (normativeDocCategory)
            {
                case "Жилищный надзор":
                    NormativeDocHelper.Current.Category = NormativeDocCategory.HousingSupervision;
                    break;
                case "Жилищный фонд":
                    NormativeDocHelper.Current.Category = NormativeDocCategory.HousingFund;
                    break;
                case "Капитальный ремонт":
                    NormativeDocHelper.Current.Category = NormativeDocCategory.Overhaul;
                    break;
                default:
                    throw new Exception("Нет такой категории нормативного документа");
            }
        }

        [When(@"пользователь сохраняет этот нормативно-правовой документ")]
        public void ЕслиПользовательСохраняетЭтотНормативно_ПравовойДокумент()
        {
            try
            {
                ds.SaveOrUpdate(NormativeDocHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот нормативно-правовой документ")]
        public void ЕслиПользовательУдаляетЭтотНормативно_ПравовойДокумент()
        {
            try
            {
                ds.Delete(NormativeDocHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому нормативно-правовому документу присутствует в справочнике нормативно-правовых документов")]
        public void ТоЗаписьПоЭтомуНормативно_ПравовомуДокументуПрисутствуетВСправочникеНормативно_ПравовыхДокументов()
        {
            ds.Get(NormativeDocHelper.Current.Id).Should().NotBeNull();
        }

        [Then(@"запись по этому нормативно-правовому документу отсутствует в справочнике нормативно-правовых документов")]
        public void ТоЗаписьПоЭтомуНормативно_ПравовомуДокументуОтсутствуетВСправочникеНормативно_ПравовыхДокументов()
        {
            ds.Get(NormativeDocHelper.Current.Id).Should().BeNull();
        }
        
        [When(@"пользователь добавляет новый пункт в нормативно-правовой документ")]
        public void ЕслиПользовательДобавляетНовыйПунктВНормативно_ПравовойДокумент()
        {
            NormativeDocItemHelper.Current = new NormativeDocItem()
            {
                NormativeDoc = NormativeDocHelper.Current
            };
        }


        [When(@"пользователь у этого пункта нормативно-правового документа заполняет поле Номер ""(.*)""")]
        public void ЕслиПользовательУЭтогоПунктаНормативно_ПравовогоДокументаЗаполняетПолеНомер(string num)
        {
            NormativeDocItemHelper.Current.Number = num;
        }

        [When(@"пользователь у этого пункта нормативно-правового документа заполняет поле Текст ""(.*)""")]
        public void ЕслиПользовательУЭтогоПунктаНормативно_ПравовогоДокументаЗаполняетПолеТекст(string text)
        {
            NormativeDocItemHelper.Current.Text = text;
        }

        [When(@"пользователь сохраняет пункт нормативно-правового документа")]
        public void ЕслиПользовательСохраняетПунктНормативно_ПравовогоДокумента()
        {
            try
            {
                dsItem.SaveOrUpdate(NormativeDocItemHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому пункту нормативно-правового документа присутствует в этом нормативно-правововом документе")]
        public void ТоЗаписьПоЭтомуПунктуНормативно_ПравовогоДокументаПрисутствуетВЭтомНормативно_ПравововомДокументе()
        {
            dsItem.Get(NormativeDocItemHelper.Current.Id).Should().NotBeNull();
        }

        [Then(@"запись по этому пункту нормативно-правового документа отсутствует в этом нормативно-правововом документе")]
        public void ТоЗаписьПоЭтомуПунктуНормативно_ПравовогоДокументаОтсутствуетВЭтомНормативно_ПравововомДокументе()
        {
            dsItem.Get(NormativeDocItemHelper.Current.Id).Should().BeNull();
        }

        [When(@"пользователь удаляет пункт нормативно-правового документа")]
        public void ЕслиПользовательУдаляетПунктНормативно_ПравовогоДокумента()
        {
            try
            {
                dsItem.Delete(NormativeDocItemHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Полное наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеПолноеНаименованиеСимволов(int length, char symbol)
        {
            NormativeDocHelper.Current.FullName = new string(symbol, length);
        }

        [Given(@"пользователь у этого нормативно-правового документа заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоНормативно_ПравовогоДокументаЗаполняетПолеНаименованиеСимволов(int length, char symbol)
        {
            NormativeDocHelper.Current.Name = new string(symbol, length);
        }

    }
}
