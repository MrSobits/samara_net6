namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
	using Bars.GkhCr.DomainService;

	/// <summary>
	/// Интерцептор для Основание претензионно исковой работы для Договоров Подряда
	/// </summary>
	public class BuilderViolatorInterceptor : EmptyDomainInterceptor<BuilderViolator>
    {
		/// <summary>
		/// Метод вызывается перед созданием объекта
		/// </summary>
		/// <param name="domain">Домен</param><param name="entity">Объект</param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeCreateAction(IDomainService<BuilderViolator> domain, BuilderViolator entity)
        {
            var service = this.Container.Resolve<IBuilderViolatorService>();
            try
            {
	            this.SetStartingDate(entity);
				service.CalculationDays(entity);

                return this.Success();
            }
            finally
            {
	            this.Container.Release(service);
            }
        }

		/// <summary>
		/// Метод вызывается перед обновлением объекта
		/// </summary>
		/// <param name="domain">Домен</param><param name="entity">Объект</param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeUpdateAction(IDomainService<BuilderViolator> domain, BuilderViolator entity)
        {
            var service = this.Container.Resolve<IBuilderViolatorService>();
            try
            {
				this.SetStartingDate(entity);
				service.CalculationDays(entity);

                return this.Success();
            }
            finally
            {
	            this.Container.Release(service);
            }
        }

		/// <summary>
		/// Метод вызывается перед удалением объекта
		/// </summary>
		/// <param name="domain">Домен</param><param name="entity">Объект</param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeDeleteAction(IDomainService<BuilderViolator> domain, BuilderViolator entity)
        {
            var violDomain = this.Container.Resolve<IDomainService<BuilderViolatorViol>>();
            try
            {
                var violList = violDomain.GetAll().Where(x => x.BuilderViolator.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var viol in violList)
                {
                    violDomain.Delete(viol);
                }

                return this.Success();
            }
            finally 
            {
	            this.Container.Release(violDomain);
            }
        }

		// При ручном создании договора дата всегда не заполнена
	    private void SetStartingDate(BuilderViolator entity)
	    {
			if (entity.StartingDate == null && entity.BuildContract.DateEndWork != null)
			{
				entity.StartingDate = entity.BuildContract.DateEndWork.Value.AddDays(1);
			}
		}
    }
}