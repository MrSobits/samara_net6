Ext.define('B4.store.ActiveUser', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ActiveUser'],

    autoLoad: true,
    model: 'B4.model.ActiveUser'
});