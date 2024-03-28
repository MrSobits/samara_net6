Ext.define('B4.controller.objectcr.Competition', {
    extend: 'B4.controller.MenuItemController',

    require: [
         'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['Competition'],

    stores: ['objectcr.Competition'],

    views: ['objectcr.CompetitionGrid'],

    mainView: 'objectcr.CompetitionGrid',
    mainViewSelector: 'objectcrcompetitiongrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                {
                    name: 'GkhCr.ObjectCr.CreateContract.View',
                    applyTo: '[action=CreateContract]',
                    selector: 'objectcrcompetitiongrid',
                    applyBy: function(cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.CreateContract.Edit',
                    applyTo: '[action=CreateContract]',
                    selector: 'objectcrcompetitiongrid',
                    applyBy: function(cmp, allowed) {
                        cmp.setDisabled(!allowed);
                    }
                }
            ]
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        actions[me.mainViewSelector] = {
            'afterrender': { fn: me.onMainViewAfterRender, scope: me },
            'rowaction': { fn: me.rowAction, scope: me }
        };

        actions[me.mainViewSelector + ' b4updatebutton'] = {
            'click': { fn: me.onGridUpdate, scope: me }
        };

        actions['objectcrcompetitiongrid button[action=CreateContract]'] = {
            'click': { fn: me.createContract, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    createContract: function (btn) {
        var me = this,
            grid = btn.up('objectcrcompetitiongrid'),
            ids = [],
            objectId = me.getContextValue(me.getMainComponent(), 'objectcrId'),
            selection = grid.getSelectionModel().getSelection();

        selection.forEach(function (entry) {
            ids.push(entry.data.Id);
        });

        if (ids.length <= 0) {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать объекты!');
            return;
        }

        me.mask('Обработка...', me.getMainComponent());
        B4.Ajax.request({
            url: B4.Url.action('CreateContract', 'ObjectCr', { objectId: objectId, lotIds: ids.join(',') }),
            timeout: 9999999
        }).next(function (response) {
            me.unmask();

            //десериализуем полученную строку
            var obj = Ext.JSON.decode(response.responseText);

            if (obj.success) {
                B4.QuickMsg.msg('Формирование договоров', 'Формирование договоров прошло успешно', 'success');
            } else {
                Ext.Msg.alert('Ошибка!', obj.message);
            }
        }).error(function (result) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    },

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectcrcompetitiongrid');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        var store = view.getStore();
        store.clearFilter(true);
        store.filter('objectId', id);
    },

    onGridUpdate: function(btn) {
        btn.up('grid').getStore().load();
    },

    rowAction: function(grid, action, record) {
        var cmpId, lotId;
        switch (action.toLowerCase()) {
            case 'edit':
                cmpId = record.get('CompetitionId');
                lotId = record.get('Id');
                Ext.History.add('competitionedit/' + cmpId + '/lot/' + lotId);
                // тут надо написать действие перехода в карточку лота
                break;
        }
    },

    onMainViewAfterRender: function() {
        var me = this,
            store = me.getMainView().getStore();

            var objectId = me.getContextValue(me.getMainComponent(), 'objectcrId');
            store.clearFilter(true);
            store.filter('objectId', objectId);
        
    }
});