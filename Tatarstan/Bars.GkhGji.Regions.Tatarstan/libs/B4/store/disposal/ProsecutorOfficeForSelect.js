Ext.define('B4.store.disposal.ProsecutorOfficeForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ProsecutorOffice'],
    model: 'B4.model.dict.ProsecutorOffice',
    autoLoad: false,

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProsecutorOfficeDict'
    }
});