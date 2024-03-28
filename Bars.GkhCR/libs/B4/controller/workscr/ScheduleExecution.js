Ext.define('B4.controller.workscr.ScheduleExecution', {
    /*
    * Контроллер раздела график выполнения работ
    */
    extend: 'B4.base.Controller',

    requires:
    [
        'B4.aspects.permission.GkhStatePermissionAspect',
  'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr'],
    stores: ['objectcr.ScheduleExecutionWork'],
    views: ['objectcr.ScheduleExecutionWorkGrid',
            'objectcr.ScheduleExecutionWorkEditWindow',
            'objectcr.scheduleexecutionwork.AddDateGrid',
            'objectcr.scheduleexecutionwork.AddDateWindow'],

    mixins: {
         context: 'B4.mixins.Context',
         mask: 'B4.mixins.MaskBody'
    },
    mainView: 'objectcr.ScheduleExecutionWorkGrid',
    mainViewSelector: 'scheduleexecutionworkgrid',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'scheduleExecutionWorkTypeWorkCrPerm',
            editFormAspectName: 'scheduleExecutionWorkGridAspect',
            permissions: [
                    { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'scheduleexecutionworkgrid' },
                    { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate', applyTo: 'button[name=additionalDateButton]', selector: 'scheduleexecutionworkgrid' }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела график выполнения работ
            */
            xtype: 'grideditwindowaspect',
            name: 'scheduleExecutionWorkGridAspect',
            storeName: 'objectcr.ScheduleExecutionWork',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'scheduleexecutionworkgrid',
            editFormSelector: 'scheduleExecutionWorkEditWindow',
            editWindowView: 'objectcr.ScheduleExecutionWorkEditWindow',
            otherActions: function (actions) {
                actions['scheduleexecutionworkgrid button[name=additionalDateButton]'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
            },
            onAddDateButtonClick: function () {
                var me = this,
                    editWindow = me.componentQuery('#scheduleExecutionWorkAddDateWindow');

                if (editWindow && !editWindow.getBox().width) {
                    editWindow = editWindow.destroy();
                }

                if (!editWindow) {
                    editWindow = me.controller.getView('objectcr.scheduleexecutionwork.AddDateWindow').create(
                        {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });

                    editWindow.show();
                }
            }
        },
        //{
        //    /*
        //    * Аспект взаимодействия таблицы и формы редактирования раздела график выполнения работ
        //    */
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'scheduleExecutionWorkGridAspect',
        //    storeName: 'objectcr.ScheduleExecutionWork',
        //    modelName: 'objectcr.TypeWorkCr',
        //    gridSelector: 'scheduleexecutionworkgrid',
        //    otherActions: function (actions) {
        //        actions['scheduleexecutionworkgrid #additionalDateButton'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
        //    },
        //    onAddDateButtonClick: function () {
        //        var me = this,
        //            editWindow = me.componentQuery('#scheduleExecutionWorkAddDateWindow');

        //        if (editWindow && !editWindow.getBox().width) {
        //            editWindow = editWindow.destroy();
        //        }

        //        if (!editWindow) {
        //            editWindow = me.controller.getView('objectcr.scheduleexecutionwork.AddDateWindow').create(
        //                {
        //                    renderTo: B4.getBody().getActiveTab().getEl()
        //                });

        //            editWindow.show();
        //        }
        //    }
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionWorkAddDateGridAspect',
            storeName: 'objectcr.ScheduleExecutionWork',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'schexworkdategrid',
            otherActions: function (actions) {
                var me = this; 
                actions['schexworkdategrid b4closebutton'] = { 'click': { fn: me.onCloseButtonClick, scope: me } };
                actions['schexworkdategrid #fillDateButton'] = { 'click': { fn: me.onFillDateButtonClick, scope: me } };
            },
            onCloseButtonClick: function (btn) {
                var editWindow = btn.up('#scheduleExecutionWorkAddDateWindow');
                if (editWindow)
                    editWindow.close();
            },
            onFillDateButtonClick: function () {
                var asp = this;
                var window = new Ext.window.Window({
                    title: 'Выберите доп. срок:',
                    width: 220,
                    bodyPadding: 10,
                    itemId: 'datePickWindow',
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    items: [{
                        xtype: 'datepicker',
                        listeners: {
                            select: function (datpick, date) {
                                Ext.Msg.confirm('Внимание', 'Вы уверены, что хотите изменить дополнительный срок для каждого вида работ? ',  function (confirmationResult) {
                                    if (confirmationResult == 'yes') {
                                        var store = asp.getGrid().getStore();
                                        store.each(function (record) {
                                            record.set('AdditionalDate', date);
                                        });
                                        
                                        datpick.up('#datePickWindow').close();
                                    } 
                                });
                            }
                        }
                    }]
                });

                window.show();

            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions[me.mainViewSelector] = { 'afterrender': { fn: me.onMainViewAfterRender, scope: me } };
        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('scheduleexecutionworkgrid');

            view.getStore().on('beforeload',
                function (s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                }, me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
    },

    onMainViewAfterRender: function () {
        var me = this,
            obj,
            aspect = me.getAspect('scheduleExecutionWorkTypeWorkCrPerm'),
            model = me.getModel('objectcr.MonitoringSmr');

        if (me.params) {
            me.mask('Загрузка', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
                objectCrId: me.params.get('Id')
            })).next(function (response) {
                obj = Ext.JSON.decode(response.responseText);

                model.load(obj.MonitoringSmrId, {
                    success: function (rec) {
                        aspect.setPermissionsByRecord(rec);
                    },
                    scope: me
                });
                me.unmask();
                return true;
            }).error(function () {
                me.unmask();
                Ext.Msg.alert('Сообщение', 'Произошла ошибка');
            });
        }
    }
});