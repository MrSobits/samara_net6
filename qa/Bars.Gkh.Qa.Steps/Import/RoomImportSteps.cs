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
    internal class RoomImportSteps : BindingBase
    {
        [Given(@"пользователь в Импорте абонентов прикрепил файл импорта ""(.*)""")]
        public void ДопустимПользовательВИмпортеАбонентовПрикрепилФайлИмпорта(string fullFileName)
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

            ScenarioContext.Current["RoomImportFileData"] = fileData;
        }

        [Given(@"пользователь в Импорте абонентов заполняет поле Заменять существующие сведения ""(.*)""")]
        public void ДопустимПользовательВИмпортеАбонентовЗаполняетПолеЗаменятьСуществующиеСведения(string replace)
        {
            bool parsedReplace;

            if (!bool.TryParse(replace, out parsedReplace))
            {
                throw new SpecFlowException(
                    "В импорте абонентов поле Заменять существующие сведения, может принимать только значения true false");
            }

            ScenarioContext.Current["RoomImportReplaceData"] = parsedReplace;
        }

        [When(@"пользователь запускает импорт абонентов")]
        public void ЕслиПользовательЗапускаетИмпортАбонентов()
        {
            var replace = ScenarioContext.Current.ContainsKey("RoomImportReplaceData")
                          && ScenarioContext.Current.Get<bool>("RoomImportReplaceData");

            var roomImportBaseParams = new BaseParams
            {
                Params =
                    new DynamicDictionary
                                                                  {
                                                                      {
                                                                          "importId",
                                                                          "Bars.Gkh.RegOperator.Imports.Room.RoomImport"
                                                                      },
                                                                      { "replaceExistRooms", replace }
                                                                  },
                Files = new Dictionary<string, FileData>()
            };

            var fileData = ScenarioContext.Current.Get<FileData>("RoomImportFileData");

            roomImportBaseParams.Files.Add("FileImport", fileData);

            var importProvider = Container.Resolve<IGkhImportService>();

            Container.Resolve<ISessionProvider>().CreateNewSession();

            using (Container.Using(importProvider))
            {
                try
                {
                    var result = importProvider.Import(roomImportBaseParams);


                    if (!result.Success)
                    {
                        ExceptionHelper.AddException("IGkhImportService.Import(RoomImport)", result.Message);
                    }

                    var importResult = (CreateTasksResult)result.Data;

                    ScenarioContext.Current["TaskEntryParentTaskId"] = importResult.ParentTaskId;
                }
                catch (Exception ex)
                {
                    ExceptionHelper.AddException("IGkhImportService.Import(RoomImport)", ex.Message);
                }
            }
        }
    }
}
