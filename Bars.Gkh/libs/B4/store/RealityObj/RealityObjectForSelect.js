/*
    ������ ���� ������������ ��� ��������� ������ �����
    ������� �� ��������� �����.
    �������� ������������ � ��� ��� ������� �����.
    url: RealityObject/List/
*/
Ext.define('B4.store.realityobj.RealityObjectForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectStore',
    model: 'B4.model.RealityObject'
});