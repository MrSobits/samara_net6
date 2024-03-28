Ext.define('B4.controller.dict.StageWorkCr', {
    /*
    * Контроллер раздела этапы работ
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: [
        'dict.StageWorkCr'
    ],
    
    stores: [
        'dict.StageWorkCr'
    ],
    
    views: [
        'dict.stageworkcr.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.stageworkcr.Grid',
    mainViewSelector: 'stageWorkCrGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'stageWorkCrGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'stageWorkCrGrid',
            permissionPrefix: 'GkhCr.Dict.StageWorkCr'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'stageWorkCrAspect',
            storeName: 'dict.StageWorkCr',
            modelName: 'dict.StageWorkCr',
            gridSelector: 'stageWorkCrGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('stageWorkCrGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.StageWorkCr').load();
    }
});