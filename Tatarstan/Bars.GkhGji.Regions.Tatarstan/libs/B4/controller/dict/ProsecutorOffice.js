Ext.define('B4.controller.dict.ProsecutorOffice', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhInlineGrid'
    ],

    models: [
        'dict.ProsecutorOffice'
    ],
    stores: [
        'dict.ProsecutorOffice'
    ],
    views: [
        'dict.prosecutoroffice.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'dict.prosecutoroffice.Grid',
    mainViewSelector: 'prosecutorofficegrid',

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'prosecutorofficeinlinegridaspect',
            gridSelector: 'prosecutorofficegrid',
            storeName: 'dict.ProsecutorOffice',
            modelName: 'dict.ProsecutorOffice'
        }
    ],

    init: function() {
        var me = this,
            actions = {
                'prosecutorofficegrid b4updatebutton': { click: { fn: me.reloadGrid, scope: me } },
                'prosecutorofficegrid button[itemId=btnSendRequest]': { click: { fn: me.sendRequest, scope: me } }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget(this.mainViewSelector);
        this.bindContext(view);
        this.application.deployView(view);
        this.reloadGrid();
    },

    sendRequest: function() {
        var me = this;
        me.mask('Отправление в ЕРП', me.getMainComponent());

        B4.Ajax.request(B4.Url.action('RequestProsecutorsOfficesToErp', 'ErpIntegration', {}))
            .next(function() {
                me.unmask();

                Ext.Msg.alert('Отправка запроса в ЕРП', 'Выполнено успешно');
                return true;
            })
            .error(function(e) {
                me.unmask();

                Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
            });
    },
    reloadGrid: function() {
        var me = this,
            grid = me.getMainView(),
            store = grid.getStore();

        store.load();
    }
});