Ext.define('B4.controller.dict.ResolveViolationClaim', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['ResolveViolationClaim'],
    stores: ['dict.ResolveViolationClaim'],

    views: ['dict.resolveviolationclaim.Grid'],

    mainView: 'dict.resolveviolationclaim.Grid',
    mainViewSelector: 'resolveviolationclaimgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'resolveviolationclaimgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'resolveviolationclaimgrid',
            permissionPrefix: 'GkhGji.Dict.ResolveViolationClaim'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'resolveviolationclaimgrid',
            name: 'surveySubjectRequirementGridAspect',
            storeName: 'dict.ResolveViolationClaim',
            modelName: 'ResolveViolationClaim'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('resolveviolationclaimgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ResolveViolationClaim').load();
    }
});