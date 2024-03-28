Ext.define('B4.controller.realityobj.Room', {
    extend: 'B4.controller.MenuItemController',
    params: null,

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.realityobj.RoomEditWindow',
        'B4.aspects.permission.realityobj.Room'
    ],

    views: ['realityobj.RoomGrid'],
    models: ['Room'],
    stores: ['realityobj.Room'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realobjroomgrid'
        },
        {
            ref: 'areaChangeValBtn',
            selector: 'roomeditwindow changevalbtn'
        },
        {
            ref: 'areaField',
            selector: 'roomeditwindow numberfield[name="Area"]'
        },
        {
            ref: 'isRoomCommonPropertyInMcdField',
            selector: 'roomeditwindow checkbox[name="IsRoomCommonPropertyInMcd"]'
        },
        {
            ref: 'logGrid',
            selector: 'roomeditwindow entityloglightgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'roomperm',
            name: 'roomPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'realobjroomgrid',
            storeName: 'realityobj.Room',
            modelName: 'Room',
            editWindowView: 'realityobj.RoomEditWindow',
            editFormSelector: 'roomeditwindow',
            otherActions: function (actions) {
                actions['roomeditwindow #cbIsRoomHasNoNumber'] = { 'change': { fn: this.controller.setRoomNumber, scope: this } };
            },
            listeners: {
                beforesave: function(asp, rec) {
                    if (rec.get('LivingArea') > rec.get('Area')) {
                        Ext.Msg.alert('Ошибка!', 'Жилая площадь не может быть больше общей площади!');
                        return false;
                    }
                    return true;
                },
                getdata: function(me, record) {
                    if (me.controller.params && (+record.get('Id') == 0)) {
                        record.set('RealityObject', { Id: me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId') });
                    }
                },
                aftersetformdata: function (asp, record) {
                    var me = this;
                    
                    var id = record.get('Id');
                    if (+id) {
                        //asp.controller.getAreaChangeValBtn().setVisible(true);
                        asp.controller.getAreaChangeValBtn().setEntityId(id);
                        asp.controller.getLogGrid().enable();
                    } else {
                        asp.controller.getAreaChangeValBtn().setVisible(false);
                        asp.controller.getAreaField().setEditable(true);
                    }
                    
                    if (me.controller.params) {
                        me.controller.params.roomId = id;
                    } else {
                        me.controller.params = {};
                        me.controller.params.roomId = id;
                    }
                    
                    asp.controller.setRoomNumber();

                    asp.controller.getIsRoomCommonPropertyInMcdField().setVisible(record.data.Type == 20);
                }
            }
        }
    ],
    
    init: function () {
        var me = this;
        me.getStore('realityobj.Room').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realobjroomgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getStore('realityobj.Room').load();
        me.getAspect('roomPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    setRoomNumber: function (id) {
        var me = this;
        if (me.controller) {
            me = me.controller;
        }

        var tfRoomNum = Ext.ComponentQuery.query('roomeditwindow #tfRoomNum');
        var cbIsRoomHasNoNumber = Ext.ComponentQuery.query('roomeditwindow #cbIsRoomHasNoNumber');

        if (tfRoomNum.length > 0 && cbIsRoomHasNoNumber.length > 0) {

            var cbVal = cbIsRoomHasNoNumber[0].value;

            if (cbVal) {
                tfRoomNum[0].setReadOnly(true);
            } else {
                tfRoomNum[0].setReadOnly(false);
            }
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.realtyId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});