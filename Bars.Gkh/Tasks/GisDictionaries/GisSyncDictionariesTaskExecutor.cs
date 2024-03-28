using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bars.Gkh.Tasks.ExecutorTest
{
    public class GisSyncDictionariesTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                string t = typeof(GisGkhDictonary).Name.ToString();

                //var dictionaryList = DictionaryManager.GetAllDictionaries("7.0.0.15");


                return new BaseDataResult("fdf");
            }
            catch(Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
