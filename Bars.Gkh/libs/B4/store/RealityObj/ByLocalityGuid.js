/**
* стор жилых домов, направлен на метод, возвращающий список домов по guid населенного пункта
*/
Ext.define('B4.store.realityobj.ByLocalityGuid', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListByLocalityGuid'
    }
});