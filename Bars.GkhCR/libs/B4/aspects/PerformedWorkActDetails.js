Ext.define('B4.aspects.PerformedWorkActDetails', {
    extend: 'B4.base.Aspect',

    alias: 'widget.performedworkactdetails',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    controller: null,

    showDetailsButtonSelector: null,

    requires: [
        
    ],
    
    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'aftersetformdata'
        );
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.showDetailsButtonSelector] = { 'click': { fn: me.onClickShowDetails, scope: me } };

        controller.control(actions);
    },
    
    onClickShowDetails: function () {
        var me = this;

        me.mask('Проверка статусов');
        B4.Ajax.request({
            url: B4.Url.action('CheckActsForDetails', 'PerformedWorkAct'),
            params: me.getParams()
        }).next(function (response) {
            me.unmask();

            var result = Ext.JSON.decode(response.responseText);
            if (!result.success && result.message) {
                Ext.Msg.alert('Внимание', result.message);
            }
            else if (result.success) {
                if (!result.message) {
                    me.showDetailsPanel();
                } else {
                    Ext.Msg.alert('Внимание', result.message, function () {
                        me.showDetailsPanel();
                    });
                }
            }

        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    showDetailsPanel: function () {
        var me = this,
            detailsPanel = Ext.ComponentQuery.query('workactregisterdetailspanel')[0];

        if (!detailsPanel) {
            detailsPanel = me.controller.getView('workactregister.DetailsPanel').create(
            {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            detailsPanel.show();

            var detailsStore = detailsPanel.down('gridpanel').getStore();
            detailsStore.on('beforeload', me.onBeforeLoad, me);
            detailsStore.load();
        }

        me.fireEvent('aftersetformdata', me, detailsPanel);
    },

    onBeforeLoad: function () {
    },

    getParams: function() {
    }
});