Ext.define('B4.controller.objectcr.ScheduleExecutionWork', {
    /*
    * Контроллер раздела график выполнения работ
    */
    extend: 'B4.controller.MenuItemController',

    requires:
        [
            'B4.aspects.permission.GkhStatePermissionAspect',
            'B4.aspects.GkhInlineGrid'
        ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr'],
    stores: ['objectcr.ScheduleExecutionWork'],
    views: ['objectcr.ScheduleExecutionWorkGrid',
        'objectcr.scheduleexecutionwork.AddDateGrid',
        'objectcr.scheduleexecutionwork.AddDateWindow'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'objectcr.ScheduleExecutionWorkGrid',
    mainViewSelector: 'scheduleexecutionworkgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'scheduleExecutionWorkObjectCrPerm',
            editFormAspectName: 'scheduleExecutionWorkGridAspect',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'scheduleexecutionworkgrid' },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate', applyTo: 'button[name=additionalDateButton]', selector: 'scheduleexecutionworkgrid' },
                {
                    name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column.FinanceSource',
                    applyTo: '[dataIndex=FinanceSourceName]',
                    selector: 'scheduleexecutionworkgrid',
                    applyBy: function (component, allowed) {
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
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'scheduleexecutionworkgrid',
            otherActions: function (actions) {
                actions['scheduleexecutionworkgrid button[name=additionalDateButton]'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
            },
            onAddDateButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionWorkAddDateWindow');

                if (editWindow && !editWindow.getBox().width) {
                    editWindow = editWindow.destroy();
                }

                if (!editWindow) {
                    editWindow = this.controller.getView('objectcr.scheduleexecutionwork.AddDateWindow').create(
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
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'schexworkdategrid',
            otherActions: function(actions) {
                actions['schexworkdategrid b4closebutton'] = { 'click': { fn: this.onCloseButtonClick, scope: this } };
                actions['schexworkdategrid #fillDateButton'] = { 'click': { fn: this.onFillDateButtonClick, scope: this } };
            },
            onCloseButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionWorkAddDateWindow');
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
            view = me.getMainView() || Ext.widget('scheduleexecutionworkgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },

    onMainViewAfterRender: function () {
        var aspect = this.getAspect('scheduleExecutionWorkObjectCrPerm'),
            objectCrId = aspect.controller.getContextValue(aspect.controller.getMainComponent(), 'objectcrId');
        this.mask('Загрузка', this.getMainComponent());
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
            objectCrId: objectCrId
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            var model = this.getModel('objectcr.MonitoringSmr');

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