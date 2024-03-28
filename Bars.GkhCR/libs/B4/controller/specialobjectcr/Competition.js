Ext.define('B4.controller.specialobjectcr.Competition', {
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

    stores: ['specialobjectcr.Competition'],

    views: ['specialobjectcr.CompetitionGrid'],

    mainView: 'specialobjectcr.CompetitionGrid',
    mainViewSelector: 'specialobjectcrcompetitiongrid',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.CreateContract.View',
                    applyTo: 'button[action=CreateContract]',
                    selector: 'specialobjectcrcompetitiongrid',
                    applyBy: function(cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.CreateContract.Edit',
                    applyTo: 'button[action=CreateContract]',
                    selector: 'specialobjectcrcompetitiongrid',
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

        actions['specialobjectcrcompetitiongrid button[action=CreateContract]'] = {
            'click': { fn: me.createContract, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    createContract: function (btn) {
        var me = this,
            grid = btn.up('specialobjectcrcompetitiongrid'),
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
            url: B4.Url.action('CreateContract', 'SpecialObjectCr', { objectId: objectId, lotIds: ids.join(',') }),
            timeout: 60*60*1000
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
            view = me.getMainView() || Ext.widget('specialobjectcrcompetitiongrid');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

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
                Ext.History.add('competitionedit/' + cmpId + '/lot/' + lotId); //??
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