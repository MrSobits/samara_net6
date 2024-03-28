namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;

    using B4;
    using B4.Utils;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class ResolutionDefinitionService: IResolutionDefinitionService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult ListTypeDefinition(BaseParams baseParams)
        {
            /*
             Поскольку в базовый енум добавляется куча типов котоыре не вовсех регионах нужны
             то тогда в этом серверном методе возвращаем типы котоыре нужны только для этого региона
            */

            var list = new List<TypeResolutionDefinitionProxy>();
            
            foreach (var type in DefinitionTypes())
            {
                list.Add(new TypeResolutionDefinitionProxy
                {
                    Id = (int)type,
                    Display = type.GetEnumMeta().Display,
                    Name = type.ToString()
                });
            }

            var total = list.Count;

            return new ListDataResult(list, total);
        }

        public virtual TypeDefinitionResolution[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new []
                {
                    TypeDefinitionResolution.Decline,
                    TypeDefinitionResolution.Deferment,
                    TypeDefinitionResolution.CorrectionError,
                    TypeDefinitionResolution.Return
                };
        }

        protected class TypeResolutionDefinitionProxy
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Display { get; set; }
        }
    }
}