namespace Bars.Gkh.RegOperator.DomainService.CashPaymentCenter
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Castle.Windsor;
    using Npgsql;
    using Bars.B4.Config;
    using System.ComponentModel;
    using System.Data.SqlTypes;

    /// <summary>
    /// Сервис привязки дома к РКЦ.
    /// <remarks>Реализация интерфейса <see cref="ICashPaymentCenterAddObjectsService"/></remarks>
    /// </summary>
    public class CashPaymentCenterRealityObjService : ICashPaymentCenterObjectsService
    {
        private readonly IDomainService<CashPaymentCenterRealObj> cashPaymentCenterRoDomain;
        private readonly IDomainService<CashPaymentCenter> cashPaymentCenterDomain;
        private readonly IDomainService<RealityObject> realityObjectDomain;
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="cashPaymentCenterRoDomain">Домен-сервис "Дом РКЦ"</param>
        /// <param name="cashPaymentCenterDomain">Домен-сервис "РКЦ"</param>
        /// <param name="realityObjectDomain">Домен-сервис "Дом"</param>
        public CashPaymentCenterRealityObjService(IWindsorContainer container,
            IDomainService<CashPaymentCenterRealObj> cashPaymentCenterRoDomain,
            IDomainService<CashPaymentCenter> cashPaymentCenterDomain,
            IDomainService<RealityObject> realityObjectDomain)
        {
            this.container = container;
            this.cashPaymentCenterRoDomain = cashPaymentCenterRoDomain;
            this.cashPaymentCenterDomain = cashPaymentCenterDomain;
            this.realityObjectDomain = realityObjectDomain;
        }

        /// <summary>
        /// Привязать дом к РКЦ
        /// </summary>
        /// <param name="cachPaymentCenterId">Идентификатор РКЦ</param>
        /// <param name="ids">Идентификаторы объектов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        /// <exception cref="NullReferenceException">Выбрасывается, если РКЦ не найден.</exception>
        /// <returns>Результат работы</returns>
        public IDataResult AddObjects(long cachPaymentCenterId, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            var existsRealityObjects =
                    this.cashPaymentCenterRoDomain.GetAll()
                        .Where(x => (x.DateStart <= dateStart && (!x.DateEnd.HasValue || x.DateEnd >= dateStart))
                            || (!dateEnd.HasValue && x.DateStart >= dateStart)
                            || (dateEnd.HasValue && x.DateStart <= dateEnd && (!x.DateEnd.HasValue || x.DateEnd >= dateEnd)))
                        .Select(x => x.RealityObject.Id)
                        .Distinct()
                        .ToList();

            var cashPaymentCenter = this.cashPaymentCenterDomain.Get(cachPaymentCenterId);
            if (cashPaymentCenter == null)
            {
                throw new NullReferenceException("Не найден расчётно-кассовый центр по указанному идентификатору");
            }

            var hasNoAddedRo = false;
            var listToSave = new List<CashPaymentCenterRealObj>();
            foreach (var id in ids)
            {
                if (!existsRealityObjects.Contains(id))
                {
                    var newDelAgentRealityObject = new CashPaymentCenterRealObj
                    {
                        RealityObject = this.realityObjectDomain.Load(id),
                        CashPaymentCenter = cashPaymentCenter,
                        DateStart = dateStart,
                        DateEnd = dateEnd
                    };

                    listToSave.Add(newDelAgentRealityObject);
                }
                else
                {
                    hasNoAddedRo = true;
                }
            }

            TransactionHelper.InsertInManyTransactions(this.container, listToSave, 10000, true, true);

            return hasNoAddedRo ? new BaseDataResult(false, "Некоторые дома имеют действующий договор. Для добавления нового договора необходимо закрыть прошлый.")
                : new BaseDataResult(true, "Дома сохранены успешно");
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
        /// Открепить дом от РКЦ
        /// </summary>
        /// <param name="ids">Идентификаторы объектов</param>
        /// <returns>Результат работы</returns>
        public IDataResult DeleteObjects(long[] ids)
        {
            var entities = cashPaymentCenterRoDomain.GetAll().Where(x => ids.Contains(x.Id)).ToArray();
            foreach (var entity in entities)
            {
                cashPaymentCenterRoDomain.Delete(entity.Id);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Врезать привязку дома к РКЦ
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
        /// <param name="ids">Идентификаторы домов</param>
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
                    this.cashPaymentCenterRoDomain.Save(
                        new CashPaymentCenterRealObj
                        {
                            RealityObject = this.realityObjectDomain.Load(id),
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
        /// <param name="ids">Идентификаторы домов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void DeleteCovering(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            this.cashPaymentCenterRoDomain.GetAll()
                .Where(x => ids.Contains(x.RealityObject.Id))
                .Where(x => x.DateStart >= dateStart)
                .WhereIf(dateEnd.HasValue,
                    x => x.DateEnd.HasValue && x.DateEnd <= dateEnd) // п.1, п.4
                                                                     // Иначе !dateEnd.HasValue п.2, п.3, п.5 - подходит и x.DateEnd значение и null
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this.cashPaymentCenterRoDomain.Delete(x));
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
        /// <param name="ids">Идентификаторы домов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void SplitIncut(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            // Врезка возможна только если задана дата конца
            if (!dateEnd.HasValue)
            {
                return;
            }

            this.cashPaymentCenterRoDomain.GetAll()
                .Where(x => ids.Contains(x.RealityObject.Id))
                .Where(x => x.DateStart < dateStart)
                .Where(x => !x.DateEnd.HasValue     // п.2
                        || x.DateEnd > dateEnd)     // п.1
                .Select(x => x.Id)
                .ToList()
                .ForEach(x =>
                {
                    // Добавить копию, с нужной датой начала.
                    // А существующую - обновить, подрезать дату конца.                                               
                    var r1 = this.cashPaymentCenterRoDomain.Load(x);
                    var r2 = new CashPaymentCenterRealObj
                    {
                        RealityObject = r1.RealityObject,
                        CashPaymentCenter = r1.CashPaymentCenter,
                        DateStart = dateEnd.Value.AddDays(1),
                        DateEnd = r1.DateEnd,
                        ObjectCreateDate = DateTime.UtcNow,
                        ObjectEditDate = DateTime.UtcNow
                    };
                    r1.DateEnd = dateStart.AddDays(-1);                    
                    r1.ObjectEditDate = DateTime.UtcNow;

                    this.cashPaymentCenterRoDomain.Update(r1);
                    this.cashPaymentCenterRoDomain.Save(r2);
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
        /// <param name="ids">Идентификаторы домов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void TruncateEnd(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            this.cashPaymentCenterRoDomain.GetAll()
                .Where(x => ids.Contains(x.RealityObject.Id))
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
                    var r = this.cashPaymentCenterRoDomain.Load(x);
                    r.DateEnd = dateStart.AddDays(-1);
                    r.ObjectEditDate = DateTime.UtcNow;
                    this.cashPaymentCenterRoDomain.Update(r);
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
        /// <param name="ids">Идентификаторы домов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        private void MoveStart(CashPaymentCenter cashPaymentCenter, long[] ids, DateTime dateStart, DateTime? dateEnd)
        {
            // Наезд возможен только если задана дата конца
            if (!dateEnd.HasValue)
            {
                return;
            }

            this.cashPaymentCenterRoDomain.GetAll()
                .Where(x => ids.Contains(x.RealityObject.Id))
                // Поймать DateEnd
                .Where(x => x.DateStart <= dateEnd)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > dateEnd)
                // Не попасть во врезку (см. метод SplitIncut)
                .Where(x => x.DateStart > dateStart)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => 
                {
                    var r = this.cashPaymentCenterRoDomain.Load(x);
                    r.DateStart = dateEnd.Value.AddDays(1);
                    r.ObjectEditDate = DateTime.UtcNow;
                    this.cashPaymentCenterRoDomain.Update(r);
                });
        }
    }
}
