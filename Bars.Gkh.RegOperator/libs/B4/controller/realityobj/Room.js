Ext.define('B4.controller.realityobj.Room', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.realityobj.RoomEditWindow',
        'B4.aspects.permission.realityobj.Room',
        'B4.aspects.Permission'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'realityobj.RoomGrid',
        'realityobj.RoomEditWindow',
        'realityobj.room.EntranceWindow',
        'realityobj.room.EntranceGrid'
    ],

    models: [
        'Room',
        'Entrance'
    ],

    stores: [
        'realityobj.Room',
        'realityobj.Entrance'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realobjroomgrid'
        },
        {
            ref: 'areaChangeValBtn',
            selector: 'roomeditwindow changevalbtn[propertyName=Area]'
        },
        {
            ref: 'ownershipTypeChangeValBtn',
            selector: 'roomeditwindow changevalbtn[propertyName=OwnershipType]'
        },
        {
            ref: 'roomNumChangeValBtn',
            selector: 'roomeditwindow changevalbtn[propertyName=RoomNum]'
        },
        {
            ref: 'chamberNumChangeValBtn',
            selector: 'roomeditwindow changevalbtn[propertyName=ChamberNum]'
        },
        {
            ref: 'areaField',
            selector: 'roomeditwindow numberfield[name="Area"]'
        },
        {
            ref: 'ownershipField',
            selector: 'roomeditwindow b4enumcombo[name="OwnershipType"]'
        },
        {
            ref: 'roomNumField',
            selector: 'roomeditwindow textfield[name="RoomNum"]'
        },
        {
            ref: 'isroomhasnonumber',
            selector: 'roomeditwindow checkbox[name="IsRoomHasNoNumber"]'
        },
        {
            ref: 'isRoomCommonPropertyInMcdField',
            selector: 'roomeditwindow checkbox[name="IsRoomCommonPropertyInMcd"]'
        },
        {
            ref: 'chamberNumField',
            selector: 'roomeditwindow textfield[name="ChamberNum"]'
        },
        {
            ref: 'logGrid',
            selector: 'roomeditwindow entityloglightgrid'
        },
        {
            ref: 'entranceWindow',
            selector: 'entranceselectwindow'
        }
    ],

    aspects: [
        {
            xtype: 'roomperm',
            name: 'roomPerm'
        },
        {
            xtype: 'permissionaspect',
            applyOn: {
                event: 'show',
                selector: 'roomeditwindow'
            },
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.CadastralNumber.Edit',
                    applyTo: 'roomeditwindow textfield[name=CadastralNumber]',
                    applyBy: function (component, enabled) {
                        component.setDisabled(!enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.CadastralNumber.View',
                    applyTo: 'roomeditwindow textfield[name=CadastralNumber]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.ChamberNum.Edit',
                    applyTo: 'roomeditwindow container[type=ChamberNum]',
                    applyBy: function (component, enabled) {
                        component.setDisabled(!enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.ChamberNum.View',
                    applyTo: 'roomeditwindow container[type=ChamberNum]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.ChamberNum.View',
                    applyTo: 'realobjroomgrid gridcolumn[dataIndex=ChamberNum]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.IsCommunal_View',
                    applyTo: 'roomeditwindow checkbox[name=IsCommunal]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.PrevAssignedRegNumber_View',
                    applyTo: 'roomeditwindow field[name=PrevAssignedRegNumber]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Tabs.InformationUnfitness_View',
                    applyTo: 'roomeditwindow [name=InformationUnfitness]',
                    applyBy: function (component, enabled) {
                        component.tab.setVisible(enabled);
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.CommunalArea_View',
                    applyTo: 'roomeditwindow [name=CommunalArea]',
                    applyBy: function (component, enabled) {
                        var communalAreaField = component.up('roomeditwindow').down('[name=IsCommunal]');
                        if (communalAreaField) {
                            component.setVisible(enabled && communalAreaField.getValue());
                        }
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Fields.IsRoomCommonPropertyInMcd_View',
                    applyTo: 'roomeditwindow checkbox[name=IsRoomCommonPropertyInMcd]',
                    applyBy: function (component, enabled) {
                        component.setVisible(enabled);
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realobjRoomGridEditAspect',
            gridSelector: 'realobjroomgrid',
            storeName: 'realityobj.Room',
            modelName: 'Room',
            editWindowView: 'realityobj.RoomEditWindow',
            editFormSelector: 'roomeditwindow',
            otherActions: function(actions) {
                var me = this;

                actions['roomeditwindow #cbIsRoomHasNoNumber'] = { 'change': { fn: me.controller.setRoomNumber, scope: me } };
                actions['roomeditwindow [name=IsCommunal]'] = { 'change': { fn: me.onIsCommunalChange, scope: me } };
                actions['roomeditwindow [name=RecognizedUnfit]'] = { 'change': { fn: me.onRecognizedUnfitChange, scope: me } };
                actions['roomeditwindow checkbox[name=HasSeparateEntrance]'] = { 'change': { fn: me.onHasSeparateEntrance, scope: me } };
                actions['roomeditwindow b4enumcombo[name=Type]'] = { 'change': { fn: me.onTypeRoom, scope: me } };
            },
            closeWindowHandler: function () {
                var me = this;
                me.controller.params = null;
                me.getForm().close();
            },
            editRecord: function (record, editRoomMode) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                model = this.getModel(record);

                id ? model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                }) : this.setFormData(new model({ Id: 0, RecognizedUnfit: B4.enums.YesNo.No }));

                this.getForm().getForm().isValid();
                this.getForm().editRoomMode = editRoomMode;
            },
            onSaveSuccess: function () {
                var form = this.getForm();
                if (form) {
                    form.close();
                }

            },
            listeners: {
                beforesave: function(asp, rec) {
                    if (rec.get('LivingArea') > rec.get('Area')) {
                        Ext.Msg.alert('Ошибка!', 'Жилая площадь не может быть больше общей площади!');
                        return false;
                    }
                    asp.controller.params = null;
                    return true;
                },
                getdata: function(me, record) {
                    if (!parseInt(record.get('Id'))) {
                        record.set('RealityObject', { Id: me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId') });
                    }
                },
                beforesetformdata: function (asp, record) {
                    var id = record.get('Id');
                    if (id == 0) {
                        record.set('OwnershipType', 70);
                    }
                },
                aftersetformdata: function (asp, record) {
                    var id = record.get('Id');
                    if (+id) {
                        //asp.controller.getAreaChangeValBtn().setVisible(true);
                        asp.controller.getAreaChangeValBtn().setEntityId(id);
                        //asp.controller.getOwnershipTypeChangeValBtn().setVisible(true);
                        asp.controller.getOwnershipTypeChangeValBtn().setEntityId(id);
                        //asp.controller.getRoomNumChangeValBtn().setVisible(true);
                        asp.controller.getRoomNumChangeValBtn().setEntityId(id);
                        asp.controller.getChamberNumChangeValBtn().setEntityId(id);
                        asp.controller.getLogGrid().enable();
                    } else {
                        asp.controller.getAreaChangeValBtn().setVisible(false);
                        asp.controller.getOwnershipTypeChangeValBtn().setVisible(false);
                        asp.controller.getRoomNumChangeValBtn().setVisible(false);
                        asp.controller.getChamberNumChangeValBtn().setVisible(false);
                        asp.controller.getAreaField().setEditable(true);
                        asp.controller.getRoomNumField().setReadOnly(false);
                        asp.controller.getChamberNumField().setReadOnly(false);
                        asp.controller.getOwnershipField().setReadOnly(false);
                    }

                    if (asp.controller.params) {
                        asp.controller.params.roomId = id;
                        asp.controller.params.IsRoomHasNoNumber = record.data.IsRoomHasNoNumber;
                        asp.controller.params.RoomNum = record.data.RoomNum;
                    } else {
                        asp.controller.params = {};
                        asp.controller.params.roomId = id;
                        asp.controller.params.IsRoomHasNoNumber = record.data.IsRoomHasNoNumber;
                        asp.controller.params.RoomNum = record.data.RoomNum;
                    }

                    if (asp.controller.getIsroomhasnonumber().value) {
                        this.componentQuery('roomeditwindow #tfRoomNum').setDisabled(true);
                    }
                }
            },

            onIsCommunalChange: function(field, newValue) {
                var communalAreaField = field.up('roomeditwindow').down('[name=CommunalArea]');
                if (communalAreaField) {
                    communalAreaField.setDisabled(!newValue);
                    communalAreaField.allowBlank = !newValue;
                    communalAreaField.setVisible(newValue);
                }
            },

            onRecognizedUnfitChange: function (field, newValue) {
                var me = this,
                    editWindow = field.up('roomeditwindow'),
                    fields = [
                        editWindow.down('[name=RecognizedUnfitReason]'),
                        editWindow.down('[name=RecognizedUnfitDocNumber]'),
                        editWindow.down('[name=RecognizedUnfitDocDate]'),
                        editWindow.down('[name=RecognizedUnfitDocFile]')
                    ];

                Ext.Array.each(fields, function (f) {
                    if (f) {
                        f.allowBlank = !(newValue === B4.enums.YesNo.Yes);
                        f.setDisabled(!(newValue === B4.enums.YesNo.Yes))
                    }
                }, me);
            },

            onHasSeparateEntrance: function (field, isChecked) {
                var panel = field.up('roomeditwindow'),
                    entranceField = panel.down('b4selectfield[name=Entrance]');

                if (entranceField) {
                    entranceField.setDisabled(isChecked);
                    entranceField.allowBlank = isChecked;
                }
                panel.getForm().isValid();
            },

            onTypeRoom: function (field, newValue) {
                var panel = field.up('roomeditwindow'),
                    entranceField = panel.down('b4selectfield[name=Entrance]'),
                    cbHasSeparateEntrance = panel.down('checkbox[name=HasSeparateEntrance]'),
                    isDisabled = newValue !== B4.enums.realty.RoomType.Living;

                if (entranceField && cbHasSeparateEntrance) {
                    entranceField.setDisabled(isDisabled);
                    cbHasSeparateEntrance.setDisabled(isDisabled);
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'roomeditwindow': {
                    afterrender: function (win) {
                        var valFielConfig;

                        B4.Ajax.request(B4.Url.action('GetParamByKey', 'RegoperatorParams', { key: 'TypeAccountNumber' }))
                            .next(function(resp) {
                                var obj = Ext.decode(resp.responseText),
                                    roomNumChngValBtn = win.down('changevalbtn[propertyName=RoomNum]'),
                                    roomNumTxtFld = win.down('textfield[name=RoomNum]');
                                if (obj.data == 10) {
                                    valFielConfig = {
                                        xtype: 'textfield',
                                        fieldLabel: 'Новое значение',
                                        maxLength: 100,
                                        maskRe: /[0-9]/
                                    };

                                    roomNumChngValBtn.valueFieldConfig = valFielConfig;
                                    roomNumTxtFld.maskRe = /[0-9]/;
                                } else {                                 
                                    valFielConfig = {
                                        xtype: 'textfield',
                                        fieldLabel: 'Новое значение',
                                        maxLength: 100,
                                        maskRe: /[\/\-a-zA-Zа-яА-Я0-9\,\.]/
                                    };

                                    roomNumChngValBtn.valueFieldConfig = valFielConfig;
                                    roomNumTxtFld.maskRe = /[\/\-a-zA-Zа-яА-Я0-9\,\.]/;
                                }
                            });
                    }
                },
                'roomeditwindow [action=redirecttopersonalaccount]': {
                    click: me.goToPersonalAccount
                },
                'roomeditwindow [action=redirecttoabonent]': {
                    click: me.goToAbonent
                },
                'realobjroomgrid [action=selectentrance]': {
                    'click': {
                        fn: me.onClickSelectEntrance,
                        scope: me
                    }
                },
                'isroomhasnonumber [action=change]': {
                    'change': {
                        fn: me.isRoomHasNoNumberChange,
                        scope: me
                    }
                },
                'entranceselectwindow b4savebutton': {
                    'click': {
                        fn: me.onClickSaveEntrance,
                        scope: me
                    }
                },
                'roomeditwindow [name=Entrance]': {
                    'beforeload': {
                        fn: me.onBeforeLoadEntrance,
                        scope: me
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },
    
    goToPersonalAccount: function() {
        var me = this,
            roomId = me.getAspect('realobjRoomGridEditAspect').controller.params.roomId,
            items = B4.getBody().items;

        var index = items.findIndexBy(function (tab) {
            return tab.urlToken != null && tab.urlToken.indexOf('regop_personal_account') === 0;
        });

        if (index != -1) {
            B4.getBody().remove(items.items[index], true);
        }

        me.application.getRouter().redirectTo('regop_personal_account/onlyroom/' + roomId);
    },

    goToAbonent: function () {
        var me = this,
            roomId = me.getAspect('realobjRoomGridEditAspect').controller.params.roomId,
            items = B4.getBody().items;

        var index = items.findIndexBy(function (tab) {
            return tab.urlToken != null && tab.urlToken.indexOf('regop_personal_acc_owner') === 0;
        });

        if (index != -1) {
            B4.getBody().remove(items.items[index], true);
        }

        me.application.getRouter().redirectTo('regop_personal_acc_owner/onlyroom/' + roomId);
    },

    setRoomNumber: function (id, newValue, oldValue, eOpts ) {
        var me = this,
            aspect = this;
        if (me.controller) {
            me = me.controller;
        }

        if (!me.params) {
            return;
        }

        var tfRoomNum = aspect.componentQuery('roomeditwindow #tfRoomNum');
        var cbIsRoomHasNoNumber = aspect.componentQuery('roomeditwindow #cbIsRoomHasNoNumber');

        tfRoomNum.setDisabled(cbIsRoomHasNoNumber.getValue());

        if (tfRoomNum && cbIsRoomHasNoNumber) {

            var cbVal = cbIsRoomHasNoNumber.value;

            me.getRoomNumChangeValBtn().setVisible(!cbVal);

            if (cbVal) {
                tfRoomNum.setReadOnly(true);
            }
        }
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView(),
            firstTime = !view,
            store;
        view = view || Ext.widget('realobjroomgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        if (firstTime) {
            store = view.getStore();
            store.on('beforeload', me.onBeforeLoad, me);
            store.load();
        }

        me.getAspect('roomPerm').setPermissionsByRecord({ getId: function () { return id; } });
        me.unmask();
    },

    edit: function (realityObjectId, roomId) {
        var me = this;
        me.index(realityObjectId);
        me.getAspect('realobjRoomGridEditAspect').editRecord(Ext.create('B4.model.Room', { Id: roomId }), true);
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.realtyId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },

    /*unmask: function () {
        return this.fireEvent("unmask");
    },*/

    onClickSelectEntrance: function() {
        var me = this,
            grid = me.getMainView(),
            selected = grid.getSelectionModel().selected,
            win,
            store,
            ids = [];

        if (!selected || selected.length <= 0) {
            B4.QuickMsg.msg('Внимание', 'Не выбраны помещения', 'warning');
            return;
        }

        Ext.each(selected.items, function(item) {
            ids.push(item.get('Id'));
        });

        win = me.getCmpInContext('entranceselectwindow');

        if (win) {
            win.close();
        }

        win = Ext.widget('entranceselectwindow', {
            renderTo: B4.getBody().getActiveTab().getEl(),
            ids: ids
        });

        win.show();

        store = win.down('entrancegrid').getStore();

        store.on('beforeload', me.onBeforeLoadEntrance, me);

        store.load();
    },

    onClickSaveEntrance: function(btn) {
        var me = this,
            win = btn.up('entranceselectwindow'),
            grid = win.down('entrancegrid'),
            selected = grid.getSelectionModel().selected;

        if (!selected || selected.length <= 0) {
            B4.QuickMsg.msg('Внимание', 'Не выбран подъезд', 'warning');
            return;
        }

        me.mask('Определение...', B4.getBody().getActiveTab());

        B4.Ajax.request({
            url: B4.Url.action('SetEntrance', 'Room'),
            params: {
                roomIds: Ext.encode(win.ids),
                entranceId: selected.items[0].get('Id')
            }
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Определение подъезда выполнено успешно', 'success');
            win.close();
            me.getMainView().getStore().load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message);
        });
    },

    onBeforeLoadEntrance: function(s, operation) {
        var me = this,
            grid = me.getMainView();

        operation.params.objectId = me.getContextValue(grid, 'realityObjectId');
    },

    isRoomHasNoNumberChange: function (val) {
        var roomfield = this.up('container').down('textfield[name=RoomNum]');
        roomfield.allowBlank = val;
    }
});