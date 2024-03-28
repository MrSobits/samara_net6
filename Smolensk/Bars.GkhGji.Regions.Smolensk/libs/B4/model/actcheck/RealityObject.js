Ext.define('B4.model.actcheck.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'Municipality' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'HaveViolation', defaultValue: 10 }, // ��� ����� ��������� � ��������� ������� ���� ����� ���� � ������������� �����������
        { name: 'ViolationCount', defaultValue: 0 },
        { name: 'Description' },
        { name: 'NotRevealedViolations' },
        { name: 'IsRealityObject', defaultValue: true }
    ]
});