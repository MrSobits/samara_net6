namespace Bars.KP60.Protocol.DomainService
{
    using System.Collections.Generic;
    using B4;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// Интерфейс дерева протокола расчетов
    /// </summary>
    public interface IProtocolService
    {
        /// <summary>
        /// Дерево протокола расчетов
        /// </summary>
        TreeData GetTree(BaseParams baseParams);
    }
}