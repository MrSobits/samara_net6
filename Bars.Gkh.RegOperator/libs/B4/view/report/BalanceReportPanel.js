Ext.define('B4.view.report.BalanceReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.balanceReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Municipality',
        'B4.store.dict.municipality.MoArea',
        'B4.store.MunicipalityByParent',
        'B4.form.SelectField'
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
                    name: 'MunicipalitiesR',
                    itemId: 'municipalityR',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.municipality.MoArea',
                    editable: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'MunicipalitiesO',
                    itemId: 'municipalityO',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.MunicipalityByParent',
                    editable: false,
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    itemId: 'tfRealityObjects',
                    fieldLabel: 'Адрес',
                    emptyText: 'Все'
                },
                {
                    xtype: 'datefield',
                    name: 'startDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Период с',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'endDate',
                    itemId: 'dfEndDate',
                    fieldLabel: 'Период по',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});