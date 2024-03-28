Ext.define('B4.controller.objectcr.DesignAssignment', {
    /*
    * Контроллер раздела задание на проектирование
    */
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.objectcr.BuildContract',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'objectcr.DesignAssignment'
    ],
    stores: [
        'objectcr.DesignAssignment'

    ],
    views: [
        'objectcr.DesignAssignmentEditWindow',
        'objectcr.DesignAssignmentGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'objectcr.DesignAssignmentGrid',
    mainViewSelector: 'designassignmentgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [

        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'designassignmentStateTransferAspect',
            gridSelector: 'designassignmentgrid',
            stateType: 'cr_obj_design_assignment',
            menuSelector: 'designassignmentGridStateMenu'
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'designassignmentStateButtonAspect',
            stateButtonSelector: 'designassignmenteditwindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState).updateGrid();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования договоров подряда
            */
            xtype: 'grideditctxwindowaspect',
            name: 'designassignmentgridWindowAspect',
            gridSelector: 'designassignmentgrid',
            editFormSelector: 'designassignmenteditwindow',
            modelName: 'objectcr.DesignAssignment',
            editWindowView: 'objectcr.DesignAssignmentEditWindow',
            otherActions: function(actions) {
                actions['designassignmenteditwindow #daTypeWorkCr'] = { 'beforeload': { fn: this.onBeforeLoadTypeWork, scope: this } };
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.ObjectCr = me.controller.getContextValue(me.controller.getMainView(), 'objectcrId');
                    }
                    me.controller.getAspect('designassignmentStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                },
                onSaveSuccess: function(asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            },
            onBeforeLoadTypeWork: function (field, options) {
                options.params = {};
                options.params.objectCrId = this.controller.getContextValue(this.controller.getMainView(), 'objectcrId');
            }
        }
    ],

    index: function (id) {

        var me = this,
            store,
            view = me.getMainView() || Ext.widget('designassignmentgrid');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    }
});