namespace Bars.Gkh.Domain.PaymentDocumentNumber
{
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Построитель номера документа
    /// </summary>
    public class PaymentDocumentNumberBuilder : IDisposable
    {
        private readonly IWindsorContainer container;
        private List<Tuple<PaymentDocumentNumberRuleBase, NumberBuilderConfig>> ruleConfigList;
        private Dictionary<string, PaymentDocumentNumberRuleBase> nameToRuleMap;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoС контейнер</param>
        public PaymentDocumentNumberBuilder(IWindsorContainer container)
        {
            this.container = container;
            this.Initialize();
        }

        /// <summary>
        /// Метод построения номера
        /// </summary>
        /// <param name="obj">Документ на оплату</param>
        /// <returns>Номер документа</returns>
        public string GetDocumentNumber(object obj)
        {
            var result = new StringBuilder();

            foreach (var rule in this.ruleConfigList)
            {
                result.Append(rule.Item1.GetNumberPart(rule.Item2, obj));
            }

            return result.ToString();
        }

        /// <summary>
        /// Метод построения номера
        /// </summary>
        /// <returns>Номер документа</returns>
        public string GetDocumentNumberExample()
        {
            var result = new StringBuilder();

            foreach (var rule in this.ruleConfigList)
            {
                result.Append(rule.Item1.GetNumberExamplePart(rule.Item2));
            }

            return result.ToString();
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            foreach (var rule in this.ruleConfigList)
            {
                this.container.Release(rule.Item1);
            }
        }

        private void Initialize()
        {
            this.nameToRuleMap = this.container.ResolveAll<PaymentDocumentNumberRuleBase>()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.First());

            var storedConfigs = this.container.GetGkhConfig<RegOperatorConfig>()
                .PaymentDocumentConfigContainer
                .PaymentDocumentConfigLegal
                .PaymentDocumentConfigLegalNumber
                .NumberBuilderConfigs;

            this.ruleConfigList = new List<Tuple<PaymentDocumentNumberRuleBase, NumberBuilderConfig>>(storedConfigs.Count);

            foreach (var numberBuilderConfig in storedConfigs)
            {
                PaymentDocumentNumberRuleBase rule;
                if (numberBuilderConfig.Order > 0 && this.nameToRuleMap.TryGetValue(numberBuilderConfig.Name, out rule))
                {
                    this.ruleConfigList.Add(Tuple.Create(rule, numberBuilderConfig));
                }
            }

            this.ruleConfigList.Sort((left, right) => left.Item2.Order.CompareTo(right.Item2.Order));
        }
    }
}
