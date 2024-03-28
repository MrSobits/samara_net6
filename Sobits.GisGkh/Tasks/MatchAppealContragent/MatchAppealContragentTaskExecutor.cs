using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    /// <summary>
    /// Задача на проставление контрагентов-юрлиц в обращениях ГИС ЖКХ
    /// </summary>
    public class MatchAppealContragentTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public MatchAppealContragentTaskExecutor()
        {
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var appealsWithoutContragent = AppealCitsDomain.GetAll()
                    .Where(x => x.ContragentCorrespondent == null)
                    .Where(x => x.GisGkhContragentGuid != null || x.GisGkhContragentGuid != "")
                    .ToList();
                List<string> guids = new List<string>();
                foreach (var appeal in appealsWithoutContragent)
                {
                    guids.Add(appeal.GisGkhContragentGuid);
                }
                var contragents = ContragentDomain.GetAll()
                    .Where(x => guids.Contains(x.GisGkhGUID)).ToDictionary(x => x.GisGkhGUID, x => x);
                int num = 0;
                foreach (var appeal in appealsWithoutContragent)
                {
                    if (contragents.ContainsKey(appeal.GisGkhContragentGuid))
                    {
                        appeal.ContragentCorrespondent = contragents[appeal.GisGkhContragentGuid];
                        AppealCitsDomain.Update(appeal);
                        num++;
                    }
                }
                return new BaseDataResult(true, $"Количество обращений, которым проставлены контрагенты: {num}");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }

        #endregion

        #region Private methods

        #endregion
    }
}
