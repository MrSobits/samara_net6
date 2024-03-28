Ext.define('B4.model.disposal.DisposalVerificationSubjectLicensing', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalVerificationSubjectLicensing'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null },
        { name: 'Active', defaultValue: null }
    ]
});