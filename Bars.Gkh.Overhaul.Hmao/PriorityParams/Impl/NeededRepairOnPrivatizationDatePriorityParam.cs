namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Параметр очередности Требовалось проведение КР на дату приватизации первого жилого помещения
    /// </summary>
    public class NeededRepairOnPrivatizationDatePriorityParam : IPriorityParams, IQualitPriorityParam
    {
        /// <summary>
        /// Словарь, который содержит конструктивный элемент дома с максимальным годом капитального ремонта
        /// <para>В качестве ключа идентификатор 3го этапа</para>
        /// </summary>
        public Dictionary<long, RealityObjectStructuralElement> StructuralElementWithLastYearDict { get; set; }

        /// <inheritdoc />
        public string Id => "NeededRepairOnPrivatizationDate";

        /// <inheritdoc />
        public string Name => "Требовалось проведение КР на дату приватизации первого жилого помещения";

        /// <inheritdoc />
        public TypeParam TypeParam => TypeParam.Qualit;

        /// <inheritdoc />
        public object GetValue(IStage3Entity obj)
        {
            if (obj.RealityObject.PrivatizationDateFirstApartment.HasValue && this.StructuralElementWithLastYearDict.ContainsKey(obj.Id))
            {
                var privatDate = obj.RealityObject.PrivatizationDateFirstApartment.Value.Year;
                var lastRepairYear = this.StructuralElementWithLastYearDict[obj.Id].LastOverhaulYear;

                return privatDate >= lastRepairYear ? YesNo.Yes : YesNo.No;
            }

            return YesNo.No;
        }

        /// <inheritdoc />
        public Type EnumType => typeof(YesNo);
    }
}