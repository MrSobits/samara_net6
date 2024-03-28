Ext.define('B4.view.report.PercentCalculationPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'percentCalculationReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.PeriodDi',
        'B4.view.dict.perioddi.Grid'
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
                    name: 'PeriodDi',
                    itemId: 'sfPeriodDi',
                    fieldLabel: 'Период',
                    store: 'B4.store.dict.PeriodDi',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    listeners: {
                        change: function (obj, newValue) {
                            var panel = obj.up('#percentCalculationReportPanel');
                            if (panel) {
                                var dateField = panel.down('#dfDate');
                                if (newValue) {
                                    dateField.setDisabled(false);
                                    dateField.setMinValue(Ext.Date.parse(newValue.DateStart, "Y-m-dTH:i:s"));
                                    dateField.setMaxValue(Ext.Date.parse(newValue.DateEnd, "Y-m-dTH:i:s"));
                                } else {
                                    dateField.setDisabled(true);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования'
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    itemId: 'dfDate',
                    disabled: true,
                    fieldLabel: 'Дата отчета'
                },
                {
                    xtype: 'b4combobox',
                    name: 'TranssferredManag',
                    itemId: 'cbTranssferredManag',
                    fieldLabel: 'Передано управление',
                    editable: false,
                    items: [[null, ' '], [true, 'Учитывать'], [false, 'Не учитывать']],
                    defaultValue: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});