Ext.define('B4.controller.SyncLog', {
    extend: 'B4.base.Controller',

    models: ['SyncAction', 'SyncSession', 'SyncActionDetails'],
    stores: ['SyncAction', 'SyncSession', 'SyncActionDetails'],
    views: [
        'synclog.DetailsWindow',
        'synclog.Panel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    mainView: 'synclog.Panel',
    mainViewSelector: 'synclogpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'synclogpanel'
        },
        {
            ref: 'sessionGrid',
            selector: 'synclogpanel #sessionGrid'
        },
        {
            ref: 'actionGrid',
            selector: 'synclogpanel #actionGrid'
        }
    ],

    init: function() {
        this.control({
            'synclogpanel #sessionGrid': {
                'select': {
                    fn: this.onSessionSelect,
                    scope: this
                },
                'deselect': {
                    fn: this.onSessionDeselect,
                    scope: this
                }
            },
            'synclogpanel #actionGrid': {
                'rowaction': {
                    fn: this.onActionGridRowAction,
                    scope: this
                }
            },
            'synclogdetailswindow #actionDetailsGrid': {
                'rowaction': {
                    fn: this.onActionDetailsGridRowAction,
                    scope: this
                }
            }
        });

        this.getStore('SyncActionDetails').on('beforeload', this.onActionDetailsBeforeLoad, this);
        this.getStore('SyncAction').on('beforeload', this.onActionBeforeLoad, this);

        this.callParent(arguments);
    },

    onActionBeforeLoad: function(s, op) {
        var sessions = this.getSessionGrid().getSelectionModel().getSelection(),
            session = sessions.length == 1 ? sessions[0] : null;

        if (!session) {
            return false;
        }

        op.params.sessionId = session.get('Id');
    },

    onActionDetailsBeforeLoad: function(s, op) {
        var actions = this.getActionGrid().getSelectionModel().getSelection(),
            action = actions.length == 1 ? actions[0] : null;

        if (!action) {
            return false;
        }

        op.params.sessionId = action.get('SessionId');
        op.params.action = action.get('Name');
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('synclogpanel');
        this.bindContext(view);
        this.application.deployView(view);

        this.getActionGrid().disable();
        this.getStore('SyncAction').loadRawData(null);
        this.getStore('SyncSession').load();
    },

    onSessionSelect: function () {
        this.getActionGrid().enable();
        this.getStore('SyncAction').load();
    },

    onSessionDeselect: function () {
        this.getActionGrid().disable();
        this.getStore('SyncAction').loadRawData(null);
    },

    onActionGridRowAction: function(grid, action, rec) {
        if (action == 'edit' || action == 'doubleclick') {
            var store = this.getStore('SyncActionDetails'),
                window = this.getSynclogDetailsWindowView().create({
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    ctxKey: this.getCurrentContextKey(),
                    listeners: {
                        show: function() {
                            window.down('b4grid').headerFilterPlugin.resetFilters();
                        },
                        close: function () {
                            store.removeAll();
                        }
                    }
                });

            grid.getSelectionModel().select(rec);
            window.setActionName(rec.get('Name')).show().center();
            store.load();
        }
    },

    onActionDetailsGridRowAction: function(grid, action, rec) {
        var me = this;
        switch (action) {
            case 'getfile':
                var id = rec.get('FileId');
                if (id > 0) {
                    window.open(B4.Url.action('Download', 'FileUpload', { id: id }));
                } else {
                    B4.QuickMsg.msg('Получение пакета', 'При сохранении пакета произошла ошибка. Файл недоступен', 'error');
                }
                break;
            case 'replay':
                me.mask('Повторение действия...', grid.ownerCt);
                B4.Ajax.request({
                    url: B4.Url.action('ReplayAction', 'SyncLog'),
                    params: {
                        id: rec.get('Id')
                    },
                    timeout: 9999999
                }).next(function() {
                    me.unmask();
                    B4.QuickMsg.msg('Повторение действия', 'Действие повторено. Подробности в окне информации', 'success');
                    grid.getStore().load();
                }).error(function(err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                    grid.getStore().load();
                });
                break;
        }
    }
});