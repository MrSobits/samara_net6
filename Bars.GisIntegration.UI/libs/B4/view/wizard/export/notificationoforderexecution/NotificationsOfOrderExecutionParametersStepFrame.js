Ext.define('B4.view.wizard.export.notificationoforderexecution.NotificationsOfOrderExecutionParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.form.MonthPicker',
        'B4.store.integrations.houseManagement.House',
        'B4.model.integrations.houseManagement.House'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    defaults: {
        margin: 5,
        padding: '10 5 0 10',
        labelWidth: 120,
        allowBlank: false,
        labelAlign: 'right',
        editable: false
    },
    items: [
        {
            xtype: 'b4monthpicker',
            name: 'ReportingPeriod',
            itemId: 'reportingPeriod',
            fieldLabel: 'Отчётный период',
            format: 'm-Y'
        },
        {
            xtype: 'b4selectfield',
            itemId: 'housesSelect',
            labelAlign: 'right',
            textProperty: 'Address',
            selectionMode: 'MULTI',
            fieldLabel: 'Дома',
            width: 300,
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
        }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#housesSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#housesSelect');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#housesSelect').getValue(),
            reportingPeriod: me.wizard.down('#reportingPeriod').getValue()
        };
    }
});