namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Exceptions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class MergingBasePersAccSteps : BindingBase
    {
        [Given(@"пользователь в слиянии ЛС заполняет заполняет поле Причина ""(.*)""")]
        public void ДопустимПользовательВСлиянииЛСЗаполняетЗаполняетПолеПричина(string reason)
        {
            ScenarioContext.Current.Add("MergeReason", reason);
        }

        [Given(@"пользователь в слиянии ЛС прикрепляет файл Документ-основание ""(.*)""")]
        public void ДопустимПользовательВСлиянииЛСПрикрепляетФайлДокумент_Основание(string documentName)
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

            ScenarioContext.Current.Add("MergeFileData", fileData);
        }

        [Given(@"пользователь в слиянии ЛС для ЛС ""(.*)"" заполняет поле Новая доля собственности ""(.*)""")]
        public void ДопустимПользовательВСлиянииЛСДляЛСЗаполняетПолеНоваяДоляСобственности(string basePersAccNum, decimal newShare)
        {
            var mergItemProxies = ScenarioContext.Current.Get<IEnumerable<MergeItemProxy>>("MergItemProxies");

            var requiredMergItem = mergItemProxies.FirstOrDefault(x => x.BasePersonalAccount.PersonalAccountNum == basePersAccNum);

            if (requiredMergItem == null)
            {
                throw new SpecFlowException(string.Format("В слиянии ЛС отсутствует ЛС с номером {0}", basePersAccNum));
            }

            requiredMergItem.NewShare = newShare;
        }


        [When(@"пользователь для выбранных ЛС вызывает операцию Слияние")]
        public void ЕслиПользовательДляВыбранныхЛСВызываетОперациюСлияние()
        {
            var mergItemProxies =
                BasePersonalAccountListHelper.Current.Select(x => new MergeItemProxy(x, x.AreaShare)).ToList();

            ScenarioContext.Current.Add("MergItemProxies", mergItemProxies);
        }

        [When(@"пользователь в слиянии ЛС сохраняет изменения")]
        public void ЕслиПользовательВСлиянииЛССохраняетИзменения()
        {
            var reason = ScenarioContext.Current.Get<string>("MergeReason");

            var document = ScenarioContext.Current.Get<FileData>("MergeFileData");

            var merger = Container.Resolve<IPersonalAccountMerger>();

            var mergItemProxies =
                ScenarioContext.Current.Get<IEnumerable<MergeItemProxy>>("MergItemProxies")
                    .Select(
                        x =>
                        new DynamicDictionary
                            {
                                { "BasePersonalAccountId", x.BasePersonalAccount.Id },
                                { "NewShare", x.NewShare }
                            })
                    .ToArray();

            var baseParams = new BaseParams
                                 {
                                     Files = new Dictionary<string, FileData> { { "Document", document } },
                                     Params =
                                         new DynamicDictionary
                                             {
                                                 { "MergeInfos", mergItemProxies },
                                                 { "Reason", reason }
                                             }
                                 };

            try
            {
                merger.Merge(PersonalAccountMergeArgs.FromParams(baseParams));
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IPersonalAccountMerger.Merge", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        private class MergeItemProxy
        {
            public MergeItemProxy(BasePersonalAccount basePersonalAccount, decimal newShare)
            {
                BasePersonalAccount = basePersonalAccount;

                NewShare = newShare;
            }

            public BasePersonalAccount BasePersonalAccount { get; set; }

            public decimal NewShare { get; set; }
        }
    }
}
