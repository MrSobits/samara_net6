using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Tasks.ExecutorTest;
using GisGkhLibrary.Services;
using System;

namespace Bars.Gkh.ExecutionAction.Impl
{
    public class GisSyncDictionariesAction : BaseExecutionAction
    {
        public override string Description => "Синхронизация справочников с ГИС ЖКХ";

        public override string Name => "Синхронизация справочников ГИС";

        public override Func<IDataResult> Action => ExecutorTest;

        private IDataResult ExecutorTest()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            string t = typeof(GisGkhDictonary).Name.ToString();
            try
            {
                //////GisGkhRequests req = new GisGkhRequests();
                //////req.TypeRequest = GisGkhTypeRequest.exportNsiList;
                //////req.ReqDate = DateTime.Now;
                //////req.RequestState = GisGkhRequestState.Formed;
                //////GisGkhRequestsDomain.Save(req);
                //var resp = NsiServiceCommonAsync.exportNsiList("10.0.1.2");
                //var resp = NsiServiceCommonAsync.GetState("edc1e90c-875c-11e9-9ed5-005056b69264");
                //var resp = HcsCapitalRepairAsync.exportRegionalProgram("75701000");
                return new BaseDataResult(true, "");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, "");
            }

            //var dictionaryList = DictionaryManager.GetAllDictionaries("10.0.1.2");            

            //try
            //{
            //    taskManager.CreateTasks(new GisSyncDictionariesTaskProvider(), new BaseParams());
            //    return new BaseDataResult(true, "Задача успешно поставлена");
            //}
            //catch (Exception e)
            //{
            //    return new BaseDataResult(false, e.Message);
            //}
            //finally
            //{
            //    Container.Release(taskManager);
            //}
        }
    }
}
