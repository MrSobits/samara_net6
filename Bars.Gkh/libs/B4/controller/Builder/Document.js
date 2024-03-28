Ext.define('B4.controller.builder.Document', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.builder.DocumentInfo'
    ],

    models: ['builder.Document'],
    stores: ['builder.Document'],
    views: [
        'builder.DocumentEditWindow',
        'builder.DocumentGrid'
    ],

    aspects: [
        {
            xtype: 'documentinfoperm',
            name: 'documentinfoperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'builderDocumentGridWindowAspect',
            gridSelector: '#builderDocumentGrid',
            editFormSelector: '#builderDocumentEditWindow',
            storeName: 'builder.Document',
            modelName: 'builder.Document',
            editWindowView: 'builder.DocumentEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.Builder = this.controller.params.get('Id');
                    }
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #docContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
            },
            onBeforeLoadContragent: function (field, options) {
                if (options.params == null) {
                    options.params = {};
                }
                options.params.showAll = true;
            }
        }
    ],
    
    params: null,
    mainView: 'builder.DocumentGrid',
    mainViewSelector: '#builderDocumentGrid',

    init: function () {
        this.getStore('builder.Document').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.Document').load();
        if (this.params) {
            var id = this.params.get('Id');
            this.getAspect('documentinfoperm').setPermissionsByRecord({ getId: function () { return id; } });
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});