Ext.define('B4.model.ExtractEgrnRight', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ExtractEgrnRight'
    },
    fields: [
        { name: 'Id' },
        { name: 'Type' },
        { name: 'Number' },
        { name: 'Share' },
        { name: 'EgrnId' }
    ]
});