namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using Castle.Windsor;

	/// <summary>
	/// Сервис для Исполнитель обращения
	/// </summary>
	public class AppealCitsExecutantService : IAppealCitsExecutantService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Добавить исполнителей
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddExecutants(BaseParams baseParams)
        {
            var appealCitsExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
            try
            {
                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var inspectorIds = baseParams.Params.GetAs<List<long>>("inspectorIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");

                var listToSave = new List<AppealCitsExecutant>();
                foreach (var inspectorId in inspectorIds)
                {
                    var newObj = new AppealCitsExecutant
                    {
                        AppealCits = new AppealCits { Id = appealCitizensId },
                        Executant = new Inspector { Id = inspectorId },
                        OrderDate = DateTime.Now,
                        PerformanceDate = performanceDate,
                        IsResponsible = false
                    };

                    listToSave.Add(newObj);
                }

                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(appealCitsExecutantDomain.Save);

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();

            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(appealCitsExecutantDomain);
            }
        }

		/// <summary>
		/// Перенаправить исполнителя
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult RedirectExecutant(BaseParams baseParams)
        {
            try
            {
                var appealCitsExecutantId = baseParams.Params.GetAs<long>("executantId");
                var newRecordIds = baseParams.Params.GetAs<long[]>("objectIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");

                if (newRecordIds == null)
                {
                    return new BaseDataResult { Success = false, Message = "Необходимо выбрать исполнителей!" };
                }

                var appealCitsExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
                var stateDomain = this.Container.Resolve<IDomainService<State>>();

                using (this.Container.Using(appealCitsExecutantDomain, stateDomain))
                {
                    var appealCitsExecutant = appealCitsExecutantDomain.Get(appealCitsExecutantId);
                    if (appealCitsExecutant.State.FinalState || appealCitsExecutant.State.Name == "Перенаправлено")
                    {
                        return new BaseDataResult { Success = false, Message = "Перенаправление не возможно. Исполнитель имеет конечный статус!" };
                    }

                    var finalState = stateDomain.GetAll()
                        .Where(x => x.Name == "Перенаправлено")
                        .FirstOrDefault(x => x.TypeId == "gji_appcits_executant");

                    if (finalState == null)
                    {
                        return new BaseDataResult { Success = false, Message = "Для исполнителя обращения отстутствует статус Перенаправлено" };
                    }

                    var listToSave = new List<AppealCitsExecutant>();

                    // Прсотавляем новый статус
                    appealCitsExecutant.State = finalState;

                    // проходим по id вбранных исполнителей и сохздаем новые записи копируя поля
                    foreach (var id in newRecordIds)
                    {
                        listToSave.Add(new AppealCitsExecutant
                        {
                            Executant = new Inspector { Id = id },
                            AppealCits = appealCitsExecutant.AppealCits,
                            Author = appealCitsExecutant.Author,
                            OrderDate = appealCitsExecutant.OrderDate,
                            PerformanceDate = performanceDate,
                            Description = appealCitsExecutant.Description
                        });
                    }

                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            appealCitsExecutantDomain.Update(appealCitsExecutant);

                            listToSave.ForEach(appealCitsExecutantDomain.Save);

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }

                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}