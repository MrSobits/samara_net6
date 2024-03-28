Ext.define('B4.controller.constructionobject.smr.Progress', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.constructionobject.smr.Progress'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['constructionobject.TypeWork'],
    stores: ['constructionobject.TypeWork'],
    views: [
        'constructionobject.smr.ProgressGrid',
        'constructionobject.smr.ProgressEditWindow'
    ],

    mainView: 'constructionobject.smr.ProgressGrid',
    mainViewSelector: 'constructionobjsmrprogressgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjsmrprogressgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectsmrprogresspermission',
            name: 'smrProgressPermissionAspect'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела ход выполнения работы
            */
            xtype: 'grideditwindowaspect',
            name: 'smrProgressGridAspect',
            modelName: 'constructionobject.TypeWork',
            gridSelector: 'constructionobjsmrprogressgrid',
            editFormSelector: 'constructionobjsmrprogresseditwindow',
            editWindowView: 'constructionobject.smr.ProgressEditWindow',

            listeners: {
                'aftersetformdata': function(asp, rec, form) {
                    var volumeOfCompletion = form.down('[name=VolumeOfCompletion]');

                    volumeOfCompletion.setMaxValue(rec.get('Volume'));
                },
                beforesave: function (asp, rec) {
                    if (rec.get('CostSum') > rec.get('Sum')) {
                        Ext.Msg.alert('Ошибка!', 'Сумма расходов не может быть больше чем плановая сумма');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjsmrprogressgrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('smrProgressPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

        view.getStore().load();
    },

    init: function () {
        var actions = {};

        actions[this.mainViewSelector] = {
            'store.beforeload': {
                fn: this.onBeforeTypeWorkLoad,
                scope: this
            }
        };

        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeTypeWorkLoad: function (_, opts) {
        var view = this.getMainView();
        if (view) {
            opts.params.objectId = this.getContextValue(view, 'constructionObjectId');
        }
    }
});