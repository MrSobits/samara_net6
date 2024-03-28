Ext.define('B4.controller.dict.AnswerContentGji', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.AnswerContentGji'],
    stores: ['dict.AnswerContentGji'],

    views: ['dict.answercontentgji.Grid'],

    mainView: 'dict.answercontentgji.Grid',
    mainViewSelector: 'answerContentGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'answerContentGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'answerContentGjiGrid',
            permissionPrefix: 'GkhGji.Dict.AnswerContent'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'answerContentGjiGridAspect',
            storeName: 'dict.AnswerContentGji',
            modelName: 'dict.AnswerContentGji',
            gridSelector: 'answerContentGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('answerContentGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.AnswerContentGji').load();
    }
});