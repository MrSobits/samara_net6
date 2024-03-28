namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Параметр очередности Превышение нормативных сроков службы КЭ и ИС
    /// </summary>
    public class ExceedStructuralElementLifeTimePriorityParam : IPriorityParams
    {
        /// <summary>
        /// Словарь, который содержит конструктивный элемент дома с максимальным годом капитального ремонта
        /// <para>В качестве ключа идентификатор 3го этапа</para>
        /// </summary>
        public Dictionary<long, RealityObjectStructuralElement> StructuralElementWithLastYearDict { get; set; }

        /// <inheritdoc />
        public string Id => "ExceedStructuralElementLifeTime";

        /// <inheritdoc />
        public string Name => "Превышение нормативных сроков службы КЭ и ИС";

        /// <inheritdoc />
        public TypeParam TypeParam => TypeParam.Quant;

        /// <inheritdoc />
        public object GetValue(IStage3Entity obj)
        {
            var value = 0;

            if (this.StructuralElementWithLastYearDict.ContainsKey(obj.Id))
            {
                var element = this.StructuralElementWithLastYearDict[obj.Id];
                value = element.LastOverhaulYear + element.StructuralElement.LifeTime - DateTime.Today.Year;
            }

            return value;
        }
    }
}