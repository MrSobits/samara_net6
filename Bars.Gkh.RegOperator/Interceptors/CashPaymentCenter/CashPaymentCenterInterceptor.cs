namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using Entities;

	/// <summary>
	/// Interceptor для <see cref="CashPaymentCenter"/>
	/// </summary>
    public class CashPaymentCenterInterceptor : EmptyDomainInterceptor<CashPaymentCenter>
    {
		/// <summary>
		/// Домен-сервис для <see cref="CashPaymentCenterRealObj"/>
		/// </summary>
		public IDomainService<CashPaymentCenterRealObj> CashPaymentCenterRealObjDomain { get; set; }

		/// <summary>
		/// Домен-сервис для <see cref="RealityObject"/>
		/// </summary>
		public IRepository<RealityObject> RealityObjectRepo { get; set; }

		/// <summary>
		/// Действие перед созданием
		/// </summary>
		/// <param name="service">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="entity">Сущность <see cref="CashPaymentCenter"/></param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeCreateAction(IDomainService<CashPaymentCenter> service, CashPaymentCenter entity)
        {
            long value;
            if (!long.TryParse(entity.Identifier, out value))
            {
                return this.Failure("Значением идентификатора может быть только целое число.");
            }

            return service.GetAll().Any(x => x.Identifier == entity.Identifier && x.Id != entity.Id) 
                ? this.Failure("Указанный идентификатор уже существует. Необходимо указать уникальный идентификатор.") 
                : this.Success();
        }

		/// <summary>
		/// Действие перед обновлением
		/// </summary>
		/// <param name="service">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="entity">Сущность <see cref="CashPaymentCenter"/></param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeUpdateAction(IDomainService<CashPaymentCenter> service, CashPaymentCenter entity)
        {
            long value;
            if (!long.TryParse(entity.Identifier, out value))
            {
                return this.Failure("Значением идентификатора может быть только целое число.");
            }

            return service.GetAll().Any(x => x.Identifier == entity.Identifier && x.Id != entity.Id) 
                ? this.Failure("Указанный идентификатор уже существует. Необходимо указать уникальный идентификатор.") 
                : this.Success();
        }

		/// <summary>
		/// Действие после обновления
		/// </summary>
		/// <param name="service">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="entity">Сущность <see cref="CashPaymentCenter"/></param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult AfterUpdateAction(IDomainService<CashPaymentCenter> service, CashPaymentCenter entity)
        {
            var realObjs = this.CashPaymentCenterRealObjDomain.GetAll()
                    .Where(x => x.CashPaymentCenter.Id == entity.Id)
                    .Select(x => x.RealityObject);

            foreach (var realObj in realObjs)
            {
                realObj.CodeErc = entity.Identifier;
	            this.RealityObjectRepo.Update(realObj);
            }

            return this.Success();
        }

		/// <summary>
		/// Действие перед удалением
		/// </summary>
		/// <param name="service">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="entity">Сущность <see cref="CashPaymentCenter"/></param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult BeforeDeleteAction(IDomainService<CashPaymentCenter> service, CashPaymentCenter entity)
        {
            var cashPaymentCenterMunicServ = this.Container.Resolve<IDomainService<CashPaymentCenterMunicipality>>();
            var cashPaymentCenterRoServ = this.Container.Resolve<IDomainService<CashPaymentCenterRealObj>>();
            var cashPaymentCenterPaServ = this.Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>();

            try
            {
                cashPaymentCenterMunicServ.GetAll().Where(x => x.CashPaymentCenter.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => cashPaymentCenterMunicServ.Delete(x));

                cashPaymentCenterRoServ.GetAll().Where(x => x.CashPaymentCenter.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => cashPaymentCenterRoServ.Delete(x));

                cashPaymentCenterPaServ.GetAll().Where(x => x.CashPaymentCenter.Id == entity.Id)
                   .Select(x => x.Id).ForEach(x => cashPaymentCenterPaServ.Delete(x));

                return this.Success();
            }
            catch (Exception)
            {
                return this.Failure("Не удалось удалить связанные записи");
            }
            finally
            {
	            this.Container.Release(cashPaymentCenterMunicServ);
	            this.Container.Release(cashPaymentCenterRoServ);
	            this.Container.Release(cashPaymentCenterPaServ);
            }
        }
    }
}