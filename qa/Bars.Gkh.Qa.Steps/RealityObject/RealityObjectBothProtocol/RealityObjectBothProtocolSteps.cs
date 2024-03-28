namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Decisions.Nso.Entities.Proxies;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol;
    using Bars.Gkh.RegOperator.Enums;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RealityObjectBothProtocolSteps : BindingBase
    {
        [Given(@"пользователь у этого протокола заполняет поле Тип протокола ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеТипПротокола(string protocolType)
        {
            var protocolT = EnumHelper.GetFromDisplayValue<CoreDecisionType>(protocolType);

            RealityObjectBothProtocolHelper.Current.Params.Add("protocolT", protocolT);
        }

        [Given(@"пользователь у этого протокола заполняет поле Номер ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеНомер(string number)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;
            var protocol = protocolParams.GetAs<DynamicDictionary>("Protocol");

            if (protocol.ContainsKey("DocumentNum"))
            {
                protocol.Remove("DocumentNum");
            }

            if (protocolParams.ContainsKey("ProtocolNumber"))
            {
                protocolParams.Remove("ProtocolNumber");
            }

            protocol.Add("DocumentNum", number);
            protocolParams.Add("ProtocolNumber", number);
        }

        [Given(@"пользователь у этого протокола заполняет поле Дата протокола ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеДатаПротокола(string protocolDate)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;
            var protocol = protocolParams.GetAs<DynamicDictionary>("Protocol");

            if (protocol.ContainsKey("ProtocolDate"))
            {
                protocol.Remove("ProtocolDate");
            }

            if (protocolParams.ContainsKey("ProtocolDate"))
            {
                protocolParams.Remove("ProtocolDate");
            }

            var date = protocolDate.DateParse();

            RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol").Add("ProtocolDate", date);
            RealityObjectBothProtocolHelper.Current.Params.Add("ProtocolDate", date);
        }

        [Given(@"пользователь у этого протокола заполняет поле Дата вступления в силу ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеДатаВступленияВСилу(string dateStart)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;
            var protocol = protocolParams.GetAs<DynamicDictionary>("Protocol");

            if (protocol.ContainsKey("DateStart"))
            {
                protocol.Remove("DateStart");
            }

            if (protocolParams.ContainsKey("DateStart"))
            {
                protocolParams.Remove("DateStart");
            }

            var date = dateStart.DateParse();

            RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol").Add("DateStart", date);
            RealityObjectBothProtocolHelper.Current.Params.Add("DateStart", date);
        }

        [Given(@"пользователь у этого протокола заполняет поле Уполномоченное лицо ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеУполномоченноеЛицо(string authorizedPerson)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;
            var protocol = protocolParams.GetAs<DynamicDictionary>("Protocol");

            if (protocol.ContainsKey("AuthorizedPerson"))
            {
                protocol.Remove("AuthorizedPerson");
            }

            if (protocolParams.ContainsKey("AuthorizedPerson"))
            {
                protocolParams.Remove("AuthorizedPerson");
            }

            RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol")
                .Add("AuthorizedPerson", authorizedPerson);
            RealityObjectBothProtocolHelper.Current.Params.Add("AuthorizedPerson", authorizedPerson);
        }

        [Given(@"пользователь у этого протокола заполняет поле Телефон уполномоченного лица ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеТелефонУполномоченногоЛица(string phoneAuthorizedPerson)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;
            var protocol = protocolParams.GetAs<DynamicDictionary>("Protocol");

            if (protocol.ContainsKey("PhoneAuthorizedPerson"))
            {
                protocol.Remove("PhoneAuthorizedPerson");
            }

            if (protocolParams.ContainsKey("PhoneAuthorizedPerson"))
            {
                protocolParams.Remove("PhoneAuthorizedPerson");
            }

            RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol")
                .Add("PhoneAuthorizedPerson", phoneAuthorizedPerson);
            RealityObjectBothProtocolHelper.Current.Params.Add("PhoneAuthorizedPerson", phoneAuthorizedPerson);
        }

        [Given(@"пользователь у этого протокола заполняет поле Протокол файлом ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеПротоколФайлом(string fileNameWithExtension)
        {
            var protocolFiles = RealityObjectBothProtocolHelper.Current.Files;

            if (protocolFiles.ContainsKey("File"))
            {
                protocolFiles.Remove("File");
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, fileNameWithExtension);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            protocolFiles.Add("File", fileData);
        }

        [Given(@"пользователь у этого протокола заполняет поле Способ формирования фонда КР ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеСпособФормированияФондаКР(string crFundFormationDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("CrFundFormationDecision"))
            {
                protocolParams.Remove("CrFundFormationDecision");
            }

            long decision;

            switch (crFundFormationDecision)
            {
                case "Специальный счет":
                    {
                        decision = 0;
                        break;
                    }

                case "Счет регионального оператора":
                    {
                        decision = 1;
                        break;
                    } 

                default:
                    {
                        throw new SpecFlowException(string.Format("Отсутствует способ формирования фонда КР {0}", crFundFormationDecision));
                    }
            }

            var crFunfFormationDicisionDictionary = new DynamicDictionary
                                                        {
                                                            { "IsChecked", true },
                                                            { "Decision", decision }
                                                        };

            protocolParams.Add("CrFundFormationDecision", crFunfFormationDicisionDictionary);
        }

        [Given(@"пользователь у этого протокола заполняет поле Владелец специального счета ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеВладелецСпециальногоСчета(string accountOwnerDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("AccountOwnerDecision"))
            {
                protocolParams.Remove("AccountOwnerDecision");
            }

            long decisionType;

            switch (accountOwnerDecision)
            {
                case "Дом":
                    {
                        decisionType = 2;
                        break;
                    }

                case "Региональный оператор":
                    {
                        decisionType = 4;
                        break;
                    }
                default:
                    {
                        throw new SpecFlowException(string.Format("Отсутствует владелец специального счёта {0}", accountOwnerDecision));
                    }
            }

            var accountOwnerDecisionDictionary = new DynamicDictionary
                                                        {
                                                            { "IsChecked", true },
                                                            { "DecisionType", decisionType }
                                                        };

            protocolParams.Add("AccountOwnerDecision", accountOwnerDecisionDictionary);
        }

        [Given(@"пользователь у этого протокола заполняет поле Кредитная организация ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеКредитнаяОрганизация(string сreditOrgDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("CreditOrgDecision"))
            {
                protocolParams.Remove("CreditOrgDecision");
            }

            var creditOrg =
                Container.Resolve<IDomainService<CreditOrg>>().GetAll().FirstOrDefault(x => x.Name == сreditOrgDecision);

            if (creditOrg == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует Кредитная организация с Наименованием \"{0}\"", сreditOrgDecision));
            }

            var decisionDynamicDict = new DynamicDictionary { { "Id", creditOrg.Id } };

            var сreditOrgDecisionDictionary = new DynamicDictionary
                                                        {
                                                            { "IsChecked", true },
                                                            { "Decision", decisionDynamicDict }
                                                        };

            protocolParams.Add("CreditOrgDecision", сreditOrgDecisionDictionary);
        }

        [Given(@"пользователь у этого протокола добавляет размер ежемесячного взноса на КР")]
        public void ДопустимПользовательДобавляетРазмерЕжемесячногоВзносаНаКР()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("MonthlyFeeAmountDecision"))
            {
                protocolParams.Remove("MonthlyFeeAmountDecision");
            }

            if (ScenarioContext.Current.ContainsKey("CurrentMonthlyPaymentDyctionary"))
            {
                ScenarioContext.Current.Remove("CurrentMonthlyPaymentDyctionary");
            }

            var newMonthlyPaymentDyctionary = new DynamicDictionary();

            var monthlyFeeAmountDecision = new DynamicDictionary
                                               {
                                                   { "IsChecked", true },
                                                   { "Decision", new List<object> { newMonthlyPaymentDyctionary } }
                                               };

            RealityObjectBothProtocolHelper.Current.Params.Add("MonthlyFeeAmountDecision", monthlyFeeAmountDecision);

            ScenarioContext.Current.Add("CurrentMonthlyPaymentDyctionary", newMonthlyPaymentDyctionary);
        }

        [Given(@"пользователь у этого взноса заполняет поле Дата с ""(.*)""")]
        public void ДопустимПользовательУЭтогоВзносаЗаполняетПолеДатаС(string startDate)
        {
            DateTime date;

            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("MonthlyFeeAmountDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует ежемесячный взнос на КР"));
            }

            if (startDate != "текущая дата" || DateTime.TryParse(startDate, out date))
            {
                throw new SpecFlowException(string.Format("Поле дата с у взноса не может быть {0}", startDate));
            }

            var parsedStartDate = startDate == "текущая дата" ? DateTime.Now.Date : date;

            var currentMonthlyPaymentDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentMonthlyPaymentDyctionary");

            if (currentMonthlyPaymentDyctionary.ContainsKey("From"))
            {
                currentMonthlyPaymentDyctionary.Remove("From");
            }

            currentMonthlyPaymentDyctionary.Add("From", parsedStartDate);
        }

        [Given(@"пользователь у этого взноса заполняет поле Дата до ""(.*)""")]
        public void ДопустимПользовательУЭтогоВзносаЗаполняетПолеДатаДо(string endDate)
        {
            DateTime date;

            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("MonthlyFeeAmountDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует ежемесячный взнос на КР"));
            }

            if (endDate != "текущая дата" || DateTime.TryParse(endDate, out date))
            {
                throw new SpecFlowException(string.Format("Поле дата до у взноса не может быть {0}", endDate));
            }

            var parsedEndDate = endDate == "текущая дата" ? DateTime.Now.Date : date;

            var currentMonthlyPaymentDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentMonthlyPaymentDyctionary");

            if (currentMonthlyPaymentDyctionary.ContainsKey("To"))
            {
                currentMonthlyPaymentDyctionary.Remove("To");
            }

            currentMonthlyPaymentDyctionary.Add("To", parsedEndDate);
        }

        [Given(@"пользователь у этого взноса заполняет поле Принятое решение ""(.*)""")]
        public void ДопустимПользовательУЭтогоВзносаЗаполняетПолеПринятоеРешение(int paymentValue)
        {
            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("MonthlyFeeAmountDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует ежемесячный взнос на КР"));
            }

            var currentMonthlyPaymentDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentMonthlyPaymentDyctionary");

            if (currentMonthlyPaymentDyctionary.ContainsKey("Value"))
            {
                currentMonthlyPaymentDyctionary.Remove("Value");
            }

            currentMonthlyPaymentDyctionary.Add("Value", paymentValue);
        }

        [Given(@"пользователь у этого протокола заполняет поле Минимальный размер фонда КР ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеМинимальныйРазмерФондаКР(string minFundAmountDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("MinFundAmountDecision"))
            {
                protocolParams.Remove("MinFundAmountDecision");
            }

            var minFundAmountDecisionDyctionary = new DynamicDictionary
                                                      {
                                                          { "IsChecked", true },
                                                          { "Decision",  minFundAmountDecision }
                                                      };

            protocolParams.Add("MinFundAmountDecision", minFundAmountDecisionDyctionary);
        }

        [Given(@"пользователь у этого протокола заполняет поле Сумма ранее накопленных средств, перечисляемая на спецсчет ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеСуммаРанееНакопленныхСредствПеречисляемаяНаСпецсчет(string accumulationTransferDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("AccumulationTransferDecision"))
            {
                protocolParams.Remove("AccumulationTransferDecision");
            }

            var accumulationTransferDecisionDyctionary = new DynamicDictionary
                                                      {
                                                          { "IsChecked", true },
                                                          { "Decision",  accumulationTransferDecision }
                                                      };

            protocolParams.Add("AccumulationTransferDecision", accumulationTransferDecisionDyctionary);
        }

        [Given(@"пользователь у этого протокола заполняет поле Ведение лицевых счетов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеВедениеЛицевыхСчетов(string accountManagementDecision)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("AccountManagementDecision"))
            {
                protocolParams.Remove("AccountManagementDecision");
            }

            long decision;

            switch (accountManagementDecision)
            {
                case "Региональным оператором":
                    {
                        decision = 0;
                        break;
                    }

                case "Собственниками":
                    {
                        decision = 10;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("Отсутствует ведение лицевых счетов {0}", accountManagementDecision));
                    }
            }

            var accountManagementDecisionDictionary = new DynamicDictionary
                                                        {
                                                            { "IsChecked", true },
                                                            { "Decision", decision }
                                                        };

            protocolParams.Add("AccountManagementDecision", accountManagementDecisionDictionary);
        }

        [Given(@"пользователь добавляет срок уплаты взноса")]
        public void ДопустимПользовательДобавляетСрокУплатыВзноса()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("PenaltyDelayDecision"))
            {
                protocolParams.Remove("PenaltyDelayDecision");
            }

            if (ScenarioContext.Current.ContainsKey("CurrentPenaltyDelayDecisionDyctionary"))
            {
                ScenarioContext.Current.Remove("CurrentPenaltyDelayDecisionDyctionary");
            }

            var newPenaltyDelayDecisionDyctionary = new DynamicDictionary();

            var penaltyDelayDecision = new DynamicDictionary
                                               {
                                                   { "IsChecked", true },
                                                   { "Decision", new List<object> { newPenaltyDelayDecisionDyctionary } }
                                               };

            protocolParams.Add("PenaltyDelayDecision", penaltyDelayDecision);

            ScenarioContext.Current.Add("CurrentPenaltyDelayDecisionDyctionary", newPenaltyDelayDecisionDyctionary);
        }

        [Given(@"пользователь у этого срока уплаты взноса заполняет поле Дата с ""(.*)""")]
        public void ДопустимПользовательУЭтогоСрокаУплатыВзносаЗаполняетПолеДатаС(string startDate)
        {
            DateTime date;

            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("PenaltyDelayDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует срок уплаты взноса"));
            }

            if (startDate != "первое число текущего месяца" || DateTime.TryParse(startDate, out date))
            {
                throw new SpecFlowException(string.Format("Поле дата у срока уплаты не может быть {0}", startDate));
            }

            var parsedStartDate = startDate == "первое число текущего месяца" ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : date;

            var currentPenaltyDelayDecisionDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentPenaltyDelayDecisionDyctionary");

            currentPenaltyDelayDecisionDyctionary.Add("From", parsedStartDate);
        }

        [Given(@"пользователь у этого срока уплаты взноса заполняет поле Дата до ""(.*)""")]
        public void ДопустимПользовательУЭтогоСрокаУплатыВзносаЗаполняетПолеДатаДо(string endDate)
        {
            DateTime date;

            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("PenaltyDelayDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует срок уплаты взноса"));
            }

            if (endDate != "последнее число текущего месяца" || DateTime.TryParse(endDate, out date))
            {
                throw new SpecFlowException(string.Format("Поле дата до у срока уплаты не может быть {0}", endDate));
            }

            var parsedEndDate = endDate == "последнее число текущего месяца" ? new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1) : date;

            var currentPenaltyDelayDecisionDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentPenaltyDelayDecisionDyctionary");

            currentPenaltyDelayDecisionDyctionary.Add("To", parsedEndDate);
        }

        [Given(@"пользователь у этого срока уплаты взноса заполняет поле Допустимая просрочка, дней ""(.*)""")]
        public void ДопустимПользовательУЭтогоСрокаУплатыВзносаЗаполняетПолеДопустимаяПросрочкаДней(long daysDelay)
        {
            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("PenaltyDelayDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует срок уплаты взноса"));
            }

            var currentPenaltyDelayDecisionDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentPenaltyDelayDecisionDyctionary");

            currentPenaltyDelayDecisionDyctionary.Add("DaysDelay", daysDelay);
        }

        [Given(@"пользователь у этого срока уплаты взноса заполняет поле допустимая просрочка, месяц ""(.*)""")]
        public void ДопустимПользовательУЭтогоСрокаУплатыВзносаЗаполняетПолеДопустимаяПросрочкаМесяц(string externalMonthDelay)
        {
            if (!RealityObjectBothProtocolHelper.Current.Params.ContainsKey("PenaltyDelayDecision"))
            {
                throw new SpecFlowException(string.Format("Отсутсвует срок уплаты взноса"));
            }

            bool internalMonthDelay;

            switch (externalMonthDelay)
            {
                case "Да":
                    {
                        internalMonthDelay = true;

                        break;
                    }

                case "Нет":
                    {
                        internalMonthDelay = false;

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format("Поле \"Допустимая просрочка месяц\" не может быть {0}", externalMonthDelay));
                    }
            }

            var currentPenaltyDelayDecisionDyctionary = ScenarioContext.Current.Get<DynamicDictionary>("CurrentPenaltyDelayDecisionDyctionary");

            if (currentPenaltyDelayDecisionDyctionary.ContainsKey("MonthDelay"))
            {
                currentPenaltyDelayDecisionDyctionary.Remove("MonthDelay");
            }

            currentPenaltyDelayDecisionDyctionary.Add("MonthDelay", internalMonthDelay);
        }

        [Given(@"пользователь у этого протокола заполняет поле Управление домом ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеУправлениеДомом(string realtyManagement)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("RealtyManagement"))
            {
                protocolParams.Remove("RealtyManagement");
            }

            protocolParams.Add("RealtyManagement", realtyManagement);
        }

        [Given(@"пользователь у этого протокола заполняет чекбокс Способ формирования фонда на счету регионального оператора")]
        public void ДопустимПользовательЗаполняетУПротоколаЧекбоксСпособФормированияФондаНаСчетуРегиональногоОператора()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("FundFormationByRegop"))
            {
                protocolParams.Remove("FundFormationByRegop");
            }

            protocolParams.Add("FundFormationByRegop", "on");
        }

        [Given(@"пользователь у этого протокола заполняет чекбокс Снос МКД")]
        public void ДопустимПользовательЗаполняетУПротоколаЧекбоксСносМКД()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("Destroy"))
            {
                protocolParams.Remove("Destroy");
            }

            protocolParams.Add("Destroy", "on");
        }

        [Given(@"пользователь у этого протокола заполняет по полю Снос МКД поле Дата ""(.*)""")]
        public void ДопустимПользовательЗаполняетУПротоколаПоПолюСносМКДПолеДата(string destroyDate)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("DestroyDate"))
            {
                protocolParams.Remove("DestroyDate");
            }

            DateTime date;

            if (!DateTime.TryParse(destroyDate, out date))
            {
                throw new SpecFlowException(string.Format("По полю \"Снос МКД\" поле \"Дата\" в протоколе не может быть {0}", destroyDate));
            }

            protocolParams.Add("DestroyDate", date);
        }

        [Given(@"пользователь у этого протокола заполняет чекбокс Реконструкция МКД")]
        public void ДопустимПользовательЗаполняетУПротоколаЧекбоксРеконструкцияМКД()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("Reconstruction"))
            {
                protocolParams.Remove("Reconstruction");
            }

            protocolParams.Add("Reconstruction", "on");
        }

        [Given(@"пользователь у этого протокола заполняет по полю Реконструкция МКД поле Дата с ""(.*)""")]
        public void ДопустимПользовательЗаполняетУПротоколаПоПолюРеконструкцияМКДПолеДатаС(string reconstructionStart)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("ReconstructionStart"))
            {
                protocolParams.Remove("ReconstructionStart");
            }

            DateTime date;

            if (!DateTime.TryParse(reconstructionStart, out date))
            {
                throw new SpecFlowException(
                    string.Format("По полю \"Реконструкция МКД\" поле \"Дата с\" в протоколе не может быть {0}", reconstructionStart));
            }

            protocolParams.Add("ReconstructionStart", date);
        }

        [Given(@"пользователь у этого протокола заполняет по полю Реконструкция МКД поле Дата по ""(.*)""")]
        public void ДопустимПользовательЗаполняетУПротоколаПоПолюРеконструкцияМКДПолеДатаПо(string reconstructionEnd)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("ReconstructionEnd"))
            {
                protocolParams.Remove("ReconstructionEnd");
            }

            DateTime date;

            if (!DateTime.TryParse(reconstructionEnd, out date))
            {
                throw new SpecFlowException(
                    string.Format("По полю \"Реконструкция МКД\" поле \"Дата по\" в протоколе не может быть {0}", reconstructionEnd));
            }

            protocolParams.Add("ReconstructionEnd", date);
        }

        [Given(@"пользователь у этого протокола заполняет чекбокс Изъятие для государственных или муниципальных нужд земельного участка, на котором расположен МКД")]
        public void ДопустимПользовательЗаполняетУПротоколаЧекбоксИзъятиеДляГосударственныхИлиМуниципальныхНуждЗемельногоУчасткаНаКоторомРасположенМКД()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("TakeLandForGov"))
            {
                protocolParams.Remove("TakeLandForGov");
            }

            protocolParams.Add("TakeLandForGov", "on");
        }

        [Given(@"пользователь у этого протокола заполняет по полю Изъятие для государственных или муниципальных нужд земельного участка, на котором расположен МКД поле Дата ""(.*)""")]
        public void ДопустимПользовательЗаполняетУПротоколаПоПолюИзъятиеДляГосударственныхИлиМуниципальныхНуждЗемельногоУчасткаНаКоторомРасположенМКДПолеДата(string takeLandForGovDate)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("TakeLandForGovDate"))
            {
                protocolParams.Remove("TakeLandForGovDate");
            }

            DateTime date;

            if (!DateTime.TryParse(takeLandForGovDate, out date))
            {
                throw new SpecFlowException(
                    string.Format("По полю \"Изъятие для государственных или муниципальных нужд земельного участка, на котором расположен МКД\" поле \"Дата\" в протоколе не может быть {0}", takeLandForGovDate));
            }

            protocolParams.Add("TakeLandForGovDate", date);
        }

        [Given(@"пользователь у этого протокола заполняет чекбокс Изъятие каждого жилого помещения в доме")]
        public void ДопустимПользовательЗаполняетУПротоколаЧекбоксИзъятиеКаждогоЖилогоПомещенияВДоме()
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("TakeApartsForGov"))
            {
                protocolParams.Remove("TakeApartsForGov");
            }

            protocolParams.Add("TakeApartsForGov", "on");
        }

        [Given(@"пользователь у этого протокола заполняет по полю Изьятие каждого жилого помещения в доме поле Дата ""(.*)""")]
        public void ДопустимПользовательЗаполняетУПротоколаПоПолюИзьятиеКаждогоЖилогоПомещенияВДомеПолеДата(string takeApartsForGovDate)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("TakeApartsForGovDate"))
            {
                protocolParams.Remove("TakeApartsForGovDate");
            }

            DateTime date;

            if (!DateTime.TryParse(takeApartsForGovDate, out date))
            {
                throw new SpecFlowException(
                    string.Format(
                    "По полю \"Изьятие каждого жилого помещения в доме\" поле \"Дата\" в протоколе не может быть {0}",
                    takeApartsForGovDate));
            }

            protocolParams.Add("TakeApartsForGovDate", date);
        }

        [Given(@"пользователь у этого протокола заполняет поле Минимальный размер взноса на КР ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеМинимальныйРазмерВзносаНаКР(string minFund)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("MinFund"))
            {
                protocolParams.Remove("MinFund");
            }

            protocolParams.Add("MinFund", minFund);
        }

        [Given(@"пользователь у этого протокола заполняет поле Максимальный размер фонда ""(.*)""")]
        public void ДопустимПользовательУЭтогоПротоколаЗаполняетПолеМаксимальныйРазмерФонда(string maxFund)
        {
            var protocolParams = RealityObjectBothProtocolHelper.Current.Params;

            if (protocolParams.ContainsKey("MaxFund"))
            {
                protocolParams.Remove("MaxFund");
            }

            protocolParams.Add("MaxFund", maxFund);
        }

        [Given(@"пользователь у этого дома редактирует протокол с номером ""(.*)""")]
        public void ДопустимПользовательУЭтогоДомаРедактируетПротоколСНомером(string protocolNum)
        {
            var protocols = RealityObjectBothProtocolHelper.GetRealityObjectProtocols(RealityObjectHelper.CurrentRealityObject.Id);

            var protocol = protocols.FirstOrDefault(x => x.ProtocolNumber == protocolNum);

            if (protocol == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "У дома по адресу {0} отсутствует протокол решения с номером {1}",
                        RealityObjectHelper.CurrentRealityObject.Address,
                        protocolNum));
            }

            var protocolDynamicDict = new DynamicDictionary
                                          {
                                              { "Id", protocol.Id }, 
                                              { "RealityObject", new DynamicDictionary
                                                                     {
                                                                         {
                                                                             "Id",
                                                                             RealityObjectHelper
                                                                             .CurrentRealityObject.Id
                                                                         }
                                                                     }
                                              }
                                          };

            RealityObjectBothProtocolHelper.Current = new BaseParams
            {
                Files = new Dictionary<string, FileData>(),

                Params =
                    new DynamicDictionary
                                                                      {
                                                                          {
                                                                              "roId",
                                                                              RealityObjectHelper.CurrentRealityObject.Id
                                                                          },
                                                                          {
                                                                              "Protocol",
                                                                              protocolDynamicDict
                                                                          },
                                                                          { "protocolT", protocol.ProtocolType }
                                                                      }
            };
        }


        [When(@"пользователь сохраняет этот протокол")]
        public void ЕслиПользовательСохраняетЭтотПротокол()
        {
            ExplicitSessionScope.CallInNewScope(
                () =>
                    {
                        var result =
                            Container.Resolve<IRealityObjectBothProtocolService>()
                                .SaveOrUpdateDecisions(RealityObjectBothProtocolHelper.Current);

                        if (!result.Success)
                        {
                            ExceptionHelper.AddException(
                                "IRealityObjectBothProtocolService.SaveOrUpdateDecisions",
                                result.Message);

                            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

                            return;
                        }

                        if (RealityObjectBothProtocolHelper.Current.Params.GetAs<CoreDecisionType>("protocolT")
                            == CoreDecisionType.Owners)
                        {
                            var ultimateDecisionProxy =
                                (UltimateDecisionProxy)ReflectionHelper.GetPropertyValue(result.Data, "data");

                            var protocolId = ultimateDecisionProxy.Protocol.Id;

                            RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol")["Id"] =
                                protocolId;

                            return;
                        }

                        var govDesicion = (GovDecision)result.Data;

                        RealityObjectBothProtocolHelper.Current.Params["Id"] = govDesicion.Id;
                    });
        }

        [When(@"пользователь удаляет этот протокол")]
        public void ЕслиПользовательУдаляетЭтотПротокол()
        {
            var paramsDict = new DynamicDictionary
                                 {
                                     { "id", RealityObjectBothProtocolHelper.Current.Params["Id"] },
                                     { "protocolType", RealityObjectBothProtocolHelper.Current.Params["protocolT"] }
                                 };

            var baseParams = new BaseParams
                                  {
                                      Params = paramsDict
                                  };

            var result = Container.Resolve<IRealityObjectBothProtocolService>()
                .Delete(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IRealityObjectBothProtocolService.Delete", result.Message);
            }
        }

        [When(@"пользователь у этого протокола формирует уведомление")]
        public void ЕслиПользовательУЭтогоПротоколаФормируетУведомление()
        {
            var decisionNotificationService = Container.Resolve<IDecisionStraightForwardService>();

            var protocolId =
                RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol").GetAs<long>("Id");

            var baseParams = new BaseParams
                                 {
                                     Params = new DynamicDictionary
                                                  {
                                                      { "protocolId", protocolId }
                                                  }
                                 };

            var result = decisionNotificationService.GetConfirm(baseParams);

            var nitificationId = (long)result.Data;

            DecisionNotificationHelper.Current =
                Container.Resolve<IDomainService<DecisionNotification>>().Get(nitificationId);
        }

        [When(@"пользователь этот протокол переводит в Статус ""(.*)""")]
        public void ЕслиПользовательЭтотПротоколПереводитВСтатус(string newStateName)
        {
            var staterProvider = Container.Resolve<IStateProvider>();

            var protocolId =
                RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol").GetAs<long>("Id");

            var protocolType = RealityObjectBothProtocolHelper.Current.Params.GetAs<CoreDecisionType>("protocolT");

            State currentState;

            if (protocolType == CoreDecisionType.Owners)
            {
                var protocol = Container.Resolve<IDomainService<RealityObjectDecisionProtocol>>().Get(protocolId);

                currentState = protocol.State;
            }
            else
            {
                var protocol = Container.Resolve<IDomainService<GovDecision>>().Get(protocolId);

                currentState = protocol.State;
            }

            var newState =
                Container.Resolve<IDomainService<State>>()
                    .FirstOrDefault(x => x.TypeId == currentState.TypeId && x.Name == newStateName);

            if (newState == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "Отсутствует статус с наименовынием {0} и типом {1}",
                        newStateName,
                        currentState.TypeId));
            }

            try
            {
                staterProvider.ChangeState(protocolId, newState.TypeId, newState, string.Empty, false);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IStateProvider.ChangeState", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [Then(@"запись по этому протоколу решения отсутствует в списке протоколов решений этого дома")]
        public void ТоЗаписьПоЭтомуПротоколуРешенияОтсутствуетВСпискеПротоколовРешенийЭтогоДома()
        {
            var currentProtocolNumber = RealityObjectBothProtocolHelper.Current.Params["ProtocolNumber"].ToString();

            var roId = RealityObjectHelper.CurrentRealityObject.Id;

            var protocols = RealityObjectBothProtocolHelper.GetRealityObjectProtocols(roId);

            protocols.Any(x => x.ProtocolNumber == currentProtocolNumber)
                .Should()
                .BeFalse(
                    string.Format(
                        "в списке протоколов решений дома по адресу {0} должен отсутствовать протокол с номером {1}",
                        RealityObjectHelper.CurrentRealityObject.Address,
                        currentProtocolNumber));
        }

        [Then(@"у этого протокола решения заполнено поле Номер ""(.*)""")]
        public void ТоУЭтогоПротоколаРешенияЗаполненоПолеНомер(string expectedProtocolNumber)
        {
            var protocols = RealityObjectBothProtocolHelper.GetRealityObjectProtocols(RealityObjectHelper.CurrentRealityObject.Id);

            var protocol =
                protocols.FirstOrDefault(
                    x =>
                    x.Id
                    == RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol")
                           .GetAs<long>("Id"));

            if (protocol == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "У дома по адресу {0} отсутствует текущий протокол решения",
                        RealityObjectHelper.CurrentRealityObject.Address));
            }

            protocol.ProtocolNumber.Should()
                .Be(
                    expectedProtocolNumber,
                    string.Format("у этого протокола номер должен быть {0}", expectedProtocolNumber));
        }

        [Then(@"у этого протокола решения заполнено поле Статус ""(.*)""")]
        public void ТоУЭтогоПротоколаРешенияЗаполненоПолеСтатус(string expectedStateName)
        {
            var protocols = RealityObjectBothProtocolHelper.GetRealityObjectProtocols(RealityObjectHelper.CurrentRealityObject.Id);

            var protocol =
                protocols.FirstOrDefault(
                    x =>
                    x.Id
                    == RealityObjectBothProtocolHelper.Current.Params.GetAs<DynamicDictionary>("Protocol")
                           .GetAs<long>("Id"));

            if (protocol == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "У дома по адресу {0} отсутствует текущий протокол решения",
                        RealityObjectHelper.CurrentRealityObject.Address));
            }

            protocol.State.Name.Should()
                .Be(expectedStateName, string.Format("у этого протокола статус должен быть {0}", expectedStateName));
        }
    }
}
