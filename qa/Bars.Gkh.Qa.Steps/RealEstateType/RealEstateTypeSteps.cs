using Bars.Gkh.Entities;
using NHibernate.Util;
using TechTalk.SpecFlow.Assist;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class RealEstateTypeSteps : BindingBase
    {

        private IDomainService<RealEstateType> ds = Container.Resolve<IDomainService<RealEstateType>>(); 
        
        [Given(@"пользователь добавляет новый Тип дома")]
        public void ДопустимПользовательДобавляетНовыйТипДома()
        {
            RealEstateTypeHelper.Current = new RealEstateType();
        }

        [Given(@"пользователь у этого Типа дома заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаДомаЗаполняетПолеНаименование(string name)
        {
            RealEstateTypeHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого Типа дома заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаДомаЗаполняетПолеКод(string code)
        {
            RealEstateTypeHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого Типа дома заполняет поле Предельная стоимость ремонта ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаДомаЗаполняетПолеПредельнаяСтоимостьРемонта(decimal cost)
        {
            RealEstateTypeHelper.Current.MarginalRepairCost = cost;
        }


        [Given(@"пользователь у этого Типа дома заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаДомаЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            RealEstateTypeHelper.Current.Name = new string(ch, count);
        }
        
        [Given(@"пользователь у этого Типа дома заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаДомаЗаполняетПолеКодСимволов(int count, char ch)
        {
            RealEstateTypeHelper.Current.Code = new string(ch, count);
        }

        [When(@"пользователь сохраняет этот Тип дома")]
        public void ЕслиПользовательСохраняетЭтотТипДома()
        {
            try
            {
                ds.SaveOrUpdate(RealEstateTypeHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"пользователь удаляет этот Тип дома")]
        public void ТоПользовательУдаляетЭтотТипДома()
        {
            try
            {
                ds.Delete(RealEstateTypeHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому Типу дома присутствует в справочнике Типов домов")]
        public void ТоЗаписьПоЭтомуТипуДомаПрисутствуетВСправочникеТиповДомов()
        {
            ds.Get(RealEstateTypeHelper.Current.Id).Should().NotBeNull(
                    string.Format(
                        "тип дома должен присутствовать в справочнике{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому Типу дома отсутствует в справочнике Типов домов")]
        public void ТоЗаписьПоЭтомуТипуДомаОтсутствуетВСправочникеТиповДомов()
        {
            ds.Get(RealEstateTypeHelper.Current.Id).Should().BeNull(
                    string.Format(
                        "тип дома должен отсутствовать в справочнике{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь добавляет Общую характеристику типа жилых домов")]
        public void ДопустимПользовательДобавляетОбщуюХарактеристикуТипаЖилыхДомов()
        {
            RealEstateTypeCommonParamHelper.Current = new RealEstateTypeCommonParam()
            {
                RealEstateType = RealEstateTypeHelper.Current
            };
        }

        [Given(@"заполняет у этого Общего параметра поле Наименование ""(.*)""")]
        public void ДопустимЗаполняетУЭтогоОбщегоПараметраПолеНаименование(string name)
        {
            RealEstateTypeCommonParamHelper.Current.CommonParamCode = name;
        }

        [Given(@"заполняет у этого Общего параметра поле Минимальное значение ""(.*)""")]
        public void ДопустимЗаполняетУЭтогоОбщегоПараметраПолеМинимальноеЗначение(string min)
        {
            RealEstateTypeCommonParamHelper.Current.Min = min;
        }

        [Given(@"заполняет у этого Общего параметра поле Максимальное значение ""(.*)""")]
        public void ДопустимЗаполняетУЭтогоОбщегоПараметраПолеМаксимальноеЗначение(string max)
        {
            RealEstateTypeCommonParamHelper.Current.Max = max;
        }

        [When(@"пользователь сохраняет Общую характеристику типа жилых домов")]
        public void ЕслиПользовательСохраняетОбщуюХарактеристикуТипаЖилыхДомов()
        {
            try
            {
                Container.Resolve<IDomainService<RealEstateTypeCommonParam>>().SaveOrUpdate(RealEstateTypeCommonParamHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой Общей характеристике присутствует в Типе дома")]
        public void ТоЗаписьПоЭтойОбщейХарактеристикеПрисутствуетВТипеДома()
        {
            Container.Resolve<IDomainService<RealEstateTypeCommonParam>>().
                Get(RealEstateTypeCommonParamHelper.Current.Id).Should().NotBeNull(String.Format("Общая характеристика должна присутствовать в Типе дома{0}", ExceptionHelper.GetExceptions()));
        }

        [When(@"пользователь удаляет Общую характеристику типа жилых домов")]
        public void ЕслиПользовательУдаляетОбщуюХарактеристикуТипаЖилыхДомов()
        {
            try
            {
                Container.Resolve<IDomainService<RealEstateTypeCommonParam>>().Delete(RealEstateTypeCommonParamHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой Общей характеристике отсутствует в Типе дома")]
        public void ТоЗаписьПоЭтойОбщейХарактеристикеОтсутствуетВТипеДома()
        {
            Container.Resolve<IDomainService<RealEstateTypeCommonParam>>().
                Get(RealEstateTypeCommonParamHelper.Current.Id).Should().BeNull(String.Format("Общая характеристика должна отсутствовать в Типе дома{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь добавляет Конструктивный элемент типа жилых домов")]
        public void ДопустимПользовательДобавляетКонструктивныйЭлементТипаЖилыхДомов()
        {
            RealEstateTypeStructElementHelper.Current = new RealEstateTypeStructElement()
            {
                RealEstateType = RealEstateTypeHelper.Current,
                StructuralElement = StructuralElementHelper.Current
            };
        }

        [Given(@"заполняет поле КЭ присутствует отсутствует в доме ""(.*)""")]
        public void ДопустимЗаполняетПолеКЭПрисутствуетОтсутствуетВДоме(bool exists)
        {
            RealEstateTypeStructElementHelper.Current.Exists = exists;
        }

        [When(@"пользователь сохраняет Конструктивный элемент типа жилых домов")]
        public void ЕслиПользовательСохраняетКонструктивныйЭлементТипаЖилыхДомов()
        {
            try
            {
                Container.Resolve<IDomainService<RealEstateTypeStructElement>>().SaveOrUpdate(RealEstateTypeStructElementHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому Конструктивному элементу присутствует в Типе дома")]
        public void ТоЗаписьПоЭтомуКонструктивномуЭлементуПрисутствуетВТипеДома()
        {
            Container.Resolve<IDomainService<RealEstateTypeStructElement>>().
               Get(RealEstateTypeStructElementHelper.Current.Id).Should().NotBeNull(String.Format("Конструктивный элемент должен присутствовать в Типе дома{0}", ExceptionHelper.GetExceptions()));
        }

        [When(@"пользователь удаляет Конструктивный элемент типа жилых домов")]
        public void ЕслиПользовательУдаляетКонструктивныйЭлементТипаЖилыхДомов()
        {
            try
            {
                Container.Resolve<IDomainService<RealEstateTypeStructElement>>().Delete(RealEstateTypeStructElementHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому Конструктивному элементу отсутствует в Типе дома")]
        public void ТоЗаписьПоЭтомуКонструктивномуЭлементуОтсутствуетВТипеДома()
        {
            Container.Resolve<IDomainService<RealEstateTypeStructElement>>().
               Get(RealEstateTypeStructElementHelper.Current.Id).Should().BeNull(String.Format("Конструктивный элемент должен отсутствовать в Типе дома{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
