namespace Bars.Gkh.Controllers.Config
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.Config;

    /// <summary>
    /// Контроллер конфигурации приложения
    /// </summary>
    public class GkhConfigController : BaseController
    {
        public IGkhConfigService ConfigService { get; set; }

        /// <summary>
        /// Получение списка дочерних элементов конфигурации
        /// </summary>
        /// <param name="parent">Идентификатор корневого элемента</param>
        /// <returns>Дочерние элементы</returns>
        public ActionResult ListItems(string parent)
        {
            return new JsonListResult(this.ConfigService.GetItems(parent));
        }

        /// <summary>
        /// Получение корневых элементов конфигурации
        /// </summary>
        /// <returns>Корневые элементы</returns>
        public ActionResult ListRoots()
        {
            var roots = this.ConfigService.GetRoots();
            return new JsonNetResult(new { expanded = true, children = this.PrepareTreeItems(roots) });
        }

        private List<object> PrepareTreeItems(MetaItem[] items)
        {
            var result = new List<object>();

            foreach (var item in items)
            {
                if (item.Children != null && item.Children.Length > 0)
                {
                    result.Add(new
                    {
                        text = item.DisplayName,
                        id = item.Id,
                        expanded = true,
                        children = this.PrepareTreeItems(item.Children)
                    });
                }
                else
                {
                    result.Add(new { text = item.DisplayName, id = item.Id, leaf = true });
                }
            }

            return result;
        }

        /// <summary>
        /// Сохранение конфигурации
        /// </summary>
        /// <param name="configs">Сериализованный словарь параметров конфигурации</param>
        /// <returns></returns>
        public ActionResult SaveAppConfig(string configs)
        {
            IDictionary<string, string> errors = null;
            var success = false;
            Exception exception = null;
            try
            {
                success = this.ConfigService.UpdateConfigs(configs, out errors);
            }
            catch (Exception e)
            {
                exception = e;
            }

            return new JsonNetResult(new { success, message = exception.Return(x => x.Message), errors });
        }
    }
}