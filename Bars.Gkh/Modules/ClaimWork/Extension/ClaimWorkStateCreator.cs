namespace Bars.Gkh.Modules.ClaimWork.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Modules.ClaimWork.Contracts;
    using Bars.Gkh.Repositories;

    using Castle.Windsor;
    using B4.Utils;

    using Bars.Gkh.Modules.ClaimWork.Entities;

    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    using DisplayAttribute = System.ComponentModel.DataAnnotations.DisplayAttribute;

    /// <summary>
    /// Расширение для создания статусов ПИР
    /// </summary>
    public static class ClaimWorkStateCreatorExtension
    {
        /// <summary>
        /// Создание статусов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">Контейнер</param>
        /// <param name="logger">Логгер</param>
        /// <param name="statesNotToAdd">Состояния, которые не нужно создавать</param>
        /// <param name="predicate"> Условие </param>
        public static void CreateStates<T>(
            this IWindsorContainer container, 
            ILogger logger,
            List<State> statesNotToAdd = null,
            Func<FieldInfo, bool> predicate = null
            ) 
            where T : IStatefulEntity
        {
            using (container.BeginScope())
            {
                try
                {
                    var stateRepo = container.Resolve<IStateRepository>();
                    var stateProv = container.Resolve<IStateProvider>();

                    using (container.Using(stateRepo, stateProv))
                    {
                        var info = stateProv.GetStatefulEntityInfo(typeof (T));
                        var existStates = stateRepo.GetAllStates<T>().ToList();

                        if (statesNotToAdd != null)
                        {
                            //пропускаем эти статусы
                            existStates.AddRange(statesNotToAdd);
                        }

                        var statesToCreate = typeof(ClaimWorkStates)
                            .GetFields(BindingFlags.Public | BindingFlags.Static)
                            .WhereIf(predicate != null, predicate)
                            .Select(x => new
                            {
                                Name = (string) x.GetValue(null),
                                Order = x.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0
                            })
                            .Where(x => !existStates.Select(y => y.Name).Contains(x.Name));

                        long outVal;
                        var maxCode = existStates.Where(x => long.TryParse(x.Code, out outVal)).SafeMax(x => long.Parse(x.Code));
                        foreach (var state in statesToCreate)
                        {
                            stateRepo.Save(new State
                            {
                                Name = state.Name,
                                Code = (++maxCode).ToString(),
                                TypeId = info.TypeId,
                                OrderValue = state.Order
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Ошибка при создании статусов");
                }
            }
        }
    }
}