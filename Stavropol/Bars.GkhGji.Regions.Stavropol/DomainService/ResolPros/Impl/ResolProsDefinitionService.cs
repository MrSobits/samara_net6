using System.Collections.Generic;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Stavropol.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Stavropol.DomainService.ResolPros.Impl
{
	public class ResolProsDefinitionService: IResolProsDefinitionService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult ListTypeDefinition(BaseParams baseParams)
        {
			var list = new List<TypeResolProsDefinitionProxy>();
            
            foreach (var type in DefinitionTypes())
            {
				list.Add(new TypeResolProsDefinitionProxy
                {
                    Id = (int)type,
                    Display = type.GetEnumMeta().Display,
                    Name = type.ToString()
                });
            }

            var total = list.Count;

            return new ListDataResult(list, total);
        }

        public virtual TypeDefinitionResolPros[] DefinitionTypes()
        {
            return new []
                {
                    TypeDefinitionResolPros.TimeAndPlaceHearing,
                    TypeDefinitionResolPros.PostponeCase,
                };
        }

        protected class TypeResolProsDefinitionProxy
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Display { get; set; }
        }
    }
}