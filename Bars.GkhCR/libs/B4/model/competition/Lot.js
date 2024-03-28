Ext.define('B4.model.competition.Lot', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CompetitionLot'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Competition', defaultValue: null },
        { name: 'LotNumber', defaultValue: null },
        { name: 'StartingPrice', defaultValue: null },
        { name: 'Subject', defaultValue: null },
        { name: 'ContractNumber', defaultValue: null },
        { name: 'ContractDate', defaultValue: null },
        { name: 'ContractFactPrice', defaultValue: null },
        { name: 'ContractFile', defaultValue: null },
        { name: 'ObjectsCount' },
        { name: 'BidCount' },
        { name: 'Winner' },
        { name: 'WinnerId' }
    ]
});