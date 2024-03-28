Ext.define('B4.store.Maplet', {
    extend: 'Ext.data.Store',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Percent' },
        { name: 'ReportMonth' }
    
    ],
    storeId: 'Maplet',
    
    proxy: {
        type: 'ajax',
        url: 'DesktopMap/GetContractors',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});