namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// ����� �������� �� ����� ����� ���� � ������ ���� ����������� �����
    /// </summary>
    public class RealObjPaymentAccOperPerfAct : BaseImportableEntity
    {
        /// <summary>
        /// �������� �� ����� ����� ����
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation PayAccOperation { get; set; }

        /// <summary>
        /// ������ � ����� ����������� �����
        /// </summary>
        public virtual PerformedWorkActPayment PerformedWorkActPayment { get; set; }
    }
}