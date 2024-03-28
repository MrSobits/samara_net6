namespace Bars.KP60.Protocol.DomainService.Impl
{
    using System.Collections.Generic;
    using B4;
    using Castle.Windsor;
    using Bars.KP60.Protocol.Entities;

    /// <summary>
    /// Реализация дерева протокола расчетов
    /// </summary>
    public class ProtocolService : IProtocolService
    {
        public IWindsorContainer Container { get; set; }

        //Дерево протокола расчетов
        public TreeData GetTree(BaseParams baseParams)
        {
            Building building;

            //Способы заполнения протокола
            building = new BuildingOthers(Container); //электроснабжение

            return building.GetTree(baseParams);
        }

    }
}


