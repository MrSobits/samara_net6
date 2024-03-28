Ext.define('B4.view.report.PrepareHeatSeasonPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'reportPrepareHeatSeasonPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.view.dict.heatseasonperiodgji.Grid',
        'B4.store.dict.HeatSeasonPeriodGji',
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
                    value: new Date(),
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'b4combobox',
                    name: 'DocType',
                    allowBlank: false,
                    itemId: 'cbDocType',
                    fieldLabel: 'Тип документа',
                    editable: false,
                    items: [[10, 'Акт промывки'], [20, 'Акт опрессовки']],
                    value: 10
                },
                {
                    xtype: 'b4combobox',
                    name: 'State',
                    itemId: 'cbState',
                    fieldLabel: 'Состояние документа',
                    editable: false,
                    items: [[false, 'Предъявлено'], [true, 'Принято']],
                    value: false
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