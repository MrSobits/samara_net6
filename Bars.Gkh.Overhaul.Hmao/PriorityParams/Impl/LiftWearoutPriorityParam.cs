namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Параметр очередности Износ конструктивного элемента "Лифт" больше или равно 90
    /// </summary>
    public class LiftWearoutPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        /// <summary>
        /// Словарь, который содержит коллекцию значений износа для КЭ Лифт
        /// <para>В качестве ключа идентификатор 3го этапа</para>
        /// </summary>
        public Dictionary<long, List<decimal>> LiftWearoutDict { get; set; }

        /// <inheritdoc />
        public string Id => "LiftWearout";

        /// <inheritdoc />
        public string Name => "Износ конструктивного элемента \"Лифт\" больше или равно 90";

        /// <inheritdoc />
        public TypeParam TypeParam => TypeParam.Qualit;

        /// <inheritdoc />
        public object GetValue(IStage3Entity obj)
        {
            if (this.LiftWearoutDict.ContainsKey(obj.Id) && this.LiftWearoutDict[obj.Id].Any(x => x >= 90))
            {
                return YesNo.Yes;
            }

            return YesNo.No;
        }

        /// <inheritdoc />
        public Type EnumType => typeof(YesNo);
    }
}