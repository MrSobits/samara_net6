Ext.define('B4.model.smev.SMEVEGRNRoom', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRNExecute',
        listAction: 'GetListRoom'
    },
    fields: [
        { name: 'RoomNum'},
        { name: 'CadastralNumber' },
    ]
});