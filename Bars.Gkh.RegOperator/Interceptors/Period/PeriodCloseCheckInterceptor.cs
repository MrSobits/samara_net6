namespace Bars.Gkh.RegOperator.Interceptors.Period
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.Entities.Period;
    public class PeriodCloseCheckInterceptor : EmptyDomainInterceptor<PeriodCloseCheck>
    {
        public IDomainService<PeriodCloseCheckHistory> HistoryDomain { get; set; }

        public IDirtyCheck DirtyCheck { get; set; }

        public IGkhUserManager UserManager { get; set; }

        /// <summary>Метод вызывается после создания объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterCreateAction(IDomainService<PeriodCloseCheck> service, PeriodCloseCheck entity)
        {
            this.HistoryDomain.Save(
                new PeriodCloseCheckHistory
                {
                    ChangeDate = DateTime.Now,
                    Check = entity,
                    IsCritical = entity.IsCritical,
                    User = this.UserManager.GetActiveUser()
                });

            return base.AfterCreateAction(service, entity);
        }

        /// <summary>Метод вызывается перед обновлением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PeriodCloseCheck> service, PeriodCloseCheck entity)
        {
            if (this.DirtyCheck.IsDirtyProperty(entity, x => x.IsCritical))
            {
                this.HistoryDomain.Save(
                new PeriodCloseCheckHistory
                {
                    ChangeDate = DateTime.Now,
                    Check = entity,
                    IsCritical = entity.IsCritical,
                    User = this.UserManager.GetActiveUser()
                });
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>Метод вызывается перед удалением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<PeriodCloseCheck> service, PeriodCloseCheck entity)
        {
            this.HistoryDomain.GetAll().Where(x => x.Check.Id == entity.Id).Select(x => x.Id).ForEach(x => this.HistoryDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}