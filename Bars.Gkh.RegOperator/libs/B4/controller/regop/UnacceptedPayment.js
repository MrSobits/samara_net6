Ext.define('B4.controller.regop.UnacceptedPayment', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.mixins.MaskBody',
        'B4.form.Window',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.QuickMsg',
        'B4.ux.button.Close'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'unacceptedpaymentpacketgrid'
        }
    ],

    models: [
        'regop.UnacceptedPayment',
        'regop.UnacceptedPaymentPacket'
    ],

    stores: [
        'regop.UnacceptedPayment',
        'regop.UnacceptedPaymentPacket'
    ],

    views: [
        'regop.unacceptedpayment.PacketGrid',
        'regop.unacceptedpayment.PaymentGrid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.Accounts.UnacceptedPayment.Accept', applyTo: 'button[action=Accept]', selector: 'unacceptedpaymentpacketgrid' }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'unacceptedpaymentpacketgrid b4updatebutton': { 'click': { fn: me.onClickUpdate } },
            'unacceptedpaymentpacketgrid button[action=Accept]': { 'click': { fn: me.onClickAccept } },
            'unacceptedpaymentpacketgrid button[action=Cancel]': { 'click': { fn: me.onClickCancel } },
            'unacceptedpaymentpacketgrid button[action=Delete]': { 'click': { fn: me.onClickDelete } },
            'unacceptedpaymentpacketgrid': {
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                },
                'itemdblclick': {
                    fn: me.rowDblClick,
                    scope: me
                },
                'render': function(grid) {
                    grid.getStore().on('beforeload', function(store, operation) {
                        operation = operation || {};
                        operation.params = operation.params || {};
                        operation.params['showAccepted'] = grid.down('[name=ShowAccepted]').getValue();
                    });
                }
            },
            'unacceptedpaymentpacketgrid checkbox[name=ShowAccepted]': {
                change: function() {
                    me.getMainView().getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView();
        if (!view) {
            view = Ext.widget('unacceptedpaymentpacketgrid');
            this.bindContext(view);
            this.application.deployView(view);
            view.getStore().load();
        }
    },

    rowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'edit':
                    this.showCharges(record);
            }
        }
    },

    rowDblClick: function (view, record) {
        if (!view || view.isDestroyed) return;
        this.showCharges(record);
    },

    onClickUpdate: function (btn) {
        btn.up('unacceptedpaymentpacketgrid').getStore().load();
    },

    showCharges: function (record) {
        var me = this,
            mainView = me.getMainView(),
            win = Ext.create('B4.form.Window', {
                width: 700,
                height: 300,
                layout: 'fit',
                modal: false,
                renderTo: mainView.getEl(),
                closeAction: 'destroy',
                items: [
                    {
                        xtype: 'unacceptedpaymentgrid'
                    }
                ],
                listeners: {
                    show: function() {
                        mainView.mask();
                    },
                    destroy: function () {
                        mainView.unmask();
                    }
                },
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            '->',
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        listeners: {
                                            'click': function (btn) {
                                                btn.up('window').close();
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

        win.down('unacceptedpaymentgrid').getStore().filter('packetId', record.getId());

        win.show();
    },

    onClickAccept: function (btn) {
        var me = this,
            grid = btn.up('unacceptedpaymentpacketgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Подтвердить выбранные оплаты?',
            anyConfirmed = false,
            allConfirmed = true,
            hasDistributePenalty = false;

        if (!selected || selected.length < 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для подтверждения!', 'error');
        } else {
            Ext.each(selected, function(item) {
                var state = item.get('State');

                if (item.get('DistributePenalty') == 10) {
                    hasDistributePenalty = true;
                }

                if (state == 10) {
                    anyConfirmed = true;
                } else if (state == 20) {
                    allConfirmed = false;
                }
            });

            if (allConfirmed) {
                B4.QuickMsg.msg('Ошибка', 'Все выбранные записи уже подтверждены', 'error');
                return;
            }

            if (anyConfirmed) {
                msg = 'Одна или несколько записей уже подтверждены, повторное подтверждение для них не будет выполнено. Подтвердить выбранные оплаты?';
            }

            if (hasDistributePenalty) {
                msg = 'Сумма будет распределена также на задолженность по пени' + '<br>' + msg;
            }

            Ext.Msg.prompt({
                title: 'Подтверждение оплат',
                msg: msg,
                multiline: 1,
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId, text) {
                    var params,
                        packetIds = [];

                    if (btnId === "ok") {
                        me.mask('Подтверждение оплат...', grid);

                        Ext.each(selected, function(item) {
                            if (item.get('State') == 20) {
                                packetIds.push(item.getId());
                            }
                        });

                        params = {
                            packetIds: Ext.JSON.encode(packetIds),
                            cprocName: text
                        };

                        me.sendRequest(params, 'AcceptPayments' 
                        ).next(function (resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Внимание', dec.message || "Успешно");
                            me.unmask();
                            store.load();
                        }).error(function (e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                            // непонимаю зачем в случае ошибки загружать стор ,не делать так
                            //store.load();
                        });
                    }
                }
            });
        }
    },
    
    sendRequest: function (params, method) {
        return B4.Ajax.request({
            url: B4.Url.action(method, 'UnacceptedPayment'),
            params: params,
            method: 'POST',
            timeout: 999999999
        });
    },
    
    onClickCancel: function(btn) {
        var me = this,
            grid = btn.up('unacceptedpaymentpacketgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Отменить выбранные оплаты?',
            anyCanceled = false,
            allCanceled = true;

        if (!selected || selected.length < 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для отмены!', 'error');
        } else {
            Ext.each(selected, function (item) {
                var state = item.get('State');

                if (state == 20) {
                    anyCanceled = true;
                } else if (state == 10) {
                    allCanceled = false;
                }
            });

            if (allCanceled) {
                B4.QuickMsg.msg('Ошибка', 'Все выбранные записи уже отменены', 'error');
                return;
            }

            if (anyCanceled) {
                msg = 'Одна или несколько записей уже отменены, повторная отмена для них не будет выполнена. Отменить выбранные оплаты?';
            }

            Ext.Msg.prompt({
                title: 'Отмена оплат',
                msg: msg,
                multiline: 1,
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId, text) {
                    var params,
                        packetIds = [];

                    if (btnId === "ok") {
                        me.mask('Отмена оплат...', grid);

                        Ext.each(selected, function (item) {
                            if (item.get('State') == 10) {
                                packetIds.push(item.getId());
                            }
                        });

                        params = {
                            packetIds: Ext.JSON.encode(packetIds),
                            cprocName: text
                        };

                        me.sendRequest(
                            params, 'CancelPayments'
                        ).next(function(resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Внимание', dec.message || "Успешно");
                            me.unmask();
                            store.load();
                        }).error(function(e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                            // непонимаю зачем в случае ошибки загружать стор ,не делать так
                            //store.load();
                        });
                    }
                }
            });
        }
    },
    
    onClickDelete: function (btn) {
        var me = this,
            grid = btn.up('unacceptedpaymentpacketgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Удалить выбранные оплаты?';

        if (!selected || selected.length < 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для удаления!', 'error');
        } else {
            Ext.Msg.prompt({
                title: 'Удаление оплат',
                msg: msg,
                multiline: 1,
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId, text) {
                    var params,
                        packetIds = [];

                    if (btnId === "ok") {
                        me.mask('Удаление оплат...', grid);

                        Ext.each(selected, function (item) {
                            if (item.get('State') == 20) {
                                packetIds.push(item.getId());
                            }
                        });

                        if (packetIds.length == 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для удаления со статусом ожидание!', 'error');
                            me.unmask();
                            return;
                        }

                        params = {
                            packetIds: Ext.JSON.encode(packetIds),
                            cprocName: text,
                            deletePayments: true
                        };

                        me.sendRequest(
                            params, 'RemovePayments'
                        ).next(function (resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Внимание', dec.message || "Успешно");
                            me.unmask();
                            store.load();
                        }).error(function (e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                            // непонимаю зачем в случае ошибки загружать стор ,не делать так
                            //store.load();
                        });
                    }
                }
            });
        }
    }
});