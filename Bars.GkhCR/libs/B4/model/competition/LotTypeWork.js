Ext.define('B4.model.competition.LotTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CompetitionLotTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Lot', defaultValue: null },
        { name: 'TypeWork', defaultValue: null },
        { name: 'RoMunicipality', defaultValue: null },
        { name: 'RoAddress', defaultValue: null },
        { name: 'ProgrammName', defaultValue: null }
    ]
});