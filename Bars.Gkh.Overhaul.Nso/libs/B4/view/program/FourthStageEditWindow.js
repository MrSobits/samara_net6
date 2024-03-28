Ext.define('B4.view.program.FourthStageEditWindow', {
    extend: 'Ext.window.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    alias: 'widget.programfourthstageeditwin',
    title: 'Изменение номера',
    modal: true,
    closeAction: 'hide',
    width: 395,
    bodyPadding: 5,
    layout: {
        align: 'stretch',
        type: 'hbox'
    },
    
    closable: false,
    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                xtype: 'numberfield',
                labelAlign: 'right',
                allowDecimals: false,
                hideTrigger: true
            },
            items: [
                {
                    name: 'CurrentIndex',
                    fieldLabel: 'Переместить запись с номера',
                    readOnly: true,
                    labelWidth: 180,
                    width: 240
                },
                {
                    name: 'DestIndex',
                    fieldLabel: 'на номер',
                    labelWidth: 70,
                    width: 130,
                    maxValue: 2147483646,
                    minValue: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Переместить',
                                    listeners: {
                                        mouseover: function (btn) {
                                            //небольшая пасхалка
                                            if (Math.random() < 0.02) {
                                                btn.setTooltip('Переместить именем Императора!');
                                            } else {
                                                btn.setTooltip('Переместить');
                                            }
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});