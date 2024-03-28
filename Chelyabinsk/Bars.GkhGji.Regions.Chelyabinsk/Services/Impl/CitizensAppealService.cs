namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Services.DataContracts;
    using Castle.Windsor;
    using Remotion.Linq.Utilities;

    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Repositories;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    public partial class CitizensAppealService : ICitizensAppealService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container { get; }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public AppealTransferResult[] ImportInfoCitizensAppealRequest(CitizenAppeal[] appeals)
        {
            if (!appeals?.Any() ?? true)
            {
                return new[]
                {
                    new AppealTransferResult
                    {
                        IsUploaded = false,
                        AdditionalInformation = "Прислан пустой запрос"
                    }
                };
            }

            var result = new LinkedList<AppealTransferResult>();
            var validationResult = this.ValidateCitizenAppeals(appeals);

            //обрабатываем прошедшие валидацию
            foreach (var appeal in validationResult.Where(x => x.Value.Success))
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    using (this.Container.Using(tr))
                    {
                        var replaceResultOnRollback = false;
                        try
                        {
                            var savingResult = this.SaveCitizenAppeal(appeal.Key);

                            if (savingResult.Success)
                            {
                                result.AddLast(
                                    new AppealTransferResult
                                    {
                                        AppealUid = appeal.Key.AppealUid.ToStr(),
                                        IsUploaded = true
                                    });
                                replaceResultOnRollback = true;
                                tr.Commit();

                                this.TryClearFiles();
                            }
                            else
                            {
                                result.AddLast(
                                    new AppealTransferResult
                                    {
                                        AppealUid = appeal.Key?.AppealUid.ToStr(),
                                        IsUploaded = false,
                                        AdditionalInformation = savingResult.Message
                                    });

                                tr.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (replaceResultOnRollback)
                            {
                                result.RemoveLast();
                                result.AddLast(
                                    new AppealTransferResult
                                    {
                                        AppealUid = appeal.Key?.AppealUid.ToStr(),
                                        IsUploaded = false,
                                        AdditionalInformation = $"Ошибка во время сохранения транзакции: {(ex.InnerException ?? ex).Message}"
                                    });
                            }
                            tr.Rollback();
                        }
                    }
                }
            }

            //обрабатываем НЕ прошедшие валидацию
            foreach (var appeal in validationResult.Where(x => !x.Value.Success))
            {
                result.AddLast(
                    new AppealTransferResult
                    {
                        AppealUid = appeal.Key?.AppealUid.ToStr(),
                        IsUploaded = false,
                        AdditionalInformation = appeal.Value.Message
                    });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Импорт сведений об отмене обращения граждан
        /// </summary>
        public CitizensAppealCancelResult[] ImportInfoCitizensAppealCancelRequest(Guid[] appealUids)
        {
            var appealService = this.Container.ResolveDomain<AppealCits>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateRepository = this.Container.Resolve<IStateRepository>();
            var result = new List<CitizensAppealCancelResult>();

            using (this.Container.Using(appealService, stateProvider, stateRepository))
            {
                var cancelStatus = stateRepository.GetAllStates<AppealCits>(x => x.Code == AppealCits.CancellationRequire)
                    .FirstOrDefault();
                foreach (var appealUid in appealUids)
                {
                    try
                    {
                        var gkhAppeal = appealService.GetAll()
                            .SingleOrDefault(x => x.AppealUid == appealUid);

                        if (gkhAppeal == null)
                        {
                            throw new ArgumentException($"Обращение с идентификатором \"{appealUid}\" не найдено");
                        }

                        if (gkhAppeal.State.Code == AppealCits.Pending || gkhAppeal.State.Code == AppealCits.Work) 
                        {

                            if (cancelStatus == null)
                            {
                                throw new Exception("Для сущности 'Обращение граждан' не найден статус 'Требует отмены'");
                            }

                            stateProvider.ChangeState(gkhAppeal.Id, cancelStatus.TypeId, cancelStatus, "Импорт сведений об отмене обращения", false);

                            result.Add(new CitizensAppealCancelResult
                            {
                                AppealUid = appealUid.ToStr(),
                                IsAppealСancel = true
                            });
                        }
                        else
                        {
                            result.Add(new CitizensAppealCancelResult
                            {
                                AppealUid = appealUid.ToStr(),
                                IsAppealСancel = false,
                                AdditionalInformation = $"Статус обращения: '{gkhAppeal.State.Name}'"
                            });
                        }

                    }
                    catch (Exception ex)
                    {
                        result.Add(new CitizensAppealCancelResult
                        {
                            AppealUid = appealUid.ToStr(),
                            IsAppealСancel = false,
                            AdditionalInformation = ex.Message
                        });
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Сервис сведений об обращениях граждан
        /// </summary>
        /// <param name="container"></param>
        public CitizensAppealService(IWindsorContainer container)
        {
            this.Container = container;
        }
    }
}
