using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh1468.DataFiller.Fillers
{
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    class UniqueIdentifierDataFiller : IDataFiller
    {
        public IWindsorContainer Container { get; set; }

        // Атрибут, значения которого надо проставить
        public MetaAttribute MetaAttribute { get; set; }

        // Дом, на основе которого берутся данные
        public RealityObject RealityObject { get; set; }

        public List<BaseProviderPassportRow> Result { get; set; }

        string IDataFiller.Code
        {
            get
            {
                return Code;
            }
        }

        public static string Code
        {
            get
            {
                return "UniqueIdentifierDataFiller";
            }
        }

        public string Name
        {
            get
            {
                return "Уникальный номер дома";
            }
        }

        public bool Multiple
        {
            get
            {
                return false;
            }
        }

        public ValueType ValueType
        {
            get
            {
                return ValueType.Int;
            }
        }

        public void To1468()
        {
            if (RealityObject == null || MetaAttribute == null || Result == null)
            {
                throw new Exception("Не переданы параметры для расчета значения!");
            }

            Result.Add(new BaseProviderPassportRow
            {
                MetaAttribute = MetaAttribute,
                Value = RealityObject.Id.ToStr() ?? string.Empty
            });
        }

        public void From1468()
        {

        }
    }
}
