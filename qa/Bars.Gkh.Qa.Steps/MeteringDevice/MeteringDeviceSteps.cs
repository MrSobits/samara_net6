namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    class MeteringDeviceSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<MeteringDevice> _cashe = new BindingBase.DomainServiceCashe<MeteringDevice>();

        [Given(@"пользователь добавляет новый прибор учета")]
        public void ДопустимПользовательДобавляетНовыйПриборУчета()
        {
            MeteringDeviceHelper.Current = new MeteringDevice();
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеНаименование(string name)
        {
            MeteringDeviceHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Класс точности ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеКлассТочности(string accuracyClass)
        {
            MeteringDeviceHelper.Current.AccuracyClass = accuracyClass;
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеОписание(string description)
        {
            MeteringDeviceHelper.Current.Description = description;
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Тип учета ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеТипУчета(string typeAccountingName)
        {
            TypeAccounting typeAccounting;

            if (typeAccountingName == "Индивидуальный")
            {
                typeAccounting = TypeAccounting.Individual;
            }
            else if (typeAccountingName == "Общедомовой")
            {
                typeAccounting = TypeAccounting.Social;
            }
            else
            {
                throw new SpecFlowException("Указан не правильный Тип учета");
            }

                MeteringDeviceHelper.Current.TypeAccounting = typeAccounting;
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            MeteringDeviceHelper.Current.Name =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Класс точности (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеКлассТочностиСимволов(int countOfSymbols, string symbol)
        {
            MeteringDeviceHelper.Current.AccuracyClass =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого прибора учета заполняет поле Описание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПрибораУчетаЗаполняетПолеОписаниеСимволов(int countOfSymbols, string symbol)
        {
            MeteringDeviceHelper.Current.Description =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }


        [When(@"пользователь сохраняет этот прибор учета")]
        public void ЕслиПользовательСохраняетЭтотПриборУчета()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(MeteringDeviceHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот прибор учета")]
        public void ЕслиПользовательУдаляетЭтотПриборУчета()
        {
            try
            {
                this._cashe.Current.Delete(MeteringDeviceHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому прибору учета присутствует в справочнике приборов учета")]
        public void ТоЗаписьПоЭтомуПриборуУчетаПрисутствуетВСправочникеПриборовУчета()
        {
            var meteringDevice = this._cashe.Current.Get(MeteringDeviceHelper.Current.Id);

            meteringDevice.Should().NotBeNull(
                string.Format("прибор учета должен присутствовать в справочнике приборов учета.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому прибору учета отсутствует в справочнике приборов учета")]
        public void ТоЗаписьПоЭтомуПриборуУчетаОтсутствуетВСправочникеПриборовУчета()
        {
            var meteringDevice = this._cashe.Current.Get(MeteringDeviceHelper.Current.Id);

            meteringDevice.Should().BeNull(
                string.Format("прибор учета должен отсутствовать в справочнике приборов учета.{0}",
                ExceptionHelper.GetExceptions()));
        }
    }
}
