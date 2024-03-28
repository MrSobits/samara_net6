/**
* Store жилых домов у которых нет текущих договоров непосредственного управления
*/
Ext.define('B4.store.realityobj.ExceptDirectManag', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjExcDirectManagStore',
    model: 'B4.model.manorg.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListExceptDirectManag'
    }
});