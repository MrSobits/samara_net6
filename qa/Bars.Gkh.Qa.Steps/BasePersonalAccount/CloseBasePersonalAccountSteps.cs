namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    using TechTalk.SpecFlow;

    [Binding]
    internal class CloseBasePersonalAccountSteps : BindingBase
    {
        [Given(@"пользователь в закрытии ЛС заполняет поле Причина ""(.*)""")]
        public void ДопустимПользовательВЗакрытииЛСЗаполняетПолеПричина(string reason)
        {
            var baseParams = ScenarioContext.Current.Get<BaseParams>("CloseBasePersonalAccountBaseParams");

            baseParams.Params.Add("Reason", reason);
        }

        [Given(@"пользователь в закрытии ЛС заполняет поле Дата закрытия ""(.*)""")]
        public void ДопустимПользовательВЗакрытииЛСЗаполняетПолеДатаЗакрытия(string closeDate)
        {
            var baseParams = ScenarioContext.Current.Get<BaseParams>("CloseBasePersonalAccountBaseParams");

            DateTime date;

            if (closeDate == "текущая дата")
            {
                date = DateTime.Today;
            }
            else
            {
                if (!DateTime.TryParse(closeDate, out date))
                {
                    throw new SpecFlowException("Не правильный формат даты в заполнении \"Дата закрытия\" в закрытии ЛС");
                }
            }

            baseParams.Params.Add("closeDate", date);
        }

        [Given(@"пользователь в закрытии ЛС заполняет поле Документ-основание ""(.*)""")]
        public void ДопустимПользовательВЗакрытииЛСЗаполняетПолеДокумент_Основание(string documentName)
        {
            var baseParams = ScenarioContext.Current.Get<BaseParams>("CloseBasePersonalAccountBaseParams");

            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, documentName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            var fileDictionary = new Dictionary<string, FileData> { { "Document", fileData } };

            baseParams.Files = fileDictionary;
        }

        [When(@"пользователь для текщего ЛС вызывает операцию Закрытие")]
        public void ЕслиПользовательДляТекщегоЛСВызываетОперациюЗакрытие()
        {
            var baseParams = new BaseParams
                                 {
                                     Params = new DynamicDictionary
                                                  {
                                                      { "accId", BasePersonalAccountHelper.Current.Id }
                                                  }
                                 };

            ScenarioContext.Current.Add("CloseBasePersonalAccountBaseParams", baseParams);
        }

        [When(@"пользователь в закрытии ЛС сохраняет изменения")]
        public void ЕслиПользовательВЗакрытииЛССохраняетИзменения()
        {
            var service = Container.Resolve<IPersonalAccountService>();

            var baseParams = ScenarioContext.Current.Get<BaseParams>("CloseBasePersonalAccountBaseParams");

            try
            {
                var result = service.ClosePersonalAccount(baseParams);

                if (!result.Success)
                {
                    ExceptionHelper.AddException("IPersonalAccountService.ClosePersonalAccount()", result.Message);

                    Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IPersonalAccountService.ClosePersonalAccount()", ex.Message);
               
                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }
    }
}
