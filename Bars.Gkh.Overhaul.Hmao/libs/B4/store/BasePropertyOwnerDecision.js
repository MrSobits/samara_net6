Ext.define('B4.store.BasePropertyOwnerDecision', {
    extend: 'B4.base.Store',
    requires: ['B4.model.BasePropertyOwnerDecision'],
    autoLoad: false,
    model: 'B4.model.BasePropertyOwnerDecision',
    sorters: [{
        property: 'ProtocolDate',
        direction: 'DESC'
    }]
});