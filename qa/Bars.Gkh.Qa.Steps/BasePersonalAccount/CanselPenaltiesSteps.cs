namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;

    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Dto;

    using FluentAssertions;

    using TechTalk.SpecFlow;
    using Utils;

    [Binding]
    internal class CanselPenaltiesSteps : BindingBase
    {
        [When(@"пользователь для выбранных ЛС вызывает операцию Отмена начисления пени")]
        public void ЕслиПользовательДляТекщегоЛСВызываетОперациюОтменаНачисленияПени()
        {
            string code = "penaltycanceloperation";

            var baseParams = new BaseParams
            {
                Params =
                {
                    { "periodId", ChargePeriodHelper.Current.Id },
                    { "ids", BasePersonalAccountListHelper.GetIds() }
                }
            };

            if (Container.Kernel.HasComponent(code))
            {
                var operation = Container.Resolve<IPersonalAccountOperation>(code);
                var operResult = operation.GetDataForUI(baseParams);

                if (!operResult.Success)
                {
                    ExceptionHelper.AddException(
                    string.Format("IPersonalAccountOperation({0}).GetDataForUI", code),
                    string.Format(operResult.Message));

                    return;
                }

                ScenarioContext.Current.Add("PenaltyCancelDataForUI", operResult.Data);
            }
            else
            {
                ExceptionHelper.AddException(
                    "ЕслиПользовательДляТекщегоЛСВызываетОперациюОтменаНачисленияПени",
                    string.Format("Не найден обработчик операции с кодом {0}", code));
            }
        }

        [When(@"пользователь в отмене начислений пени заполняет заполняет поле Причина ""(.*)""")]
        public void ЕслиПользовательВОтменеНачисленийЗаполняетЗаполняетПолеПричина(string reason)
        {
            ScenarioContext.Current.Add("PenaltyCancelReason", reason);
        }

        [When(@"пользователь в отмене начислений пени прикрепляет файл Документ-основание ""(.*)""")]
        public void ЕслиПользовательВОтменеНачисленийПрикрепляетФайлДокумент_Основание(string documentName)
        {
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

            ScenarioContext.Current.Add("PenaltyCancelDocument", fileData);
        }

        [When(@"пользователь в отмене начислений пени сохраняет изменения")]
        public void ЕслиПользовательВОтменеНачисленийСохраняетИзменения()
        {
            var code = "penaltycanceloperation";

            var penaltyCancelDataForUI = ScenarioContext.Current.Get<IEnumerable<object>>("PenaltyCancelDataForUI");
            var records = penaltyCancelDataForUI.Select(
                x =>
                new ModifiedRecord
                    {
                        Id = ReflectionHelper.GetPropertyValue<long>(x, "Id"),
                        CancellationSum =
                            ReflectionHelper.GetPropertyValue<decimal>(x, "CancellationSum")
                    });

            var baseParams = new BaseParams();

            var reason = ScenarioContext.Current.Get<string>("PenaltyCancelReason");

            var fileData = ScenarioContext.Current.Get<FileData>("PenaltyCancelDocument");


            var fileDictionary = new Dictionary<string, FileData> { { "Document", fileData } };

            baseParams.Files = fileDictionary;

            baseParams.Params.Add("Reason", reason);
            baseParams.Params.Add("periodId", ChargePeriodHelper.Current.Id);
            baseParams.Params.Add("records", records);

            if (Container.Kernel.HasComponent(code))
            {
                var operation = Container.Resolve<IPersonalAccountOperation>(code);
                var operResult = operation.Execute(baseParams);

                if (!operResult.Success)
                {
                    ExceptionHelper.AddException(
                    string.Format("IPersonalAccountOperation({0}).Execute", code),
                    string.Format(operResult.Message));

                    return;
                }

                ScenarioContext.Current.Add("PenaltyCancelOperationExecutionResult", operResult.Data);
            }
            else
            {
                ExceptionHelper.AddException(
                    "ЕслиПользовательВОтменеНачисленийСохраняетИзменения",
                    string.Format("Не найден обработчик операции с кодом {0}", code));
            }
        }

        [Then(@"в форме Отмены начисления пени присутствует запись по отменяемым начислениям с ЛС ""(.*)""")]
        public void ЕслиВФормеОтменыНачисленияПениПрисутствуетЗаписьПоОтменяемымНачислениямСЛС(string basePersonalAccountNum)
        {
            var result = ScenarioContext.Current.Get<IEnumerable<object>>("PenaltyCancelDataForUI");

            var requiredRecord =
                result.FirstOrDefault(
                    x => ReflectionHelper.GetPropertyValue<string>(x, "PersonalAccountNum") == basePersonalAccountNum);

            requiredRecord.Should()
                .NotBeNull(
                    string.Format(
                        "в форме Отмены начисления пени должна присутствовать запись по отменяемым начислениям с ЛС {0}",
                        basePersonalAccountNum));

            ScenarioContext.Current.Add("PenaltyCancelCurrentRecord", requiredRecord);
        }

        [Then(@"у этой записи по отменяемым начислениям пени заполнено поле Муниципальный район ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямПениЗаполненоПолеМуниципальныйРайон(string municipality)
        {
            var currentRecord = ScenarioContext.Current["PenaltyCancelCurrentRecord"];

            ReflectionHelper.GetPropertyValue<string>(currentRecord, "Municipality")
                .Should()
                .Be(
                    municipality,
                    string.Format(
                        "у этой записи по отменяемым начислениям пени поле Муниципальный район должно быть {0}",
                        municipality));
        }

        [Then(@"у этой записи по отменяемым начислениям пени заполнено поле Адрес ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямПениЗаполненоПолеАдрес(string address)
        {
            var currentRecord = ScenarioContext.Current["PenaltyCancelCurrentRecord"];

            ReflectionHelper.GetPropertyValue<string>(currentRecord, "Address")
                .Should()
                .Be(
                    address,
                    string.Format(
                        "у этой записи по отменяемым начислениям пени поле Адрес должно быть {0}",
                        address));
        }

        [Then(@"у этой записи по отменяемым начислениям пени заполнено поле Сумма начислений за период ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямПениЗаполненоПолеСуммаНачисленийЗаПериод(decimal penalty)
        {
            var currentRecord = ScenarioContext.Current["PenaltyCancelCurrentRecord"];

            ReflectionHelper.GetPropertyValue<decimal>(currentRecord, "Penalty")
                .Should()
                .Be(
                    penalty,
                    string.Format(
                        "у этой записи по отменяемым начислениям пени поле Сумма начислений за период должно быть {0}",
                        penalty));
        }


        [Then(@"у этой записи по отменяемым начислениям пени заполнено поле Отменить начисления в размере ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямПениЗаполненоПолеОтменитьНачисленияВРазмере(decimal cancellationSum)
        {
            var currentRecord = ScenarioContext.Current["PenaltyCancelCurrentRecord"];

            ReflectionHelper.GetPropertyValue<decimal>(currentRecord, "CancellationSum")
                .Should()
                .Be(
                    cancellationSum,
                    string.Format(
                        "у этой записи по отменяемым начислениям пени поле Отменить начисления в размере должно быть {0}",
                        cancellationSum));
        }
    }
}
