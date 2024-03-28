Ext.define('B4.store.passport.PartTreeStore', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.passport.Part'],
    model: 'B4.model.passport.Part',
    defaultRootProperty: 'Childrens'
});