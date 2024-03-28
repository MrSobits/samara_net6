Ext.define('B4.model.manorg.contract.ManOrgContractService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'Contract' },
        { name: 'Service' },
        { name: 'Name' },
        { name: 'StartDate' },
        { name: 'EndDate' }
    ]
});