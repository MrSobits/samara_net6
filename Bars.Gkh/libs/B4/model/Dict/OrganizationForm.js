Ext.define('B4.model.dict.OrganizationForm', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'organizationForm'
    },
    fields: [
        { name: 'Id', useNull: true },
        {
            name: 'Name'
        },
        {
            name: 'Code'
        },
        {
            name: 'OkopfCode'
        }
    ]
});