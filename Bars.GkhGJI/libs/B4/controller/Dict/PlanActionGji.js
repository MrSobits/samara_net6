Ext.define('B4.controller.dict.PlanActionGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.PlanActionGji'],
    stores: ['dict.PlanActionGji'],
    views: ['dict.planactiongji.Grid'],

    mainView: 'dict.planactiongji.Grid',
    mainViewSelector: 'planActionGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'planActionGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'planActionGjiGrid',
            permissionPrefix: 'GkhGji.Dict.PlanActionGji'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'planActionGjiGridAspect',
            storeName: 'dict.PlanActionGji',
            modelName: 'dict.PlanActionGji',
            gridSelector: 'planActionGjiGrid',
            listeners: {
                beforesave: function (me, store) {
                    var result = true;
                    Ext.each(store.getModifiedRecords(), function (rec) {
                        if (!rec.get('Name') || !rec.get('DateStart') || !rec.get('DateEnd')) {
                            result = false;
                            return;
                        }
                    });

                    if (!result) {
                        Ext.Msg.alert('Ошибка сохранения!', 'Для каждой записи необходимо заполнить все поля!');
                    }

                    return result;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('planActionGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PlanActionGji').load();
    }
});