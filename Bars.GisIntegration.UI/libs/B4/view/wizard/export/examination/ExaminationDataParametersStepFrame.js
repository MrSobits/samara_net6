Ext.define('B4.view.wizard.export.examination.ExaminationDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.inspection.Examination',
        'B4.model.integrations.inspection.Examination'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'Examinations',
        flex: 1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Number',
        selectionMode: 'MULTI',
        fieldLabel: 'Проверки',
        store: 'B4.store.integrations.inspection.Examination',
        model: 'B4.model.integrations.inspection.Examination',
        columns: [
            {
                text: 'Основание проверки',
                dataIndex: 'Base',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Номер проверки',
                dataIndex: 'Number',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата начала обследования',
                dataIndex: 'DateStart',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.gte
                }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата окончания обследования',
                dataIndex: 'DateEnd',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.lte
                }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'KindCheck',
                flex: 1,
                text: 'Вид проверки',
                filter: {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    url: '/KindCheckGji/List',
                    editable: false,
                    storeAutoLoad: false,
                    emptyItem: { Name: '-' },
                    valueField: 'Name'
                }
            },
            {
                text: 'Юридическое лицо',
                dataIndex: 'ContragentName',
                flex: 1,
                filter: { xtype: 'textfield' }
            }
        ],
        updateDisplayedText: function (data) {
            var me = this,
                text;

            if (Ext.isString(data)) {
                text = data;
            }
            else {
                text = data && data[me.textProperty] ? data[me.textProperty] : '';
                if (Ext.isEmpty(text) && Ext.isArray(data)) {
                    text = Ext.Array.map(data, function(record) {
                        var recordText = record[me.textProperty];
                        return (Ext.isEmpty(recordText) ? '-' : recordText);
                    }).join();
                }
            }

            me.setRawValue.call(me, text);
        }
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#Examinations').setDisabled(multiSuppliers);
    },

    firstInit: function() {
        var me = this,
            sf = me.wizard.down('#Examinations');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: me.wizard.down('#Examinations').getValue()
        };
    }
});