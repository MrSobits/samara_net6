Ext.define('B4.store.manorglicense.RequestPerson', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.RequestPerson'],
    autoLoad: false,
    storeId: 'manOrgRequestPersonStore',
    model: 'B4.model.manorglicense.RequestPerson'
});