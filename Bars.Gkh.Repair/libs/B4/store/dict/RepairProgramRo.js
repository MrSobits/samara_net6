Ext.define('B4.store.dict.RepairProgramRo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'Address', 'Municipality'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairProgram',
        listAction: 'ListAvailableRealtyObjects'
    }
});