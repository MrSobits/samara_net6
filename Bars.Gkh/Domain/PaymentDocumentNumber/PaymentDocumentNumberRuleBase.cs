namespace Bars.Gkh.Domain.PaymentDocumentNumber
{
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Bars.Gkh.Enums;
    using Castle.Windsor;
    using System;

    /// <summary>
    /// ��������� ������� ���������� ������ ��������� �� ������
    /// </summary>
    public abstract class PaymentDocumentNumberRuleBase
    {
        private IWindsorContainer container;
        private string name;
        /// <summary>
        /// ����������� ������
        /// </summary>
        protected PaymentDocumentNumberRuleBase(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// ������������ ������� (���)
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? (this.name = this.GetType().FullName);
            }
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// ������������ �� �������
        /// </summary>
        public virtual bool IsRequired
        {
            get { return false; }
        }

        /// <summary>
        /// ������������ ������� �� ���������
        /// </summary>
        public virtual NumberBuilderConfig DefaultConfig
        {
            get
            {
                return new NumberBuilderConfig
                {
                    Name = this.Name,
                    SymbolsLocation = SymbolsLocation.All,
                    SymbolsCount = 0
                };
            }
        }

        /// <summary>
        /// ��������� ���������� ��������
        /// </summary>
        public virtual ColumnConfig SymbolsCountConfig
        {
            get
            {
                return new ColumnConfig
                {
                    Editable = false,
                    Visible = false
                };
            }
        }

        /// <summary>
        /// ��������� ������������ ��������
        /// </summary>
        public virtual ColumnConfig SymbolsLocationConfig
        {
            get
            {
                return new ColumnConfig
                {
                    Editable = false,
                    Visible = true
                };
            }
        }

        /// <summary>
        /// ����� ���������� ����� ������
        /// </summary>
        /// <param name="numberBuilderConfig">������������ �������</param>
        /// <param name="obj">�������� �� ������</param>
        /// <returns>����� ������</returns>
        public abstract string GetNumberPart(NumberBuilderConfig numberBuilderConfig, object obj);

        /// <summary>
        /// ����� ���������� ������� ����� ������
        /// </summary>
        /// <param name="numberBuilderConfig">������������ �������</param>
        /// <returns>����� ������</returns>
        public virtual string GetNumberExamplePart(NumberBuilderConfig numberBuilderConfig)
        {
            return this.GetNumberPart(numberBuilderConfig, null);
        }
        /// <summary>
        /// �������� ����� ������ � ������������ � �������������
        /// </summary>
        /// <param name="numberBuilderConfig">������������ �������</param>
        /// <param name="numberPart">����� ������</param>
        /// <returns>������������ ������</returns>
        protected string HandleString(NumberBuilderConfig numberBuilderConfig, string numberPart)
        {
            string result;

            if (string.IsNullOrEmpty(numberPart))
            {
                result = string.Empty;
            }
            else if (numberBuilderConfig.SymbolsLocation == SymbolsLocation.All)
            {
                result = numberPart;
            }
            else if (numberBuilderConfig.SymbolsLocation == SymbolsLocation.Start)
            {
                var startIndex = 0;
                var count = Math.Min(numberPart.Length, numberBuilderConfig.SymbolsCount);
                result = numberPart.Substring(startIndex, count);
            }
            else
            {
                var startIndex = Math.Max(numberPart.Length - numberBuilderConfig.SymbolsCount, 0);
                var count = numberPart.Length - startIndex;
                result = numberPart.Substring(startIndex, count);
            }

            return result;
        }
    }

    public class ColumnConfig
    {
        public bool Editable { get; set; }
        public bool Visible { get; set; }
    }
}