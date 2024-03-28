Ext.define('B4.model.administration.instruction.Instructions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Instruction'
    },
    fields: [
        { name: 'Id', useNull: true},
        { name: 'DisplayName' },
        { name: 'Code' },
        { name: 'File' },
        { name: 'InstructionGroup', defaultValue: null }
    ]
});