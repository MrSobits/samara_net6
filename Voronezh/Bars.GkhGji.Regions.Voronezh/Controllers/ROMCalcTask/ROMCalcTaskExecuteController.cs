namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.Modules.States;
    using System.Net;
    public class ROMCalcTaskExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public ROMCalcTaskExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<ROMCalcTask> ROMCalcTaskDomain { get; set; }

        public IDomainService<ROMCalcTaskManOrg> ROMCalcTaskManOrgDomain { get; set; }

        public IDomainService<ROMCategory> ROMCategoryDomain { get; set; }

        public IDomainService<ROMCategoryMKD> ROMCategoryMKDDomain { get; set; }


        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {

            var romCalcTask = ROMCalcTaskDomain.Get(taskId);
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
            var romCalcTaskManOrg = ROMCalcTaskManOrgDomain.GetAll()
                .Where(x => x.ROMCalcTask == romCalcTask).ToList();
            if (romCalcTask.CalcState == "Рассчитано")
            {
                return JsFailure("Данная задача уже выполнена, повторный расчет запрещен");
            }
            else
            {
                foreach (ROMCalcTaskManOrg record in romCalcTaskManOrg)
                {
                    sw.WriteLine(record.Contragent.ShortName + " - запуск расчета");
                    ROMCategory romCategory = new ROMCategory();
                    romCategory.CalcDate = romCalcTask.CalcDate;
                    romCategory.Contragent = record.Contragent;
                    romCategory.YearEnums = romCalcTask.YearEnums;
                    romCategory.Inspector = romCalcTask.Inspector;
                    romCategory.KindKND = romCalcTask.KindKND;
                    romCategory.ObjectCreateDate = DateTime.Now;
                    romCategory.ObjectEditDate = DateTime.Now;
                    romCategory.ObjectVersion = 1;
                    var stateProvider = Container.Resolve<IStateProvider>();
                    try
                    {
                        stateProvider.SetDefaultState(romCategory);

                    }
                    catch
                    {
                        return JsFailure("Для расчета не задан начальный статус");
                        sw.WriteLine(record.Contragent.ShortName + " - ошибка");

                    }
                    try
                    {
                        ROMCategoryDomain.Save(romCategory);
                        sw.WriteLine(record.Contragent.ShortName + " - расчет завершен");
                    }
                    catch (Exception e)
                    {
                        sw.WriteLine(record.Contragent.ShortName + " - ошибка - " + e.Message);
                    }

                }
            }

            romCalcTask.CalcState = "Рассчитано";
            sw.Flush();
            stream.Position = 0;
            romCalcTask.FileInfo = _fileManager.SaveFile(stream, "result.txt");
            ROMCalcTaskDomain.Update(romCalcTask);
        //    sw.Close();
           


            return JsSuccess();
        }

    }
}
