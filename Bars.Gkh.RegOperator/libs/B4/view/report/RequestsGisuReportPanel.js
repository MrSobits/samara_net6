Ext.define('B4.view.report.RequestsGisuReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.requestsGisuReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.store.RealtyObjectByMo',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.FinanceSource',
        'B4.store.dict.ProgramCr'
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
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',

                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'FinSource',
                    fieldLabel: 'Разрез финансирования',
                    store: 'B4.store.dict.FinanceSource',
                    itemId: 'sfFinanceSource',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Address',
                    itemId: 'tfAddress',
                    fieldLabel: 'Адреса',
                    emptyText: 'Все адреса'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ManagingOrganizations',
                    itemId: 'tfManagingOrganization',
                    fieldLabel: 'Управляющие организации',
                    emptyText: 'Все УО'
                },
                {
                    xtype: 'datefield',
                    name: 'startDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});