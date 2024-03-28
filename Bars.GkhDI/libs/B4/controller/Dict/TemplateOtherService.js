Ext.define('B4.controller.dict.TemplateOtherService', {
    extend: 'B4.base.Controller',

    requires:
        [
            'B4.aspects.GridEditWindow',
            'B4.aspects.permission.dict.TemplateOtherService'
        ],

    models:
        [
            'dict.TemplateOtherService'
        ],
    stores:
        [
            'dict.TemplateOtherService'
        ],
    views: [
        'dict.templateotherservice.Grid',
        'dict.templateotherservice.EditWindow'
    ],

    mainView: 'dict.templateotherservice.Grid',
    mainViewSelector: 'templateotherservicegrid',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'templateotherserviceperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'templateOtherServiceGridWindowAspect',
            gridSelector: 'templateotherservicegrid',
            editFormSelector: 'templateotherserviceeditwindow',
            storeName: 'dict.TemplateOtherService',
            modelName: 'dict.TemplateOtherService',
            editWindowView: 'dict.templateotherservice.EditWindow'
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('dict.TemplateOtherService').load();
    }
});