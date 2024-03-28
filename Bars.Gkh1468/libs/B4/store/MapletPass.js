Ext.define('B4.store.MapletPass', {
    extend: 'Ext.data.Store',
    fields: [
        { name: 'id', useNull: true },
        { name: 'Name' },
        { name: 'Percent' },
        { name: 'HouseType' },
        { name: 'ReportMonth' }
    ],

    proxy: {
        type: 'ajax',
        url: 'DesktopMap/GetPassports',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});