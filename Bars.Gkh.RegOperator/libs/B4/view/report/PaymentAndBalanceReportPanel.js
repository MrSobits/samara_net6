Ext.define('B4.view.report.PaymentAndBalanceReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.chargeReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.MunicipalityTree'
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
                    name: 'startDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Дата с',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'endDate',
                    itemId: 'dfEndDate',
                    fieldLabel: 'Дата по',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'fiasMunicipalitiesTrigerField',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выбор муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Address',
                    itemId: 'sfAddress',
                    store: 'B4.store.RealtyObjectByMo',
                    fieldLabel: 'Адрес',
                    editable: false,
                    allowBlank: false,
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});