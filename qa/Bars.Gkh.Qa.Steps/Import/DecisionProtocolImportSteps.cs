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
    class DecisionProtocolImportSteps : BindingBase
    {
        [Given(@"пользователь в импорте протоколов решений прикрепил файл импорта ""(.*)""")]
        public void ДопустимПользовательВИмпортеПротоколовРешенийПрикрепилФайлИмпорта(string fullFileName)
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

            ScenarioContext.Current["DecisionProtocolImportFileData"] = fileData;
        }

        [When(@"пользователь запускает импорт протоколов решений")]
        public void ЕслиПользовательЗапускаетИмпортПротоколовРешений()
        {
            var personalAccountImportBaseParams = new BaseParams
                                                      {
                                                          Params =
                                                              new DynamicDictionary
                                                                  {
                                                                      {
                                                                          "importId",
                                                                          "Bars.Gkh.RegOperator.Imports.DecisionProtocol.DecisionProtocolImport"
                                                                      }
                                                                  },
                                                          Files = new Dictionary<string, FileData>()
                                                      };
            
            var fileData = ScenarioContext.Current.Get<FileData>("DecisionProtocolImportFileData");

            personalAccountImportBaseParams.Files.Add("FileImport", fileData);

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
