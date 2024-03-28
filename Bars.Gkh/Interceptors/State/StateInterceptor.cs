namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;

    public class StateServiceInterceptor : EmptyDomainInterceptor<State>
    {


        public override IDataResult AfterCreateAction(IDomainService<State> service, State entity)
        {
            // Перед сохранением проставляем начальный статус всем объектам данной сущности у кого статус null
            var statefulEntitiesManifestServiceArray = Container.ResolveAll<IStatefulEntitiesManifest>();
            foreach (var statefulEntitiesManifestService in statefulEntitiesManifestServiceArray)
            {
                var statefulEntitiesManifest =
                    statefulEntitiesManifestService.GetAllInfo()
                                                   .FirstOrDefault(x => x.TypeId == entity.TypeId);

                if (entity.StartState && statefulEntitiesManifest != null)
                {
                    var repositoryType = typeof(IRepository<>).MakeGenericType(statefulEntitiesManifest.Type);
                    var repository = this.Container.Resolve(repositoryType);
                    var entityObjectArray = repositoryType.GetMethod("GetAll").Invoke(repository, null)
                                        .To<IQueryable<IStatefulEntity>>()
                                        .Where(x => x.State == null)
                                        .ToArray();

                    var updateMetod = repositoryType.GetMethod("Update");

                    foreach (var entityObject in entityObjectArray)
                    {
                        entityObject.State = new State { Id = entity.Id };
                        updateMetod.Invoke(repository, new object[] { entityObject });
                    }
                }
            }

            return new BaseDataResult();
        }

        public override IDataResult AfterUpdateAction(IDomainService<State> service, State entity)
        {
            // Перед обновлением проставляем начальный статус всем объектам данной сущности у кого статус null
            var statefulEntitiesManifestServiceArray = this.Container.ResolveAll<IStatefulEntitiesManifest>();
            foreach (var statefulEntitiesManifestService in statefulEntitiesManifestServiceArray)
            {
                var statefulEntitiesManifest = statefulEntitiesManifestService
                    .GetAllInfo()
                    .FirstOrDefault(x => x.TypeId == entity.TypeId);

                if (entity.StartState && statefulEntitiesManifest != null)
                {
                    var repositoryType = typeof(IRepository<>).MakeGenericType(statefulEntitiesManifest.Type);
                    var repository = Container.Resolve(repositoryType);
                    var entityObjectArray = repositoryType.GetMethod("GetAll").Invoke(repository, null)
                                      .To<IQueryable<IStatefulEntity>>()
                                      .Where(x => x.State == null)
                                      .ToArray();

                    var updateMetod = repositoryType.GetMethod("Update");

                    foreach (var entityObject in entityObjectArray)
                    {
                        entityObject.State = new State { Id = entity.Id };
                        updateMetod.Invoke(repository, new object[] { entityObject });
                    }
                }
            }

            return new BaseDataResult();
        }

		public override IDataResult BeforeDeleteAction(IDomainService<State> service, State entity)
	    {
		    var statefulEntitiesManifestServiceArray = this.Container.ResolveAll<IStatefulEntitiesManifest>();
		    foreach (var statefulEntitiesManifestService in statefulEntitiesManifestServiceArray)
		    {
			    var statefulEntitiesManifest = statefulEntitiesManifestService
				    .GetAllInfo()
				    .FirstOrDefault(x => x.TypeId == entity.TypeId);

			    if (entity.StartState && statefulEntitiesManifest != null)
			    {
				    var repositoryType = typeof (IRepository<>).MakeGenericType(statefulEntitiesManifest.Type);
				    var repository = Container.Resolve(repositoryType);
				    var entityExists = repositoryType.GetMethod("GetAll").Invoke(repository, null)
					    .To<IQueryable<IStatefulEntity>>()
					    .Any(x => x.State != null && x.State.Id == entity.Id);
				    if (entityExists)
				    {
					    return new BaseDataResult(false,
						    "Нельзя удалить статус, так как он используется в \"{0}\"".FormatUsing(statefulEntitiesManifest.Name));
				    }
			    }
		    }

		    return new BaseDataResult();
	    }

        public override IDataResult BeforeCreateAction(IDomainService<State> service, State entity)
        {
            if (entity.TypeId == "gkh_regop_personal_account")
            {
                if (entity.Code.ToInt() < 1 || entity.Code.ToInt() > 4)
                {
                    return BaseDataResult.Error("Статус с таким кодом не может быть добавлен");
                }

                var result = service.GetAll().Any(x => entity.Code != "2" && x.Code == entity.Code && x.TypeId == "gkh_regop_personal_account");
                if (result)
                {
                    return BaseDataResult.Error("Статус с таким кодом уже добавлен");
                }

                if (entity.FinalState)
                {
                    result = service.GetAll().Any(x => x.FinalState && x.TypeId == "gkh_regop_personal_account");
                    if (result)
                    {
                        return BaseDataResult.Error("Признак уже указан для другого статуса");
                    }
                }
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<State> service, State entity)
        {
            if (entity.TypeId == "gkh_regop_personal_account")
            {
                if (entity.Code.ToInt() < 1 || entity.Code.ToInt() > 4)
                {
                    return BaseDataResult.Error("Статус с таким кодом не может быть добавлен");
                }

                if (entity.FinalState)
                {
                    var result = service.GetAll().Any(x => x.FinalState && x.TypeId == "gkh_regop_personal_account" && x.Id != entity.Id);
                    if (result)
                    {
                        return BaseDataResult.Error("Признак уже указан для другого статуса");
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}
