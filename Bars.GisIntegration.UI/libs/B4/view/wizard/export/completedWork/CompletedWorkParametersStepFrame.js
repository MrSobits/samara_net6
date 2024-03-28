Ext.define('B4.view.wizard.export.workingplan.CompletedWorkParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.store.integrations.services.CompletedWork',
        'B4.model.integrations.services.CompletedWork'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'CompletedWorks',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Программы текущего ремонта',
        store: 'B4.store.integrations.services.CompletedWork',
        model: 'B4.model.integrations.services.CompletedWork',
        columns: [
            {
                text: 'Программа текущего ремонта',
                dataIndex: 'Address',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Дата акта',
                dataIndex: 'ActDate',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                dataIndex: 'ActNumber',
                flex: 1,
                text: 'Номер акта',
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#CompletedWorks').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#CompletedWorks');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedRepairObjects: multiSuppliers ? 'ALL' : me.wizard.down('#CompletedWorks').getValue()
        };
    }
});