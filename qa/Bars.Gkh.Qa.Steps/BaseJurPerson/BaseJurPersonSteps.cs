namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class BaseJurPersonSteps : BindingBase
    {
        private DynamicDictionary paramsDictionary = new DynamicDictionary();

        private dynamic ds
        {
            get
            {
                Type entityType = Type.GetType("Bars.GkhGji.Entities.BaseJurPerson, Bars.GkhGji");
                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новую плановую проверку юридических лиц")]
        public void ДопустимПользовательДобавляетНовуюПлановуюПроверкуЮридическихЛиц()
        {
            paramsDictionary.Add("Id","0");
            paramsDictionary.Add("TypeBase", "30");
            paramsDictionary.Add("DisposalId", "");
            paramsDictionary.Add("InspectionYear", "");
            paramsDictionary.Add("InspectionNum", "");
            paramsDictionary.Add("InspectionNumber", "");
            paramsDictionary.Add("PhysicalPerson", "");
            paramsDictionary.Add("PhysicalPersonInfo", "");
            paramsDictionary.Add("TypeBaseJuralPerson", 10);
            paramsDictionary.Add("TypeFact", 10);
            paramsDictionary.Add("TypeForm", 10);
            paramsDictionary.Add("DisposalNumber", "");
            paramsDictionary.Add("InspectorNames", "");
            paramsDictionary.Add("RealityObjectCount", "");
        }
        
        [Given(@"пользователь у этой плановой проверки юридических лиц заполняет поле Тип юридического лица ""(.*)""")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеТипЮридическогоЛица(string contragentType)
        {
            switch (contragentType)
            {
                case "Управляющая организация":
                    paramsDictionary.Add("TypeJurPerson", "10");
                    break;
                case "Поставщик коммунальных услуг":
                    paramsDictionary.Add("TypeJurPerson", "20");
                    break;
                case "Орган местного самоуправления":
                    paramsDictionary.Add("TypeJurPerson", "30");
                    break;
                case "Орган государственной власти":
                    paramsDictionary.Add("TypeJurPerson", "40");
                    break;
                case "Поставщик жилищных услуг":
                    paramsDictionary.Add("TypeJurPerson", "60");
                    break;
                case "Организация-арендатор":
                    paramsDictionary.Add("TypeJurPerson", "70");
                    break;
                case "Региональный оператор":
                    paramsDictionary.Add("TypeJurPerson", "80");
                    break;
                case "Обслуживающая компания":
                    paramsDictionary.Add("TypeJurPerson", "90");
                    break;
                case "ТСЖ, ЖСК, специализированный кооператив":
                    paramsDictionary.Add("TypeJurPerson", "100");
                    break;
                case "Организация - собственник":
                    paramsDictionary.Add("TypeJurPerson", "110");
                    break;
                case "Ресурсоснабжающая организация":
                    paramsDictionary.Add("TypeJurPerson", "120");
                    break;
                default:
                    throw new SpecFlowException("Нет такого типа юридического лица: " + contragentType);
            }
        }
        
        [Given(@"пользователь у этой плановой проверки юридических лиц заполняет поле Юридическое лицо этим контрагентом")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеЮридическоеЛицоЭтимКонтрагентом()
        {
            paramsDictionary.Add("Contragent", ContragentHelper.CurrentContragent.Id);
        }
        
        [Given(@"пользователь у этой плановой проверки юридических лиц заполняет поле План этим планом")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеПланЭтимПланом()
        {
            paramsDictionary.Add("Plan", PlanJurPersonGjiHelper.Current.Id);
        }
        
        [Given(@"пользователь у этой плановой проверки юридических лиц заполняет поле Дата начала проверки ""(.*)""")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеДатаНачалаПроверки(string dateStart)
        {
            paramsDictionary.Add("DateStart", DateTime.Now);
        }
        
        [Given(@"пользователь у этой плановой проверки юридических лиц заполняет поле Инспекторы этим инспектором")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеИнспекторыЭтимИнспектором()
        {
            paramsDictionary.Add("JurPersonInspectors", InspectorHelper.Current.Id);
        }
        
        [When(@"пользователь сохраняет эту плановую проверку юридических лиц")]
        public void ЕслиПользовательСохраняетЭтуПлановуюПроверкуЮридическихЛиц()
        {
            BaseParams baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "records", new List<DynamicDictionary>
                                       {
                                           this.paramsDictionary
                                       }
                    }
                }
            };

            try
            {
                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
                BaseJurPersonHelper.Current = this.ds.Save(baseParams).Data[0];
                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
        }
        
        [When(@"пользователь удаляет эту плановую проверку юридических лиц")]
        public void ЕслиПользовательУдаляетЭтуПлановуюПроверкуЮридическихЛиц()
        {
            try
            {
                ds.Delete(BaseJurPersonHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой плановой проверки юридических лиц присутствует в справочнике плановых проверок юридических лиц")]
        public void ТоЗаписьПоЭтойПлановойПроверкиЮридическихЛицПрисутствуетВСправочникеПлановыхПроверокЮридическихЛиц()
        {
            ((object)ds.Get(BaseJurPersonHelper.Current.Id)).Should()
                .NotBeNull(
                    string.Format(
                        "запись по этой плановой проверки юридических лиц должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой плановой проверки юридических лиц отсутствует в справочнике плановых проверок юридических лиц")]
        public void ТоЗаписьПоЭтойПлановойПроверкиЮридическихЛицОтсутствуетВСправочникеПлановыхПроверокЮридическихЛиц()
        {
            ((object)ds.Get(BaseJurPersonHelper.Current.Id)).Should()
                .BeNull(
                    string.Format(
                        "запись по этой плановой проверки юридических лиц должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлена плановая проверка юридических лиц")]
        public void ДопустимДобавленаПлановаяПроверкаЮридическихЛиц(Table table)
        {
            var typeJurPerson = table.Rows[0]["TypeJurPerson"];
            ДопустимПользовательДобавляетНовуюПлановуюПроверкуЮридическихЛиц();
            ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицЗаполняетПолеТипЮридическогоЛица(typeJurPerson);
            paramsDictionary.Add("Contragent", ContragentHelper.CurrentContragent.Id);
            paramsDictionary.Add("Plan", PlanJurPersonGjiHelper.Current.GetType().GetProperty("Id").GetValue(PlanJurPersonGjiHelper.Current));
            paramsDictionary.Add("DateStart", DateTime.Now);
            paramsDictionary.Add("JurPersonInspectors", InspectorHelper.Current.Id);
            ЕслиПользовательСохраняетЭтуПлановуюПроверкуЮридическихЛиц();
        }
    }
}
