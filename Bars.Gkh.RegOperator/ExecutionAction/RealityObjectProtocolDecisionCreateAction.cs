namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;

    public class RealityObjectProtocolDecisionCreateAction : BaseExecutionAction
    {
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomain { get; set; }

        public IRobjectDecisionService RobjectDecisionService { get; set; }

        public override string Description => "Создает протоколы решений собственников жилых помещений на всех исправных и многоквартирных домах";

        public override string Name => "РегОператор - Создание протоколов решений собственников жилых помещений на всех исправных и многоквартирных домах";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            return this.InTransaction(this.CreateProtocols);
        }

        private void CreateProtocols()
        {
            var existingProtocolRoIds = this.RealityObjectDecisionProtocolDomain.GetAll()
                .Select(x => x.RealityObject.Id);

            var realityObjectIds = this.RealityObjectDomain.GetAll()
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                .Where(x => !existingProtocolRoIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToArray();

            var protocol = new DynamicDictionary();
            protocol.Add("RealityObject", new DynamicDictionary());
            protocol.Add("ProtocolDate", "01.07.2014");
            protocol.Add("DateStart", "01.07.2014");
            protocol.Add("File", null);

            var crFundFormationDecision = new DynamicDictionary();
            crFundFormationDecision.Add("Id", "");
            crFundFormationDecision.Add("IsChecked", true);
            crFundFormationDecision.Add("Decision", 1);

            var @params = new BaseParams();
            @params.Params["Protocol"] = protocol;
            @params.Params["CrFundFormationDecision"] = crFundFormationDecision;
            @params.Files = new Dictionary<string, FileData>();

            var i = 1;
            foreach (var roId in realityObjectIds)
            {
                var protocolDict = @params.Params["Protocol"] as DynamicDictionary;
                if (protocolDict != null)
                {
                    protocolDict["DocumentNum"] = i;
                    var roDict = protocolDict["RealityObject"] as DynamicDictionary;
                    if (roDict != null)
                    {
                        roDict["Id"] = roId;
                    }
                }

                this.RobjectDecisionService.SaveOrUpdateDecisions(@params);
                i++;
            }
        }

        private BaseDataResult InTransaction(Action action)
        {
            string errorMsg = null;

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    errorMsg = exc.Message;

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        errorMsg = string.Format("{0}; {1}", errorMsg, e.Message);
                    }
                }
            }

            return new BaseDataResult(errorMsg == null, errorMsg);
        }
    }
}