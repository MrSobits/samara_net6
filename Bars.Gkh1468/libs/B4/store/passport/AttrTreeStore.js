Ext.define('B4.store.passport.AttrTreeStore', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.passport.Attribute'],
    model: 'B4.model.passport.Attribute',
    defaultRootProperty: 'Childrens',
    proxy: {
        type: 'memory'
    }
});