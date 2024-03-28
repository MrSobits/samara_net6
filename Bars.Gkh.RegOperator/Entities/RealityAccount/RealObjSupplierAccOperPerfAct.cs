namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using GkhCr.Entities;

    /// <summary>
    /// ����� �������� �� �������� � ������������ �  ����� ����������� �����
    /// </summary>
    public class RealObjSupplierAccOperPerfAct : BaseImportableEntity
    {
        /// <summary>
        ///  �������� ����� �� �������� � ������������
        /// </summary>
        public virtual RealityObjectSupplierAccountOperation SupplierAccOperation { get; set; }

        /// <summary>
        /// ��� ����������� �����
        /// </summary>
        public virtual PerformedWorkAct PerformedWorkAct { get; set; }
    }
}