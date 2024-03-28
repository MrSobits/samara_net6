namespace Bars.GkhGji.Regions.Stavropol.NumberRule
{
    using System;

    using Bars.GkhGji.Regions.Stavropol.NumberRule.Impl;

    public interface IDefinitionNumberRule
    {
        /// <summary>
        /// Устанавливает значение номера определения исходя из максимального в текущем году + 1
        /// </summary>
        int SetNumber(DateTime definitionDate);

        /// <summary>
        /// Проверяет номер определения на уникальность
        /// </summary>
        DefinitionNumberRule.Definition CheckNumber(DateTime definitionDate, int number, long entityId, Type entityType);
    }
}