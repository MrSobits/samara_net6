Ext.define('B4.controller.housinginspection.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['HousingInspection'],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'housinginspection.EditPanel',
    mainViewSelector: 'housinginspectionEditPanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.HousingInspection.Edit', applyTo: 'b4savebutton', selector: 'housinginspectionEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'housinginspectioneditPanelAspect',
            editPanelSelector: 'housinginspectionEditPanel',
            modelName: 'HousingInspection'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('housinginspectionEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'housinginspectionId', 'id');
        me.application.deployView(view, 'housinginspection_info');

        me.getAspect('housinginspectioneditPanelAspect').setData(id);
    }
});