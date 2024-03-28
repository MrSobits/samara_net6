Ext.define('B4.model.disposal.InspFoundationCheck', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalInspFoundationCheck'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NormDocId', defaultValue: null },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: null }
    ]
});