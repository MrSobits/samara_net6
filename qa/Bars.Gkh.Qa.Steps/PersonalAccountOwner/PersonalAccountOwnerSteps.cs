namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Enums;

    using FluentAssertions;

    using NHibernate.Util;

    using NUnit.Framework;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class PersonalAccountOwnerSteps : BindingBase
    {
        private IDomainService dsIndividual
        {
            get
            {
                Type entityType = IndividualAccountOwnerHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        private IDomainService dsLegal
        {
            get
            {
                Type entityType = LegalAccountOwnerHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"добавлена льготная категория")]
        public void ДопустимДобавленаЛьготнаяКатегория(Table table)
        {
            PrivilegedCategoryHelper.Current = table.CreateInstance<PrivilegedCategory>();
            
            var dsPrivilegedCategory = (IDomainService)Container.Resolve<IDomainService<PrivilegedCategory>>();

            var id = (long)PrivilegedCategoryHelper.Current.Id;

            try
            {
                if (id == 0)
                {
                    dsPrivilegedCategory.Save(PrivilegedCategoryHelper.Current);
                }
                else
                {
                    dsPrivilegedCategory.Update(PrivilegedCategoryHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"добавлен контрагент с организационно правовой формой")]
        public void ДопустимДобавленКонтрагентСОрганизационноПравовойФормой(Table table)
        {
            ContragentHelper.CurrentContragent = table.CreateInstance<Contragent>();
            ContragentHelper.CurrentContragent.OrganizationForm = OrganizationFormHelper.CurrentOrganizationForm;

            try
            {
                Container.Resolve<IDomainService<Contragent>>().SaveOrUpdate(ContragentHelper.CurrentContragent);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"пользователь добавляет абонента типа Счет физ\.лица")]
        public void ДопустимПользовательДобавляетАбонентаТипаСчетФиз_Лица()
        {
            var owner = new IndividualAccountOwner();

            IndividualAccountOwnerHelper.Current = owner;

            PersonalAccountOwnerHelper.Current = owner;
        }

        [Given(@"пользователь добавляет абонента типа Счет юр\.лица")]
        public void ДопустимПользовательДобавляетАбонентаТипаСчетЮр_Лица()
        {
            var owner = new LegalAccountOwner();

            PersonalAccountOwnerHelper.Current = owner;

            LegalAccountOwnerHelper.Current = owner;
        }

        [Given(@"пользователь в реестре абонентов открывает карточку абонента с ФИО ""(.*)""")]
        public void ДопустимПользовательВРеестреАбонентовОткрываетКарточкуАбонентаСФИО(string name)
        {
            var persAccOwner =
                Container.Resolve<IDomainService<PersonalAccountOwner>>().GetAll().FirstOrDefault(x => x.Name.Trim() == name);

            if (persAccOwner.OwnerType == PersonalAccountOwnerType.Individual)
            {
                var ds = Container.Resolve<IDomainService<IndividualAccountOwner>>();

                using (Container.Using(ds))
                {
                    IndividualAccountOwnerHelper.Current = ds.Get(persAccOwner.Id);
                }
            }
            else
            {
                var ds = Container.Resolve<IDomainService<LegalAccountOwner>>();

                using (Container.Using(ds))
                {
                    LegalAccountOwnerHelper.Current = ds.Get(persAccOwner.Id);
                }
            }

            if (persAccOwner == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует Абонент с ФИО {0}", name));
            }

            PersonalAccountOwnerHelper.Current = persAccOwner;
        }


        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Фамилия (.*) символо ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеФамилияСимволо(int count, char ch)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("Surname", new string(ch, count));
        }

        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Имя (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеИмяСимволов(int count, char ch)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("FirstName", new string(ch, count));
        }

        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Отчество (.*) знаков ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеОтчествоЗнаков(int count, char ch)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("SecondName", new string(ch, count));
        }

        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Серия документа (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеСерияДокументаСимволов(int count, char ch)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentitySerial", new string(ch, count));
        }


        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Льготная категория")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеЛьготнаяКатегория()
        {
            IndividualAccountOwnerHelper.ChangeCurrent("PrivilegedCategory", PrivilegedCategoryHelper.Current);
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Фамилия ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеФамилия(string surname)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("Surname", surname);
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Имя ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеИмя(string firstName)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("FirstName", firstName);
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Отчество ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеОтчество(string secondName)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("SecondName", secondName);
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Дата рождения ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеДатаРождения(string birthDate)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("BirthDate", DateTime.Parse(birthDate));
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Тип документа ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеТипДокумента(string identityType)
        {
            switch (identityType)
            {
                case "Паспорт":
                    IndividualAccountOwnerHelper.ChangeCurrent("IdentityType", 10);
                    break;

                case "Свидетельство о рождении":
                    IndividualAccountOwnerHelper.ChangeCurrent("IdentityType", 20);
                    break;
            }
        }

        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Номер документа (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеНомерДокументаСимволов(int count, char ch)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentityNumber", new string(ch, count));
        }


        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Серия документа ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеСерияДокумента(string identitySerial)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentitySerial", identitySerial);
        }
        
        [Given(@"пользователь у этого абонента типа Счет физ\.лица заполняет поле Номер документа ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетФиз_ЛицаЗаполняетПолеНомерДокумента(string idenNum)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentityNumber", idenNum);
        }
        
        [Given(@"пользователь у этого абонента типа Счет юр\.лица заполняет поле Льготная категория")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетЮр_ЛицаЗаполняетПолеЛьготнаяКатегория()
        {
            LegalAccountOwnerHelper.ChangeCurrent("PrivilegedCategory", PrivilegedCategoryHelper.Current);
        }
        
        [Given(@"пользователь у этого абонента типа Счет юр\.лица заполняет поле Контрагент")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетЮр_ЛицаЗаполняетПолеКонтрагент()
        {
            LegalAccountOwnerHelper.ChangeCurrent("Contragent", ContragentHelper.CurrentContragent);
        }
        
        [Given(@"пользователь у этого абонента типа Счет юр\.лица заполняет поле ИНН ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетЮр_ЛицаЗаполняетПолеИНН(string inn)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("Inn", inn);
        }
        
        [Given(@"пользователь у этого абонента типа Счет юр\.лица заполняет поле КПП ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетЮр_ЛицаЗаполняетПолеКПП(string kpp)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("Kpp", kpp);
        }
        
        [Given(@"пользователь у этого абонента типа Счет юр\.лица заполняет поле Печатать акт при печати документов на оплату ""(.*)""")]
        public void ДопустимПользовательУЭтогоАбонентаТипаСчетЮр_ЛицаЗаполняетПолеПечататьАктПриПечатиДокументовНаОплату(bool printAct)
        {
            LegalAccountOwnerHelper.ChangeCurrent("PrintAct", printAct);
        }

        [Given(@"пользователь добавляет запись по сведению о помещении абоненту типа Счет физ\.лица")]
        public void ЕслиПользовательДобавляетЗаписьПоСведениюОПомещенииАбонентуТипаСчетФиз_Лица()
        {
            var methodCreateInstance = Type.GetType("Bars.Gkh.RegOperator.Entities.PersonalAccountOwner, Bars.Gkh.RegOperator")
                    .GetMethod("CreateAccount");
        }

        [Given(@"пользователь у этой записи по сведению о помещении заполняет поле Дата открытия ЛС ""(.*)"" абоненту типа Счет физ\.лица")]
        public void ЕслиПользовательУЭтойЗаписиПоСведениюОПомещенииЗаполняетПолеДатаОткрытияЛСАбонентуТипаСчетФиз_Лица(string dataOpen)
        {
            ScenarioContext.Current["dateOpen"] = dataOpen.DateParse();
        }

        [Given(@"пользователь у этой записи по сведению о помещении заполняет поле Жилой дом абоненту типа Счет физ\.лица")]
        public void ЕслиПользовательУЭтойЗаписиПоСведениюОПомещенииЗаполняетПолеЖилойДомАбонентуТипаСчетФиз_Лица()
        {
            ScenarioContext.Current["realityObjId"] = RealityObjectHelper.CurrentRealityObject.Id;

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                                                  {
                                                      {
                                                          "realtyObjectId",
                                                          RealityObjectHelper.CurrentRealityObject.Id
                                                      }
                                                  }
            };

            var result = Container.Resolve<IPersonalAccountService>()
                .GetTarifForRealtyObject(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IPersonalAccountService.GetTarifForRealtyObject", result.Message);
            }
        }
        
        [When(@"пользователь сохраняет этого абонента типа Счет физ\.лица")]
        public void ЕслиПользовательСохраняетЭтогоАбонентаТипаСчетФиз_Лица()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.dsIndividual.Save(IndividualAccountOwnerHelper.Current);
                }
                else
                {
                    this.dsIndividual.Update(IndividualAccountOwnerHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этого абонента типа Счет физ\.лица")]
        public void ЕслиПользовательУдаляетЭтогоАбонентаТипаСчетФиз_Лица()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                this.dsIndividual.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"пользователь у этой записи по сведению о помещении заполняет поле № квартиры/помещения абоненту типа Счет физ\.лица")]
        public void ЕслиПользовательУЭтойЗаписиПоСведениюОПомещенииЗаполняетПолеКвартирыПомещенияАбонентуТипаСчетФиз_Лица()
        {
            ScenarioContext.Current["IndividualAccountOwnerRoom"] = RoomHelper.Current;
        }
        
        [Given(@"у этого № квартиры/помещения заполняет поле Доля собственности ""(.*)"" абоненту типа Счет физ\.лица")]
        public void ЕслиУЭтогоКвартирыПомещенияЗаполняетПолеДоляСобственностиАбонентуТипаСчетФиз_Лица(decimal areaShare)
        {
            ScenarioContext.Current["areaShare"] = areaShare;
        }
        
        [When(@"пользователь сохраняет эту запись по сведению о помещении абоненту типа Счет физ\.лица")]
        public void ЕслиПользовательСохраняетЭтуЗаписьПоСведениюОПомещенииАбонентуТипаСчетФиз_Лица()
        {
            var room = ScenarioContext.Current.Get<Room>("IndividualAccountOwnerRoom");

            var baseParams = new BaseParams
                                 {
                                     Params =
                                         {
                                             {
                                                 "AccountOwner",
                                                 IndividualAccountOwnerHelper.GetPropertyValue("Id")
                                             },
                                             { "OpenDate", ScenarioContext.Current["dateOpen"] },
                                             {
                                                 "Rooms",
                                                 new List<object>
                                                     {
                                                         new DynamicDictionary()
                                                             {
                                                                 { "Id", room.Id },
                                                                 {
                                                                     "AreaShare",
                                                                     (decimal)
                                                                     ScenarioContext.Current[
                                                                         "areaShare"]
                                                                 }
                                                             }
                                                     }
                                             }
                                         }
                                 };

            var service = Container.Resolve<IPersonalAccountCreateService>();
            try
            {
                var result = service.CreateNewAccount(baseParams);

                if (!result.Success)
                {
                    ExceptionHelper.TestExceptions.Add(
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        result.Message);

                    Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

                    return;
                }

                var data = (IList)result.Data;

                var basePersonalAcc = data.Cast<object>().First();
                ScenarioContext.Current["personalAccountId"] =
                    basePersonalAcc.GetType().GetProperty("Id").GetValue(basePersonalAcc);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }
        
        [Then(@"запись по этому абоненту присутствует в разделе абонентов типа Счет физ\.лица")]
        public void ТоЗаписьПоЭтомуАбонентуПрисутствуетВРазделеАбонентовТипаСчетФиз_Лица()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            var current = this.dsIndividual.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому абоненту должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому абоненту отсутствует в разделе абонентов типа Счет физ\.лица")]
        public void ТоЗаписьПоЭтомуАбонентуОтсутствуетВРазделеАбонентовТипаСчетФиз_Лица()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            var current = this.dsIndividual.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому абоненту должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [When(@"пользователь сохраняет этого абонента типа Счет юр\.лица")]
        public void ЕслиПользовательСохраняетЭтогоАбонентаТипаСчетЮр_Лица()
        {
            var id = (long)LegalAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.dsLegal.Save(LegalAccountOwnerHelper.Current);
                }
                else
                {
                    this.dsLegal.Update(LegalAccountOwnerHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"пользователь удаляет этого абонента Тип абонента Счет юр\.лица")]
        public void ТоПользовательУдаляетЭтогоАбонентаТипАбонентаСчетЮр_Лица()
        {
            var id = (long)LegalAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                this.dsLegal.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому абоненту присутствует в разделе абонентов типа Счет юр\.лица")]
        public void ТоЗаписьПоЭтомуАбонентуПрисутствуетВРазделеАбонентовТипаСчетЮр_Лица()
        {
            var id = (long)LegalAccountOwnerHelper.GetPropertyValue("Id");
            var current = this.dsLegal.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому абоненту должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому абоненту отсутствует в разделе абонентов Тип абонента Счет юр\.лица")]
        public void ТоЗаписьПоЭтомуАбонентуОтсутствуетВРазделеАбонентовТипАбонента()
        {
            var id = (long)LegalAccountOwnerHelper.GetPropertyValue("Id");
            var current = this.dsLegal.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому абоненту должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому сведению о помещении присутствует в сведениях о помещении по этому абоненту типа Счет физ\.лица")]
        public void ТоЗаписьПоЭтомуСведениюОПомещенииПрисутствуетВСведенияхОПомещенииПоЭтомуАбонентуТипаСчетФиз_Лица()
        {
            if (!ScenarioContext.Current.ContainsKey("personalAccountId"))
            {
                throw new SpecFlowException(
                    string.Format(
                        "В ScenarioContext.Current отсутствует personalAccountId. {0}",
                        ExceptionHelper.GetExceptions()));
            }

            var ds = Container.Resolve<IDomainService<BasePersonalAccount>>();

            ds.Get(ScenarioContext.Current["personalAccountId"])
                .Should()
                .NotBeNull(
                    String.Format(
                        "запись по этому абоненту должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлен абонент типа Счет физ\.лица")]
        public void ДопустимДобавленАбонентТипаСчетФиз_Лица(Table table)
        {
            var methodCreateInstance = typeof(TableHelperExtensionMethods).GetMethod("CreateSet", new[] { typeof(Table) });

            Type type = Type.GetType("Bars.Gkh.RegOperator.Entities.IndividualAccountOwner, Bars.Gkh.RegOperator");

            MethodInfo genericCreateInstance = methodCreateInstance.MakeGenericMethod(type);

            IndividualAccountListHelper.Current = (List<IndividualAccountOwner>) genericCreateInstance.Invoke(null, new[] {table});

            IndividualAccountOwnerHelper.Current = IndividualAccountListHelper.Current.First();

            foreach (var individual in IndividualAccountListHelper.Current)
            {
                var id = individual.Id;
            
                try
                {
                    if (id == 0)
                    {
                        dsIndividual.Save(individual);
                    }
                    else
                    {
                        dsIndividual.Update(individual);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
        }

        [Given(@"добавлен абонент с типом Счет юр\.лица")]
        public void ДопустимДобавленАбонентСТипомСчетЮр_Лица(Table table)
        {
            var contragent = Container.Resolve<IDomainService<Contragent>>().GetAll().Where(x => x.Name == table.Rows[0]["Contragent"]);
            
            LegalAccountOwnerHelper.Current = new LegalAccountOwner();

            LegalAccountOwnerHelper.ChangeCurrent("PrivilegedCategory", PrivilegedCategoryHelper.Current);
            LegalAccountOwnerHelper.ChangeCurrent("Contragent", contragent.FirstOrDefault());
            LegalAccountOwnerHelper.ChangeCurrent("PrintAct", false);

            ЕслиПользовательСохраняетЭтогоАбонентаТипаСчетЮр_Лица();
        }


        [Given(@"добавлено помещение абоненту типа Счет физ\.лица")]
        public void ДопустимДобавленоПомещениеАбонентуТипаСчетФиз_Лица(Table table)
        {
            var context = (GkhContext)ApplicationContext.Current;

            string openDate = "";
            string address = "";
            string roomNum = "";
            decimal areaShare = 0;

            bool regionFound = false;

            foreach (var row in table.Rows)
            {
                if (row["region"] == context.Region)
                {
                    openDate = row["OpenDate"];

                    var areaSharestring = row["AreaShare"].Replace(
                        ",",
                        CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

                    areaShare = decimal.Parse(areaSharestring, CultureInfo.InvariantCulture);
                    regionFound = true;
                    address = row["RealityObjectAddress"].Replace(",", string.Empty);
                    roomNum = row["RoomNum"];
                }
            }

            if (!regionFound)
            {
                Assert.Ignore();
            }

            var room =
                Container.Resolve<IDomainService<Room>>()
                    .GetAll()
                    .Where(x => x.RoomNum == roomNum && x.RealityObject.Address == address)
                    .ToList();

            if (room.IsEmpty())
            {
                throw new SpecFlowException(string.Format("не найдено помещения №{0} по адресу{1}", roomNum, address));
            }

            var baseParams = new BaseParams
                                 {
                                     Params =
                                         {
                                             {
                                                 "AccountOwner",
                                                 IndividualAccountOwnerHelper.GetPropertyValue("Id")
                                             },
                                             { "OpenDate", DateTime.Parse(openDate) },
                                             {
                                                 "Rooms",
                                                 new List<object>
                                                     {
                                                         new DynamicDictionary()
                                                             {
                                                                 {
                                                                     "Id",
                                                                     room
                                                                     .FirstOrDefault
                                                                     ()
                                                                     .Id
                                                                 },
                                                                 {
                                                                     "AreaShare",
                                                                     areaShare
                                                                 }
                                                             }
                                                     }
                                             }
                                         }
                                 };

            var serv = Container.Resolve<IPersonalAccountCreateService>();

            var result = serv.CreateNewAccount(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.TestExceptions.Add("IPersonalAccountCreateService.CreateNewAccount", result.Message);

                return;
            }

            var resultList = (IList)result.Data;

            var basePersonalAccount = resultList.Cast<dynamic>().First();

            Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();

            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

            BasePersonalAccountHelper.Current = BasePersonalAccountHelper.DomainService.Get(basePersonalAccount.Id);
        }

        [Given(@"добавлено помещение абоненту типа Счет физ\.лица с Фио ""(.*)""")]
        public void ДопустимДобавленоПомещениеАбонентуТипаСчетФиз_ЛицаСФио(string accountOwnerFio, Table table)
        {
            IQueryable<dynamic> accountOwnerQuery = IndividualAccountOwnerHelper.DomainService.GetAll();

            //IEnumerable<dynamic> accountOwnerList = accountOwnerQuery.ToList();

            var accountOwner = Enumerable
                .First(accountOwnerQuery, x => ReflectionHelper.GetPropertyValue(x, "Name") == accountOwnerFio);

            var context = (GkhContext)ApplicationContext.Current;

            string openDate = "";
            string address = "";
            string roomNum = "";
            decimal areaShare = 0;

            bool regionFound = false;

            foreach (var row in table.Rows)
            {
                if (row["region"] == context.Region)
                {
                    openDate = row["OpenDate"];
                    areaShare = decimal.Parse(row["AreaShare"], CultureInfo.InvariantCulture);
                    regionFound = true;
                    address = row["RealityObjectAddress"].Replace(",", string.Empty);
                    roomNum = row["RoomNum"];
                }
            }

            if (!regionFound)
            {
                Assert.Ignore();
            }

            var room =
                Container.Resolve<IDomainService<Room>>()
                    .GetAll()
                    .Where(x => x.RoomNum == roomNum && x.RealityObject.Address == address)
                    .ToList();

            if (room.IsEmpty())
            {
                throw new SpecFlowException(string.Format("не найдено помещения №{0} по адресу{1}", roomNum, address));
            }

            var baseParams = new BaseParams
            {
                Params =
                {
                    {"AccountOwner", accountOwner.Id},
                    {"OpenDate", DateTime.Parse(openDate)},
                    {
                        "Rooms", new List<object>
                        {
                            new DynamicDictionary()
                            {
                                {"Id", room.FirstOrDefault().Id},
                                {"AreaShare", areaShare}
                            }
                        }
                    }
                }
            };

            try
            {
                var serv = Container.Resolve<IPersonalAccountCreateService>();

                var result = ((IList)((BaseDataResult)serv.CreateNewAccount(baseParams)).Data).Cast<dynamic>().First();

                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

                BasePersonalAccountHelper.Current = BasePersonalAccountHelper.DomainService.Get(result.Id);
                BasePersonalAccountListHelper.Current.Add(BasePersonalAccountHelper.Current);

            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"добавлено помещение абонентам типа Счет физ\.лица с Фио")]
        public void ДопустимДобавленоПомещениеАбонентамТипаСчетФиз_ЛицаСФио(Table table)
        {
            var individualAcoountList = IndividualAccountListHelper.Current;
            BasePersonalAccountListHelper.Current = new List<BasePersonalAccount>();

            foreach (var individualAccountOwner in individualAcoountList)
            {
                ДопустимДобавленоПомещениеАбонентуТипаСчетФиз_ЛицаСФио(individualAccountOwner.Name, table);
            }
        }

        [Given(@"добавлено помещение абоненту типа Счет юр\.лица")]
        public void ДопустимДобавленоПомещениеАбонентуТипаСчетЮр_Лица(Table table)
        {
            var context = (GkhContext)ApplicationContext.Current;

            string openDate = "";
            string address = "";
            string roomNum = "";
            decimal areaShare = 0;

            bool regionFound = false;

            foreach (var row in table.Rows)
            {
                if (row["region"] == context.Region)
                {
                    openDate = row["OpenDate"];
                    var areaSharestring = row["AreaShare"].Replace(
                        ",",
                        CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

                    areaShare = decimal.Parse(areaSharestring, CultureInfo.InvariantCulture);
                    regionFound = true;
                    address = row["RealityObjectAddress"];
                    roomNum = row["RoomNum"];
                }
            }

            if (!regionFound)
            {
                Assert.Ignore();
            }

            var room =
                Container.Resolve<IDomainService<Room>>()
                    .GetAll()
                    .Where(x => x.RoomNum == roomNum && x.RealityObject.Address == address)
                    .ToList();

            if (room.IsEmpty())
            {
                throw new SpecFlowException(string.Format("не найдено помещения №{0} по адресу{1}", roomNum, address));
            }

            var baseParams = new BaseParams
            {
                Params =
                {
                    {"AccountOwner", LegalAccountOwnerHelper.GetPropertyValue("Id")},
                    {"OpenDate", DateTime.Parse(openDate)},
                    {
                        "Rooms", new List<object>
                        {
                            new DynamicDictionary()
                            {
                                {"Id", room.FirstOrDefault().Id},
                                {"AreaShare", areaShare}
                            }
                        }
                    }
                }
            };

            var serv = Container.Resolve<IPersonalAccountCreateService>();

            var result = serv.CreateNewAccount(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, result.Message);

                return;
            }

            var data = (IList)result.Data;

            var basePersonalAccount = data.First();

            BasePersonalAccountHelper.Current = (BasePersonalAccount)basePersonalAccount;
        }
    }
}
