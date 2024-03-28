Ext.define('B4.controller.supplyresourceorg.Activity', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhEditPermissionAspect'
    ],

    models: ['SupplyResourceOrg'],
    views: ['supplyresourceorg.ActivityPanel'],

    mainView: 'supplyresourceorg.ActivityPanel',
    mainViewSelector: 'supplyresorgactivitypanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'supplyResOrgActivityPanelAspect',
            editPanelSelector: 'supplyresorgactivitypanel',
            modelName: 'SupplyResourceOrg',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #btnActivityPanelRefresh'] = { 'click': { fn: this.btnActivityPanelRefresh, scope: this} };
            },

            btnActivityPanelRefresh: function() {
                this.setData(this.controller.getContextValue(this.controller.getMainView(), 'supplyresorgId'));
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

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgactivitypanel');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        me.getAspect('supplyResOrgActivityPanelAspect').setData(id);
    }
});