Ext.define('B4.model.realityobj.DirectManagContract', {
    extend: 'B4.model.manorg.contract.Base',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectDirectManagContract'
    },
    fields: [
        { name: 'IsServiceContract', defaultValue: false },
        { name: 'DateStartService' },
        { name: 'DateEndService' },
        { name: 'ServContractFile' },
        { name: 'ManagingOrganization' }
    ]
});