Ext.define('B4.model.RosRegExtractReg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractReg'
    },
    fields: [
        { name: 'Id' },
        { name: 'Reg_ID_Record' },
        { name: 'Reg_RegNumber' },
        { name: 'Reg_Type' },
        { name: 'Reg_Name' },
        { name: 'Reg_RegDate' },
        { name: 'Reg_ShareText' }
    ]
});