namespace Bars.Gkh
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Modules.Ecm7.Framework;

    public interface IViewCollection
    {
        int Number { get; }

        string GetDeleteScript(DbmsKind dbmsKind, string name);

        string GetCreateScript(DbmsKind dbmsKind, string name);

        List<string> GetCreateAll(DbmsKind dbmsKind);

        List<string> GetDropAll(DbmsKind dbmsKind);
    }
}