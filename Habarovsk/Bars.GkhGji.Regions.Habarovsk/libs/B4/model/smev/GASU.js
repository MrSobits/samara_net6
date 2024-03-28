Ext.define('B4.model.smev.GASU', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GASU'
    },
    fields: [
        { name: 'Id' },
        { name: 'RequestDate' },
        { name: 'Answer' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'Inspector' },
        { name: 'RequestState', defaultValue: 0 },
        { name: 'MessageId' },
        { name: 'GasuMessageType', defaultValue: 10  }
       
    ]
});