Ext.define('B4.controller.TableLock', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditForm',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['TableLock'],
    stores: ['TableLock'],
    views: [
        'tablelock.Grid'
    ],

    mainView: 'tablelock.Grid',
    mainViewSelector: 'tableLockGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'tableLockGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.TableLock.Edit', applyTo: 'button[action=UnlockAll]', selector: 'tableLockGrid' },
                {
                    name: 'Administration.TableLock.Edit',
                    applyTo: 'b4deletecolumn',
                    selector: 'tableLockGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditformaspect',
            name: 'tableLockGridEditWindowAspect',
            gridSelector: 'tableLockGrid',
            storeName: 'TableLock',
            modelName: 'TableLock',

            deleteRecord: function(record) {
                var c = this.controller;

                c.mask('Разблокировка...');
                B4.Ajax.request({
                        url: B4.Url.action('Unlock', 'TableLock'),
                        method: 'POST',
                        timeout: 999999,
                        params: {
                            'tableName': record.get('TableName'),
                            'action': record.get('Action')
                        }
                    })
                    .next(function() {
                        c.unmask();
                        c.getMainView().getStore().load();
                    })
                    .error(function() {
                        c.unmask();
                        Ext.Msg.alert('Ошибка', 'Не удалось снять блокировку');
                    });
            }
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions['button[action=UnlockAll]'] = {
            'click': {
                fn: me.unlockAll,
                scope: me
            }
        };

        me.control(actions);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('tableLockGrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    unlockAll: function() {
        var me = this;

        me.mask('Разблокировка...');
        B4.Ajax.request({
                url: B4.Url.action('UnlockAll', 'TableLock'),
                timeout: 999999
            })
            .next(function() {
                me.getMainView().getStore().load();
                me.unmask();
            })
            .error(function() {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Не удалось снять блокировку');
            });
    }
});