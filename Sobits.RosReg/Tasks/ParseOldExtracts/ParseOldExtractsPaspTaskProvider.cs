using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;

using Castle.Windsor;

using System;
using System.Collections.Generic;

namespace Sobits.RosReg.Tasks.ExtractParse
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Sobits.RosReg.Entities;

    public class ParseOldExtractsPaspTaskProvider : ITaskProvider
    {
        private const short NumInds = 2000;
        
        public string TaskCode => "ParseOldExtractsPaspTaskProvider";
        
        private readonly IWindsorContainer container;
        public ParseOldExtractsPaspTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }
        //public IDomainService<ExtractEgrnRightInd> ExtractEgrnRightIndDomain { get; set; }

        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var rep = this.container.Resolve<IRepository<ExtractEgrnRightInd>>();
            
            var descrs = new List<TaskDescriptor>();
            
            var inds = rep.GetAll()
                .Where(x => x.DocIndNumber == null && x.DocIndSerial == null)
                .Select(x => x.Id)
                .ToList();


            ProcessByPortion(
                done =>
                {
                    var args = DynamicDictionary.Create();

                    args.SetValue("inds", inds.Skip(done).Take(NumInds).ToArray());

                    descrs.Add(
                        new TaskDescriptor(
                            "Заполнение паспорных данных из старых выписок",
                            ParseOldExtractsPaspTaskExecutor.Id,
                            new BaseParams(){Params = args})
                    );
                }, 
                inds.Count,
                NumInds);


            return new CreateTasksResult(descrs.ToArray());
        }

        

        public static void ProcessByPortion(Action<int> action, int totalCount, int portion)
        {
            var done = 0;
            while (done < totalCount)
            {
                action(done);
                done += portion;
            }
        }
    }
}