Ext.define('B4.model.publicservorg.contractpart.BudgetOrgContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BudgetOrgContract',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null },
        { name: 'Organization', defaultValue: null },
        { name: 'TypeCustomer', defaultValue: null }
    ]
});