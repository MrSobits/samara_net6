Ext.define('B4.model.RosRegExtractBig', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractBig'
    },
    fields: [
        { name: 'Id' },
        { name: 'CadastralNumber' },
        { name: 'Address' },
        { name: 'ExtractDate' },
        { name: 'ExtractNumber' },
        { name: 'RoomArea' }
    ]
});