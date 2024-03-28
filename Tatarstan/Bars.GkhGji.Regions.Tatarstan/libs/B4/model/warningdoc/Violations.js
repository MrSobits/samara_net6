Ext.define('B4.model.warningdoc.Violations', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningDocViolations'
    },
    fields: [
        { name: 'Id' },
        { name: 'WarningDoc' },
        { name: 'RealityObject' },
        { name: 'NormativeDoc' },
        { name: 'Description' },
        { name: 'Violations' },
        { name: 'TakenMeasures' },
        { name: 'DateSolution' }
    ]
});