Ext.define('B4.store.ActiveOperator', {
    extend: 'Ext.data.Store',
    model: 'B4.model.administration.Operator',
    autoLoad: true,
    storeId: 'ActiveOperator',
    
    proxy: {
        type: 'ajax',
        url: 'Operator/GetActiveOperator',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});