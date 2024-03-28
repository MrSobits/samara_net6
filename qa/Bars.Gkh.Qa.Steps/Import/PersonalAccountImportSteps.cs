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
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PersonalAccountImportSteps : BindingBase
    {
        [Given(@"пользователь в реестре ЛС вызывает Импорт - Импорт лицевых счетов")]
        public void ДопустимПользовательВРеестреЛСВызываетИмпорт_ИмпортЛицевыхСчетов()
        {
            ScenarioContext.Current["PersonalAccountImportBaseParams"] = new BaseParams
                                                                             {
                                                                                 Params =
                                                                                     new DynamicDictionary
                                                                                         {
                                                                                             {
                                                                                                 "importId",
                                                                                                 "Bars.Gkh.RegOperator.Imports.PersonalAccountImport"
                                                                                             }
                                                                                         },
                                                                                         Files = new Dictionary<string, FileData>()
                                                                             };
        }

        [Given(@"пользователь в реестре ЛС в форме импорта ЛС выбирает файл ""(.*)""")]
        public void ДопустимПользовательВРеестреЛСВФормеИмпортаЛСВыбираетФайл(string fullFileName)
        {
            var personalAccountImportBaseParams =
                ScenarioContext.Current.Get<BaseParams>("PersonalAccountImportBaseParams");

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

            personalAccountImportBaseParams.Files.Add("FileImport", fileData); 
        }

        [When(@"пользователь в реестре ЛС запускает импорт ЛС")]
        public void ЕслиПользовательВРеестреЛСЗапускаетИмпортЛС()
        {
            var personalAccountImportBaseParams =
                ScenarioContext.Current.Get<BaseParams>("PersonalAccountImportBaseParams");

            var importProvider = Container.Resolve<IGkhImportService>();

            Container.Resolve<ISessionProvider>().CreateNewSession();

            using (Container.Using(importProvider))
            {
                try
                {
                    var result = importProvider.Import(personalAccountImportBaseParams);

                    if (!result.Success)
                    {
                        ExceptionHelper.AddException("IGkhImportService.Import(PersonalAccounImport)", result.Message);
                    }

                    var importResult = (CreateTasksResult)result.Data;

                    ScenarioContext.Current["TaskEntryParentTaskId"] = importResult.ParentTaskId;
                }
                catch (Exception ex)
                {
                    ExceptionHelper.AddException("IGkhImportService.Import(PersonalAccounImport)", ex.Message);
                }
            }
        }
    }
}
