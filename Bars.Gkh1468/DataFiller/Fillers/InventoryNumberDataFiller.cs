using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh1468.DataFiller.Fillers
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    class InventoryNumberDataFiller : IDataFiller
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
                return "InventoryNumberDataFiller";
            }
        }

        public string Name
        {
            get
            {
                return "Инвентарный номер";
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
                return ValueType.String;
            }
        }

        public void To1468()
        {
            
            if (RealityObject == null || MetaAttribute == null || Result == null)
            {
                throw new Exception("Не переданы параметры для расчета значения!");
            }

            var inventoryNumber = Container.Resolve<IDomainService<TehPassportValue>>()
                         .GetAll()
                         .Where(x => x.TehPassport.RealityObject.Id == RealityObject.Id)
                         .Where(x => x.FormCode == "Form_1")
                         .Where(x => x.CellCode == "18:1")
                         .Select(x => x.Value)
                         .FirstOrDefault();

            Result.Add(new BaseProviderPassportRow
            {
                MetaAttribute = MetaAttribute,
                Value = inventoryNumber ?? string.Empty
            });
        }

        public void From1468()
        {
           
        }
    }
}
