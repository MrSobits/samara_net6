Ext.define('B4.model.RisPackage', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Type' },
        { name: 'Name' },
        { name: 'Signed' }
        //{ name: 'NotSignedDataLength' },
        //{ name: 'SignedDataLength' }
    ],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});