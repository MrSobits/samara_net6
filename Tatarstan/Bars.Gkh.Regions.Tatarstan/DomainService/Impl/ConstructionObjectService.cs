namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
	using System;
	using System.Linq;

	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.Modules.Security;
	using Bars.B4.Modules.States;
	using Bars.Gkh.Regions.Tatarstan.DomainService;
	using Bars.Gkh.Regions.Tatarstan.Entities;

	using Castle.Windsor;

	/// <summary>
	/// Интерфейс сервис для <see cref="ConstructionObject"/>
	/// </summary>
	public class ConstructionObjectService : IConstructionObjectService
	{
		private const string StateTypeId = "gkh_construct_obj";

		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Провайдер статусов
		/// </summary>
		public IStateProvider StateProvider { get; set; }

		/// <summary>
		/// Домен-сервис для <see cref="State"/>
		/// </summary>
		public IDomainService<State> StateDomainService { get; set; }

		/// <summary>
		/// Кэш для переходов статусов
		/// </summary>
		public IStateTransfersCache StateTransfersCache { get; set; }

		/// <summary>
		/// Репозиторий для <see cref="UserRole"/>
		/// </summary>
		public IRepository<UserRole> UserRoleRepository { get; set; }

		/// <summary>
		/// Интерфейс идентификатора пользователя
		/// </summary>
		public IUserIdentity UserIdentity { get; set; }

		/// <summary>
		/// Массово сменить статусы объектов строительства
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult MassChangeState(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<long[]>("ids");
            var newStateId = baseParams.Params.GetAs<long>("newStateId");
            var oldStateId = baseParams.Params.GetAs<long>("oldStateId");

            if (ids.Length == 0 || newStateId == 0 || oldStateId == 0)
            {
	            return new BaseDataResult(false, "Не удалось получить объекты строительства или статусы");
            }

	        try
	        {
		        var newState = this.StateDomainService.Load(newStateId);
		        var oldState = this.StateDomainService.Load(oldStateId);

		        var stateTransfers = this.StateTransfersCache.GetTransfers().ToArray();

		        var roles = this.UserRoleRepository.GetAll()
			        .Where(userRole => userRole.User.Id == this.UserIdentity.UserId)
			        .Select(x => x.Role.Id)
			        .ToList();

		        var hasPermission = roles.Any(
			        x => stateTransfers.Where(y => y.CurrentState.Id == oldState.Id)
				        .Where(y => y.NewState.Id == newState.Id)
				        .Select(y => y.Role.Id)
				        .Contains(x));

				if (!hasPermission)
				{
					return new BaseDataResult(false, "Нет прав на изменение статуса");
				}

		        foreach (var id in ids)
		        {
			        this.StateProvider.ChangeState(id, StateTypeId, newState, "Массовая смена статуса", true);
		        }

		        return new BaseDataResult(true);
	        }
	        catch (Exception ex)
	        {
		        return new BaseDataResult(false, string.Format("Произошла ошибка: {0}", ex.Message));
	        }
        }
    }
}