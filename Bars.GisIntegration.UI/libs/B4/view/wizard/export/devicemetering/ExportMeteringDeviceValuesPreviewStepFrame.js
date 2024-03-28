Ext.define('B4.view.wizard.export.devicemetering.ExportMeteringDeviceValuesPreviewStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.enums.MeteringDeviceValueType',
        'B4.enums.MeteringDeviceType',

        'B4.form.ComboBox',
        'B4.form.SelectField',

        'B4.store.integrations.houseManagement.House',
        'B4.model.integrations.houseManagement.House'
    ],

    layout: { type: 'vbox', align: 'stretch' },
    items: [
        {
            xtype: 'container',
            layout: { type: 'vbox', align: 'stretch' },
            defaults: {
                margin: '5 5 5 5',
                padding: '10 5 0 10',
                labelWidth: 200,
                allowBlank: false,
                labelAlign: 'right',
                editable: false
            },
            items: [
                {
                    fieldLabel: 'Тип прибора учета',
                    xtype: 'b4combobox',
                    displayField: 'Display',
                    valueField: 'Value',
                    items: B4.enums.MeteringDeviceType.getItemsWithEmpty([null, 'Все']),
                    name: 'MeteringDeviceType',
                    allowBlank: true
                },
                {
                    xtype: 'b4combobox',
                    fieldLabel: 'Тип показаний ПУ',
                    name: 'MeteringDeviceValueType',
                    displayField: 'Display',
                    valueField: 'Value',
                    items: B4.enums.MeteringDeviceValueType.getItemsWithEmpty([null, ' - ']),
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'realityObjects',
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
                        }
                    ]
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#realityObjects').setDisabled(multiSuppliers);
    },

    firstInit: function() {
        var me = this,
            sf = me.wizard.down('#realityObjects');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function() {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#realityObjects').getValue(),
            meteringDeviceType: me.wizard.down('b4combobox[name=MeteringDeviceType]').getValue(),
            meteringDeviceValueType: me.wizard.down('b4combobox[name=MeteringDeviceValueType]').getValue()
        };
    }
});