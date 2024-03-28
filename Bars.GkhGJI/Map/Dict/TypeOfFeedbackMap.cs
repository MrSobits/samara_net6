using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Map.Dict
{
    class TypeOfFeedbackMap : BaseEntityMap<TypeOfFeedback>
    {
        public TypeOfFeedbackMap() :
               base("Виды обатной связи", "GJI_DICT_TYPE_OF_FEEDBACK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE");
            this.Property(x => x.Name, "Name").Column("NAME");
        }
    }
}
