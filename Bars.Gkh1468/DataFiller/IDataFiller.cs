namespace Bars.Gkh1468.DataFiller
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;

    using ValueType = Bars.Gkh1468.Enums.ValueType;

    public interface IDataFiller
    {
        MetaAttribute MetaAttribute { get; set; }

        RealityObject RealityObject { get; set; }

        List<BaseProviderPassportRow> Result { get; set; }

        string Code { get; }

        string Name { get; }

        bool Multiple { get; }

        ValueType ValueType { get; }

        void To1468();

        void From1468();
    }
}