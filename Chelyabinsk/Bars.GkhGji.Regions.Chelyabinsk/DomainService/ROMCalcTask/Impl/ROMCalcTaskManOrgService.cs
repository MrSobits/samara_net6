namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
  

    using Castle.Windsor;

    public class ROMCalcTaskManOrgService : IROMCalcTaskManOrgService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<ROMCalcTask> ROMCalcTaskDomain { get; set; }

        public IDomainService<ROMCalcTaskManOrg> ROMCalcTaskManOrgDomain { get; set; }

        public IDataResult AddManOrg(BaseParams baseParams)
        {
            var taskId = baseParams.Params.ContainsKey("taskId") ? baseParams.Params["taskId"].ToLong() : 0;
            var manorgIds = baseParams.Params.ContainsKey("manorgIds") ? baseParams.Params["manorgIds"].ToString() : "";
            var listIds = new List<long>();
            var task = ROMCalcTaskDomain.Get(taskId);

            if (task == null)
            {
                return new BaseDataResult(false, "Не удалось определить задачу по Id " + task.ToStr());
            }


            var contragentIds = manorgIds.Split(',').Select(id => id.ToLong()).ToList();

            listIds.AddRange(ROMCalcTaskManOrgDomain.GetAll()
                                .Where(x => x.ROMCalcTask.Id == taskId)
                                .Select(x => x.Contragent.Id)
                                .Distinct()
                                .ToList());
            if (contragentIds.Count > 0)
            {
                task.CalcState = "Готово к расчету";
            }

            foreach (var newId in contragentIds)
            {

                // Если среди существующих документов уже есть такой документ то пролетаем мимо
                if (listIds.Contains(newId))
                    continue;

                // Если такого решения еще нет то добалвяем
                var newObj = new ROMCalcTaskManOrg();
                newObj.Contragent = ContragentDomain.Get(newId);
                newObj.ROMCalcTask = task;
                newObj.ObjectVersion = 1;
                newObj.ObjectCreateDate = DateTime.Now;
                newObj.ObjectEditDate = DateTime.Now;

                ROMCalcTaskManOrgDomain.Save(newObj);
            }
            ROMCalcTaskDomain.Update(task);

            return new BaseDataResult();
        }


    }
}