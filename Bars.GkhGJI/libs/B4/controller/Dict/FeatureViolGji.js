Ext.define('B4.controller.dict.FeatureViolGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.FeatureViolGji'],
    stores: ['dict.FeatureViolGji'],

    views: ['dict.featureviolgji.Grid'],

    mainView: 'dict.featureviolgji.Grid',
    mainViewSelector: 'featureViolGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'featureViolGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'featureViolGjiGrid',
            permissionPrefix: 'GkhGji.Dict.FeatureViol'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'featureViolGjiGridGridAspect',
            storeName: 'dict.FeatureViolGji',
            modelName: 'dict.FeatureViolGji',
            gridSelector: 'featureViolGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('featureViolGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.FeatureViolGji').load();
    }
});