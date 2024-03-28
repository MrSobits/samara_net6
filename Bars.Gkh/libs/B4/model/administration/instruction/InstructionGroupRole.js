Ext.define('B4.model.administration.instruction.InstructionGroupRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InstructionGroupRole'
    },
    fields: [
        { name: 'Id', useNull: true},
        { name: 'InstructionGroup' },
        { name: 'Role' }
    ]
});