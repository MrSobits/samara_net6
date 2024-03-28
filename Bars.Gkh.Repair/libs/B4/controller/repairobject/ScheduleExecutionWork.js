Ext.define('B4.controller.repairobject.ScheduleExecutionWork', {
    /*
    * Контроллер раздела график выполнения работ объекта текущего ремонта
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['repairobject.RepairWork'],
    stores: ['repairobject.ScheduleExecutionWork'],
    views: ['repairobject.scheduleexecutionwork.Grid',
            'repairobject.scheduleexecutionwork.AddDateGrid',
            'repairobject.scheduleexecutionwork.AddDateWindow'],

    mixins: { mask: 'B4.mixins.MaskBody' },
    mainView: 'repairobject.scheduleexecutionwork.Grid',
    mainViewSelector: 'scheduleExecutionRepairWorkGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'scheduleExecutionRepairWorkGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'scheduleExecutionWorkPerm',
            permissions: [
                { name: 'GkhRepair.RepairObject.ScheduleExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'scheduleExecutionRepairWorkGrid' }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionRepairWorkGridAspect',
            storeName: 'repairobject.ScheduleExecutionWork',
            modelName: 'repairobject.RepairWork',
            gridSelector: 'scheduleExecutionRepairWorkGrid',
            otherActions: function (actions) {
                actions['scheduleExecutionRepairWorkGrid button[name=additionalDateButton]'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
            },
            onAddDateButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionRepairWorkAddDateWindow');

                if (editWindow && !editWindow.getBox().width) {
                    editWindow = editWindow.destroy();
                }

                if (!editWindow) {
                    editWindow = this.controller.getView('repairobject.scheduleexecutionwork.AddDateWindow').create(
                        {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });

                    editWindow.show();
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionRepairWorkAddDateGridAspect',
            storeName: 'repairobject.ScheduleExecutionWork',
            modelName: 'repairobject.RepairWork',
            gridSelector: '#scheduleExecutionRepairWorkAddDateGrid',
            otherActions: function (actions) {
                actions['#scheduleExecutionRepairWorkAddDateGrid b4closebutton'] = { 'click': { fn: this.onCloseButtonClick, scope: this } };
                actions['#scheduleExecutionRepairWorkAddDateGrid #fillDateButton'] = { 'click': { fn: this.onFillDateButtonClick, scope: this } };
            },
            onCloseButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionRepairWorkAddDateWindow');
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
                                Ext.Msg.confirm('Внимание', 'Вы уверены, что хотите изменить дополнительный срок для каждого вида работ? ', function (confirmationResult) {
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
        this.getStore('repairobject.ScheduleExecutionWork').on('beforeload', this.onBeforeLoad, this);
        var actions = {};
        this.control(actions);
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('repairobject.ScheduleExecutionWork').load();
        if (this.params) {
            this.getAspect('scheduleExecutionWorkPerm').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.repairObjectId = this.params.get('Id');
        }
    }
});