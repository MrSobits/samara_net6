Ext.define('B4.store.view.WarningDoc', {
    requires: ['B4.model.WarningDoc'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.WarningDoc',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningDoc',
        listAction: 'ListView'
    }
});