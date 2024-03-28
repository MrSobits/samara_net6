Ext.define('B4.view.wizard.export.workingplan.WorkingPlanDataParametersStepFrame', {
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
        itemId: 'RepairPrograms',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Программы текущего ремонта',
        store: 'B4.store.integrations.services.RepairProgram',
        model: 'B4.model.integrations.services.RepairProgram',
        columns: [
            {
                text: 'Программа текущего ремонта',
                dataIndex: 'Name',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Период',
                dataIndex: 'Period',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'b4enumcolumn',
                dataIndex: 'State',
                flex: 1,
                text: 'Состояние',
                enumName: 'B4.enums.TypeProgramRepairState',
                filter: true
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#RepairPrograms').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#RepairPrograms');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedRepairObjects: multiSuppliers ? 'ALL' : me.wizard.down('#RepairPrograms').getValue()
        };
    }
});