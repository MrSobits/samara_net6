Ext.define('B4.controller.manorg.Registry', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GridEditWindow'],

    models: ['manorg.ManagingOrgRegistry'],
    stores: ['manorg.ManagingOrgRegistry'],
    views: [
        'manorg.RegistryGrid',
        'manorg.RegistryEdit'
    ],


    mainView: 'manorg.RegistryGrid',
    mainViewSelector: 'manorgreggrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'manorgreggrid',
            editFormSelector: 'manorgregedit',
            storeName: 'manorg.ManagingOrgRegistry',
            modelName: 'manorg.ManagingOrgRegistry',
            editWindowView: 'manorg.RegistryEdit',
            listeners: {
                beforesetformdata: function (asp, record) {
                    var manorg;
                    if (+record.get('Id')) {
                        manorg = record.get('ManagingOrganization');
                        record.set('ManagingOrganization', manorg.Id);
                    }
                },
                getdata: function (asp, record) {
                    if (asp.controller.params) {
                        record.set('ManagingOrganization', asp.controller.params.get('Id'));
                    }
                    if (+record.get('Id') && record.get('Doc')) {
                        record.set('Doc', record.get('Doc').Id);
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('manorg.ManagingOrgRegistry').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('manorg.ManagingOrgRegistry').load();
    },

    onBeforeLoad: function (store, operation, type) {
        var me = this;
        if (me.params) {
            operation.params.manorgId = me.params.get('Id');
            operation.params.typeMode = type;
        }
    }
});