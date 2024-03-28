Ext.define('B4.controller.manorglicense.EditLicenseGis', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GjiDocumentRegister'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'manorglicense.LicenseGis',
        'manorglicense.LicenseResolutionGis',
        'view.Resolution'
    ],

    models: [
        'manorglicense.LicenseGis',
        'manorglicense.LicenseResolutionGis',
        'Resolution',
        'InspectionGji'
    ],

    views: [
        'manorglicense.EditLicenseGisPanel',
        'manorglicense.LicenseResolutionGisGrid'
    ],

    mainView: 'manorglicense.EditLicenseGisPanel',
    mainViewSelector: 'manOrgLicenseGisEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'manOrgLicenseGisEditPanel'
        },
        {
            ref: 'resolutionGisGrid',
            selector: 'manorglicensegridgis'
        },
        {
            ref: 'resGisGrid',
            selector: 'manorglicenseresolutiongisgrid'
        }
    ],
    
    aspects: [
         {
             xtype: 'gjidocumentregisteraspect',
             name: 'docsGjiRegistrResolutionGisGridEditFormAspect',
             gridSelector: 'manorglicenseresolutiongisgrid',
             modelName: 'Resolution',
             storeName: 'view.Resolution'
         },
         {
             xtype: 'gkheditpanel',
             name: 'manOrgLicenseGisEditPanelAspect',
             editPanelSelector: 'manOrgLicenseGisEditPanel',
             modelName: 'manorglicense.LicenseGis',
          
             listeners: {
                 
                 aftersetpaneldata: function (asp, rec, panel) {
                     
                     var me = this,
                         resolutionGisGrid = panel.down('manorglicenseresolutiongisgrid'),
                         docStore = resolutionGisGrid.getStore();
                     docStore.clearFilter(true);
                     docStore.filter('id', me.objectId);
                 }
             }
         }
    ],
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manOrgLicenseGisEditPanel');
  
        me.bindContext(view);
        me.setContextValue(view, 'id', id);
        me.application.deployView(view, 'licensegis_info');
        me.getAspect('manOrgLicenseGisEditPanelAspect').setData(id);
    },

    init: function() {
        var me = this,
            actions = {};

        me.callParent(arguments);
    }
});