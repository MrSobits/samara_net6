Ext.define('B4.view.wizard.export.meteringdevicedata.MeteringDeviceDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.store.integrations.houseManagement.House',
        'B4.model.integrations.houseManagement.House'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'housesSelect',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Address',
        selectionMode: 'MULTI',
        fieldLabel: 'Дома',
        store: 'B4.store.integrations.houseManagement.House',
        model: 'B4.model.integrations.houseManagement.House',
        columns: [
            {
                text: 'Адрес',
                dataIndex: 'Address',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'b4enumcolumn',
                dataIndex: 'HouseType',
                flex: 1,
                text: 'Тип дома',
                enumName: 'B4.enums.HouseType',
                filter: true
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#housesSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#housesSelect');
        
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
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#housesSelect').getValue()
        };
    }
});