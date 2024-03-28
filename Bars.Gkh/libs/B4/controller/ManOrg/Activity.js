Ext.define('B4.controller.manorg.Activity', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.manorg.Activity'
    ],

    models: ['ManagingOrganization'],
    views: ['manorg.ActivityPanel'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgactivityperm'
        },
        {
            xtype: 'gkheditpanel',
            name: 'manorgActivityPanelAspect',
            editPanelSelector: 'manorgactivitypanel',
            modelName: 'ManagingOrganization',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #btnActivityPanelRefresh'] = { 'click': { fn: this.btnActivityPanelRefresh, scope: this } };
                actions[this.editPanelSelector + ' #cbActivityGroundsTermination'] = { 'change': { fn: this.cbActivityGroundsTerminationChange, scope: this } };
            },

            cbActivityGroundsTerminationChange: function (field, newValue) {
                var panel = this.getPanel();
                var dfActivityDateEnd = panel.down('#dfActivityDateEnd');
                dfActivityDateEnd.allowBlank = newValue != '10' ? false : true;
            },

            btnActivityPanelRefresh: function () {
                if (this.controller.params) {
                    this.setData(this.controller.params.id);
                }
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var cbActivityGroundsTermination = panel.down('#cbActivityGroundsTermination');
                    var lbActivityGroundsTerminationLabel = panel.down('#lbActivityGroundsTerminationLabel');
                    if (rec.get('Contragent').ContragentState == 20 || rec.get('Contragent').ContragentState == 30 || rec.get('Contragent').ContragentState == 40) {
                        cbActivityGroundsTermination.setDisabled(true);
                        lbActivityGroundsTerminationLabel.setText('Контрагент закончил деятельность');
                    }
                    else {
                        cbActivityGroundsTermination.setDisabled(false);
                        lbActivityGroundsTerminationLabel.setText('');
                    }
                }
            }
        }
    ],

    params: {},
    mainView: 'manorg.ActivityPanel',
    mainViewSelector: 'manorgactivitypanel',

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgactivitypanel');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.Service').load();

        me.getAspect('manorgActivityPanelAspect').setData(me.params.id);
    }
});