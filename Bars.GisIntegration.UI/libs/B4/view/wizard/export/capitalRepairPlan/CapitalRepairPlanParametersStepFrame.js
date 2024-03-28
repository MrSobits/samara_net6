Ext.define('B4.view.wizard.export.capitalRepairPlan.CapitalRepairPlanParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.store.integrations.capitalrepair.Municipality',
        'B4.model.integrations.capitalrepair.Municipality'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'Municipalities',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Муниципальные образования',
        store: 'B4.store.integrations.capitalrepair.Municipality',
        model: 'B4.model.integrations.capitalrepair.Municipality',
        columns: [
            {
                text: 'Наименование',
                dataIndex: 'Name',
                flex: 1,
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#Municipalities').setDisabled(multiSuppliers);
    },

    firstInit: function() {
        var me = this,
            sf = me.wizard.down('#Municipalities');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            SelectedMunicipalityIds: multiSuppliers ? 'ALL' : me.wizard.down('#Municipalities').getValue()
        };
    }
});