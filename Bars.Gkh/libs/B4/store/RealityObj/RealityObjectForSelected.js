/*
    ������ ���� ������������ ��� ��������� ��������� �����
    �������� � �������� ������� ����������� ��������.
    ����� ��������� ������ ����� ���� ��������� ����� � �������
    ����������� ������� ������� ����� ��������� �� ������.
    �������� ������������ � ��� ��� ������� �����.
*/
Ext.define('B4.store.realityobj.RealityObjectForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectedStore',
    model: 'B4.model.RealityObject'
});