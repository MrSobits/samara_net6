Ext.define('B4.controller.dict.RiskCategory', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RiskCategory'],
    stores: ['dict.RiskCategory'],

    views: ['dict.riskcategory.Grid'],

    mainView: 'dict.riskcategory.Grid',
    mainViewSelector: 'riskCategoryGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'riskCategoryGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'riskCategoryGrid',
            permissionPrefix: 'GkhGji.Dict.RiskCategory'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'riskCategoryGridAspect',
            storeName: 'dict.RiskCategory',
            modelName: 'dict.RiskCategory',
            gridSelector: 'riskCategoryGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('riskCategoryGrid');

        B4.Ajax.request({
            url: B4.Url.action('ListItems', 'GkhConfig'),
            params: {
                parent: 'ErknmIntegrationConfig'
            }
        }).next(function (resp) {
            var res = Ext.JSON.decode(resp.responseText),
                RiskCategoryId = res.data.find(x => x.id == 'ErknmIntegrationConfig.RiskCategoryId').value;
            view.down('textfield[name=KnmTypeId]').setValue(RiskCategoryId);
        });

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RiskCategory').load();
    }
});