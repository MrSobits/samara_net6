Ext.define('B4.controller.dict.TypeCustomer', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'publicserviceGridAspect',
            storeName: 'dict.TypeCustomer',
            modelName: 'dict.TypeCustomer',
            gridSelector: 'typecustomergrid'
        },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typecustomergrid',
            permissionPrefix: 'Gkh1468.Dictionaries.TypeCustomer'
        },
    ],
    
    models: ['dict.TypeCustomer'],
    stores: ['dict.TypeCustomer'],
    views: ['dict.typecustomer.Grid'],

    mainView: 'dict.typecustomer.Grid',
    mainViewSelector: 'typecustomergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typecustomergrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typecustomergrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeCustomer').load();
    }
});