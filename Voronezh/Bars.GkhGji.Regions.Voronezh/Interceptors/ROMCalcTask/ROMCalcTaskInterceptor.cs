namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;

    class ROMCalcTaskInterceptor : EmptyDomainInterceptor<ROMCalcTask>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<ROMCalcTaskManOrg> ROMCalcTaskManOrgDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ROMCalcTask> service, ROMCalcTask entity)
        {
            try {   

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                {
                    entity.Inspector = thisOperator.Inspector;
                    entity.ObjectCreateDate = DateTime.Now;
                    entity.ObjectEditDate = DateTime.Now;
                    entity.CalcState = "Не выбраны организации для расчета";
                }
                else
                    return Failure("Расчет категорий риска доступен только сотрудникам ГЖИ");

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать задачу");
            }
            
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ROMCalcTask> service, ROMCalcTask entity)
        {
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                {
                    entity.Inspector = thisOperator.Inspector;
                    entity.ObjectEditDate = DateTime.Now;
                    var orgList = ROMCalcTaskManOrgDomain.GetAll()
                        .Where(x => x.ROMCalcTask == entity)
                        .Select(x => x.Contragent.Id).ToList();
                    if (orgList.Count > 0 && entity.CalcState != "Рассчитано")
                    {
                        entity.CalcState = "Ожидает расчета";
                    }
                    else if(entity.CalcState != "Рассчитано")
                    {
                        entity.CalcState = "Не выбраны организации для расчета";
                    }
                }
                else
                    return Failure("Расчет категорий риска доступен только сотрудникам ГЖИ");

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ROMCalcTask> service, ROMCalcTask entity)
        {
            try
            {
                var reportRow = ROMCalcTaskManOrgDomain.GetAll()
               .Where(x => x.ROMCalcTask.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    ROMCalcTaskManOrgDomain.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }
           
        }
    }
}
