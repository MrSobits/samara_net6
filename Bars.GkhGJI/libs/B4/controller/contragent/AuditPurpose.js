Ext.define('B4.controller.contragent.AuditPurpose', {
    extend: 'B4.controller.MenuItemController',
   
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'Contragent'
    ],
    
    stores: [
        'contragent.AuditPurpose'
    ],

    views: [
        'contragent.AuditPurposePanel',
        'contragent.AuditPurposeGrid',
        'contragent.AuditPurposeEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentauditpurposepanel'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.AuditPurpose.Edit', applyTo: 'b4savebutton', selector: 'contragentauditpurposepanel' },
                { name: 'Gkh.Orgs.Contragent.Register.AuditPurpose.LastInspDate', applyTo: '[name=LastInspDate]', selector: 'contragentauditpurposeeditwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.setReadOnly(false);
                            else component.setReadOnly(true);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'contragentAuditPurposeEditPanelAspect',
            editPanelSelector: 'contragentauditpurposepanel',
            modelName: 'Contragent'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentAuditPurposeEditWindowAspect',
            gridSelector: 'auditpurposegrid',
            editFormSelector: 'contragentauditpurposeeditwin',
            storeName: 'contragent.AuditPurpose',
            modelName: 'contragent.AuditPurpose',
            editWindowView: 'contragent.AuditPurposeEditWindow',
            editRecord: function (record) {
                var me = this;

                
                if (record.get('EntityId') == 0) {
                    record.phantom = true;
                }

                me.setFormData(record);
            },
            listeners: {
                beforesave: function(asp, record) {
                    record.set('Id', record.get('EntityId'));
                }
            }
        }
    ],
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentauditpurposepanel'),
            grid = view.down('auditpurposegrid');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        me.getAspect('contragentAuditPurposeEditPanelAspect').setData(id);
        grid.getStore().filter('contragentId', id);
        
    }
});