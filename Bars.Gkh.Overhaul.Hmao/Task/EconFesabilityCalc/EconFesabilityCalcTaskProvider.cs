using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Task
{
    public class EconFesabilityCalcTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;
        
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public EconFesabilityCalcTaskProvider (IWindsorContainer container)
        {
            this.container = container;
        } 

        public string TaskCode => "EconFesabilityCalcTaskProvider";

        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var realityObjects = this.container.ResolveDomain<RealityObject>();
            var municipalities = this.container.ResolveDomain<Municipality>();

            using (this.container.Using(realityObjects, municipalities))
            {
                var descrs = new List<TaskDescriptor>();

                municipalities.GetAll()
                    .ToList()
                    .ForEach(mun =>
                    {
                        var roIds = realityObjects.GetAll()
                        .Where(x=> x.Municipality == mun)
                  .Where(x => x.TypeHouse == Enums.TypeHouse.ManyApartments)
                  .Where(x => x.ConditionHouse == Enums.ConditionHouse.Serviceable).Select(x => x.Id).ToArray();
                        var yearStart = baseParams.Params.GetAs<int>("yearStart");
                        var yearEnd = baseParams.Params.GetAs<int>("yearEnd");



                        var args = DynamicDictionary.Create();

                        args.SetValue("roIds", roIds.ToArray());
                        args.SetValue("yearStart", yearStart);
                        args.SetValue("yearEnd", yearEnd);
                        if (roIds.Count() > 0)
                        {
                            descrs.Add(
                                new TaskDescriptor(
                                   "Рассчет Целесообразности ремонта " + mun.Name,
                                   EconFesabilityCalcTaskExecutor.Id,
                                    new BaseParams { Params = args })
                                );
                        }
                    });
              

                //var take = 500;
                //ProcessByPortion(
                //    done =>
                //    {
                //        var args = DynamicDictionary.Create();

                //        args.SetValue("roIds", roIds.Skip(done).Take(take).ToArray());
                //        args.SetValue("yearStart", yearStart);
                //        args.SetValue("yearEnd", yearEnd);

                //        descrs.Add(
                //            new TaskDescriptor(
                //               "Рассчет Целесообразности ремонта",
                //               EconFesabilityCalcTaskExecutor.Id,
                //                new BaseParams { Params = args })
                //            );
                //    },
                //    (int)roIds.Count(),
                //    take);

                var result = new CreateTasksResult(descrs.ToArray());                              

                return result;
            }
            
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
