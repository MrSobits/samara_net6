Ext.define('B4.controller.dict.Official', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.Official'],
    stores: ['dict.Official', 'administration.Operator'],
    views: [
        'dict.official.Grid',
        'dict.official.EditWindow'
    ],

    mainView: 'dict.official.Grid',
    mainViewSelector: 'officialgrid',

    refs: [{
        ref: 'mainView',
        selector: 'officialgrid'
    }],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.Dict.Official.Create', applyTo: 'b4addbutton', selector: 'officialgrid' },
                { name: 'GkhCr.Dict.Official.Edit', applyTo: 'b4savebutton', selector: 'officialwin' },
                {
                    name: 'GkhCr.Dict.Official.Delete', applyTo: 'b4deletecolumn', selector: 'officialgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gridEditWindow',
            gridSelector: 'officialgrid',
            storeName: 'dict.Official',
            modelName: 'dict.Official',
            editFormSelector: 'officialwin',
            editWindowView: 'dict.official.EditWindow'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('officialgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});