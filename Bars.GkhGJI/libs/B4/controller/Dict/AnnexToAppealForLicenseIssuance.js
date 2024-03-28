Ext.define('B4.controller.dict.AnnexToAppealForLicenseIssuance', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.AnnexToAppealForLicenseIssuance'],
    stores: ['dict.AnnexToAppealForLicenseIssuance'],

    views: ['dict.annextoappealforlicenseissuance.Grid'],

    mainView: 'dict.annextoappealforlicenseissuance.Grid',
    mainViewSelector: 'annextoappealforlicenseissuancegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'annextoappealforlicenseissuancegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'annextoappealforlicenseissuancegrid',
            permissionPrefix: 'GkhGji.Dict.AnnexToAppealForLicenseIssuance',
            permissions: [
                {
                    name: 'Create',
                    applyTo: 'b4addbutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Delete',
                    applyTo: 'b4deletecolumn',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Edit',
                    applyTo: 'b4savebutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'annextoappealforlicenseissuancegrid',
            name: 'annexToAppealForLicenseIssuanceGridAspect',
            storeName: 'dict.AnnexToAppealForLicenseIssuance',
            modelName: 'dict.AnnexToAppealForLicenseIssuance'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('annextoappealforlicenseissuancegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.AnnexToAppealForLicenseIssuance').load();
    }
});