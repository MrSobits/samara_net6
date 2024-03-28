Ext.define('B4.model.competition.LotBid', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CompetitionLotBid'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Lot', defaultValue: null },
        { name: 'Builder', defaultValue: null },
        { name: 'IncomeDate' },
        { name: 'Points' },
        { name: 'Price' },
        { name: 'PriceNds' },
        { name: 'IsWinner', defaultValue: false }
    ]
});