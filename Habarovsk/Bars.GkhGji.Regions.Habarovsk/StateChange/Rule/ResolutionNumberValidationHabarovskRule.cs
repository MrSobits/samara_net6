﻿namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Regions.Habarovsk.StateChange;

    /// <summary>
    /// Постановление
    /// </summary>
    public class ResolutionNumberValidationHabarovskRule : BaseDocNumberValidationHabarovskRule
    {
        /// <inheritdoc />
        public override string Id => "gji_vor_resolution_validation_number";

        /// <inheritdoc />
        public override string TypeId => "gji_document_resol";

        /// <inheritdoc />
        public override string Name => "Воронеж - Присвоение номера постановления";

        /// <inheritdoc />
        public override string Description => "Воронеж - Данное правило присваивает номер постановления";
    }
}