Ext.define('B4.controller.SuspenseAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.store.BasePropertyOwnerDecision'
    ],

    stores: [
        'regop.SuspenseAccount'   
    ],

    views: [
        'suspenseaccount.NewGrid',
        'suspenseaccount.SuspenseAccountAddWindow',
        'suspenseaccount.DistributionSelectWindow',
        'suspenseaccount.NewAddWindow'
    ],

    models: [
        'regop.SuspenseAccount'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.Accounts.SuspenseAccount.Add', applyTo: 'b4addbutton', selector: 'newsuspenseaccountgrid'},
                { name: 'GkhRegOp.Accounts.SuspenseAccount.Enroll', applyTo: 'button[action=checkin]', selector: 'newsuspenseaccountgrid' },
                {
                    name: 'GkhRegOp.Accounts.SuspenseAccount.CancelDistribution',
                    applyTo: 'button[action=undodistribution]',
                    selector: 'newsuspenseaccountgrid',
                    applyBy: function (cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.SuspenseAccount.CancelEnrollment',
                    applyTo: 'button[action=undocheckin]',
                    selector: 'newsuspenseaccountgrid',
                    applyBy: function(cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'suspenceAccountGridWindowAspect',
            gridSelector: 'newsuspenseaccountgrid',
            editFormSelector: 'suspenceaccountnewaddwindow',
            modelName: 'regop.SuspenseAccount',
            editWindowView: 'suspenseaccount.NewAddWindow',
            listeners: {
                beforesetformdata: function (asp, record) {
                    if (record && record.getId() == 0) {
                        record.set('SuspenseAccountTypePayment', 10);
                    };
                }
            },
            rowDblClick: function() {},
            rowAction: function() {}
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'newsuspenseaccountgrid'
        }
    ],

    checkInWindowView: 'suspenseaccount.DistributionSelectWindow',
    checkInWindowSelector: 'distributionselectwin',

    init: function () {
        var me = this;

        me.control({
            'newsuspenseaccountgrid button[action=undodistribution]': { 'click': { fn: me.undodistribution, scope: me } },
            'newsuspenseaccountgrid button[action=undocheckin]': { 'click': { fn: me.undocheckin, scope: me } },
            'newsuspenseaccountgrid button[action=checkin]': { 'click': { fn: me.checkIn, scope: me } },
            'newsuspenseaccountgrid [name=ShowDistributed]': { 'change': { fn: me.onChangeShowDistributed, scope: me } },
            'distributionselectwin button[action=selectdistribution]': { 'click': { fn: me.onSelectDistribution, scope: me } }
        });
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('newsuspenseaccountgrid'),
            store;
        me.bindContext(view);
        me.application.deployView(view);
        
        store = view.getStore();
        store.on('beforeload', function (st, options) {
            options.params.showDistr = view.down('[name=ShowDistributed]').getValue();
        });

        store.load();
    },

    /**
    * отмена распределения
    */
    undodistribution: function () {
        var me = this,
            grid = me.getMainView(),
            sm = grid.getSelectionModel(),
            rec = sm.getLastSelected();

        if (!rec) {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать запись!');
            return;
        }

        Ext.Msg.confirm('Отмена распределения', 'Отменить распределение?', function (btnId) {
            if (btnId === "yes") {
                me.mask('Отмена распределения...');
                B4.Ajax.request({
                    url: B4.Url.action('Undo', 'Distribution'),
                    method: 'POST',
                    params: {
                        distributionId: rec.getId(),
                        distributionSource: 20
                    }
                }).next(function () {
                    Ext.Msg.alert('Результат', 'Распределение отменено');
                    me.getMainView().getStore().load();
                    me.unmask();
                }).error(function (response) {
                    var message = "Ошибка отмены распределения";
                    if (response && response.message) {
                        message = response.message;
                    }

                    Ext.Msg.alert('Ошибка', message);

                    me.unmask();
                });
            }
        });
    },

    /**
    * отмена начисления
    */
    undocheckin: function () {
        var me = this,
            grid = me.getMainView(),
            sm = grid.getSelectionModel(),
            rec = sm.getLastSelected();

        if (!rec) {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать запись!');
            return;
        }

        Ext.Msg.confirm('Отмена зачисления', 'Отменить зачисление?', function (btnId) {
            if (btnId === "yes") {
                me.mask('Отмена зачисления...');
                B4.Ajax.request({
                    url: B4.Url.action('UndoCheckin', 'Distribution'),
                    method: 'POST',
                    params: {
                        distributionId: rec.getId(),
                        distributionSource: 20
                    }
                }).next(function () {
                    Ext.Msg.alert('Результат', 'Зачисление отменено');
                    me.getMainView().getStore().load();
                    me.unmask();
                }).error(function (response) {
                    var message = "Ошибка отмены распределения";
                    if (response && response.message) {
                        message = response.message;
                    }

                    Ext.Msg.alert('Ошибка', message);

                    me.unmask();
                });
            }
        });
    },

    onSelectDistribution: function (fld) {
        var me = this,
            win = fld.up('window'),
            cbx = win.down('[name=DistributionType]'),
            route = cbx.getValue(),
            rec;

        if (!route) {
            B4.QuickMsg.msg('Ошибка', 'Выберите тип распределения!', 'error');
            return;
        }

        rec = cbx.getStore().findRecord('Name', cbx.getRawValue());

        me.application.redirectTo(Ext.String.format('{0}/{1}&{2}&{3}&{4}',
            route,
            win.distributionId,
            rec.raw.Code,
            ('' + win.sum).replace('.', 'dot'),
            20));

        win.close();
    },

    checkIn: function() {
        var me = this,
            grid = me.getMainView(),
            rec = grid.getSelectionModel().getLastSelected();

        if (!rec) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        if (rec.get('DistributeState') != 20) {
            B4.QuickMsg.msg('Ошибка', 'Распределять можно только записи со статусом "Не распределен"!', 'error');
            return;
        }

        me.mask('Проверка даты...', grid);

        B4.Ajax.request({
            url: B4.Url.action('/Distribution/CheckDate'),
            params: {
                date: rec.get('DateReceipt')
            }
        }).next(function () {
            me.unmask();
            me.continueCheckIn(rec);
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
        });
    },

    continueCheckIn: function (rec) {
        var me = this,
            editWindow;

        editWindow = me.getCmpInContext(me.checkInWindowSelector);

        if (editWindow && !editWindow.getBox().width) {
            editWindow = editWindow.destroy();
        }

        if (!editWindow) {
            editWindow = me.getView(me.checkInWindowView).create({
                modal: true,
                closeAction: 'destroy',
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                distributionId: rec.getId(),
                distributionSource: 20,
                sum: rec.get('Sum'),
                renderTo: B4.getBody().getActiveTab().getEl()
            });
        }

        editWindow.show();
        editWindow.getForm().isValid();
    },
    
    onChangeShowDistributed: function (comp) {
        comp.up('newsuspenseaccountgrid').getStore().load();
    }
});