Ext.define('B4.view.wizard.export.notificationsoforderexecutioncancellation.NotificationsOfOrderExecutionCancellationDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.form.MonthPicker',
        'B4.model.integrations.payment.NotificationOfOrderExecution',
        'B4.store.integrations.payment.NotificationOfOrderExecution'
    ],
    layout: 'hbox',
    mixins: ['B4.mixins.window.ModalMask'],
    items: [{
        xtype: 'b4selectfield',
        itemId: 'selectedData',
        name: 'selectedData',
        flex: 1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'OrderNum',
        selectionMode: 'MULTI',
        fieldLabel: 'Уведомления о выполнении распоряжения',
        store: 'B4.store.integrations.payment.NotificationOfOrderExecution',
        model: 'B4.model.integrations.payment.NotificationOfOrderExecution',
        windowCfg: {
            width: 900
        },
        columns: [
            {
                text: 'Период (месяц, год)',
                dataIndex: 'OrderDate',
                flex: 1,
                filter: { xtype: 'b4monthpicker' },
                renderer: Ext.util.Format.dateRenderer('m-Y')
            },
            {
                text: 'Адрес',
                dataIndex: 'Жилой дом',
                flex: 2,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Номер лицевого счета',
                dataIndex: 'AccountNumber',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Номер распоряжения',
                dataIndex: 'OrderNum',
                flex: 1,
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('b4selectfield[name=selectedData]').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('b4selectfield[name=selectedData]');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedData: multiSuppliers ? 'ALL' : me.wizard.down('b4selectfield[name=selectedData]').getValue()
        };
    }
});