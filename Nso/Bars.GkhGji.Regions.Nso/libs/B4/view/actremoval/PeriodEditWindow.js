Ext.define('B4.view.actremoval.PeriodEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 250,
    bodyPadding: 5,
    itemId: 'actRemovalPeriodEditWindow',
    title: 'Форма даты и времени проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateCheck',
                    fieldLabel: 'Дата проверки',
                    format: 'd.m.Y'
                },
                {
                    fieldLabel: 'Время начала',
                    name: 'TimeStart',
                    xtype: 'timefield',
                    format: 'H:i',
                    submitFormat: 'Y-m-d H:i:s',
                    minValue: '8:00',
                    maxValue: '22:00'
                },
                {
                    fieldLabel: 'Время окончания',
                    name: 'TimeEnd',
                    xtype: 'timefield',
                    format: 'H:i',
                    submitFormat: 'Y-m-d H:i:s',
                    minValue: '8:00',
                    maxValue: '22:00'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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