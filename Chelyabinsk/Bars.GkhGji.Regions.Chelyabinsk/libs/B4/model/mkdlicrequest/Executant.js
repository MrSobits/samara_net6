Ext.define('B4.model.mkdlicrequest.Executant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestExecutant'
    },
    fields: [
        { name: 'MKDLicRequest', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Author', defaultValue: null },
        { name: 'Controller', defaultValue: null },
        { name: 'OrderDate' },
        { name: 'PerformanceDate' },
        { name: 'ExecutantZji' },
        { name: 'IsResponsible', defaultValue: false },
        { name: 'OnApproval', defaultValue: false },
        { name: 'Description' },
        { name: 'State', defaultValue: null },
        { name: 'Resolution', defaultValue: null }
    ]
});