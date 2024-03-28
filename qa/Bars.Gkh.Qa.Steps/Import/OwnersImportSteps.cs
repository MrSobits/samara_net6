namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class OwnersImportSteps : BindingBase
    {
        [Given(@"пользователь в импорте собственников заполняет поле Дата открытия лицевого счета ""(.*)""")]
        public void ЕслиПользовательВИмпортеСобственниковЗаполняетПолеДатаОткрытияЛицевогоСчета(string accountCreateDate)
        {
            ScenarioContext.Current["OwnersImportAccountCreateDate"] = accountCreateDate.DateParse();
        }

        [Given(@"пользователь в импорте собственников прикрепляет файл импорта ""(.*)""")]
        public void ЕслиПользовательВИмпортеСобственниковПрикрепляетФайлИмпорта(string fullFileName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, fullFileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            ScenarioContext.Current["OwnersImportFileData"] = fileData;
        }

        [When(@"пользователь запускает импорт собственников")]
        public void ЕслиПользовательЗапускаетИмпортСобственников()
        {
            var fileData = ScenarioContext.Current.Get<FileData>("OwnersImportFileData");
            var accountCreateDate = ScenarioContext.Current["OwnersImportAccountCreateDate"];

            var fileDictionary = new Dictionary<string, FileData> { { "FileImport", fileData } };

            var paramDict = new DynamicDictionary
                                {
                                    { "importId", "Bars.Gkh.RegOperator.Imports.Account.OwnersImport" },
                                    { "AccountCreateDate", accountCreateDate }
                                };

            var baseParams = new BaseParams
            {
                Files = fileDictionary,
                Params = paramDict
            };

            var importProvider = Container.Resolve<IGkhImportService>();

            Container.Resolve<ISessionProvider>().CreateNewSession();

            using (Container.Using(importProvider))
            {
                try
                {
                    var result = importProvider.Import(baseParams);

                    if (!result.Success)
                    {
                        ExceptionHelper.AddException("IGkhImportService.Import(OwnersImport)", result.Message);
                    }

                    var importResult = (CreateTasksResult)result.Data;

                    ScenarioContext.Current["TaskEntryParentTaskId"] = importResult.ParentTaskId;
                }
                catch (Exception ex)
                {
                    ExceptionHelper.AddException("IGkhImportService.Import(OwnersImport)", ex.Message);
                }
            }
        }
    }
}
