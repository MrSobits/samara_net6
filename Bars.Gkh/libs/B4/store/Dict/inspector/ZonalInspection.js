Ext.define('B4.store.dict.inspector.ZonalInspection', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ZonalInspection'],
    autoLoad: false,
    storeId: 'inspectorZoanalInsp',
    model: 'B4.model.dict.ZonalInspection',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Inspector',
        listAction: 'ListZonalInspection'
    }
});
