/*
    Данный стор предназначен для храанения выбранных домов
    действий с сервером никаких производить недолжен.
    После массового выбора домов идет обработка стора и вручную
    формируются объекты которые нужно отправить на сервер.
    Например используется в ГЖИ для выборка домов.
*/
Ext.define('B4.store.transferrf.RealObjForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjForSelectedStore',
    model: 'B4.model.transferrf.RealityObject'
});