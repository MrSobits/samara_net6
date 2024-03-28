Ext.define('B4.controller.dict.PlanJurPersonGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.PlanJurPersonGji'],
    stores: ['dict.PlanJurPersonGji'],
    views: ['dict.planjurpersongji.Grid'],

    mainView: 'dict.planjurpersongji.Grid',
    mainViewSelector: 'planJurPersonGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'planJurPersonGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'planJurPersonGjiGrid',
            permissionPrefix: 'GkhGji.Dict.PlanJurPerson'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'planJurPersonGjiGridAspect',
            storeName: 'dict.PlanJurPersonGji',
            modelName: 'dict.PlanJurPersonGji',
            gridSelector: 'planJurPersonGjiGrid',
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
        var view = this.getMainView() || Ext.widget('planJurPersonGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PlanJurPersonGji').load();
    }
});