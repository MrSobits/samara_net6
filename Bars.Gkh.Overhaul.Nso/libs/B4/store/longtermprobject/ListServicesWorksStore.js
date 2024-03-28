Ext.define('B4.store.longtermprobject.ListServicesWorksStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.longtermprobject.ListServicesWorksModel'],
    autoLoad: false,
    model: 'B4.model.longtermprobject.ListServicesWorksModel',
    storeId: 'listservicesworksstore'
});