Ext.define('B4.controller.objectoutdoorcr.TypeWorkRealityObjectOutdoor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.TypeWorkRealityObjectOutdoor',
        'B4.aspects.permission.TypeWorkRealityObjectOutdoorEdit',
        'B4.enums.TypeWorkCrHistoryAction'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoor',
        'objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoorHistory'
    ],

    stores: [
        'objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoor',
        'objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoorHistory'
    ],

    views: [
        'objectoutdoorcr.typeworkrealityobjectoutdoor.Panel',
        'objectoutdoorcr.typeworkrealityobjectoutdoor.Grid',
        'objectoutdoorcr.typeworkrealityobjectoutdoor.HistoryGrid',
        'objectoutdoorcr.typeworkrealityobjectoutdoor.EditWindow'
    ],

    mainView: 'objectoutdoorcr.typeworkrealityobjectoutdoor.Panel',
    mainViewSelector: 'typeworkrealityobjectoutdoorpanel',

    aspects: [
        {
            xtype: 'typeworkrealityobjectoutdoorperm'
        },
        {
            xtype: 'typeworkrealityobjectoutdooreditstateperm',
            name: 'typeworkrealityobjectoutdooreditstateperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'typeWorkRealityObjectOutdoorGridWindowAspect',
            gridSelector: 'typeworkrealityobjectoutdoorgrid',
            editFormSelector: 'typeworkrealityobjectoutdooreditwindow',
            modelName: 'objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoor',
            editWindowView: 'objectoutdoorcr.typeworkrealityobjectoutdoor.EditWindow',

            otherActions: function(actions) {
                actions[this.gridSelector + ' [action=addMainWork]'] = { 'click': { fn: this.onAddWork, scope: this } };
                actions[this.gridSelector + ' [action=addAdditionalWork]'] = { 'click': { fn: this.onAddWork, scope: this } };
            },
            listeners: {
                beforesave: function(asp, record) {
                    record.set('ObjectOutdoorCr', asp.controller.getContextValue(asp.controller.getMainView(), 'objectOutdoorCrId'));
                    return record;
                },

                aftersetformdata: function (asp, rec, form) {
                    form.down('[name=WorkRealityObjectOutdoor]').setDisabled(rec.getId());
                    form.down('[name=WorkRealityObjectOutdoor]').getStore().on('beforeload', this.onBeforeLoad, this);
                    asp.controller.getAspect('typeworkrealityobjectoutdooreditstateperm').loadPerms(rec);
                }
            },

            onAddWork: function (btn) {
                this.controller.setContextValue(this.controller.getMainView(), 'needMainWork', btn.action === 'addMainWork');
                this.editRecord(null);
            },

            onBeforeLoad: function (store, operation) {
                operation.params.needMainWork = this.controller.getContextValue(this.controller.getMainView(), 'needMainWork') === true;
            },
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'typeworkrealityobjectoutdoorpanel [name=typeworkrealityobjectoutdoortabpanel]': { 'tabchange': { fn: me.changeTab, scope: me } },
            'typeworkrealityobjectoutdoorhistorygrid button[action=recover]': { 'click': { fn: me.recover, scope: me } },
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.setContextValue(view, 'objectOutdoorCrId', id);
        me.application.deployView(view, 'object_outdoor_cr_info');
        
        view.down('typeworkrealityobjectoutdoorgrid').getStore().on('beforeload', me.onBeforeLoad, me);
        view.down('typeworkrealityobjectoutdoorhistorygrid').getStore().on('beforeload', me.onBeforeLoad, me);

        view.down('typeworkrealityobjectoutdoorgrid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.objectOutdoorCrId = this.getContextValue(this.getMainView(), 'objectOutdoorCrId');
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        newTab.getStore().load();
    },

    recover: function (btn) {
        var me = this,
            grid = btn.up('typeworkrealityobjectoutdoorhistorygrid'),
            records = grid.getSelectionModel().getSelection();

        if (records.length === 0) {
            Ext.Msg.alert('Сообщение', 'Выберите объекты для восстановления!');
            return false;
        }

        var record = records[0];
        if (record.get('TypeAction') != B4.enums.TypeWorkCrHistoryAction.Removal) {
            Ext.Msg.alert('Восстановление вида работы', 'Необходимо выбрать одну запись с признаком "Действие" = "Удаление"!');
            return false;
        }

        Ext.Msg.confirm('Восстановление объекта', 'Восстановить выбранный объект?', function (result) {
            if (result === 'yes') {
                me.mask('Восстановление', B4.getBody().getActiveTab());
                B4.Ajax.request({
                    url: B4.Url.action('Recover', 'TypeWorkRealityObjectOutdoorHistory'),
                    timeout: 9999999,
                    params: {
                        id: record.getId()
                    }
                }).next(function () {
                    B4.QuickMsg.msg('Восстановление объекта', 'Объект успешно восстановлен', 'success');
                    grid.getStore().load();
                    me.unmask();
                }).error(function (error) {
                    B4.QuickMsg.msg('Восстановление объекта', error.message || error, 'error');
                    me.unmask();
                });
            }
        }, me);
    }
});