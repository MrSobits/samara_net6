Ext.define('B4.controller.constructionobject.smr.Schedule', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.constructionobject.smr.Schedule'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['constructionobject.TypeWork'],
    stores: ['constructionobject.TypeWork'],
    views: [
        'constructionobject.smr.ScheduleGrid'
    ],

    mainView: 'constructionobject.smr.ScheduleGrid',
    mainViewSelector: 'constructionobjsmrschedulegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjsmrschedulegrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectsmrschedulepermission',
            name: 'smrSchedulePermissionAspect'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'smrScheduleGridAspect',
            modelName: 'constructionobject.TypeWork',
            gridSelector: 'constructionobjsmrschedulegrid',
            listeners: {
                'beforesave': function(asp, store) {
                    var errors = [];

                    store.each(function (r) {
                        var workName = r.get('WorkName'),
                            dateStartWork = r.get('DateStartWork'),
                            dateEndWork = r.get('DateEndWork'),
                            deadline = r.get('Deadline'),
                            formattedDeadline = Ext.Date.format(deadline, 'd.m.Y');

                        if (dateStartWork > dateEndWork && dateEndWork != null) {
                            errors.push('Дата начала работ <b>' + workName + '</b> должна быть меньше даты окончания работ');
                        }
                        if (deadline) {
                            if (dateStartWork > deadline) {
                                errors.push('Дата начала работ <b>' + workName + '</b> не может быть больше предельного срока окончания - ' + formattedDeadline);
                            }
                            if (dateEndWork > deadline) {
                                errors.push('Дата окончания работ <b>' + workName + '</b> не может быть больше предельного срока окончания - ' + formattedDeadline);
                            }
                        }
                    });

                    if (errors.length > 0) {
                        Ext.Msg.alert('Ошибка', errors.join('<br/>'));
                        return false;
                    }
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjsmrschedulegrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('smrSchedulePermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

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