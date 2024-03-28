Ext.define('B4.store.service.ContragentForProvider', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'programCr',
    model: 'B4.model.Contragent',
    proxy: {
        autoLoad: false,
        type: 'ajax',
        url: B4.Url.action('List', 'Contragent', {
            showAll: true
        }),
        reader: {
            type: 'json',
            root: 'data',
            idProperty: 'Id',
            totalProperty: 'totalCount',
            successProperty: 'success',
            messageProperty: 'message'
        }
    }
});