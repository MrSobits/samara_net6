Ext.define('B4.view.BilConnectionEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.bilconnectioneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    title: 'Настройки подколючения',
    closeAction: 'hide',

    requires: [
        'B4.model.BilConnection',         
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Connection',
                    fieldLabel: 'Подключение',
                    allowBlank: false,
                    maxLength: 255
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});