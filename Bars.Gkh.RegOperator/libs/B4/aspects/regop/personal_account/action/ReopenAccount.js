Ext.define('B4.aspects.regop.personal_account.action.ReopenAccount', {
    extend: 'B4.base.Aspect',

    alias: 'widget.reopenaccountaspect',

    requires: [
        'B4.mixins.MaskBody'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    accountOperationCode: 'ReOpenAccountOperation',
    reopenWindowSelector: 'reopenaccountoperationwin',

    init: function (controller) {
        var me = this;

        me.callParent(arguments);
    },

    showReOpenAccountWin: function (recs) {
        var me = this,
            win = me.getForm();

        win.accIds = Ext.Array.map(recs, function (el) { return el.get('Id'); });
        win.show();
        win.down('form').getForm().isValid();
    },

    closeWindow: function () {
        var me = this,
        win = me.getForm();
        win.close();
    },

    onApplyReopen: function () {
        var me = this,
            win = me.getForm(),
            form = win.down('form').getForm();

        if (!form.isValid()) {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
            return;
        }

        me.mask('Открытие счета...');

        form.submit({
            url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            params: {
                operationCode: me.accountOperationCode,
                accids: win.accIds
            },
            success: function (f, action) {
                var resp = Ext.decode(action.response.responseText);
                Ext.Msg.alert('Результат', resp.message || 'Выполнено успешно');
                me.controller.getMainView().getStore().load();
                me.unmask();
            },
            failure: function (_, response) {
                var resp = Ext.decode(response.response.responseText);

                Ext.Msg.alert('Ошибка', resp.message);

                me.unmask();
            }
        });
    },

    getForm: function () {
        var me = this,
        win = me.componentQuery('[name=' + me.reopenWindowSelector + ']');

        if (!win) {
            win = Ext.create('Ext.window.Window', {
                modal: true,
                bodyPadding: 5,
                bodyStyle: Gkh.bodyStyle,
                title: 'Повторное открытие счета',
                closeAction: 'destroy',
                name: me.reopenWindowSelector,
                layout: 'fit',
                callback: null,
                items: [
                    {
                        xtype: 'form',
                        unstyled: true,
                        border: false,
                        layout: { type: 'vbox', align: 'stretch' },
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                            {
                                xtype: 'textfield',
                                name: 'Reason',
                                fieldLabel: 'Причина',
                                allowBlank: true,
                                margin: '2px'
                            },
                            {
                                xtype: 'b4filefield',
                                name: 'Document',
                                fieldLabel: 'Документ-основание',
                                margin: '2px'
                            },
                            {
                                xtype: 'checkbox',
                                name: 'OpenBeforeClose',
                                fieldLabel: 'Открыть ранее даты закрытия',
                                margin: '2px'
                            },
                            {
                                xtype: 'datefield',
                                fieldLabel: 'Дата повторного открытия',
                                name: 'openDate',
                                allowBlank: false,
                                margin: '2px',
                                maxValue: new Date()
                            }
                        ]
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        docked: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4savebutton',
                                        text: 'Подтвердить'
                                    }
                                ]
                            },
                            '->',
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        text: 'Отменить',
                                        handler: function (b) {
                                            b.up('window').close();
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

            win.down('b4savebutton').on('click', me.onApplyReopen, me);
            win.down('b4savebutton').on('click', me.closeWindow, me);
        }

        return win;
    }
});