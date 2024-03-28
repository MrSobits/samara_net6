Ext.define('B4.model.ExtractEgrn', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ExtractEgrn'
    },
    fields: [
        { name: 'Id' },
        { name: 'CadastralNumber' },
        { name: 'Area' },
        { name: 'Type' },
        { name: 'Purpose' },
        { name: 'Address' },
        { name: 'ExtractDate' },
        { name: 'RoomId' },
        { name: 'Room_id' },
        { name: 'IsMerged' },
        { name: 'ExtractId' }, 
        { name: 'RoomAdr' },
        { name: 'RightsCount' }
    ]
});