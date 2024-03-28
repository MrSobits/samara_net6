namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// �������� �� ����� ����� ����
    /// </summary>
    public class RealityObjectPaymentAccountOperation : BaseImportableEntity
    {
        /// <summary>
        /// ���� ��������
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// ���� ����� ����
        /// </summary>
        public virtual RealityObjectPaymentAccount Account { get; set; }

        /// <summary>
        /// ����� ��������
        /// </summary>
        public virtual decimal OperationSum { get; set; }

        /// <summary>
        /// ��� ��������
        /// </summary>
        public virtual PaymentOperationType OperationType { get; set; }

        /// <summary>
        /// ������ ��������
        /// </summary>
        public virtual OperationStatus OperationStatus { get; set; }
    }
}