/*
    ������ ���� ������������ ��� ��������� ��������� �����
    �������� � �������� ������� ����������� ��������.
    ����� ��������� ������ ����� ���� ��������� ����� � �������
    ����������� ������� ������� ����� ��������� �� ������.
    �������� ������������ � ��� ��� ������� �����.
*/
Ext.define('B4.store.transferrf.RealObjForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectedStore',
    model: 'B4.model.transferrf.RealityObject'
});