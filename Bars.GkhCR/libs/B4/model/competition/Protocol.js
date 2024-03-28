Ext.define('B4.model.competition.Protocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CompetitionProtocol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Competition', defaultValue: null },
        { name: 'TypeProtocol', defaultValue: 10 },
        { name: 'File', defaultValue: null },
        { name: 'SignDate' },
        { name: 'ExecDate' },
        { name: 'ExecTime' },
        { name: 'Note' },
        { name: 'IsCancelled' }
    ]
});