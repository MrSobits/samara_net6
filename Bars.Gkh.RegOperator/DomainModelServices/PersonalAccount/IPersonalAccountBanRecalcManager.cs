namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерфейс создания запрета перерасчета
    /// </summary>
    public interface IPersonalAccountBanRecalcManager
    {
        /// <summary>
        /// Сохранение созданных изменений
        /// </summary>
        void SaveBanRecalcs();

        /// <summary>
        /// Создание запрета перерасчета
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="dateStart">Дата начала</param>
        /// <param name="dateEnd">Дата окончания</param>
        /// <param name="type">Тип запрета перерасчета</param>
        /// <param name="fileInfo">Файл</param>
        /// <param name="reason">Причина</param>
        void CreateBanRecalc(BasePersonalAccount account, DateTime dateStart, DateTime dateEnd, BanRecalcType type, FileInfo fileInfo, string reason);
    }
}