namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Интерцептор для <see cref="SpecialTypeWorkCr"/>
    /// </summary>
    public class SpecialTypeWorkCrServiceInterceptor : EmptyDomainInterceptor<SpecialTypeWorkCr>
    {
        public IWindsorContainer Container { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SpecialTypeWorkCr> service, SpecialTypeWorkCr entity)
        {
            // на всякий случай выставляем в true, создаваемые записи не могут быть не активными
            entity.IsActive = true;
            return this.ValidateTypeWork(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<SpecialTypeWorkCr> service, SpecialTypeWorkCr entity)
        {
            var historyService = this.Container.Resolve<ISpecialTypeWorkCrHistoryService>();

            using (this.Container.Using(historyService))
            {
                // Запускаем создание истории для добавления вида работы 
                historyService.HistoryAfterCreation(entity);
                return this.Success();
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialTypeWorkCr> service, SpecialTypeWorkCr entity)
        {
            var result = this.ValidateTypeWork(service, entity);

            if (!result.Success)
            {
                return result;
            }

            // проверяем суммы и объем по работе
            this.CheckSumAndVolume(entity);

            // добавляем запись в архив, если изменены поля находящиеся во вкладке: Ход выполнения работ или Численность рабочих
            var archiveSmrService = this.Container.Resolve<IDomainService<SpecialArchiveSmr>>();

            using (this.Container.Using(archiveSmrService))
            {
                // последняя запись в архиве по данному виду работ
                var lastChangedTypeWorkCr = archiveSmrService.GetAll()
                    .Where(x => x.TypeWorkCr.Id == entity.Id)
                    .OrderByDescending(x => x.DateChangeRec).ThenByDescending(x => x.Id)
                    .FirstOrDefault();

                // проверка изменились ли поля
                if (this.IsRecordChanged(lastChangedTypeWorkCr, entity))
                {
                    TypeArchiveSmr typeArchiveSmr;
                    if (lastChangedTypeWorkCr == null)
                    {
                        typeArchiveSmr = entity.CountWorker != null ? TypeArchiveSmr.WorkersCount : TypeArchiveSmr.ProgressExecutionWork;
                    }
                    else
                    {
                        typeArchiveSmr = entity.CountWorker != lastChangedTypeWorkCr.CountWorker
                            ? TypeArchiveSmr.WorkersCount
                            : TypeArchiveSmr.ProgressExecutionWork;
                    }

                    var newArchiveSmr = new SpecialArchiveSmr
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

                return this.Success();
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialTypeWorkCr> service, SpecialTypeWorkCr entity)
        {
            var estCalcDomain = this.Container.Resolve<IDomainService<SpecialEstimateCalculation>>();
            var perfActDomain = this.Container.Resolve<IDomainService<SpecialPerformedWorkAct>>();
            var historyDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrHistory>>();
            var archiveSmr = this.Container.Resolve<IDomainService<SpecialArchiveSmr>>();
            var removalDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrRemoval>>();

            using (this.Container.Using(estCalcDomain, perfActDomain, historyDomain, archiveSmr, removalDomain))
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => estCalcDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Cметный расчет по работе" : null,
                    id => perfActDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Акт выполненных работ" : null,
                    id => removalDomain.GetAll().Any(x => x.TypeWorkCr.Id == id) ? "Причина удаления работы" : null,
                    id => entity.IsDpkrCreated && historyDomain.GetAll().Any(x => x.TypeWorkCr.Id == id)
                        ? "История изменения"
                        : null // История для работ сформирвоанных из ДПКР не может быть удалена 
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + $" {str}; ");
                    message = $"Существуют связанные записи в следующих таблицах: {message}";
                    return this.Failure(message);
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

                return this.Success();
            }
        }

        public virtual bool InvalidVolume(SpecialTypeWorkCr entity)
        {
            return entity.Volume < entity.VolumeOfCompletion;
        }

        private BaseDataResult ValidateTypeWork(IDomainService<SpecialTypeWorkCr> service, SpecialTypeWorkCr entity)
        {
            var controlDateDomain = this.Container.Resolve<IDomainService<ControlDate>>();
            var municipalityLimitDateDomain = this.Container.Resolve<IDomainService<ControlDateMunicipalityLimitDate>>();

            using (this.Container.Using(controlDateDomain, municipalityLimitDateDomain))
            {
                var dateStart = entity.ObjectCr.ProgramCr.Period.DateStart;
                var dateEnd = entity.ObjectCr.ProgramCr.Period.DateEnd;
                var periodName = entity.ObjectCr.ProgramCr.Period.Name;
                var programName = entity.ObjectCr.ProgramCr.Name;

                if (entity.YearRepair.HasValue &&
                    (dateStart.Year > entity.YearRepair.Value ||
                        (dateEnd.HasValue && dateEnd.Value.Year < entity.YearRepair)))
                {
                    return this.Failure($"Год ремонта не может выходить за период '{periodName}' краткосрочной программы '{programName}'");
                }

                if (this.InvalidVolume(entity))
                {
                    return this.Failure("Объем выполнения не может быть больше чем плановый объем!");
                }

                if (entity.Sum < entity.CostSum)
                {
                    return this.Failure("Сумма расходов не может быть больше чем плановая сумма!");
                }

                if (service.GetAll().Any(x => entity.YearRepair.HasValue 
                    && x.ObjectCr.Id == entity.ObjectCr.Id 
                    && x.YearRepair == entity.YearRepair 
                    && x.Work.Id == entity.Work.Id
                    && x.Id != entity.Id))
                {
                    return this.Failure("Есть запись с данной работой в этом году");
                }

                var municipalityLimitDate = municipalityLimitDateDomain
                    .FirstOrDefault(x => x.ControlDate.ProgramCr.Id == entity.ObjectCr.ProgramCr.Id
                        && x.ControlDate.Work.Id == entity.Work.Id
                        && x.Municipality.Id == entity.ObjectCr.RealityObject.Municipality.Id)?.LimitDate;

                var controlDate = municipalityLimitDate ?? controlDateDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == entity.ObjectCr.ProgramCr.Id && x.Work.Id == entity.Work.Id)
                    .Select(x => x.Date)
                    .FirstOrDefault();

                if (entity.DateStartWork.HasValue)
                {
                    if (entity.DateEndWork.HasValue)
                    {
                        if (entity.DateEndWork <= entity.DateStartWork)
                        {
                            return this.Failure("Дата начала работ должна быть меньше даты окончания работ");
                        }
                    }
                    else if (entity.DateStartWork > (entity.AdditionalDate ?? controlDate))
                    {
                        return this.Failure(
                            $"Дата начала работы  {entity.Work.Name} не может превышать предельный срок - {(entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()}");
                    }
                }

                if (entity.DateEndWork > (entity.AdditionalDate ?? controlDate))
                {
                    return this.Failure(
                        $"Дата окончания работ {entity.Work.Name} не может быть больше предельного срока окончания - {(entity.AdditionalDate ?? controlDate).ToDateTime().ToShortDateString()}");
                }

                return this.Success();
            }
        }

        private void CheckSumAndVolume(SpecialTypeWorkCr work)
        {
            var actsDomain = this.Container.Resolve<IDomainService<SpecialPerformedWorkAct>>();

            using (this.Container.Using(actsDomain))
            {
                var acts = actsDomain.GetAll()
                    .Where(x => x.TypeWorkCr.Id == work.Id)
                    .Select(x => new { x.Sum, x.Volume })
                    .ToArray();

                var actsTotalSum = acts.Select(x => x.Sum).Sum();
                var actsTotalVolume = acts.Select(x => x.Volume).Sum();

                if (work.Sum < actsTotalSum || work.Volume < actsTotalVolume)
                {
                    throw new ValidationException(
                        "Введенные сумма и/или объем по видам работ меньше, чем указано в актах выполненных работ. Сохранение отменено.");
                }
            }
        }

        private bool IsRecordChanged(SpecialArchiveSmr oldValue, SpecialTypeWorkCr newValue)
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