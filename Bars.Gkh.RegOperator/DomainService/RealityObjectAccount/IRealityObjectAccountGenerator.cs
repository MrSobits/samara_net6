namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using System.Linq;
    using B4.DataAccess;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс для создания различных счетов дома
    /// </summary>
    public interface IRealityObjectAccountGenerator
    {
        /// <summary>
        /// Создвть счета начислений дома
        /// </summary>
        /// <param name="collection">Список объектов недвижимости</param>
        void GenerateChargeAccounts(IQueryable<RealityObject> collection);

        /// <summary>
        /// Создать счета оплат дома
        /// </summary>
        /// <param name="collection">Список объектов недвижимости</param>
        void GeneratePaymentAccounts(IQueryable<RealityObject> collection);

        /// <summary>
        /// Создать счета расчета с поставщиками дома
        /// </summary>
        /// <param name="collection">Список объектов недвижимости</param>
        void GenerateSupplierAccounts(IQueryable<RealityObject> collection);

        /// <summary>
        /// Создать счета субсидий дома
        /// </summary>
        /// <param name="collection">Список объектов недвижимости</param>
        void GenerateSubsidyAccounts(IQueryable<RealityObject> collection);

        /// <summary>
        /// Создать счет начислений дома
        /// </summary>
        /// <param name="realityObject">Объект недвижимости</param>
        void GenerateChargeAccount(RealityObject realityObject);

        /// <summary>
        /// Создать счет оплат дома
        /// </summary>
        /// <param name="realityObject">Объект недвижимости</param>
        void GeneratePaymentAccount(RealityObject realityObject);

        /// <summary>
        /// Создать счет расчета с поставщиками дома
        /// </summary>
        /// <param name="realityObject">Объект недвижимости</param>
        void GenerateSupplierAccount(RealityObject realityObject);

        /// <summary>
        /// Создать счет субсидий дома
        /// </summary>
        /// <param name="realityObject">Объект недвижимости</param>
        void GenerateSubsidyAccount(RealityObject realityObject);

        /// <summary>
        /// Создать номер счета
        /// </summary>
        /// <typeparam name="T">Тип счета, реализующий <see cref="IRealityObjectAccount"/></typeparam>
        /// <param name="ro">Объект недвижимости</param>
        /// <param name="onlyForThisRo">Вычислять номер счета только для переданного объекта</param>
        /// <returns>Номер счета</returns>
        string GenerateAccountNumber<T>(RealityObject ro, bool onlyForThisRo = false) where T : PersistentObject, IRealityObjectAccount;
    }
}