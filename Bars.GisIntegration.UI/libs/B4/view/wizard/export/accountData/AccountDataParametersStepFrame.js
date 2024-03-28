Ext.define('B4.view.wizard.export.accountData.AccountDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.houseManagement.House',
        'B4.model.integrations.houseManagement.House'
    ],
    layout: 'anchor',
    defaults: {
        anchor: '100%',
        margin: 5,
        labelWidth: 150
    },
    items: [
        {
            xtype: 'b4selectfield',
            name: 'House',
            flex: 1,
            modalWindow: true,
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
                    text: 'Тип дома',
                    dataIndex: 'HouseType',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ]
        }
    ],

    init: function() {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('b4selectfield[name=House]').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('b4selectfield[name=House]');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        return {
            houseList: multiSuppliers ? 'ALL' : me.wizard.down('b4selectfield[name=House]').getValue()
        };
    }
});