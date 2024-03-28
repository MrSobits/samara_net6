Ext.define('B4.controller.claimwork.PartialClaimWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['claimwork.FlattenedClaimWork'],
    stores: ['claimwork.PartialClaimWork'],
    views: [
        //'claimwork.flattenedclaimwork.EditPanel',
        'claimwork.partialclaimwork.EditWindow',
        'claimwork.partialclaimwork.Grid'
    ],
    parentId: null,
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'claimwork.partialclaimwork.Grid',
    mainViewSelector: 'partialclaimworkgrid',
    editWindowSelector: 'partialclaimworkEditWindow',
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'partialclaimworkGridWindowAspect',
            gridSelector: 'partialclaimworkgrid',
            editFormSelector: '#partialclaimworkEditWindow',
            storeName: 'claimwork.PartialClaimWork',
            modelName: 'claimwork.FlattenedClaimWork',
            editWindowView: 'claimwork.partialclaimwork.EditWindow',
            listeners: {
                aftersetformdata: function(asp, rec, form) {
                    var me = this;
                    //debugger;
                    parentId = rec.getId();
                }
            }
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'partialclaimworkgrid'
        }
    ],
    moveToArchive: function (btn) {
        var me = this
        view = me.getMainView(),
            store = view.getStore(),
            records = view
                .getSelectionModel().getSelection(),
            recIds = [];
        Ext.each(records,
            function(rec) {
                recIds.push(rec.get('Id'));
            });
        me.mask('Выгрузка...', Ext.getBody());
        B4.Ajax.request({
                url: B4.Url.action('MoveToArchive', 'ArchivedClaimWork'),
                method: 'POST',
                timeout: 100 * 60 * 60 * 3,
                params: {
                    recIds: recIds
                }
            })
            .next(function(response) {
                var obj = Ext.JSON.decode(response.responseText);
                me.unmask();
                store.load();
                var alertWindow = Ext.Msg.alert('Результат выполнения', 'Перемещение в архив ПИР успешно');
            })
            .error(function(error) {
                me.unmask();
                Ext.Msg.alert('Ошибка', error.message);
            });
    },
    init: function() {
        var me = this;
        var me = this,
            actions = [];

        actions['partialclaimworkgrid button[action=MoveToArchive]'] = { click: me.moveToArchive, scope: me }

        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('partialclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);
        //view.getStore().load();
    }
});