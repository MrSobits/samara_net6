Ext.define('B4.view.wizard.export.acknowledgmentexporter.AcknowledgmentDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.form.MonthPicker',
        'B4.model.integrations.bills.Acknowledgment',
        'B4.store.integrations.bills.Acknowledgment'
    ],
    layout: 'hbox',
    items: [
        {
            xtype: 'b4selectfield',
            itemId: 'selectedOrders',
            padding: '10 5 0 10',
            labelWidth: 130,
            labelAlign: 'right',
            flex: 1,
            textProperty: 'OrderNum',
            selectionMode: 'MULTI',
            fieldLabel: 'Запросы на проведение квитирования',
            store: 'B4.store.integrations.bills.Acknowledgment',
            model: 'B4.model.integrations.bills.Acknowledgment',
            columns: [
                {
                    text: 'Период (месяц, год)',
                    dataIndex: 'Period',
                    flex: 1,
                    filter: { xtype: 'b4monthpicker' },
                    renderer: Ext.util.Format.dateRenderer('m-Y')
                },
                {
                    text: 'Жилой дом',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер платежного документа',
                    dataIndex: 'PaymentDocumentNumber',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер извещения о принятии к исполнению распоряжения',
                    dataIndex: 'OrderNum',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#selectedOrders').setDisabled(multiSuppliers);
    },
    
    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#selectedOrders');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function() {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#selectedOrders').getValue()
        };
    }
});