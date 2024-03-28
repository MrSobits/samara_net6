Ext.define('B4.controller.servorg.Documentation', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['servorg.Documentation'],
    stores: ['servorg.Documentation'],
    views: ['servorg.DocumentationGrid', 'servorg.DocumentationEditWindow'],

    mainView: 'servorg.DocumentationGrid',
    mainViewSelector: 'servorgdocumentationgrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'servorgDocumentationGridWindowAspect',
            gridSelector: 'servorgdocumentationgrid',
            editFormSelector: 'servorgdocumentationeditwindow',
            storeName: 'servorg.Documentation',
            modelName: 'servorg.Documentation',
            editWindowView: 'servorg.DocumentationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.ServiceOrganization = asp.controller.getContextValue(asp.controller.getMainView(), 'servorgId');
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('servorg.Documentation').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgdocumentationgrid');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.servorgId = this.getContextValue(this.getMainView(), 'servorgId');
    }
});