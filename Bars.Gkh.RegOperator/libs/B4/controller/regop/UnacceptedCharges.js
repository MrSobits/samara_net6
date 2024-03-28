Ext.define('B4.controller.regop.UnacceptedCharges', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.form.Window',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.QuickMsg'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'unacceptedchargepacketgrid'
        }
    ],

    views: [
        'regop.unacceptedcharges.PacketGrid',
        'regop.unacceptedcharges.ChargesGrid'
    ],

    models: [
        'regop.UnacceptedChargePacket'
    ],

    init: function () {
        var me = this;

        me.control({
            'unacceptedchargepacketgrid b4updatebutton': { 'click': { fn: me.updateGrid } },
            'unacceptedchargepacketgrid button[action="Accept"]': { 'click': { fn: me.accept } },
            'unacceptedchargepacketgrid button[action="Export"]': { 'click': { fn: me.exportData } },
            'unacceptedchargepacketgrid': {
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                },
                'itemdblclick': {
                    fn: me.rowDblClick,
                    scope: me
                }
            }
        });
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('unacceptedchargepacketgrid');

        me.bindContext(view);
        me.application.deployView(view);
    },

    rowAction: function(grid, action, record) {
        if (!grid || grid.isDestroyed) return;
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'edit':
                    this.showCharges(record);
            }
        }
    },

    rowDblClick: function(view, record) {
        if (!view || view.isDestroyed) return;
        this.showCharges(record);
    },

    updateGrid: function(btn) {
        btn.up('unacceptedchargepacketgrid').getStore().load();
    },

    showCharges: function (record) {
        var me = this,
            mainView = me.getMainView();
           
        var win = Ext.create('B4.form.Window', {
            width: 1200,
            height: 500,
            layout: 'fit',
            modal: false,
            closeAction: 'destroy',
            listeners: {
                show: function() {
                    mainView.mask();
                },
                destroy: function() {
                    mainView.unmask();
                }
            },
            renderTo: mainView.getEl(),
            items: [
                {
                    xtype: 'unacceptedchargegrid',
                    border: false
                }
            ]
        });

        win.down('unacceptedchargegrid').getStore().filter('packetId', record.getId());

        win.show();
    },


    accept: function (btn) {
        var me = this,
            grid = btn.up('unacceptedchargepacketgrid'),
            selected = grid.getSelectionModel().getLastSelected();

        if (!selected) {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать начисление для подтверждения!');
        } else {
            
            if (selected.get('PacketState') === 10) {
                B4.QuickMsg.msg('Ошибка', 'Запись уже подтверждена, невозможно подтвердить запись дважды!', 'error');
                return;
            }
            if (selected.get('PacketState') === 30) {
                B4.QuickMsg.msg('Ошибка', 'Запись обрабатывается, невозможно подтвердить запись дважды!', 'error');
                return;
            }

            Ext.Msg.prompt({
                title: 'Подтверждение начислений',
                msg: 'Подтвердить выбранные начисления?',
                multiline: 1,
                buttons: Ext.Msg.OKCANCEL,
                fn: function(btnId, text) {
                    if (btnId === "ok") {
                        me.mask('Подтверждение начислений...');
                        B4.Ajax.request({
                            url: B4.Url.action('Accept', 'Charge'),
                            timeout: 9999999,
                            params: {
                                id: selected.getId(),
                                cprocName: text
                            },
                            method: 'POST'
                        }).next(function (response) {
                            var obj = Ext.JSON.decode(response.responseText);

                            if (obj.Success) {
                                me.unmask();
                                B4.QuickMsg.msg('Подтверждение начисления', 'Успешно', 'success');

                            } else {
                                B4.QuickMsg.msg('Ошибка!', obj.message, 'error');
                                me.unmask();
                            }
                            me.getMainComponent().getStore().load();

                        }).error(function (e) {
                            B4.QuickMsg.msg('Ошибка!', e.message || 'При подтверждении возникла ошибка', 'error');
                            me.unmask();
                        });
                    }
                }
            });
        }
    },
    
    exportData: function (button) {
        var grid = button.up('unacceptedchargepacketgrid'),
            record = grid.getSelectionModel().getSelection();

        if (!record || record.length < 1) {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать запись!');
            return false;
        }

        Ext.DomHelper.append(document.body, {
            tag: 'iframe',
            id: 'downloadIframe',
            frameBorder: 0,
            width: 0,
            height: 0,
            css: 'display:none;visibility:hidden;height:0px;',
            src: B4.Url.action('GetUnacceptedChargesExport', 'UnacceptedChargesExport', { packId: record[0].getId() })
        });
    }
});