Ext.define('B4.controller.specialobjectcr.ScheduleExecutionWork', {
    /*
    * Контроллер раздела график выполнения работ
    */
    extend: 'B4.controller.MenuItemController',

    requires:
    [
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhInlineGrid'
    ],

    models: [
        'specialobjectcr.TypeWorkCr',
        'specialobjectcr.MonitoringSmr'
    ],
    stores: [
        'specialobjectcr.ScheduleExecutionWork'
    ],
    views: [
        'specialobjectcr.ScheduleExecutionWorkGrid',
        'specialobjectcr.scheduleexecutionwork.AddDateGrid',
        'specialobjectcr.scheduleexecutionwork.AddDateWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'specialobjectcr.ScheduleExecutionWorkGrid',
    mainViewSelector: 'specialobjectcrscheduleexecutionworkgrid',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'scheduleExecutionWorkObjectCrPerm',
            editFormAspectName: 'scheduleExecutionWorkGridAspect',
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'specialobjectcrscheduleexecutionworkgrid'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate',
                    applyTo: 'button[name=additionalDateButton]',
                    selector: 'specialobjectcrscheduleexecutionworkgrid'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column.FinanceSource',
                    applyTo: '[dataIndex=FinanceSourceName]',
                    selector: 'specialobjectcrscheduleexecutionworkgrid',
                    applyBy: function(component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела график выполнения работ
            */
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionWorkGridAspect',
            modelName: 'specialobjectcr.TypeWorkCr',
            gridSelector: 'specialobjectcrscheduleexecutionworkgrid',
            otherActions: function (actions) {
                actions[this.gridSelector + ' button[name=additionalDateButton]'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
            },
            onAddDateButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionSpecialWorkAddDateWindow');

                if (editWindow && !editWindow.getBox().width) {
                    editWindow = editWindow.destroy();
                }

                if (!editWindow) {
                    editWindow = this.controller.getView('specialobjectcr.scheduleexecutionwork.AddDateWindow').create(
                        {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });

                    editWindow.show();
                }

                var store = editWindow.down('grid').getStore();
                store.clearFilter(true);
                store.filter('objectCrId', this.controller.getContextValue(this.controller.getMainView(), 'objectcrId'));
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionWorkAddDateGridAspect',
            modelName: 'specialobjectcr.TypeWorkCr',
            gridSelector: 'specialobjectcrschexworkdategrid',
            otherActions: function(actions) {
                actions[this.gridSelector + ' b4closebutton'] = { 'click': { fn: this.onCloseButtonClick, scope: this } };
                actions[this.gridSelector + ' #fillDateButton'] = { 'click': { fn: this.onFillDateButtonClick, scope: this } };
            },
            onCloseButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionSpecialWorkAddDateWindow');
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
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrscheduleexecutionworkgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },

    onMainViewAfterRender: function () {
        var aspect = this.getAspect('scheduleExecutionWorkObjectCrPerm'),
            objectCrId = aspect.controller.getContextValue(aspect.controller.getMainComponent(), 'objectcrId');
        this.mask('Загрузка', this.getMainComponent());

        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'SpecialMonitoringSmr', {
            objectCrId: objectCrId
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            var model = this.getModel('specialobjectcr.MonitoringSmr');

            model.load(obj.MonitoringSmrId, {
                success: function (rec) {
                    aspect.setPermissionsByRecord(rec);
                },
                scope: this
            });
            this.unmask();
            return true;
        }, this).error(function () {
            this.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        }, this);
    }
});