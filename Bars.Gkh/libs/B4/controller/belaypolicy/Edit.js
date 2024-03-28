Ext.define('B4.controller.belaypolicy.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel'
    ],

    models: ['BelayPolicy'],
    views: ['belaypolicy.EditPanel'],
    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'belayPolicyEditPanel',
            editPanelSelector: '#belayPolicyEditPanel',
            modelName: 'BelayPolicy',
            listeners: {
                savesuccess: function (asp, rec) {
                    asp.setData(rec.getId());
                }
            }
        }
    ],
    
    params: null,
    mainView: 'belaypolicy.EditPanel',
    mainViewSelector: '#belayPolicyEditPanel',

    init: function () {
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('belayPolicyEditPanel').setData(this.params.get('Id'));
        }
    }
});