namespace Bars.Gkh.Repair.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    public class RepairWorkServiceInterceptor : EmptyDomainInterceptor<RepairWork>
    {
        public IDomainService<RepairWorkArchive> repairWorkArchiveDomain { get; set; }

        public IDomainService<RepairControlDate> RepairControlDateDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<RepairWork> service, RepairWork entity)
        {
            if (service.GetAll().Any(x => x.RepairObject.Id == entity.RepairObject.Id && x.Work.Id == entity.Work.Id))
            {
                return Failure("Запись с такой работой уже существует");
            }

            return this.ValidateRepairWork(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RepairWork> service, RepairWork entity)
        {
            return this.ValidateRepairWork(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<RepairWork> service, RepairWork entity)
        {
            this.CreateArhiveRec(entity);

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<RepairWork> service, RepairWork entity)
        {
            // последняя запись в архиве по данному виду работ
            var lastChangedRepairWork = this.repairWorkArchiveDomain.GetAll()
                .Where(x => x.RepairWork.Id == entity.Id)
                .OrderByDescending(x => x.DateChangeRec)
                .ThenByDescending(x => x.Id)
                .FirstOrDefault();

            if (this.IsRecordChanged(lastChangedRepairWork, entity))
            {
                this.CreateArhiveRec(entity);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RepairWork> service, RepairWork entity)
        {
            var archiveRepairWorkIds = this.repairWorkArchiveDomain.GetAll()
                 .Where(x => x.RepairWork.Id == entity.Id)
                 .Select(x => x.Id)
                 .ToArray();

            foreach (var archiveRepairWorkId in archiveRepairWorkIds)
            {
                this.repairWorkArchiveDomain.Delete(archiveRepairWorkId);
            }

            return Success();
        }

        private BaseDataResult ValidateRepairWork(IDomainService<RepairWork> service, RepairWork entity)
        {
            if (entity.Volume < entity.VolumeOfCompletion)
            {
                return Failure("Объем выполнения не может быть больше чем плановый объем!");
            }

            if (entity.Sum < entity.CostSum)
            {
                return Failure("Сумма расходов не может быть больше чем плановая сумма!");
            }

            var controlDate = this.RepairControlDateDomain.GetAll()
                         .Where(x => x.RepairProgram.Id == entity.RepairObject.RepairProgram.Id && x.Work.Id == entity.Work.Id)
                         .Select(x => x.Date)
                         .FirstOrDefault();


            if (entity.DateStart.HasValue)
            {
                if (entity.DateEnd.HasValue)
                {
                    if (entity.DateEnd <= entity.DateStart)
                    {
                        return Failure("Дата начала работ должна быть меньше даты окончания работ");
                    }

                    if (entity.DateStart > (entity.AdditionalDate ?? controlDate))
                    {
                        return
                            Failure(
                                string.Format(
                                    "Дата начала работы  {0} не может превышать предельный срок - {1}",
                                    entity.Work.Name,
                                    (entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()));
                    }
                }

                if (entity.DateEnd > (entity.AdditionalDate ?? controlDate))
                {
                    return
                        Failure(
                            string.Format(
                                "Дата окончания работ {0} не может быть больше предельного срока окончания - {1}",
                                entity.Work.Name,
                                (entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()));
                }
            }

            return this.Success();
        }

        private bool IsRecordChanged(RepairWorkArchive oldValue, RepairWork newValue)
        {
            if (oldValue == null)
            {
                return true;
            }

            return newValue.PercentOfCompletion != oldValue.PercentOfCompletion
                || newValue.VolumeOfCompletion != oldValue.VolumeOfCompletion 
                || newValue.CostSum != oldValue.CostSum;
        }

        private void CreateArhiveRec(RepairWork entity)
        {
            repairWorkArchiveDomain.Save(
                new RepairWorkArchive
                {
                    RepairWork = entity,
                    DateChangeRec = DateTime.Now,
                    CostSum = entity.CostSum,
                    PercentOfCompletion = entity.PercentOfCompletion,
                    VolumeOfCompletion = entity.VolumeOfCompletion
                });
        }
    }
}