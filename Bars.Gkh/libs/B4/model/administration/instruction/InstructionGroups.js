Ext.define('B4.model.administration.instruction.InstructionGroups', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InstructionGroup'
    },
    fields: [
        { name: 'Id', useNull: true},
        { name: 'DisplayName' },
        { name: 'Role' }
    ]
});