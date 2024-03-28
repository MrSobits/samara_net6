Ext.define('B4.view.gasuindicator.ValuePanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.gasuindicatorvaluepanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.view.gasuindicator.ValueGrid'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    layout: {
        type: 'border',
        padding: 3
    },
    defaults: {
        split: true
    },
    title: 'Сведения для отправки в ГАСУ',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name:'Month',
                                    operand: CondExpr.operands.eq,
                                    labelWidth: 110,
                                    width:200,
                                    labelAlign: 'right',
                                    storeAutoLoad: false,
                                    editable: false,
                                    fieldLabel: 'Месяц отправки',
                                    displayField: 'Month',
                                    valueField: 'Id',
                                    fields: ['Id', 'Month'],
                                    items: [[1, 'Январь'], [2, 'Февраль'], [3, 'Март'], [4, 'Апрель'], [5, 'Май'], [6, 'Июнь'], [7, 'Июль'], [8, 'Август'], [9, 'Сентябрь'], [10, 'Октябрь'], [11, 'Ноябрь'], [12, 'Декабрь']]
                                },
                                {
                                    xtype: 'component',
                                    width: 5
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'Year',
                                    labelWidth: 110,
                                    width: 200,
                                    labelAlign: 'right',
                                    operand: CondExpr.operands.eq,
                                    storeAutoLoad: false,
                                    editable: false,
                                    fieldLabel: 'Год отправки',
                                    displayField: 'Year',
                                    valueField: 'Year',
                                    fields: ['Year'],
                                    url: '/GasuIndicatorValue/GetListYears'
                                },
                                {
                                    xtype: 'component',
                                    width: 5
                                },
                                {
                                    xtype: 'button',
                                    text: 'Заполнить перечень показателей',
                                    action: 'CreateValues',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'component',
                                    width: 5
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить сведения в ГАСУ',
                                    action: 'SendService',
                                    iconCls: 'icon-table-go'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    region:'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'gasuindicatorvaluegrid',
                            flex: 1,
                            border: 0
                        }
                    ]
                }
            ]
        });
        
        me.callParent(arguments);
    }
});