Ext.define('B4.store.view.AdminCase', {
    requires: ['B4.model.AdminCase'],
    extend: 'B4.base.Store',
    autoLoad: false,

    model: 'B4.model.AdminCase',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCase',
        listAction: 'ListView'
    }
});