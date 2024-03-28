using System;
using Bars.B4.Utils;

namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class WorkHelper : BindingBase
    {
        /// <summary>
        /// Текущий вид работ
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentWork"))
                {
                    throw new SpecFlowException("Нет текущего вида работ");
                }

                var work = ScenarioContext.Current.Get<dynamic>("CurrentWork");

                return work;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentWork"))
                {
                    ScenarioContext.Current.Remove("CurrentWork");
                }

                ScenarioContext.Current.Add("CurrentWork", value);
            }
        }

        /// <summary>
        /// добавленный финансовые источники
        /// </summary>
        public static List<long> FinSources
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("WorkFinSources"))
                {
                    ScenarioContext.Current.Add("WorkFinSources", new List<long>());
                }

                return ScenarioContext.Current.Get<List<long>>("WorkFinSources");
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("WorkFinSources"))
                {
                    ScenarioContext.Current.Remove("WorkFinSources");
                }

                ScenarioContext.Current.Add("WorkFinSources", value);
            }
        }

        public static bool ContainsFinSource(int type)
        {
            int idWork = (int) WorkHelper.Current.Id;

            var workServiceType =
                Type.GetType("Bars.Gkh.Overhaul.Entities.WorkTypeFinSource, Bars.Gkh.Overhaul");
            var genericDomainServ = typeof (IDomainService<>).MakeGenericType(workServiceType);
            dynamic workTypeFinServ = Container.Resolve(genericDomainServ);

            IEnumerable<dynamic> workTypeFinServ2 = workTypeFinServ.GetAll();

            var workTypeFinSources = workTypeFinServ2.Where((x) => (int)(x.Work.Id) == idWork);

            return workTypeFinSources.Any(x => (int)x.GetType().GetProperty("TypeFinSource").GetValue(x) == type);
        }

        public static int GetFinSource(string name)
        {
            int type;

            switch (name)
            {
                case "Бюджет фонда":
                    {
                        type = 10;

                        break;
                    }

                case "Бюджет региона":
                    {
                        type = 20;

                        break;
                    }

                case "Бюджет МО":
                    {
                        type = 30;

                        break;
                    }

                case "Средства собственника":
                    {
                        type = 40;

                        break;
                    }

                case "Иные источники":
                    {
                        type = 50;

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format("типа источника финансирования с наименованием {0} не существует", name));
                    }
            }

            return type;
        }
    }
}
