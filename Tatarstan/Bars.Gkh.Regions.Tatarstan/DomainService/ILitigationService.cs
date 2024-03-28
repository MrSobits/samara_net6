namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Dto.PaymentsGku;

    /// <summary>
    /// Интерфейс сервиса судебных распоряжений по ЖКУ
    /// </summary>
    public interface ILitigationService
    {
        /// <summary>
        /// Получить регистрационные номера ИП по адресу ПГМУ
        /// </summary>
        /// <param name="addressId">Идентификатор адреса ПГМУ</param>
        string GetIndEntrRegistrationNumbers(long addressId);

        /// <summary>
        /// Получить информацию об оплатах ЖКУ
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IEnumerable<PaymentGkuListDto> PaymentList(BaseParams baseParams);
    }
}