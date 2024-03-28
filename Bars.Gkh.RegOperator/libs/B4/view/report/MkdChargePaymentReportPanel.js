Ext.define('B4.view.report.MkdChargePaymentReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.mkdchargepaymentreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.enums.MethodFormFundCr',
        'B4.view.Control.GkhTriggerField',
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    listeners: {
                        render: function(datefield) {
                            datefield.setValue(new Date(2014, 5, 1));
                        }
                    },
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Окончание периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    name: 'Fund',
                    fieldLabel: 'Способ формирования фонда КР',
                    emptyText: 'Не задано',
                    editable: false,
                    displayField: 'Display',
                    store: B4.enums.MethodFormFundCr.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ParentMu',
                    fieldLabel: 'Муниципальные районы',
                    emptyText: 'Все',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Наличие в ДПКР',
                    store: B4.enums.YesNoNotSet.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'HasInDpkr',
                    emptyText: 'Не задано'
                }
            ]
        });

        me.callParent(arguments);
    }
});