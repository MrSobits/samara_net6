Ext.define('B4.model.controlorg.ControlOrganizationControlTypeRelation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlOrganizationControlTypeRelation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlOrganization', defaultValue: null },
        { name: 'ControlType', defaultValue: null }
    ]
});