Ext.define('B4.controller.dict.MeasuresReduceCosts', {
    extend: 'B4.base.Controller',
    requires: [
       'B4.aspects.InlineGrid',
       'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.MeasuresReduceCosts'],
    stores: ['dict.MeasuresReduceCosts'],
    views: ['dict.measuresreducecosts.Grid'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.measuresreducecosts.Grid',
    mainViewSelector: 'measuresReduceCostsGrid',

    refs: [{
        ref: 'mainView',
        selector: 'measuresReduceCostsGrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'measuresReduceCostsGrid',
            permissionPrefix: 'GkhDi.Dict.MeasuresReduceCosts'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'measuresReduceCostsGridAspect',
            storeName: 'dict.MeasuresReduceCosts',
            modelName: 'dict.MeasuresReduceCosts',
            gridSelector: 'measuresReduceCostsGrid'
        }],

    index: function () {
        var view = this.getMainView() || Ext.widget('measuresReduceCostsGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.MeasuresReduceCosts').load();
    }
});