Ext.define('B4.model.Builder', {
    extend: 'B4.base.Model',
    requires: [
            'B4.enums.GroundsTermination',
            'B4.enums.YesNoNotSet'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Builder'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentId' },
        { name: 'ContragentName' },
        { name: 'ContragentPhone'},
        { name: 'ConsentInfo', defaultValue: 30 },
        { name: 'WorkWithoutContractor', defaultValue: 30 },
        { name: 'AdvancedTechnologies', defaultValue: 30 },
        { name: 'Description' },
        { name: 'Rating' },
        { name: 'TaxInfoPhone' },
        { name: 'TaxInfoAddress' },
        { name: 'ActivityDateStart' },
        { name: 'ActivityDateEnd' },
        { name: 'ActivityDescription' },
        { name: 'ActivityGroundsTermination', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'File', defaultValue: null },
        { name: 'FileLearningPlan', defaultValue: null },
        { name: 'FileManningShedulle', defaultValue: null }
    ]
});