namespace Bars.Gkh.RegOperator.DomainService
{
	using System;
	using System.Linq;
	using B4;
	using B4.DataAccess;
	using B4.Modules.FileStorage;
	using B4.Modules.Security;
	using B4.Utils;
	using Entities.PersonalAccount;
	using Gkh.Domain;
	using Gkh.Entities;
	using Gkh.Utils;

    /// <summary>
	/// Сервис для Информация по начисленным льготам
	/// </summary>
	public class PersonalAccountBenefitsService : IPersonalAccountBenefitsService
	{
		private readonly IFileManager fileManager;
		private readonly IRepository<User> userRepo;
		private readonly IUserIdentity userIdentity;
		private readonly IDomainService<EntityLogLight> logDomain;
		private readonly IDomainService<PersonalAccountBenefits> personalAccountBenefitsDomain;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="fileManager">Интерфейс представлющий методы для работы с файлами</param>
		/// <param name="userRepo">Репозиторий для Пользователь</param>
		/// <param name="userIdentity">Интерфейс идентификатора пользователя</param>
		/// <param name="logDomain">Домен сервис для Легковесная сущность для хранения изменения сущности</param>
		/// <param name="personalAccountBenefitsDomain">Домен сервис для Информация по начисленным льготам</param>
		public PersonalAccountBenefitsService(
			IFileManager fileManager,
			IRepository<User> userRepo,
			IUserIdentity userIdentity,
			IDomainService<EntityLogLight> logDomain,
            IDomainService<PersonalAccountBenefits> personalAccountBenefitsDomain)
		{
			this.fileManager = fileManager;
			this.userRepo = userRepo;
			this.userIdentity = userIdentity;
			this.logDomain = logDomain;
			this.personalAccountBenefitsDomain = personalAccountBenefitsDomain;
		}

		/// <summary>
		/// Сменить значение Сумма
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult UpdateSum(BaseParams baseParams)
	    {
			var value = baseParams.Params.GetAs<decimal>("value");
			var reason = baseParams.Params.GetAs<string>("reason");
			var entityId = baseParams.Params.GetAsId("entityId");

			try
			{
				var accountBenefits = this.personalAccountBenefitsDomain.Get(entityId);
				var oldValue = accountBenefits.Sum;
				accountBenefits.Sum = value;

				this.personalAccountBenefitsDomain.Update(accountBenefits);

				var file = baseParams.Files.FirstOrDefault();
				FileInfo fileInfo = null;
				if (file.Value != null)
				{
					fileInfo = this.fileManager.SaveFile(file.Value);
				}

				var login = this.userRepo.Get(this.userIdentity.UserId).Return(u => u.Login);
				this.logDomain.Save(new EntityLogLight
				{
					ClassName = "PersonalAccountBenefits",
					EntityId = entityId,
					PropertyName = "Sum",
					PropertyValue = value.ToStr(),
					PropertyDescription = oldValue.RegopRoundDecimal(2).ToStr(),
                    DateActualChange = DateTime.Now,
					DateApplied = DateTime.UtcNow,
					Document = fileInfo,
					ParameterName = "pa_benefits_sum",
					Reason = reason,
                    User = login.IsEmpty() ? "anonymous" : login
				});
			}
			catch (Exception e)
			{
				return new BaseDataResult(false, e.Message);
			}

			return new BaseDataResult(true, "Смена значения суммы начисленной льготы прошла успешно");
		}
    }
}