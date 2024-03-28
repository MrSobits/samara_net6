Ext.define('B4.view.report.HeatSeasonReadinessReport', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'HeatSeasonReadinessReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.HeatSeasonPeriodGji',
        'B4.view.dict.heatseasonperiodgji.Grid'
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
                    name: 'HeatSeasonPeriodGji',
                    itemId: 'sfHeatSeasonPeriodGji',
                    fieldLabel: 'Отопительный сезон',
                    store: 'B4.store.dict.HeatSeasonPeriodGji',
                   

                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false,
                    value: new Date()
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'HeatTypes',
                    itemId: 'cbHeatType',
                    fieldLabel: 'Типы отопления',
                    emptyText: 'Не задано'
                },
                {
                    xtype: 'b4combobox',
                    name: 'RealtyObjectType',
                    itemId: 'cbRoType',
                    fieldLabel: 'Тип дома',
                    editable: false,
                    items: [[30, 'Многоквартирный'], [40, 'Общежитие'], [50, 'Многоквартирный и общежитие']],
                    value: 50
                }
            ]
        });

        me.callParent(arguments);
    }
});