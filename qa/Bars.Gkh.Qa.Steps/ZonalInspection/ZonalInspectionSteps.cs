using System;
using System.Linq;
using FluentAssertions;

namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class ZonalInspectionSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<ZonalInspection> _cashe = new BindingBase.DomainServiceCashe<ZonalInspection>();

        [Given(@"пользователь добавляет новую зональную жилищную инспекцию")]
        public void ДопустимПользовательДобавляетНовуюЗональнуюЖилищнуюИнспекцию()
        {
            ZonalInspectionHelper.Current = new ZonalInspection();
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеНаименование(string name)
        {
            ZonalInspectionHelper.Current.Name = name;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Зональное наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеЗональноеНаименование(string zoneName)
        {
            ZonalInspectionHelper.Current.ZoneName = zoneName;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Наименование для бланка ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеНаименованиеДляБланка(string blankName)
        {
            ZonalInspectionHelper.Current.BlankName = blankName;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Краткое наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеКраткоеНаименование(string shortName)
        {
            ZonalInspectionHelper.Current.ShortName = shortName;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Адрес ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеАдрес(string address)
        {
            ZonalInspectionHelper.Current.Address = address;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет второе поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетВтороеПолеНаименование(string nameSecond)
        {
            ZonalInspectionHelper.Current.NameSecond = nameSecond;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет второе поле Зональное наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетВтороеПолеЗональноеНаименование(string zonalNameSecond)
        {
            ZonalInspectionHelper.Current.ZoneNameSecond = zonalNameSecond;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет второе поле Наименование для бланка ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетВтороеПолеНаименованиеДляБланка(string blankNameSecond)
        {
            ZonalInspectionHelper.Current.BlankNameSecond = blankNameSecond;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет второе поле Краткое наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетВтороеПолеКраткоеНаименование(string shortNameSecond)
        {
            ZonalInspectionHelper.Current.ShortNameSecond = shortNameSecond;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет второе поле Адрес ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетВтороеПолеАдрес(string addressSecond)
        {
            ZonalInspectionHelper.Current.AddressSecond = addressSecond;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле E-mail ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеE_Mail(string email)
        {
            ZonalInspectionHelper.Current.Email = email;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле Телефон ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеТелефон(string phone)
        {
            ZonalInspectionHelper.Current.Phone = phone;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле ОКАТО ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеОКАТО(string okato)
        {
            ZonalInspectionHelper.Current.Okato = okato;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле Индекс ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеИндекс(string index)
        {
            ZonalInspectionHelper.Current.IndexOfGji = index;
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеКод(string code)
        {
            ZonalInspectionHelper.Current.DepartmentCode = code;
        }

        [Given(@"добавлена зонально жилищная инспекция")]
        public void ДопустимДобавленаЗональноЖилищнаяИнспекция(Table zonalInspectionTable)
        {
            var zonalInspection = zonalInspectionTable.CreateInstance<ZonalInspection>();

            this._cashe.Current.SaveOrUpdate(zonalInspection);

            ZonalInspectionHelper.Current = zonalInspection;
        }

        [Given(@"пользователь у этой зональной жилищной инспекции добавляет муниципальное образование")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциДобавляетМуниципальноеОбразование()
        {
            var zonalInspectionMunicipality = new ZonalInspectionMunicipality
            {
                Municipality = MunicipalityHelper.Current,
                ZonalInspection = ZonalInspectionHelper.Current
            };

            _cashe.Get<ZonalInspectionMunicipality>().Save(zonalInspectionMunicipality);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.Name =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Зональное наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеЗональноеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.ZoneName =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Наименование для бланка (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеНаименованиДляБланкаСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.BlankName =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле Код отдела (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеКодОтделаСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.DepartmentCode =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет первое поле Адрес (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПервоеПолеАдресСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.Address =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле E-mail (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеE_MailСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.Email =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле Телефон (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеТелефонСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.Phone =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой зональной жилищной инспекци заполняет поле ОКАТО (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЗональнойЖилищнойИнспекциЗаполняетПолеОкатоСимволов(int countOfSymbols, string symbol)
        {
            ZonalInspectionHelper.Current.Okato =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [When(@"пользователь сохраняет эту зональную жилищную инспекцию")]
        public void ЕслиПользовательСохраняетЭтуЗональнуюЖилищнуюИнспекцию()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(ZonalInspectionHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту зональной жилищной инспекции")]
        public void ЕслиПользовательУдаляетЭтуЗональнойЖилищнойИнспекции()
        {
            try
            {
                this._cashe.Current.Delete(ZonalInspectionHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой зональной жилищной инспекции присутствует в справочнике зональных жилищных инспекций")]
        public void ТоЗаписьПоЭтойЗональнойЖилищнойИнспекцииПрисутствуетВСправочникеЗональныхЖилищныхИнспекций()
        {
            var zonalInspection = this._cashe.Current.Get(ZonalInspectionHelper.Current.Id);

            zonalInspection.Should().NotBeNull(
                string.Format("зонально жилищная инспекция должна присутствовать в справочнике зональных жилищных инспекций.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой зональной жилищной инспекции отсутствует в справочнике зональных жилищных инспекций")]
        public void ТоЗаписьПоЭтойЗональнойЖилищнойИнспекцииОтсутствуетВСправочникеЗональныхЖилищныхИнспекций()
        {
            var zonalInspection = this._cashe.Current.Get(ZonalInspectionHelper.Current.Id);

            zonalInspection.Should().BeNull(
                string.Format("зонально жилищная инспекция должна отсутствовать в справочнике зональных жилищных инспекций.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этой зональной жилищной инспекции в списке муниципальных образований присутствует это муниципальное образование")]
        public void ТоУЭтойЗональнойЖилищнойИнспекцииВСпискеМуниципальныхОбразованийПрисутствуетЭтоМуниципальноеОбразование()
        {
            var zonalInspectionMunicipality = _cashe.Get<ZonalInspectionMunicipality>().GetAll()
                .FirstOrDefault(x => x.ZonalInspection.Id == ZonalInspectionHelper.Current.Id
                                     && x.Municipality.Id == MunicipalityHelper.Current.Id);

            zonalInspectionMunicipality.Should()
                .NotBeNull(string.Format(
                    "У этой зональной жилищной инспекции в списке муниципальных образований должно присутствовать это муниципальное образование. {0}"
                    , ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этой зональной жилищной инспекции в списке инспекторов присутствует этот инспектор")]
        public void ТоУЭтойЗональнойЖилищнойИнспекцииВСпискеИнспекторовПрисутствуетЭтотИнспектор()
        {
            var zonalInspectionMunicipality = _cashe.Get<ZonalInspectionInspector>().GetAll()
                .FirstOrDefault(x => x.ZonalInspection.Id == ZonalInspectionHelper.Current.Id
                                     && x.Inspector.Id == InspectorHelper.Current.Id);

            zonalInspectionMunicipality.Should()
                .NotBeNull(string.Format(
                    "У этой зональной жилищной инспекции в списке инспекторов должнен присутствовать этот инспектор. {0}"
                    , ExceptionHelper.GetExceptions()));
        }

    }
}
