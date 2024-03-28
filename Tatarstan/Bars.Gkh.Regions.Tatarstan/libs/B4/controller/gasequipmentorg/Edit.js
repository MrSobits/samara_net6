Ext.define('B4.controller.gasequipmentorg.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['GasEquipmentOrg'],

    views: [
        'regoperator.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'gasequipmentorg.EditPanel',
    mainViewSelector: 'gasequipmentorgEditPanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.GasEquipmentOrg.Edit', applyTo: 'b4savebutton', selector: 'gasequipmentorgEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'gasequipmentorgeditPanelAspect',
            editPanelSelector: 'gasequipmentorgEditPanel',
            modelName: 'GasEquipmentOrg',
            afterSetPanelData: function (aspect, rec, panel) {
                panel.setDisabled(false);
                aspect.controller.setContextValue(panel, 'contragentId', rec.get('ContragentId'))
            }
        }
    ],

    init: function() {
        this.callParent(arguments);
    },

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('gasequipmentorgEditPanel'),
            contactStore = view.down('b4selectfield[name=Contact]').getStore();

        contactStore.on('beforeload', me.onBeforeLoad, me);

        me.bindContext(view);
        me.setContextValue(view, 'gasequipmentorgId', 'id');
        me.application.deployView(view, 'gasequipmentorg_info');

        me.getAspect('gasequipmentorgeditPanelAspect').setData(id);
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView(),
            contragentId = operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});