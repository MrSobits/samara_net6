Ext.define('B4.model.disposal.FormatDataExport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    fields: [
        { name: 'Id' },
        { name: 'DisposalId' },
        { name: 'ActCheckId' },
        { name: 'TypeBase' },
        { name: 'IsPlanned' },
        { name: 'CheckDate' },
        { name: 'ContragentName' },
        { name: 'MunicipalityName' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'Disposal',
        listAction: 'ListForExport'
    }
});