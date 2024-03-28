namespace Bars.GkhCr.DomainService
{
    using System;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using System.Linq;
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.IoC;

    using Castle.Windsor;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;

    using LogMap;

    public class SpecialTypeWorkCrHistoryService : ISpecialTypeWorkCrHistoryService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод  восстановления записи из истории
        /// </summary>
        public IDataResult Restore(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var twHistoryDomain = this.Container.ResolveDomain<SpecialTypeWorkCrHistory>();
            var twRemovalDomain = this.Container.ResolveDomain<SpecialTypeWorkCrRemoval>();
            var twDomain = this.Container.ResolveRepository<SpecialTypeWorkCr>(); // тут нужен именно IRepository

            using (this.Container.Using(twHistoryDomain, twHistoryDomain, twDomain))
            {
                var history = twHistoryDomain.Load(id);

                if (history == null)
                    return new BaseDataResult(false, $"Не удалось получить запись истории по Id = {id}");

                var typeWork = twDomain.Load(history.TypeWorkCr.Id);

                if (typeWork.IsActive)
                    return new BaseDataResult(false, "Данный вид работы уже восстановлен и не являеться удаленным");

                // Делаем запись вида работ активной так как ее восстановили
                typeWork.IsActive = true;
                twDomain.Update(typeWork);

                twRemovalDomain.GetAll()
                    .Where(x => x.TypeWorkCr.Id == typeWork.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => twRemovalDomain.Delete(x));

                // удаляем запись об удалении так как ее восстановили
                twHistoryDomain.Delete(history.Id);

                return new BaseDataResult();
            }
        }

        public SpecialTypeWorkCrHistory HistoryAfterCreation(SpecialTypeWorkCr typeWork, int? newYear = null)
        {
            SpecialTypeWorkCrHistory result = null;
 
            var programChangeDomain = this.Container.Resolve<IDomainService<ProgramCrChangeJournal>>();
            var twHistoryDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrHistory>>();
            var user = this.Container.Resolve<IUserIdentity>();

            using (this.Container.Using(programChangeDomain, twHistoryDomain, user))
            {
                result = new SpecialTypeWorkCrHistory
                {
                    TypeWorkCr = typeWork,
                    TypeAction = TypeWorkCrHistoryAction.Creation,
                    TypeReason = TypeWorkCrReason.NotSet,
                    FinanceSource = typeWork.FinanceSource,
                    Volume = typeWork.Volume,
                    Sum = typeWork.Sum,
                    YearRepair = typeWork.YearRepair,
                    NewYearRepair = newYear.HasValue ? newYear : typeWork.YearRepair,
                    UserName = user.Name
                };
                
                twHistoryDomain.Save(result);

                return result;

            }
        }

        public SpecialTypeWorkCrHistory HistoryAfterChange(SpecialTypeWorkCr typeWork, SpecialTypeWorkCr oldValue)
        {
            SpecialTypeWorkCrHistory result = null;

            if (!(oldValue.Volume != typeWork.Volume 
                || oldValue.YearRepair != typeWork.YearRepair
                || oldValue.Sum != typeWork.Sum
                || (oldValue.FinanceSource != null && typeWork.FinanceSource == null)
                || (oldValue.FinanceSource == null && typeWork.FinanceSource != null)
                || (oldValue.FinanceSource != null && typeWork.FinanceSource != null
                    && oldValue.FinanceSource.Id != typeWork.FinanceSource.Id)))
            {
                return result;
            }

            var twHistoryDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrHistory>>();
            var user = this.Container.Resolve<IUserIdentity>();
            try
            {
                // получаем запись о создании вида работы
                // Если запись о создании имеется, то всегда берем год ремонта из этой записи чтобы не тащить год ремонта из ДПКР 
                var addHistory =
                    twHistoryDomain.FirstOrDefault(
                        x => x.TypeWorkCr.Id == typeWork.Id && x.TypeAction == TypeWorkCrHistoryAction.Creation);
                
                result = new SpecialTypeWorkCrHistory();

                result.TypeWorkCr = oldValue;
                result.TypeAction = TypeWorkCrHistoryAction.Modification;
                result.TypeReason = TypeWorkCrReason.NotSet;
                result.FinanceSource = oldValue.FinanceSource;
                result.Volume = oldValue.Volume;
                result.Sum = oldValue.Sum;
                result.YearRepair = addHistory != null ? addHistory.YearRepair : oldValue.YearRepair;
                result.NewYearRepair = typeWork.YearRepair;
                result.UserName = user.Name;

                twHistoryDomain.Save(result);

                return result;

            }
            finally
            {
                this.Container.Release(twHistoryDomain);
            }
        }

        public SpecialTypeWorkCrHistory HistoryAfterRemove(SpecialTypeWorkCrRemoval typeWorkRemoval)
        {
            SpecialTypeWorkCrHistory result = null;

            var twHistoryDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrHistory>>();
            var typeRemovalDomain = this.Container.Resolve<IDomainService<SpecialTypeWorkCrRemoval>>();
            var fileInfoService = this.Container.Resolve<IDomainService<FileInfo>>();
            var user = this.Container.Resolve<IUserIdentity>();
            var auditInfo = this.Container.Resolve<RequestingUserInformation>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var logDomain = this.Container.Resolve<IDomainService<LogEntity>>();

            using (this.Container.Using(twHistoryDomain, typeRemovalDomain, fileInfoService, user, auditInfo, sessionProvider, logDomain))
            {
                var typeWork = typeWorkRemoval.TypeWorkCr;

                var twHistoryCreation = twHistoryDomain.FirstOrDefault(
                        x => x.TypeWorkCr.Id == typeWork.Id && x.TypeAction == TypeWorkCrHistoryAction.Creation);

                if (twHistoryCreation != null)
                {
                    var twHistoryModification = twHistoryDomain.FirstOrDefault(
                        x => x.TypeWorkCr.Id == typeWork.Id && x.TypeAction == TypeWorkCrHistoryAction.Modification);

                    //если есть запись об изменении, она может ссылаться на тот же файл, поэтому удаляем файл только в случае отсутствия данной записи
                    if (twHistoryModification == null)
                    {
                        var fileDoc = typeRemovalDomain.GetAll()
                            .Where(x => x.TypeWorkCr.Id == typeWork.Id)
                            .Where(x => x.TypeReason == TypeWorkCrReason.NotSet)
                            .OrderByDescending(z => z.ObjectCreateDate)
                            .Select(x => x.FileDoc)
                            .FirstOrDefault();

                        if (fileDoc != null)
                        {
                            fileInfoService.Delete(fileDoc.Id);
                        }
                    }

                    twHistoryDomain.Delete(twHistoryCreation.Id);
                }

                result = new SpecialTypeWorkCrHistory
                {
                    TypeWorkCr = typeWork,
                    TypeAction = TypeWorkCrHistoryAction.Removal,
                    TypeReason = typeWorkRemoval.TypeReason,
                    FinanceSource = typeWork.FinanceSource,
                    Volume = typeWork.Volume,
                    Sum = typeWork.Sum,
                    YearRepair = typeWorkRemoval.YearRepair,
                    NewYearRepair = typeWorkRemoval.NewYearRepair,
                    UserName = user.Name,
                    StructElement = typeWorkRemoval.StructElement
                };


                twHistoryDomain.Save(result);

                //TypeWorkCr не удаляется физически, поэтому невозможно получить запись типа Удаление в логе
                //через механизм AuditLogMap. Создаем вручную.
                var log = new LogEntity
                {
                    SessionId = sessionProvider.GetCurrentSession().GetSessionImplementation().SessionId.ToString(),
                    ActionKind = ActionKind.Delete,
                    ChangeDate = DateTime.UtcNow,
                    EntityDescription = SpecialTypeWorkCrLogMap.GetDescription(typeWork),
                    EntityId = typeWork.Id,
                    EntityType = typeof (SpecialTypeWorkCr).FullName,
                    UserId = user.UserId.ToString(),
                    UserIpAddress = auditInfo.RequestIpAddress,
                    UserDescription = user.Name,
                    UserLogin = user.Name,
                    UserName = user.Name
                };
                logDomain.Save(log);

                return result;
            }
        }
    }
}
