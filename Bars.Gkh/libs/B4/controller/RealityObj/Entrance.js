Ext.define('B4.controller.realityobj.Entrance', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'Entrance'
    ],

    stores: [
        'realityobj.Entrance'
    ],

    views: [
        'realityobj.entrance.Grid',
        'realityobj.entrance.EditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjentrancegrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'entrancepermissionaspect',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.Entrance.Create',
                    applyTo: 'b4addbutton',
                    selector: 'realityobjentrancegrid'
                },
                {
                    name: 'Gkh.RealityObject.Register.Entrance.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'realityobjentrancewindow'
                },
                {
                    name: 'Gkh.RealityObject.Register.Entrance.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'realityobjentrancegrid',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjentranceGridWindowAspect',
            gridSelector: 'realityobjentrancegrid',
            editFormSelector: 'realityobjentrancewindow',
            modelName: 'Entrance',
            editWindowView: 'realityobj.entrance.EditWindow',
            listeners: {
                beforesave: function(asp, record) {
                    if (!record.getId()) {
                        record.set('RealityObject',
                            asp.controller.getContextValue(asp.controller.getMainView(), 'realityObjectId'));
                    }
                },
                aftersetformdata: function(asp, rec, form) {
                    var grid = form.down('[name=RoomGrid]'),
                        store = grid.getStore(),
                        entranceId = rec.getId(),
                        roId = asp.controller.getContextValue(asp.controller.getMainView(), 'realityObjectId');

                    store.on('beforeload', function(store, operation) {
                        Ext.apply(operation.params, {
                             entranceId: entranceId,
                             realtyId: roId
                        });
                    });

                    store.load();
                }
            },
            otherActions: function(actions) {
                var asp = this;
                actions[asp.editFormSelector + ' [name=RealEstateType]'] = {
                    'change': {
                        fn: asp.onRetChanged,
                        scope: asp
                    }
                };
            },

            onRetChanged: function(f, val) {
                var me = this,
                    roId = me.controller.getContextValue(me.controller.getMainView(), 'realityObjectId'),
                    retId = val ? val.Id : 0,
                    tariff = me.getForm().down('[name=Tariff]');

                tariff.disable();
                if (!retId) {
                    tariff.setValue();
                    return;
                }

                B4.Ajax.request({
                        url: B4.Url.action('GetTariff', 'Entrance'),
                        params: {
                            roId: roId,
                            retId: retId
                        }
                    })
                    .next(function(response) {
                        if (response) {
                            var res = Ext.decode(response.responseText);

                            if (res.data) {
                                tariff.setValue(res.data);
                            }
                        }

                        tariff.enable();
                    })
                    .error(function(result) {
                        Ext.Msg.alert('Ошибка получения тарифа', Ext.isString(result.responseData)
                            ? result.responseData
                            : result.responseData.message);
                    });
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjentrancegrid'),
            store,
            aspect;

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        store = view.getStore();
        store.on('beforeload', me.onBeforeLoad, me);
        store.load();

        aspect = me.getAspect('entrancepermissionaspect');
        aspect.setPermissionsByRecord({
            getId: function() {
                return id;
            }
        });
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});