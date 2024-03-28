Ext.define('B4.store.MpRole', {
    extend: 'B4.base.Store',
    requires: ['B4.model.MpRole'],
    autoLoad: false,
    model: 'B4.model.MpRole',
    sorters: [{
        property: 'Name',
        direction: 'ASC'
    }],
});