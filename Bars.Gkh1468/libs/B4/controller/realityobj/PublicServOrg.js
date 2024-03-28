Ext.define('B4.controller.realityobj.PublicServOrg', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.PublicServiceOrg'
    ],
    stores: [
        'realityobj.PublicServiceOrg'
    ],
    views: [
        'realityobj.PublicServiceOrgGrid'
    ],

    mainView: 'realityobj.PublicServiceOrgGrid',
    mainViewSelector: 'realobjpublicserviceorggrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'realObjPublicServOrgGridWindowAspect',
            gridSelector: '#realObjPublicServOrgGrid',
            storeName: 'realityobj.PublicServiceOrg',
            modelName: 'realityobj.PublicServiceOrg',
            editRecord: function (record) {
                if (record.get('PublicServiceOrgId')) {
                    Ext.History.add(Ext.String.format('publicservorgedit/{0}/contract?contractId={1}', record.get('PublicServiceOrgId'), record.get('ContractId')));
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('realityobj.PublicServiceOrg').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('realobjpublicserviceorggrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
        operation.params.fromContract = false;
    },

    onLaunch: function () {
        this.getStore('realityobj.PublicServiceOrg').load();
    }
});