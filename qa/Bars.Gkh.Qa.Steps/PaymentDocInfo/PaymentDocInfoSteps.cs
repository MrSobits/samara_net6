namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PaymentDocInfoSteps : BindingBase
    {
        [Given(@"пользователь добавляет новую информацию для физ\.лиц")]
        public void ДопустимПользовательДобавляетНовуюИнформациюДляФиз_Лиц()
        {
            PaymentDocInfoHelper.Current = Activator.CreateInstance(PaymentDocInfoHelper.Type);
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Дата действия с ""(.*)""")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеДатаДействияС(string dateStart)
        {
            DateTime date;

            if (!DateTime.TryParse(dateStart, out date))
            {
                throw new SpecFlowException("Не правильный формат даты в заполнении \"Дата действия с\" в информации для физ.лиц");
            }

            PaymentDocInfoHelper.Current.DateStart = date;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Дата действия по ""(.*)""")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеДатаДействияПо(string dateEnd)
        {
            DateTime date;

            if (!DateTime.TryParse(dateEnd, out date))
            {
                throw new SpecFlowException("Не правильный формат даты в заполнении \"Дата действия по\" в информации для физ.лиц");
            }

            PaymentDocInfoHelper.Current.DateEnd = date;
        }


        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Информационное поле ""(.*)""")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеИнформационноеПоле(string information)
        {
            PaymentDocInfoHelper.Current.Information = information;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Способ формирования фонда ""(.*)""")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеСпособФормированияФонда(string fundFormationTypeDisplay)
        {
            var enumType = Type.GetType("Bars.Gkh.RegOperator.Enums.FundFormationType, Bars.Gkh.RegOperator");

            var enumValue = EnumHelper.GetFromDisplayValue(enumType, fundFormationTypeDisplay);

            PaymentDocInfoHelper.Current.FundFormationType = enumValue;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Для региона ""(.*)""")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеДляРегиона(bool isForRegion)
        {
            PaymentDocInfoHelper.Current.IsForRegion = isForRegion;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Муниципальный район")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеМуниципальныйРайон()
        {
            PaymentDocInfoHelper.Current.Municipality = MunicipalityHelper.Current;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Муниципальное образование")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеМуниципальноеОбразование()
        {
            PaymentDocInfoHelper.Current.MoSettlement = MunicipalityHelper.Current;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Жилой дом")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеЖилойДом()
        {
            PaymentDocInfoHelper.Current.RealityObject = RealityObjectHelper.CurrentRealityObject;
        }

        [Given(@"пользователь у этой информации для физ\.лиц заполняет поле Населенный пункт")]
        public void ДопустимПользовательУЭтойИнформацииДляФиз_ЛицЗаполняетПолеНаселенныйПункт(Table table)
        {
            var context = (GkhContext)ApplicationContext.Current;

            if (table.Rows.All(x => x["region"] != context.Region))
            {
                Assert.Ignore();
            }

            var currentRegionRow = table.Rows.First(x => x["region"] == context.Region);

            var locality = currentRegionRow["Locality"];

            var fiasRep = Container.Resolve<IFiasRepository>();

            var list = fiasRep.GetPlacesDinamicAddress(locality).ToList();

            if (list.IsEmpty())
            {
                throw new Exception("Не найдено ни одного Фиаса по заданному населённому пункту");
            }

            var fiasGuid = list.First().GuidId;

            var fias = fiasRep.GetAll().FirstOrDefault(x => x.AOGuid == fiasGuid);

            if (fias == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует Фиас по населённому пункту {0}", locality));
            }

            PaymentDocInfoHelper.Current.Locality = fias;
        }

        [When(@"пользователь сохраняет эту информацию для физ\.лиц")]
        public void ЕслиПользовательСохраняетЭтуИнформациюДляФиз_Лиц()
        {
            try
            {
                if (PaymentDocInfoHelper.Current.Id > 0)
                {
                    PaymentDocInfoHelper.DomainService.Update(PaymentDocInfoHelper.Current);
                }
                else
                {
                    PaymentDocInfoHelper.DomainService.Save(PaymentDocInfoHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту информацию для физ\.лиц")]
        public void ЕслиПользовательУдаляетЭтуИнформациюДляФиз_Лиц()
        {
            try
            {
                PaymentDocInfoHelper.DomainService.Delete(PaymentDocInfoHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        [Then(@"запись по этой информации для физ\.лиц присутствует в разделе информации для физ\.лиц")]
        public void ТоЗаписьПоЭтойИнформацииДляФиз_ЛицПрисутствуетВРазделеИнформацииДляФиз_Лиц()
        {
            var paymentDocInfo = PaymentDocInfoHelper.DomainService.Get(PaymentDocInfoHelper.Current.Id) as object;

            paymentDocInfo.Should().NotBeNull(
                string.Format(
                "запись по этой информации для физ.лиц должна присутствовать в разделе информации для физ.лиц. {0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой информации для физ\.лиц отсутствует в разделе информации для физ\.лиц")]
        public void ТоЗаписьПоЭтойИнформацииДляФиз_ЛицОтсутствуетВРазделеИнформацииДляФиз_Лиц()
        {
            var paymentDocInfo = PaymentDocInfoHelper.DomainService.Get(PaymentDocInfoHelper.Current.Id) as object;

            paymentDocInfo.Should().BeNull(
                string.Format(
                "запись по этой информации для физ.лиц должна отсутствовать в разделе информации для физ.лиц. {0}",
                ExceptionHelper.GetExceptions()));
        }
    }
}
