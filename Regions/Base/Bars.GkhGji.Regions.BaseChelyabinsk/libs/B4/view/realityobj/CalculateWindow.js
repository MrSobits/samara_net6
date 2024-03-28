Ext.define('B4.view.realityobj.CalculateWindow', {

    extend: 'B4.form.Window',

    modal: true,
    closable: true,
    width: 340,
    height: 150,
   // minHeight: 300,
    title: 'Параметры расчета целесообразности',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    alias: 'widget.calculatefesabilitywindow',

    requires: [
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
            {
                items: [
                    {
                        xtype: 'container',
                        layout: 'vbox',
                        padding: '10 10 10 10',
                        defaults: {
                            labelWidth: 120,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'numberfield',
                                name: 'YearStart',
                                fieldLabel: 'Рассчитать с года',
                                itemId: 'yearStart',
                                flex: 1,
                                minValue: 2010,
                                maxValue: 2100

                            },
                            {
                                xtype: 'numberfield',
                                name: 'YearEnd',
                                fieldLabel: 'По год',
                                itemId: 'yearEnd',
                                flex: 1,
                                minValue: 2010,
                                maxValue: 2100

                            }

                        ]
                    }
                                                        
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'bottom',
                        itemId: 'toptoolbar',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'button',
                                text: 'Расчет',
                                action: 'ExecuteCalculationCalcFesabiliti',
                                iconCls: 'icon-cog-go'
                            }
                        ],
                    },
                ]
            }
        );

        me.callParent(arguments);
    }


    
});