Ext.define('B4.store.dict.ServiceTypeForSelect', {
    extend: 'Ext.data.Store',
    requires: [
        'B4.model.dict.ServiceType',
        'B4.enums.TypeGroupServiceDi'
    ],
    model: 'B4.model.dict.ServiceType',
    data: B4.enums.TypeGroupServiceDi.getItemsMeta()
});