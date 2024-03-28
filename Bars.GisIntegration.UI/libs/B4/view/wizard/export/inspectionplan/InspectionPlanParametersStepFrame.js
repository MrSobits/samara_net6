Ext.define('B4.view.wizard.export.inspectionplan.InspectionPlanParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.store.integrations.inspection.Plan',
        'B4.model.integrations.inspection.Plan'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'planesSelect',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Планы проверок',
        store: 'B4.store.integrations.inspection.Plan',
        model: 'B4.model.integrations.inspection.Plan',
        columns: [
            {
                text: 'Наименование',
                dataIndex: 'Name',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата утверждения плана',
                dataIndex: 'DateApproval',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата утверждения',
                dataIndex: 'DateApproval',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата начала действия',
                dataIndex: 'DateStart',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата окончания действия',
                dataIndex: 'DateEnd',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#planesSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#planesSelect');
        
        sf.on('change', function (field, newValue, oldValue, eOpts) {
            me.fireEvent('selectionchange', me);
        }, me);

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#planesSelect').getValue()
        };
    }
});