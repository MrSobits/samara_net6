Ext.define('B4.view.report.TransferFundReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.transferfundreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.ManagingOrganization'
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
                    xtype: 'b4selectfield',
                    name: 'ManagingOrganization',
                    textProperty: 'ContragentName',
                    fieldLabel: 'Управляющая организация',
                    store: 'B4.store.ManagingOrganization',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'RealObjs',
                    fieldLabel: 'Жилые дома',
                    emptyText: 'Выберите дом',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});