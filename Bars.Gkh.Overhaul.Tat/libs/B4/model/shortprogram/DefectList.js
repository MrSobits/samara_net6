Ext.define('B4.model.shortprogram.DefectList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramDefectList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ShortObject' },
        { name: 'Work' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'Sum' },
        { name: 'Volume' },
        { name: 'CostPerUnitVolume' },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});