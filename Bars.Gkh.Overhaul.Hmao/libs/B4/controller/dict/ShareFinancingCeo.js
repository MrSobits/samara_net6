Ext.define('B4.controller.dict.ShareFinancingCeo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.sharefinancingceo.Grid',
        'B4.view.dict.sharefinancingceo.EditWindow',

        'B4.store.dict.ShareFinancingCeo',
        'B4.model.dict.ShareFinancingCeo',

        'B4.aspects.GridEditCtxWindow',

        'B4.aspects.permission.GkhGridPermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'dict.ShareFinancingCeo'
    ],

    models: [
        'dict.ShareFinancingCeo'
    ],

    views: [
        'dict.sharefinancingceo.Grid',
        'dict.sharefinancingceo.EditWindow'
    ],
    
    mainView: 'dict.sharefinancingceo.Grid',
    mainViewSelector: 'sharefinancingceopanel',
    aspects: [
        {
            xtype: 'grideditctxwindowaspect',
            name: 'sharefinancingceoGridAspect',
            gridSelector: 'sharefinancingceopanel',
            editFormSelector: 'sharefinancingceowindow',
            storeName: 'dict.ShareFinancingCeo',
            modelName: 'dict.ShareFinancingCeo',
            editWindowView: 'dict.sharefinancingceo.EditWindow'
        },
        {
            xtype: 'gkhgridpermissionaspect',
            gridSelector: 'sharefinanceceopanel',
            permissionPrefix: 'Ovrhl.Dictionaries.ShareFinancingCeo'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.Dictionaries.ShareFinancingCeo.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'sharefinancingceowindow'
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'sharefinancingceopanel' }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('sharefinancingceopanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
        this.reloadYears();
    },

    init: function() {

        this.control({
            'sharefinancingceowindow b4selectfield[name=CommonEstateObject]': {
                beforeload: {
                    fn: function(store, operation) {
                        operation.params = operation.params || {};

                        operation.params.hideNotIncluded = true;
                    }
                }
            }
        });

        this.callParent(arguments);
    }
});