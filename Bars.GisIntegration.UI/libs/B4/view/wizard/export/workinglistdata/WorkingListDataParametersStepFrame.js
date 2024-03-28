Ext.define('B4.view.wizard.export.workinglistdata.WorkingListDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.store.integrations.services.RepairObject',
        'B4.model.integrations.services.RepairObject'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'RepairObjects',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Address',
        selectionMode: 'MULTI',
        fieldLabel: 'Объекты текущего ремонта',
        store: 'B4.store.integrations.services.RepairObject',
        model: 'B4.model.integrations.services.RepairObject',
        columns: [
            {
                text: 'Программа текущего ремонта',
                dataIndex: 'RepairProgramName',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Период',
                dataIndex: 'RepairProgramPeriod',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'b4enumcolumn',
                dataIndex: 'RepairProgramState',
                flex: 1,
                text: 'Состояние',
                enumName: 'B4.enums.TypeProgramRepairState',
                filter: true
            },
            {
                text: 'Адрес',
                dataIndex: 'Address',
                flex: 1,
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#RepairObjects').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#RepairObjects');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedRepairObjects: multiSuppliers ? 'ALL' : me.wizard.down('#RepairObjects').getValue()
        };
    }
});