namespace Bars.GkhGji.Regions.Tula.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;

    using Bars.Gkh.Import;
    using B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using System.Threading;
    using Castle.Windsor;

    /// <summary>
    /// Это пустая балванка дял того чтобы корректно в логе загрузок отображалось поле Тип импорта
    /// </summary>
    public class AppealsImport : IGkhImport
    {
        public virtual IWindsorContainer Container { get; set; }

        public static string Id = "DeloServiceImport";

        public virtual string Key
        {
            get { return Id; }
        }

        public virtual string CodeImport
        {
            get { return "DeloServiceImport"; }
        }

        public virtual string Name
        {
            get { return "Импорт обращений из АСЭД ДЕЛО"; }
        }

        public virtual string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public virtual string PermissionName
        {
            get { return "Import.RoImport.View"; }
        }
        public virtual ImportResult Import(BaseParams baseParams)
        {
            return new ImportResult();
        }

        public virtual bool Validate(BaseParams baseParams, out string message)
        {
            message = "";
            return false;
        }

        public string[] Dependencies { get; }

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return new BaseDataResult();
        }

        public string ExecutorCode { get { return Key; } }
    }
}