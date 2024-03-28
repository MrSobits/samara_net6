Ext.define('B4.model.cscalculation.RoomList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CSCalculationOperations',
        listAction: 'GetListRoom'
    },
    fields: [
        { name: 'Id' },
        { name: 'RoomNum'},
        { name: 'CadastralNumber' },
    ]
});