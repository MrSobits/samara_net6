Ext.define('B4.view.report.FinanceModelDpkrPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportFinanceModelDpkrPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.MunicipalityByOperator'
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
                    fieldLabel: 'Муниципальное образование',
                    name: 'Municipality',
                    store: 'B4.store.dict.MunicipalityByOperator',
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    minValue: 2,
                    maxValue: 30,
                    name: 'Period',
                    fieldLabel: 'Период группировки',
                    value: 6
                }
            ]
        });

        me.callParent(arguments);
    }
});