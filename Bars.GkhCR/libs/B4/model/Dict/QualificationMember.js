Ext.define('B4.model.dict.QualificationMember', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualificationMember'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Period', defaultValue: true },
        { name: 'PeriodName' },
        { name: 'IsPrimary', defaultValue: false },
        { name: 'Role' },
        { name: 'RoleName' }
    ]
});