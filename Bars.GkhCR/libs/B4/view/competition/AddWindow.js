Ext.define('B4.view.competition.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.competitionaddwin',
    title: 'Конкурс',
    closeAction: 'hide',
    trackResetOnLoad: true,
    itemId: 'competitionaddwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'NotifNumber',
                    fieldLabel: 'Номер извещения',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'NotifDate',
                    fieldLabel: 'Дата извещения',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});