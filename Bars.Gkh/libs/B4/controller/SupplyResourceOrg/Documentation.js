Ext.define('B4.controller.supplyresourceorg.Documentation', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhGridPermissionAspect',
        'B4.aspects.permission.GkhEditPermissionAspect'
    ],

    models: ['supplyresourceorg.Documentation'],
    stores: ['supplyresourceorg.Documentation'],
    views: ['supplyresourceorg.DocumentationGrid', 'supplyresourceorg.DocumentationEditWindow'],

    mainView: 'supplyresourceorg.DocumentationGrid',
    mainViewSelector: 'supplyresorgdocumentationgrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'supplyResOrgDocumentationGridWindowAspect',
            gridSelector: 'supplyresorgdocumentationgrid',
            editFormSelector: 'supplyresorgdocumentationeditwindow',
            storeName: 'supplyresourceorg.Documentation',
            modelName: 'supplyresourceorg.Documentation',
            editWindowView: 'supplyresourceorg.DocumentationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.SupplyResourceOrg = this.controller.getContextValue(this.controller.getMainView(), 'supplyresorgId');
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('supplyresourceorg.Documentation').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgdocumentationgrid');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        view.getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'supplyresorgId');
    }
});