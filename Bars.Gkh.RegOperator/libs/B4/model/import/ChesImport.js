Ext.define('B4.model.import.ChesImport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Date' },
        { name: 'Period' },
        { name: 'Task' },
        { name: 'State' },
        { name: 'User' },
        { name: 'AnalysisState' },
        { name: 'LoadedFiles' }
    ]
});