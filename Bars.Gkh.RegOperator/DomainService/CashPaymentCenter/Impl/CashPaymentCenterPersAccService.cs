namespace Bars.Gkh.RegOperator.DomainService.CashPaymentCenter
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;
    using Castle.Windsor;
    using Bars.B4.Config;
    using Npgsql;

    /// <summary>
    /// Сервис привязки лицевого счёта к РКЦ.
    /// <remarks>Реализация интерфейса <see cref="ICashPaymentCenterAddObjectsService"/></remarks>
    /// </summary>
    public class CashPaymentCenterPersAccService : ICashPaymentCenterObjectsService
    {
        private readonly IDomainService<CashPaymentCenterPersAcc> cashPaymentCenterPersAccDomain;
        private readonly IDomainService<CashPaymentCenter> cashPaymentCenterDomain;
        private readonly IDomainService<BasePersonalAccount> basePersonalAccountDomain;
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="cashPaymentCenterPersAccDomain">Домен-сервис "Лицевой счёт РКЦ"</param>
        /// <param name="cashPaymentCenterDomain">Домен-сервис "РКЦ"</param>
        /// <param name="basePersonalAccountDomain">Домен-сервис "Лицевой счёт"</param>
        public CashPaymentCenterPersAccService(IWindsorContainer container,
            IDomainService<CashPaymentCenterPersAcc> cashPaymentCenterPersAccDomain,
            IDomainService<CashPaymentCenter> cashPaymentCenterDomain,
            IDomainService<BasePersonalAccount> basePersonalAccountDomain)
        {
            this.container = container;
            this.cashPaymentCenterPersAccDomain = cashPaymentCenterPersAccDomain;
            this.cashPaymentCenterDomain = cashPaymentCenterDomain;
            this.basePersonalAccountDomain = basePersonalAccountDomain;
        }

        /// <summary>
        /// Привязать лицевой счёт к РКЦ
        /// </summary>
        /// <param name="cachPaymentCenterId">Идентификатор РКЦ</param>
        /// <param name="ids">Идентификаторы объектов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        /// <exception cref="NullReferenceException">Выбрасывается, если РКЦ не найден.</exception>
        /// <returns>Результат работы</returns>
        public IDataResult AddObjects(long cachPaymentCenterId, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            var existPersAccInPeriod =
                    this.cashPaymentCenterPersAccDomain.GetAll()
                        .Where(x => (x.DateStart <= dateStart && (!x.DateEnd.HasValue || x.DateEnd >= dateStart))
                            || (!dateEnd.HasValue && x.DateStart >= dateStart)
                            || (dateEnd.HasValue && x.DateStart <= dateEnd && (!x.DateEnd.HasValue || x.DateEnd >= dateEnd)))
                        .Select(x => x.PersonalAccount.Id)
                        .Distinct()
                        .ToList();

            var cashPaymentCenter = this.cashPaymentCenterDomain.Get(cachPaymentCenterId);
            if (cashPaymentCenter == null)
            {
                throw new NullReferenceException("Не найден расчётно-кассовый центр по указанному идентификатору");
            }

            //var hasNoAddedPersAcc = false;
            var listToSave = new List<CashPaymentCenterPersAcc>();
            var listIds = new List<long>();
            foreach (var id in ids)
            {
                if (!existPersAccInPeriod.Contains(id))
                {
                    var newDelAgentPersAcc = new CashPaymentCenterPersAcc
                    {
                        PersonalAccount = this.basePersonalAccountDomain.Load(id),
                        CashPaymentCenter = cashPaymentCenter,
                        DateStart = dateStart,
                        DateEnd = dateEnd
                    };

                    listToSave.Add(newDelAgentPersAcc);
                }
                else
                {
                    listIds.Add(id);
                }
            }

            if (listIds.Count > 0)
            {
                cashPaymentCenterPersAccDomain.GetAll()
                    .Where(x => listIds.Contains(x.PersonalAccount.Id))
                    .AsEnumerable()
                    .ForEach(x =>
                        {
                            cashPaymentCenterPersAccDomain.Delete(x.Id);
                        });

                listIds.ForEach(x =>
                {
                    var newDelAgentPersAcc = new CashPaymentCenterPersAcc
                    {
                        PersonalAccount = this.basePersonalAccountDomain.Load(x),
                        CashPaymentCenter = cashPaymentCenter,
                        DateStart = dateStart,
                        DateEnd = dateEnd
                    };

                    listToSave.Add(newDelAgentPersAcc);
                });
            }

            TransactionHelper.InsertInManyTransactions(this.container, listToSave, 10000, true, true);

            return listIds.Count > 0 ? new BaseDataResult(false, "Лицевые счета сохранены успешно, для некоторых счетов заменен договор в выбранном периоде.")
            : new BaseDataResult(true, "Лицевые счета сохранены успешно");
        }

        /// <summary>
        /// Открепить лицевой счёт от РКЦ
        /// </summary>        
        /// <param name="ids">Идентификаторы объектов</param>
        /// <returns>Результат работы</returns>
        public IDataResult DeleteObjects(long[] ids)
        {
            var entities = cashPaymentCenterPersAccDomain.GetAll().Where(x => ids.Contains(x.Id)).ToArray();
            foreach (var entity in entities)
            {
                cashPaymentCenterPersAccDomain.Delete(entity.Id);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Установить расчётно-кассовый центр, из которого вызвана функция,
        /// всем Л/С без расчётно-кассового центра
        /// </summary>
        /// <param name="cashPaymentCenterId">Идентификатор РКЦ</param>
        /// <returns>Результат работы</returns>
        public IDataResult SetCashPaymentCenters(long cashPaymentCenterId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(container.Resolve<IConfigProvider>().GetConfig().ConnString);
            connection.Open();

            var firstOfJanuary = new DateTime(DateTime.Now.Year, 1, 1);
            string sqlString = "INSERT INTO regop_cashpay_pers_acc(object_version, object_create_date, object_edit_date, cashpaym_center_id, pers_acc_id, date_start, date_end, import_entity_id)"
                + $@"(select 1, current_date, current_date, {cashPaymentCenterId}, id, '{firstOfJanuary.ToString("dd.MM.yyyy")}'::date, null, null from regop_pers_acc where id in"
                + "(select rpa.id from regop_pers_acc rpa left join REGOP_CASHPAY_PERS_ACC rcpa on rcpa.pers_acc_id = rpa.id where rcpa.id is null));";
            NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);

            command.ExecuteNonQuery();
            connection.Close();

            return new BaseDataResult();
        }

        /// <summary>
        /// Врезать привязку лицевого счёта к РКЦ
        /// </summary>
        /// <remarks>
        /// В период действия текущего договора «врезается» период действия нового договора. Например:
        /// Действующий договор: c 2010 по 2020 год. Врезается договор другого РКЦ с периодом: c 2015 по 2016 год. 
        /// В результате будет 3 записи:
        /// 1) Старый РКЦ с 2010 по 2014.
        /// 2) Новый РКЦ с 2015 по 2016.
        /// 3) Старый РКЦ с 2017 по 2020.
        /// </remarks>
        /// <param name="cachPaymentCenterId">Идентификатор РКЦ</param>
        /// <param name="ids">Идентификаторы лицевых счетов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        /// <returns>Результат работы</returns>
        public IDataResult InsertObjects(long cachPaymentCenterId, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            var cashPaymentCenter = this.cashPaymentCenterDomain.Get(cachPaymentCenterId);
            if (cashPaymentCenter == null)
            {
                throw new NullReferenceException("Не найден расчётно-кассовый центр по указанному идентификатору");
            }

            this.container.InTransaction(() => 
            { 
                // 1. Те договоры, которые «накрываются» - удалить
                this.DeleteCovering(cashPaymentCenter, ids, dateStart, dateEnd);
                // 2. Те договры, в которые идёт врезка «в середину» - разбить на 2
                this.SplitIncut(cashPaymentCenter, ids, dateStart, dateEnd);
                // 3. Те договоры, на которые «наезжает» дата начала - укоротить
                this.TruncateEnd(cashPaymentCenter, ids, dateStart, dateEnd);
                // 4. Те договоры, на которые «наезжает» дата конца - переоткрыть
                this.MoveStart(cashPaymentCenter, ids, dateStart, dateEnd);

                // Добавить «свои» записи
                foreach (var id in ids)
                {
                    this.cashPaymentCenterPersAccDomain.Save(
                        new CashPaymentCenterPersAcc
                        {
                            PersonalAccount = this.basePersonalAccountDomain.Load(id),
                            CashPaymentCenter = cashPaymentCenter,
                            DateStart = dateStart,
                            DateEnd = dateEnd
                        });
                }
            });

            return new BaseDataResult();
        }

        /// <summary>
        /// Те договоры, которые «накрываются» - удалить
        /// </summary>
        /// <remarks>
        ///           |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~|
        /// 1) ---------------|=====================|------------>                
        ///
        ///           |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// 2) ---------------|=====================|------------>
        ///
        ///           |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// 3) ---------------|==================================>
        ///
        ///           |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~|
        /// 4) -------|=================================|-------->                
        ///
        ///           |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// 5) -------|==========================================>
        /// </remarks>
        /// <param name="cashPaymentCenter">РКЦ</param>
        /// <param name="ids">Идентификаторы лицевых счетов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void DeleteCovering(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            this.cashPaymentCenterPersAccDomain.GetAll()
                .Where(x => ids.Contains(x.PersonalAccount.Id))
                .Where(x => x.DateStart >= dateStart)
                .WhereIf(dateEnd.HasValue,
                    x => x.DateEnd.HasValue && x.DateEnd <= dateEnd) // п.1, п.4
                                                                     // Иначе !dateEnd.HasValue п.2, п.3, п.5 - подходит и x.DateEnd значение и null
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this.cashPaymentCenterPersAccDomain.Delete(x));
        }

        /// <summary>
        /// Те договры, в которые идёт врезка «в середину» - разбить на 2
        /// </summary>
        /// <remarks>
        ///                    |~~~~~~~~~~|
        /// 1) ----------|=====================|----------------->
        ///
        ///                    |~~~~~~~~~~|
        /// 2) ----------|=======================================>
        /// </remarks>
        /// <param name="cashPaymentCenter">РКЦ</param>
        /// <param name="ids">Идентификаторы лицевых счетов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void SplitIncut(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            // Врезка возможна только если задана дата конца
            if (!dateEnd.HasValue)
            {
                return;
            }
            
            this.cashPaymentCenterPersAccDomain.GetAll()
                .Where(x => ids.Contains(x.PersonalAccount.Id))
                .Where(x => x.DateStart < dateStart)
                .Where(x => !x.DateEnd.HasValue     // п.2
                        || x.DateEnd > dateEnd)     // п.1
                .Select(x => x.Id)
                .ToList()
                .ForEach(x =>
                {
                    // Добавить копию, с нужной датой начала.
                    // А существующую - обновить, подрезать дату конца.                                               
                    var r1 = this.cashPaymentCenterPersAccDomain.Load(x);
                    var r2 = new CashPaymentCenterPersAcc
                    {
                        PersonalAccount = r1.PersonalAccount,
                        CashPaymentCenter = r1.CashPaymentCenter,
                        DateStart = dateEnd.Value.AddDays(1),
                        DateEnd = r1.DateEnd,
                        ObjectCreateDate = DateTime.UtcNow,
                        ObjectEditDate = DateTime.UtcNow
                    };
                    r1.DateEnd = dateStart.AddDays(-1);                    
                    r1.ObjectEditDate = DateTime.UtcNow;

                    this.cashPaymentCenterPersAccDomain.Update(r1);
                    this.cashPaymentCenterPersAccDomain.Save(r2);
                });            
        }

        /// <summary>
        /// Те договоры, на которые «наезжает» дата начала - укоротить
        /// </summary>
        /// <remarks>
        ///                                  *|~~~~~~~~~~|
        /// 1) ---------------|=====================|------------>
        ///
        ///                               *|~~~~~~~~~~~~~~~~~~~~~
        /// 2) ---------------|=====================|------------>
        ///
        ///                                        *|~~~~~~|
        /// 3) ---------------|=====================|------------>                
        ///
        ///                                        *|~~~~~~~~~~~
        /// 4) ---------------|=====================|------------>
        ///
        ///                             *|~~~~~~~~~~|
        /// 5) ---------------|=====================|------------>
        ///
        ///                             *|~~~~~~~~~~~~~~~~~~~~~~~
        /// 6) ---------------|==================================>
        /// </remarks>
        /// <param name="cashPaymentCenter">РКЦ</param>
        /// <param name="ids">Идентификаторы лицевых счетов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void TruncateEnd(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            this.cashPaymentCenterPersAccDomain.GetAll()
                .Where(x => ids.Contains(x.PersonalAccount.Id))
                // Поймать DateStart
                .Where(x => x.DateStart < dateStart)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= dateStart)
                // Не попасть во врезку (см. метод SplitIncut)
                .WhereIf(dateEnd.HasValue, // п.1, п.3, п.5
                    x => x.DateEnd.HasValue && x.DateEnd <= dateEnd)
                // Если бесконечность, то условия не надо (п.2, п.4, п.6)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x =>
                {
                    var r = this.cashPaymentCenterPersAccDomain.Load(x);
                    r.DateEnd = dateStart.AddDays(-1);
                    r.ObjectEditDate = DateTime.UtcNow;
                    this.cashPaymentCenterPersAccDomain.Update(r);
                });
        }

        /// <summary>
        /// Те договоры, на которые «наезжает» дата конца - переоткрыть            
        /// </summary>
        /// <remarks>
        ///           |~~~~~~~~~~~~~~~|*
        /// 1) ---------------|=====================|------------>
        ///
        ///            |~~~~~~~~~~~~~~~|*
        /// 2) ---------------|==================================>
        ///
        ///        |~~~~~~~~~~|*
        /// 3) ---------------|=====================|------------>
        ///
        ///        |~~~~~~~~~~|*
        /// 4) ---------------|==================================>
        /// </remarks>
        /// <param name="cashPaymentCenter">РКЦ</param>
        /// <param name="ids">Идентификаторы лицевых счетов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void MoveStart(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            // Наезд возможен только если задана дата конца
            if (!dateEnd.HasValue)
            {
                return;
            }

            this.cashPaymentCenterPersAccDomain.GetAll()
                .Where(x => ids.Contains(x.PersonalAccount.Id))
                // Поймать DateEnd
                .Where(x => x.DateStart <= dateEnd)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > dateEnd)
                // Не попасть во врезку (см. метод SplitIncut)
                .Where(x => x.DateStart > dateStart)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => 
                {
                    var r = this.cashPaymentCenterPersAccDomain.Load(x);
                    r.DateStart = dateEnd.Value.AddDays(1);
                    r.ObjectEditDate = DateTime.UtcNow;
                    this.cashPaymentCenterPersAccDomain.Update(r);
                });            
        }
    }
}
