namespace Bars.KP60.Protocol.DomainService
{
    using System;
    using System.Collections.Generic;
    using B4;
    using Entities;

    /// <summary>
    /// Интерфейс сервис построения дерева протокола
    /// </summary>
    public interface IBuildTree
    {
        /// <summary>
        /// "Вырастить" дерево
        /// </summary>
        void BuildTree();

        /// <summary>
        /// Получить построенное дерево
        /// </summary>
        /// <param name="accountData"></param>
        /// <returns></returns>
        TreeData GetTree(BaseParams baseParams);
    }
}