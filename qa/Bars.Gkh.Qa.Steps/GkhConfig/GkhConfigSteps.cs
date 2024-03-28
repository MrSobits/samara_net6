namespace Bars.Gkh.Qa.Steps.FeatureViolGji
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Web.Mvc;
    using B4.Controller.Provider;
    using B4.Utils;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Enums.PaymentDocumentOptions;

    using DomainService.Config;

    using FluentAssertions;

    using Newtonsoft.Json;
    using NHibernate.Linq.Functions;
    using TechTalk.SpecFlow;
    using Utils;

    using ExceptionHelper = Bars.Gkh.Qa.Steps.ExceptionHelper;

    [Binding]
    public class GkhConfigSteps : BindingBase
    {
        private Dictionary<string, object> ConfigsDictionary = new Dictionary<string, object>();

        [Given(@"пользователь в единых настройках приложения заполняет поле Счет регионального оператора ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеСчетРегиональногоОператора(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.HouseCalculationConfig.RegopCalcAccount"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеСпециальныйСчетРегиональногоОператора(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.HouseCalculationConfig.RegopSpecialCalcAccount"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Специальный счет ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеСпециальныйСчет(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.HouseCalculationConfig.SpecialCalcAccount"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Не выбран ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеНеВыбран(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.HouseCalculationConfig.Unknown"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Рассчитывать пени ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеРассчитыватьПени(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentPenaltiesNodeConfig.CalculatePenalty"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Включить блокировку ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеВключитьБлокировку(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.OperationLock.Enabled"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Запрещать выполнение операций с лицевыми счетами после расчета ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеЗапрещатьВыполнениеОперацийСЛицевымиСчетамиПослеРасчета(string p0)
        {
            ConfigsDictionary["RegOperator.GeneralConfig.OperationLock.PreserveLockAfterCalc"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Формат документов на оплату ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеФорматДокументовНаОплату(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.FileFormat"] 
                = EnumHelper.GetFromDisplayValue<FileFormat>(p0).ToString("d");
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Размер документа ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеРазмерДокумента(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.PaperFormat"]
                = EnumHelper.GetFromDisplayValue<PaperFormat>(p0).ToString("d");
        }


        [Given(@"пользователь в единых настройках приложения заполняет поле Метод сжатия изображения ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеМетодСжатияИзображения(string p0)
        {
            string stiPdfImageCompressionMethod;

            // сделал так как нет ссылки на стимул репорт
            switch (p0)
            {
                case "Jpeg":
                    {
                        stiPdfImageCompressionMethod = "1";

                        break;
                    }

                case "Flate":
                    {
                        stiPdfImageCompressionMethod = "2";

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("Отсутствует Метод сжатия изображения {0}", p0));
                    }
            }

            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.QualityOptions.ImageCompressionMethod"]
                = stiPdfImageCompressionMethod;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Вид документов на оплату ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеВидДокументовНаОплату(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PaymentDocFormat"]
                = EnumHelper.GetFromDisplayValue<PaymentDocumentFormat>(p0).ToString("d");
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Количество лицевых счетов в квитанции для физических лиц ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеКоличествоЛицевыхСчетовВКвитанцииДляФизическихЛиц(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument"] = p0;
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Расположение 2 документов на 1 листе ""(.*)""")]
        public void ТоПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеРасположениеДокументовНаЛисте(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.SortOptions.DocumentsPerSheet"]
               = EnumHelper.GetFromDisplayValue<TwoDocumentsPerSheet>(p0).ToString("d");
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Группировка по организационно-правовой форме ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеГруппировкаПоОрганизационно_ПравовойФорме(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigLegal.OrgFormGroup"]
               = EnumHelper.GetFromDisplayValue<OrgFormGroup>(p0).ToString("d");
        }

        [Given(@"пользователь в единых настройках приложения в настройках печати квитанции\(юр\.лица\) добавляет запись ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияВНастройкахПечатиКвитанцииЮр_ЛицаДобавляетЗапись(string p0)
        {
            var orgForm = Container.Resolve<IDomainService<OrganizationForm>>().FirstOrDefault(x => x.Name == p0);
            
            ConfigsDictionary["RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigLegal.OrganizationForms"]
               = JsonConvert.SerializeObject(orgForm);
        }

        [Given(@"пользователь в единых настройках приложения заполняет поле Расчитывать пени ""(.*)""")]
        public void ДопустимПользовательВЕдиныхНастройкахПриложенияЗаполняетПолеРасчитыватьПени(string p0)
        {
            ConfigsDictionary["RegOperator.PaymentPenaltiesNodeConfig.CalculatePenalty"] = p0;
        }

        [When(@"пользователь сохраняет настройки")]
        public void ДопустимПользовательСохраняетНастройки()
        {
            var configString = JsonConvert.SerializeObject(this.ConfigsDictionary);

            var configService = Container.Resolve<IGkhConfigService>();

            IDictionary<string, string> errors;
            try
            {
                var success = configService.UpdateConfigs(configString, out errors);

                if (!success)
                {
                    foreach (var error in errors)
                    {
                        ExceptionHelper.AddException(error.Key, error.Value);

                        Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IGkhConfigService.UpdateConfigs", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }

            this.ConfigsDictionary.Clear();
        }

        [Then(@"в единых настройках приложения заполнено поле Включить блокировку ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеВключитьБлокировку(string operationLock)
        {
            object currentValue;

            bool expectedOperLock;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.GeneralConfig.OperationLock.Enabled", out currentValue);

            bool.TryParse(operationLock, out expectedOperLock);

            currentValue.Should()
                .Be(
                    expectedOperLock,
                    string.Format(
                        "в единых настройках приложения поле Включить блокировку должно быть {0}",
                        operationLock));
        }

        [Then(@"в единых настройках приложения заполнено поле Формат документов на оплату ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеФорматДокументовНаОплату(string fileFormat)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.FileFormat", out currentValue);

            var expectedFileFormat = EnumHelper.GetFromDisplayValue<FileFormat>(fileFormat);

            currentValue.Should()
                .Be(
                    expectedFileFormat,
                    string.Format(
                        "в единых настройках приложения поле Формат документов на оплату должно быть {0}",
                        fileFormat));
        }

        [Then(@"в единых настройках приложения заполнено поле Размер документа ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеРазмерДокумента(string paperFormat)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.PaperFormat", out currentValue);

            var expectedPaperFormat = EnumHelper.GetFromDisplayValue<PaperFormat>(paperFormat);

            currentValue.Should()
                .Be(
                    expectedPaperFormat,
                    string.Format("в единых настройках приложения поле Размер документа должно быть {0}", paperFormat));
        }

        [Then(@"в единых настройках приложения заполнено поле Метод сжатия изображения ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеМетодСжатияИзображения(string stiPdfImageCompressionMethod)
        {
            dynamic currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue(
                    "RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.QualityOptions.ImageCompressionMethod",
                    out currentValue);

            var parsedValue = (string)currentValue.ToString("d");

            string expectedStiPdfImageCompressionMethod;

            // сделал так как нет ссылки на стимул репорт
            switch (stiPdfImageCompressionMethod)
            {
                case "Jpeg":
                    {
                        expectedStiPdfImageCompressionMethod = "1";

                        break;
                    }

                case "Flate":
                    {
                        expectedStiPdfImageCompressionMethod = "2";

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("Отсутствует Метод сжатия изображения {0}", stiPdfImageCompressionMethod));
                    }
            }

            parsedValue.Should()
                .Be(
                    expectedStiPdfImageCompressionMethod,
                    string.Format(
                        "в единых настройках приложения поле Метод сжатия изображения должно быть {0}",
                        stiPdfImageCompressionMethod));
        }

        [Then(@"в единых настройках приложения заполнено поле Вид документов на оплату ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеВидДокументовНаОплату(string paymentDocumentFormat)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PaymentDocFormat", out currentValue);

            var expectedPaymentDocumentFormat = EnumHelper.GetFromDisplayValue<PaymentDocumentFormat>(paymentDocumentFormat);

            currentValue.Should()
                .Be(
                    expectedPaymentDocumentFormat,
                    string.Format("в единых настройках приложения поле Вид документов на оплату должно быть {0}", paymentDocumentFormat));
        }

        [Then(@"в единых настройках приложения заполнено поле Количество лицевых счетов в квитанции для физических лиц ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеКоличествоЛицевыхСчетовВКвитанцииДляФизическихЛиц(int physicalAccountsPerDocument)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument", out currentValue);

            currentValue.Should()
                .Be(
                    physicalAccountsPerDocument,
                    string.Format(
                        "в единых настройках приложения поле Количество лицевых счетов в квитанции для физических лиц должно быть {0}",
                        physicalAccountsPerDocument));
        }

        [Then(@"в единых настройках приложения заполнено поле Расположение 2 документов на 1 листе ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеРасположениеДокументовНаЛисте(string documentsPerSheet)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue(
                    "RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.SortOptions.DocumentsPerSheet",
                    out currentValue);

            var expectedDocumentsPerSheet = EnumHelper.GetFromDisplayValue<TwoDocumentsPerSheet>(documentsPerSheet);

            currentValue.Should()
                .Be(
                    expectedDocumentsPerSheet,
                    string.Format(
                        "в единых настройках приложения поле Расположение 2 документов на 1 листе должно быть {0}",
                        documentsPerSheet));
        }

        [Then(@"в единых настройках приложения заполнено поле Группировка по организационно-правовой форме ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеГруппировкаПоОрганизационно_ПравовойФорме(string orgFormGroup)
        {
            object currentValue;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigLegal.OrgFormGroup", out currentValue);

            var expectedOrgFormGroup = EnumHelper.GetFromDisplayValue<OrgFormGroup>(orgFormGroup);

            currentValue.Should()
                .Be(
                    expectedOrgFormGroup,
                    string.Format("в единых настройках приложения поле Группировка по организационно-правовой форме должно быть {0}", orgFormGroup));
        }

        [Then(@"в единых настройках приложения в настройках печати квитанции\(юр\.лица\) отсутствует запись ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияВНастройкахПечатиКвитанцииЮр_ЛицаОтсутствуетЗапись(string orgFormName)
        {
            var configService = Container.Resolve<IGkhConfigService>();

            var orgForms = configService.GetItems(
                "RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigLegal.OrganizationForms");

            orgForms.Any(x => x.DisplayName == orgFormName)
                .Should()
                .BeFalse(
                    string.Format(
                        @"в единых настройках приложения в настройках печати квитанции\(юр\.лица\) должно отсутствовать запись {0}",
                        orgFormName));
        }

        [Then(@"в единых настройках приложения заполнено поле Расчитывать пени ""(.*)""")]
        public void ТоВЕдиныхНастройкахПриложенияЗаполненоПолеРасчитыватьПени(string сalculatePenalty)
        {
            object currentValue;

            bool expectedCalculatePenalty;

            var configService = Container.Resolve<IGkhConfigService>();

            configService.GetAllConfigs()
                .TryGetValue("RegOperator.PaymentPenaltiesNodeConfig.CalculatePenalty", out currentValue);

            bool.TryParse(сalculatePenalty, out expectedCalculatePenalty);

            currentValue.Should()
                .Be(
                    expectedCalculatePenalty,
                    string.Format(
                        "в единых настройках приложения поле Расчитывать пени должно быть {0}",
                        сalculatePenalty));
        }
    }
}
