Ext.define('B4.controller.manorg.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.realityobj.RealityObjectFields',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.BackForward'
    ],

    models: ['ManagingOrganization'],
    views: ['manorg.EditPanel'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.Managing.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'manorgEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'manorgEditPanel',
            editPanelSelector: 'manorgEditPanel',
            modelName: 'ManagingOrganization',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbOfficialSite731'] = { 'change': { fn: this.cbOfficialSite731Change, scope: this } };
                actions[this.editPanelSelector + ' combobox[name=TypeManagement]'] = { 'change': { fn: this.cbTypeManagementChange, scope: this } };
            },
            cbOfficialSite731Change: function (cb, newValue) {
                var panel = this.getPanel();
                panel.down('#tfOfficialSite').setDisabled(!newValue);
            },
            cbTypeManagementChange: function (cb, newValue) {
                var panel = this.getPanel();
                if (newValue == 20 || newValue == 40)
                {
                    panel.down('b4filefield[name=DispatchFile]').show();
                }
                else {
                    panel.down('b4filefield[name=DispatchFile]').hide();
                }
                
            },
            listeners: {
                //нужен для того, чтобы информация о типе управления была всегда актуальной для управления домами
                savesuccess: function (asp, record) {
                    asp.controller.params = asp.controller.params || {};
                    asp.controller.params.TypeManagement = record.get('TypeManagement');
                }
            }
        }
    ],

    params: null,
    mainView: 'manorg.EditPanel',
    mainViewSelector: 'manorgEditPanel',

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manorgEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        this.getAspect('manorgEditPanel').setData(id);
        
    }
});