Ext.define('B4.model.realityobj.MeteringDevice', {
    extend: 'B4.base.Model',
    requires: ['B4.enums.TypeAccounting'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectMeteringDevice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'MeteringDevice', defaultValue: null },
        { name: 'AccuracyClass' },
        { name: 'Description' },
        { name: 'DateRegistration' },
        { name: 'TypeAccounting', defaultValue: 10 },
        { name: 'DateInstallation' },
        { name: 'SerialNumber' },
        { name: 'AddingReadingsManually' },
        { name: 'NecessityOfVerificationWhileExpluatation' },
        { name: 'PersonalAccountNum' },
        { name: 'DateFirstVerification' }
    ]
});