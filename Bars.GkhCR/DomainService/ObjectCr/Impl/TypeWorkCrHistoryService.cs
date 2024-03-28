namespace Bars.GkhCr.DomainService
{
    using System;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using System.Linq;
    using B4.Modules.NHibernateChangeLog;
    using Castle.Windsor;
    using Bars.B4.Modules.FileStorage;
    using LogMap;

    public class TypeWorkCrHistoryService : ITypeWorkCrHistoryService
    {

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод  восстановления записи из истории
        /// </summary>
        public IDataResult Restore(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);

            var twHistoryDomain = Container.ResolveDomain<TypeWorkCrHistory>();
            var twRemovalDomain = Container.ResolveDomain<TypeWorkCrRemoval>();
            var twDomain = Container.ResolveRepository<TypeWorkCr>(); // тут нужен именно IRepository
            var dpkrTypeWorkService = Container.Resolve<IDpkrTypeWorkService>();
            try
            {
                var history = twHistoryDomain.Load(id);

                if (history == null)
                    return new BaseDataResult(false, string.Format("Не удалось получить запись истории по Id = {0}", id));

                var typeWork = twDomain.Load(history.TypeWorkCr.Id);

                if (typeWork.IsActive)
                    return new BaseDataResult(false, "Данный вид работы уже восстановлен и не являеться удаленным");

                if (!dpkrTypeWorkService.HasTypeWorkReferenceInDpkr(typeWork))
                {
                    return new BaseDataResult(false,
                        "Долгосрочная программа актуализирована на основе данных текущей Краткосрочной программы. Вид работ или КЭ был удален из Долгосрочной программы. Восстановление записи невозможно.");
                }

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
            finally
            {
                Container.Release(twHistoryDomain);
                Container.Release(twDomain);
            }
        }

        public TypeWorkCrHistory HistoryAfterCreation(TypeWorkCr typeWork, int? newYear = null)
        {
            TypeWorkCrHistory result = null;
 
            var programChangeDomain = Container.Resolve<IDomainService<ProgramCrChangeJournal>>();
            var twHistoryDomain = Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var user = Container.Resolve<IUserIdentity>();

            try
            {
                // Запись истории для Создания вида работ создается в том случае если
                // Программе разрешено добавлять работы из ДПКР + Запись создается вручную , тоест ьзапись вида работ не сформирована из ДПКР 
                // (это нужно для Актуализации чтобы пониманить какие записи были добавлены после того как программа уже сформирована)
                // иначе не добавляем
                if (!(typeWork.IsDpkrCreated == false && typeWork.ObjectCr.ProgramCr.AddWorkFromLongProgram == AddWorkFromLongProgram.Use))
                {
                    return result;
                }

                result = new TypeWorkCrHistory();

                result.TypeWorkCr = typeWork;
                result.TypeAction = TypeWorkCrHistoryAction.Creation;
                result.TypeReason = TypeWorkCrReason.NotSet;
                result.FinanceSource = typeWork.FinanceSource;
                result.Volume = typeWork.Volume;
                result.Sum = typeWork.Sum;
                result.YearRepair = typeWork.YearRepair;
                result.NewYearRepair = newYear.HasValue ? newYear : typeWork.YearRepair;
                result.UserName = user.Name;

                twHistoryDomain.Save(result);

                return result;

            }
            finally
            {
                Container.Release(programChangeDomain);
                Container.Release(twHistoryDomain);
            }

        }

        public TypeWorkCrHistory HistoryAfterChange(TypeWorkCr typeWork, TypeWorkCr oldValue)
        {
            TypeWorkCrHistory result = null;

            if (!(oldValue.Volume != typeWork.Volume || oldValue.YearRepair != typeWork.YearRepair
                || oldValue.Sum != typeWork.Sum
                || (oldValue.FinanceSource != null && typeWork.FinanceSource == null)
                || (oldValue.FinanceSource == null && typeWork.FinanceSource != null)
                || (oldValue.FinanceSource != null && typeWork.FinanceSource != null
                    && oldValue.FinanceSource.Id != typeWork.FinanceSource.Id)))
            {
                return result;
            }

            var twHistoryDomain = Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var user = Container.Resolve<IUserIdentity>();
            try
            {
                // Запись истории для изменения вида работ создается просто для всех видов работ котоорые добавляются в программу сформирвоанную из ДПКР
                // иначе недобавляем 
                if (typeWork.ObjectCr.ProgramCr.AddWorkFromLongProgram == AddWorkFromLongProgram.NotUse)
                {
                    return result;
                }

                // получаем запись о создании вида работы
                // Если запись о создании имеется, то всегда берем год ремонта из этой записи чтобы не тащить год ремонта из ДПКР 
                var addHistiry =
                    twHistoryDomain.FirstOrDefault(
                        x => x.TypeWorkCr.Id == typeWork.Id && x.TypeAction == TypeWorkCrHistoryAction.Creation);
                
                result = new TypeWorkCrHistory();

                result.TypeWorkCr = oldValue;
                result.TypeAction = TypeWorkCrHistoryAction.Modification;
                result.TypeReason = TypeWorkCrReason.NotSet;
                result.FinanceSource = oldValue.FinanceSource;
                result.Volume = oldValue.Volume;
                result.Sum = oldValue.Sum;
                result.YearRepair = addHistiry != null ? addHistiry.YearRepair : oldValue.YearRepair;
                result.NewYearRepair = typeWork.YearRepair;
                result.UserName = user.Name;

                twHistoryDomain.Save(result);

                return result;

            }
            finally
            {
                Container.Release(twHistoryDomain);
            }
        }

        public TypeWorkCrHistory HistoryAfterRemove(TypeWorkCrRemoval typeWorkRemoval)
        {
            TypeWorkCrHistory result = null;

            var twHistoryDomain = Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var typeRemovalDomain = Container.Resolve<IDomainService<TypeWorkCrRemoval>>();
            var fileInfoService = Container.Resolve<IDomainService<FileInfo>>();
            var user = Container.Resolve<IUserIdentity>();
            var auditInfo = Container.Resolve<RequestingUserInformation>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var logDomain = Container.Resolve<IDomainService<LogEntity>>();

            try
            {
                var typeWork = typeWorkRemoval.TypeWorkCr;

                // Запись истории для удаления Работы насамом деле у работы выставляеся IsActive = false и она непоказывается но фиически запись не удаляется
                if (typeWork.ObjectCr.ProgramCr.AddWorkFromLongProgram == AddWorkFromLongProgram.NotUse)
                {
                    return result;
                }

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

                result = new TypeWorkCrHistory();

                result.TypeWorkCr = typeWork;
                result.TypeAction = TypeWorkCrHistoryAction.Removal;
                result.TypeReason = typeWorkRemoval.TypeReason;
                result.FinanceSource = typeWork.FinanceSource;
                result.Volume = typeWork.Volume;
                result.Sum = typeWork.Sum;
                result.YearRepair = typeWorkRemoval.YearRepair;
                result.NewYearRepair = typeWorkRemoval.NewYearRepair;
                result.UserName = user.Name;
                result.StructElement = typeWorkRemoval.StructElement;

                twHistoryDomain.Save(result);

                //TypeWorkCr не удаляется физически, поэтому невозможно получить запись типа Удаление в логе
                //через механизм AuditLogMap. Создаем вручную.
                var log = new LogEntity
                {
                    SessionId = sessionProvider.GetCurrentSession().GetSessionImplementation().SessionId.ToString(),
                    ActionKind = ActionKind.Delete,
                    ChangeDate = DateTime.UtcNow,
                    EntityDescription = TypeWorkCrLogMap.GetDescription(typeWork),
                    EntityId = typeWork.Id,
                    EntityType = typeof (TypeWorkCr).FullName,
                    UserId = user.UserId.ToString(),
                    UserIpAddress = auditInfo.RequestIpAddress,
                    UserDescription = user.Name,
                    UserLogin = user.Name,
                    UserName = user.Name
                };
                logDomain.Save(log);

                return result;

            }
            finally
            {
                Container.Release(twHistoryDomain);
                Container.Release(typeRemovalDomain);
                Container.Release(fileInfoService);
                Container.Release(auditInfo);
                Container.Release(sessionProvider);
                Container.Release(logDomain);
            }
        }
    }
}
