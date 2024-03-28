namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;

    using B4;
    using B4.Utils;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class ProtocolDefinitionService: IProtocolDefinitionService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult ListTypeDefinition(BaseParams baseParams)
        {
            /*
             Поскольку в базовый енум добавляется куча типов котоыре не вовсех регионах нужны
             то тогда в этом серверном методе возвращаем типы котоыре нужны только для этого региона
            */

            var list = new List<TypeProtocolDefinitionProxy>();
            
            foreach (var type in DefinitionTypes())
            {
                list.Add(new TypeProtocolDefinitionProxy
                {
                    Id = (int)type,
                    Display = type.GetEnumMeta().Display,
                    Name = type.ToString()
                });
            }

            var total = list.Count;

            return new ListDataResult(list, total);
        }

        public virtual TypeDefinitionProtocol[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new []
                {
                    TypeDefinitionProtocol.TimeAndPlaceHearing,
                    TypeDefinitionProtocol.ReturnProtocol, 
                    TypeDefinitionProtocol.PostponeCase, 
                    TypeDefinitionProtocol.About,
                    TypeDefinitionProtocol.TransferCase
                };
        }

        protected class TypeProtocolDefinitionProxy
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Display { get; set; }
        }
    }
}