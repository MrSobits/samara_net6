namespace Bars.Gkh.RegOperator.Entities.Wallet
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Enums;

    using Domain;

    using DomainModelServices;

    using Refactor;

    using ValueObjects;

    /// <summary>Интерфейс кошелька</summary>
    public interface IWallet : ITransferParty
    {
        /// <summary>Уникальный идентификатор кошелька</summary>
        string WalletGuid { get; }

        /// <summary>Закинуть деньги на кошелек</summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Операция на кошельке</returns>
        Transfer StoreMoney(TransferBuilder tranferBuilder);

        /// <summary>Снять деньги</summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Операция на кошельке</returns>
        Transfer TakeMoney(TransferBuilder tranferBuilder);

        /// <summary>
        /// Метод создает трансфер переноса средств на другой кошелёк (не влияет на баланс)
        /// </summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Созданные трансферы</returns>
        Transfer MoveToAnotherWallet(TransferBuilder tranferBuilder);

        /// <summary>
        /// Метод создает трансфер переноса средств на на кошелек другого ЛС
        /// </summary>
        /// <param name="source">Построитель источника средств</param>
        /// <param name="target">Построитель цели средств</param>
        /// <param name="targetWallet">Целевой кошелек</param>
        /// <returns>Созданные трансферы</returns>
        IList<Transfer> MoveToAnotherAccount(TransferBuilder source, TransferBuilder target, IWallet targetWallet);

        /// <summary>
        /// Блокирование денег на кошельке
        /// </summary>
        /// <param name="operation">Операция</param>
        /// <param name="amount">Сумма</param>
        /// <param name="targetGuid">Целевой Guid</param>
        /// <returns></returns>
        MoneyLock LockMoney(MoneyOperation operation, decimal amount, string targetGuid);

        /// <summary>
        /// Операция снятия блокирования денег
        /// </summary>
        /// <param name="moneyLock">Блокировка денег</param>
        /// <param name="operation">Операция</param>
        void UnlockMoney(MoneyLock moneyLock, MoneyOperation operation);

        /// <summary>
        /// Тип владельца
        /// </summary>
        WalletOwnerType OwnerType { get; set; }

        /// <summary>
        /// Тип кошелька
        /// </summary>
        WalletType WalletType { get; set; }
    }
}