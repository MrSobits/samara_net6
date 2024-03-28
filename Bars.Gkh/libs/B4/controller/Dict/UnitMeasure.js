Ext.define('B4.controller.dict.UnitMeasure', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'unitMeasureGrid',
            permissionPrefix: 'Gkh.Dictionaries.UnitMeasure'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'unitMeasureGridAspect',
            storeName: 'dict.UnitMeasure',
            modelName: 'dict.UnitMeasure',
            gridSelector: 'unitMeasureGrid'
        }
    ],
    
    models: ['dict.UnitMeasure'],
    stores: ['dict.UnitMeasure'],
    views: ['dict.unitmeasure.Grid'],

    mainView: 'dict.unitmeasure.Grid',
    mainViewSelector: 'unitMeasureGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'unitMeasureGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('unitMeasureGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.UnitMeasure').load();
    }
});