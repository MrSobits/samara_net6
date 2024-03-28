Ext.define('B4.view.report.RaionChargeReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.raionChargeReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.municipality.MoArea',
        'B4.store.MunicipalityByParent',
        'B4.store.regop.ClosedChargePeriod'
    ],

    initComponent: function() {
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
                    name: 'ParentMu',
                    fieldLabel: 'Муниципальный район',
                    allowBlank:false,
                    emptyText: 'Выберите муниципальный район',
                    store: 'B4.store.dict.municipality.MoArea',
                    editable: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    emptyText: 'Все МО',
                    store: 'B4.store.MunicipalityByParent',
                    selectionMode: 'MULTI',
                    editable: false,
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    textProperty: 'Name',
                    allowBlank: false,
                    editable: false,
                    name: 'ChargePeriod',
                    windowCfg: {
                        modal: true
                    },
                    trigger2Cls: '',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Дата открытия',
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            dataIndex: 'StartDate',
                            flex: 1,
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Дата закрытия',
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            dataIndex: 'EndDate',
                            flex: 1,
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Состояние',
                            dataIndex: 'IsClosed',
                            flex: 1,
                            renderer: function (value) {
                                return value ? 'Закрыт' : 'Открыт';
                            }
                        }
                    ],
                    fieldLabel: 'Период'
                }
            ]
        });

        me.callParent(arguments);
    }
});