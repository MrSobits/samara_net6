/*
    Данный стор предназначен для массового выбора домов
    натроен на серверный метод.
    Например используется в ГЖИ для выборка домов.
    url: RealityObject/List/
*/
Ext.define('B4.store.transferrf.RealObjForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectStore',
    model: 'B4.model.transferrf.RealityObject'
});