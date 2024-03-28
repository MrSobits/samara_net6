Ext.define('B4.model.dict.ConfigurationReferenceInformationKndTor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConfigurationReferenceInformationKndTor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TorId' },
        { name: 'Value' },
        { name: 'Type' }
    ]
});