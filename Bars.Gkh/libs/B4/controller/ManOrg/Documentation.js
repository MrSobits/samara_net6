Ext.define('B4.controller.manorg.Documentation', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.Documentation'
    ],

    models: ['manorg.Documentation'],
    stores: ['manorg.Documentation'],
    views: [
        'manorg.DocumentationEditWindow',
        'manorg.DocumentationGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgdocperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgDocumentationGridWindowAspect',
            gridSelector: 'manorgdocumentationgrid',
            editFormSelector: '#manorgDocumentationEditWindow',
            storeName: 'manorg.Documentation',
            modelName: 'manorg.Documentation',
            editWindowView: 'manorg.DocumentationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('ManagingOrganization', this.controller.params.id);
                    }
                }
            }
        }
    ],

    params: {},
    mainView: 'manorg.DocumentationGrid',
    mainViewSelector: 'manorgdocumentationgrid',

    init: function () {
        this.getStore('manorg.Documentation').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },
  
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgdocumentationgrid');

        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.Documentation').load();
    }
});