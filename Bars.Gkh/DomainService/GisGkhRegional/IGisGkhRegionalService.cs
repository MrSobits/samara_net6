namespace Bars.Gkh.DomainService.GisGkhRegional
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для получения региональных данных при работе с ГИС ЖКХ
    /// </summary>
    public interface IGisGkhRegionalService
    {
        /// <summary>
        /// Получение рубрики обращения ФКР
        /// </summary>
        /// <param name="code">Код темы обращения (справочник 220)</param>
        int GetCitizenSuggestionRubric(string code);

        /// <summary>
        /// Проверка, что пользователь из ФКР
        /// </summary>
        bool UserIsCr();

        /// <summary>
        /// Проверка, что пользователь из ГЖИ
        /// </summary>
        bool UserIsGji();
    }
}