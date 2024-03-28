Ext.define('B4.model.activitytsj.MemberTsj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjMember'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Year' },
        { name: 'ActivityTsj', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'State', defaultValue: null }
    ]
});