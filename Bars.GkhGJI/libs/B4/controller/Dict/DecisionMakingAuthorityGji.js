Ext.define('B4.controller.dict.DecisionMakingAuthorityGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.DecisionMakingAuthorityGji'],
    stores: ['dict.DecisionMakingAuthorityGji'],

    views: ['dict.decisionmakingauthoritygji.Grid'],

    mainView: 'dict.decisionmakingauthoritygji.Grid',
    mainViewSelector: 'decisionMakingAuthorityGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'decisionMakingAuthorityGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'decisionMakingAuthorityGjiGrid',
            permissionPrefix: 'GkhGji.Dict.DecisionMakingAuthorityGji'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'decisionMakingAuthorityGjiGridAspect',
            storeName: 'dict.DecisionMakingAuthorityGji',
            modelName: 'dict.DecisionMakingAuthorityGji',
            gridSelector: 'decisionMakingAuthorityGjiGrid'
        }
    ],

    index: function() {
        var view = this.getMainView() || Ext.widget('decisionMakingAuthorityGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.DecisionMakingAuthorityGji').load();
    }
});