/*
    ������ ���� ������������ ��� ��������� ������ �����
    ������� �� ��������� �����.
    �������� ������������ � ��� ��� ������� �����.
    url: RealityObject/List/
*/
Ext.define('B4.store.transferrf.RealObjForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectStore',
    model: 'B4.model.transferrf.RealityObject'
});