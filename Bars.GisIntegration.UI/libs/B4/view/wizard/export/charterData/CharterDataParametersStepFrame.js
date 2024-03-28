Ext.define('B4.view.wizard.export.charterData.CharterDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    layout: 'hbox',
    mixins: ['B4.mixins.window.ModalMask'],
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'chartersSelect',
        name: 'CharterParameter',
        flex: 1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'DocNum',
        selectionMode: 'MULTI',
        fieldLabel: 'Уставы',
        store: 'B4.store.integrations.houseManagement.Charter',
        model: 'B4.model.integrations.houseManagement.Charter',
        columns: [
            {
                text: 'Номер',
                dataIndex: 'DocNum',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата заключения',
                dataIndex: 'SigningDate',
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

        me.wizard.down('#chartersSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#chartersSelect');
        
        sf.on('change', function () {
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
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#chartersSelect').getValue()
        };
    }
});