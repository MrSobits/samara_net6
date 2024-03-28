namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class TypeWorkCrServiceInterceptor : EmptyDomainInterceptor<TypeWorkCr>
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PSDWorks> PSDWorksCrDomain { get; set; }

        public IDomainService<AdditWork> AdditWorkDomain { get; set; }

        public IDomainService<TypeWorkCrAddWork> TypeWorkCrAddWorkDomain { get; set; }

        public IDomainService<ArchiveMultiplyContragentSmr> ArchiveMCDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            // на всякий случай выставляем в true, создаваемые записи не могут быть не активными
            entity.IsActive = true;
            return ValidateTypeWork(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            var historyService = Container.Resolve<ITypeWorkCrHistoryService>();
            
            try
            {
                // Запускаем создание истории для добавления вида работы 
                historyService.HistoryAfterCreation(entity);
                return this.Success();
            }
            finally 
            {
                Container.Release(historyService);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            try
            {
                if (entity.Work.Name == "Разработка ПСД" || entity.Work.IsPSD)
                {
                    var addworkList = AdditWorkDomain.GetAll()
                        .Where(x => x.Work == entity.Work).OrderBy(x=> x.Code).ToList();
                    var typewaddw = TypeWorkCrAddWorkDomain.GetAll()
                        .Where(x => x.TypeWorkCr == entity).FirstOrDefault();
                    if (typewaddw == null && entity.DateStartWork.HasValue && entity.DateEndWork.HasValue)
                    {
                        int daysgone = 0;
                        foreach (AdditWork adw in addworkList)
                        {
                            int daysInt = Convert.ToInt16(adw.Percentage);
                            TypeWorkCrAddWork newWork = new TypeWorkCrAddWork
                            {
                                TypeWorkCr = entity,
                                AdditWork = adw,
                                DateStartWork = entity.DateStartWork.Value.AddDays(daysgone),
                                DateEndWork = entity.DateStartWork.Value.AddDays(daysInt),
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                ObjectVersion = 1,
                                Queue = adw.Queue,
                                Required = false
                            };
                            TypeWorkCrAddWorkDomain.Save(newWork);
                            daysgone += daysInt;

                        }
                    }
                }
            }
            catch
            { }
            var archContr = ArchiveMCDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.Id)
                .Select(x => x.Contragent).ToList();

            if (archContr.Count == 0)
            {
                entity.CostSum = 0;
                entity.PercentOfCompletion = 0;
                entity.VolumeOfCompletion = 0;
                entity.ManufacturerName = "";
                return Success();
            }

            var archSum = ArchiveMCDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.Id)
                .SafeSum(x => x.CostSum);

            int archCount = ArchiveMCDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.Id).Count();


            var archPercents = ArchiveMCDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.Id)
                .SafeSum(x => x.PercentOfCompletion);

            archPercents = Decimal.Round(archPercents / archCount, 1);

            var archVolume = ArchiveMCDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.Id)
                .SafeSum(x => x.VolumeOfCompletion);

            var psdworks = PSDWorksCrDomain.GetAll()
                .Where(x => x.PSDWork == entity)
                .Select(x => x.Cost).ToList();

            decimal itogo = psdworks.Sum();

            if (itogo > 0)
            {
                entity.Sum = itogo;
            }

            entity.CostSum = archSum;
            entity.PercentOfCompletion = archPercents;
            entity.VolumeOfCompletion = archVolume;
            // проверяем суммы и объем по работе
            CheckSumAndVolume(entity);

            var result = ValidateTypeWork(service, entity);

            if (!result.Success)
            {
                return result;
            }
            else
            {
                if (archContr.Count == 1)
                {
                    entity.ManufacturerName = archContr[0].ShortName;
                }
                else
                {
                    entity.ManufacturerName = "";
                }
      
            }

            // добавляем запись в архив, если изменены поля находящиеся во вкладке: Ход выполнения работ или Численность рабочих
            var archiveSmrService = Container.Resolve<IDomainService<ArchiveSmr>>();

            // последняя запись в архиве по данному виду работ
            var lastChangedTypeWorkCr = archiveSmrService.GetAll()
                                         .Where(x => x.TypeWorkCr.Id == entity.Id)
                                         .OrderByDescending(x => x.DateChangeRec).ThenByDescending(x => x.Id)
                                         .FirstOrDefault();

            // проверка изменились ли поля
            if (IsRecordChanged(lastChangedTypeWorkCr, entity))
            {
                TypeArchiveSmr typeArchiveSmr;
                if (lastChangedTypeWorkCr == null)
                {
                    typeArchiveSmr = entity.CountWorker != null ? TypeArchiveSmr.WorkersCount : TypeArchiveSmr.ProgressExecutionWork;
                }
                else
                {
                    typeArchiveSmr = entity.CountWorker != lastChangedTypeWorkCr.CountWorker ? TypeArchiveSmr.WorkersCount : TypeArchiveSmr.ProgressExecutionWork;
                }

                var newArchiveSmr = new ArchiveSmr
                {
                    ManufacturerName = entity.ManufacturerName,
                    PercentOfCompletion = entity.PercentOfCompletion,
                    StageWorkCr = entity.StageWorkCr,
                    VolumeOfCompletion = entity.VolumeOfCompletion,
                    CostSum = entity.CostSum,
                    CountWorker = entity.CountWorker,
                    DateChangeRec = DateTime.Now,
                    TypeArchiveSmr = typeArchiveSmr,
                    TypeWorkCr = entity
                };

                // сохраняем запись в архив
                archiveSmrService.Save(newArchiveSmr);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            var estCalcDomain = this.Container.Resolve<IDomainService<EstimateCalculation>>();
            var perfActDomain = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            var historyDomain = this.Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var archiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var removalDomain = this.Container.Resolve<IDomainService<TypeWorkCrRemoval>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                               {
                                   id => estCalcDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Cметный расчет по работе" : null,
                                   id => perfActDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Акт выполненных работ" : null,
                                   id => removalDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Причина удаления работы" : null,
                                   id => entity.IsDpkrCreated && historyDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "История изменения" : null // История для работ сформирвоанных из ДПКР не может быть удалена 
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                var archiveSmrIds = archiveSmr.GetAll()
                     .Where(x => x.TypeWorkCr.Id == entity.Id)
                     .Select(x => x.Id)
                     .ToArray();

                foreach (var id in archiveSmrIds)
                {
                    archiveSmr.Delete(id);
                }

                // Поскольку работа не из ДПКР то удаляем историю 
                var historyIds = historyDomain.GetAll()
                     .Where(x => x.TypeWorkCr.Id == entity.Id)
                     .Select(x => x.Id)
                     .ToArray();

                foreach (var id in historyIds)
                {
                    historyDomain.Delete(id);
                }

                return Success();
            }
            finally 
            {
                this.Container.Release(estCalcDomain);
                this.Container.Release(perfActDomain);
                this.Container.Release(historyDomain);
                this.Container.Release(archiveSmr);
            }
        }

        public virtual bool InvalidVolume(TypeWorkCr entity)
        {
            return entity.Volume < entity.VolumeOfCompletion;
        }

        private BaseDataResult ValidateTypeWork(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            var dateStart = entity.ObjectCr.ProgramCr.Period.DateStart;
            var dateEnd = entity.ObjectCr.ProgramCr.Period.DateEnd;
            var periodName = entity.ObjectCr.ProgramCr.Period.Name;
            var programName = entity.ObjectCr.ProgramCr.Name;

            if (entity.YearRepair.HasValue && (dateStart.Year-2 > entity.YearRepair.Value || (dateEnd.HasValue && dateEnd.Value.Year+2 < entity.YearRepair)))
            {
                return Failure(string.Format("Год ремонта не может выходить за период '{0}' краткосрочной программы '{1}'", periodName, programName));
            }

            if (InvalidVolume(entity))
            {
                return Failure("Объем выполнения не может быть больше чем плановый объем!");
            }

            if (entity.Sum < entity.CostSum)
            {
                return Failure("Сумма расходов не может быть больше чем плановая сумма!");
            }

            if (service.GetAll().Any(x => entity.YearRepair.HasValue && x.ObjectCr.Id == entity.ObjectCr.Id && x.YearRepair == entity.YearRepair && x.Work.Id == entity.Work.Id && x.Id != entity.Id))
            {
                return Failure("Есть запись с данной работой в этом году");
            }

            var controlDateMunicipalityLimitDateDomain = this.Container.Resolve<IDomainService<ControlDateMunicipalityLimitDate>>();
            var controlDateDomain = this.Container.Resolve<IDomainService<ControlDate>>();

            DateTime? controlDate;
            using (this.Container.Using(controlDateMunicipalityLimitDateDomain, controlDateDomain))
            {
                var municipalityControlDate = controlDateMunicipalityLimitDateDomain
                    .FirstOrDefault(x => x.ControlDate.ProgramCr.Id == entity.ObjectCr.ProgramCr.Id
                        && x.ControlDate.Work.Id == entity.Work.Id
                        && x.Municipality.Id == entity.ObjectCr.RealityObject.Municipality.Id)?.LimitDate;

                controlDate = municipalityControlDate ?? controlDateDomain
                    .GetAll()
                    .Where(x => x.ProgramCr.Id == entity.ObjectCr.ProgramCr.Id && x.Work.Id == entity.Work.Id)
                    .Select(x => x.Date)
                    .FirstOrDefault();
            }

            if (entity.DateStartWork.HasValue)
            {
                if (entity.DateEndWork.HasValue)
                {
                    if (entity.DateEndWork <= entity.DateStartWork)
                    {
                        return Failure("Дата начала работ должна быть меньше даты окончания работ");
                    }
                }
                else if (entity.DateStartWork > (entity.AdditionalDate ?? controlDate))
                {
                    return Failure(string.Format("Дата начала работы  {0} не может превышать предельный срок - {1}",
                                                     entity.Work.Name, (entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()));
                }
            }

            if (entity.DateEndWork > (entity.AdditionalDate ?? controlDate))
            {
                return Failure(string.Format("Дата окончания работ {0} не может быть больше предельного срока окончания - {1}",
                         entity.Work.Name, (entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()));
            }

            return this.Success();
        }

        private void CheckSumAndVolume(TypeWorkCr work)
        {
            var acts = Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                .Where(x => x.TypeWorkCr.Id == work.Id)
                .Select(x => new { x.Sum, x.Volume })
                .ToArray();

            var actsTotalSum = acts.Select(x => x.Sum).Sum();
            var actsTotalVolume = acts.Select(x => x.Volume).Sum();

            if (work.Sum < actsTotalSum || work.Volume < actsTotalVolume)
            {
                throw new ValidationException("Введенные сумма и/или объем по видам работ меньше, чем указано в актах выполненных работ. Сохранение отменено.");
            }
        }

        private bool IsRecordChanged(ArchiveSmr oldValue, TypeWorkCr newValue)
        {
            if (oldValue != null)
            {
                if (newValue.ManufacturerName != oldValue.ManufacturerName
                    || newValue.PercentOfCompletion != oldValue.PercentOfCompletion
                    || newValue.StageWorkCr != oldValue.StageWorkCr
                    || newValue.VolumeOfCompletion != oldValue.VolumeOfCompletion
                    || newValue.CostSum != oldValue.CostSum
                    || newValue.CountWorker != oldValue.CountWorker)
                {
                    return true;
                }
            }
            else
            {
                if (newValue.ManufacturerName != string.Empty
                    || newValue.PercentOfCompletion != null
                    || newValue.StageWorkCr != null
                    || newValue.VolumeOfCompletion != null
                    || newValue.CostSum != null
                    || newValue.CountWorker != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}