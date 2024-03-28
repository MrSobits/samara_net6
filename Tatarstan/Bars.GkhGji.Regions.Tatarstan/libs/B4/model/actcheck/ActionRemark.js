Ext.define('B4.model.actcheck.ActionRemark', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckActionRemark'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckAction' },
        { name: 'Remark' },
        { name: 'MemberFio' }
    ]
});