namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Проверка ОКТМО с деревом МО
    /// </summary>
    public class CheckFiasOktmoAction : BaseExecutionAction
    {
        public IRepository<LogOperation> LogOperationDomain { get; set; }

        public IDomainService<Fias> FiasDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IFileManager FileManager { get; set; }

        public override string Description => "Проверка ОКТМО с деревом МО";

        public override string Name => "Проверка ОКТМО с деревом МО";

        public override Func<IDataResult> Action => this.CheckOktmo;

        public BaseDataResult CheckOktmo()
        {
            var logOperation = new LogOperation
            {
                StartDate = DateTime.UtcNow,
                Comment = "Проверка ОКТМО с деревом МО",
                OperationType = LogOperationType.CheckOktmo,
                EndDate = DateTime.UtcNow,
                User = this.UserManager.GetActiveUser()
            };

            var finalStr = new StringBuilder();

            finalStr.AppendLine("Муниципальное образование; ОКТМО ФИАС; ОКТМО дерево МО; Статус проверки");

            var fiasDict = this.FiasDomain.GetAll()
                .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today && x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new
                {
                    x.AOGuid,
                    x.OKTMO
                }).ToDictionary(x => x.AOGuid.ToLower());

            var municipalityList = this.MunicipalityDomain.GetAll().ToList();

            foreach (var mo in municipalityList)
            {
                var fiasItem = fiasDict.Get(mo.FiasId.ToLower());

                if (fiasItem != null)
                {
                    if (fiasItem.OKTMO.IsNotEmpty() && mo.Oktmo.IsNotEmpty() && fiasItem.OKTMO.ToStr().ToLower() == mo.Oktmo.ToLower())
                    {
                        finalStr.AppendLine($"{mo.Name}; {fiasItem.OKTMO}; {mo.Oktmo}; Успешно");
                        continue;
                    }

                    finalStr.AppendLine($"{mo.Name}; {fiasItem.OKTMO}; {mo.Oktmo}; Не успешно");
                }
            }
           
            var log = Encoding.GetEncoding(1251).GetBytes(finalStr.ToString());

            var logFile = this.FileManager.SaveFile($"{logOperation.OperationType.GetDisplayName()}.csv", log);

            logOperation.LogFile = logFile;

            this.LogOperationDomain.Save(logOperation);

            return new BaseDataResult();
        }
    }
}