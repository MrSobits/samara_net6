namespace Bars.Gkh.Qa.Steps.Import
{
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

    using ExceptionHelper = Bars.Gkh.Qa.Steps.ExceptionHelper;

    [Binding]
    internal class PersonalAccountPaymentImportSteps : BindingBase
    {
        [Given(@"пользователь в Реестре ЛС вызывает импорт начислений dbf")]
        public void ДопустимПользовательВРеестреЛСВызываетИмпортНачисленийDbf()
        {
            var personalAccountPaymentImportBaseParams = new BaseParams
                                                             {
                                                                 Params =
                                                                     new DynamicDictionary
                                                                         {
                                                                             {
                                                                                 "importId",
                                                                                 "Bars.Gkh.RegOperator.Imports.PersonalAccountPaymentImport"
                                                                             }
                                                                         },
                                                                 Files = new Dictionary<string, FileData>()
                                                             };

            ScenarioContext.Current["PersonalAccountPaymentImportBaseParams"] = personalAccountPaymentImportBaseParams;
        }

        [Given(@"пользователь в импорте начислений dbf прикрепил файл импорта ""(.*)""")]
        public void ДопустимПользовательВИмпортеНачисленийDbfПрикрепилФайлИмпорта(string fullFileName)
        {
            var personalAccountPaymentImportBaseParams =
                ScenarioContext.Current.Get<BaseParams>("PersonalAccountPaymentImportBaseParams");

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

            personalAccountPaymentImportBaseParams.Files = new Dictionary<string, FileData>
                                                               {
                                                                   {"FileImport", fileData}
                                                               };
        }

        [When(@"пользователь запускает импорт начислений dbf")]
        public void ЕслиПользовательЗапускаетИмпортНачисленийDbf()
        {
            var personalAccountPaymentImportBaseParams =
               ScenarioContext.Current.Get<BaseParams>("PersonalAccountPaymentImportBaseParams");

            var importProvider = Container.Resolve<IGkhImportService>();

            Container.Resolve<ISessionProvider>().CreateNewSession();

            using (Container.Using(importProvider))
            {
                var result = importProvider.Import(personalAccountPaymentImportBaseParams);

                if (!result.Success)
                {
                    ExceptionHelper.AddException("IGkhImportService.Import(PersonalAccountPaymentImport)", result.Message);
                }

                var importResult = (CreateTasksResult)result.Data;

                ScenarioContext.Current["TaskEntryParentTaskId"] = importResult.ParentTaskId;
            }
        }
    }
}
