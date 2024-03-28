Ext.define('B4.controller.realityobj.CouncilApartmentHouse', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.realityobj.Council'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'realityobj.Protocol',
        'realityobj.Councillors'
    ],

    stores: [
        'realityobj.Councillors'
    ],

    views: [
        'realityobj.CouncilApartmentHouseEditPanel',
        'realityobj.CouncillorsGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'councilapartmenthouseeditpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'realityobjcouncilperm',
            name: 'realityObjCouncilPerm'
        },
        {
            xtype: 'gkheditpanel',
            name: 'councilApartmentHouseAspect',
            editPanelSelector: 'councilapartmenthouseeditpanel',
            modelName: 'realityobj.Protocol',
            listeners: {
                getdata: function (me, record) {
                    if (!record.get('Id')) {
                        record.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                    }
                },
                savesuccess: function (me) {
                    me.controller.onMainViewAfterRender();
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'councillorsAspect',
            storeName: 'realityobj.Councillors',
            modelName: 'realityobj.Councillors',
            gridSelector: '#councillorsGrid',
            saveButtonSelector: '#councillorsGrid #councilSaveButton',
            listeners: {
                beforesave: function (me, store) {
                    store.each(function (record) {
                        if (!record.get('Id')) {
                            record.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                        }
                    });

                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this,
            councilResultValue,
            actions = {};

        me.getStore('realityobj.Councillors').on('beforeload', me.onBeforeLoad, me);

        
        actions['councilapartmenthouseeditpanel'] = {
             'afterrender': { fn: me.onMainViewAfterRender, scope: me }
        };
        
        actions['#cbCouncilResult'] = {
            'change': {
                fn: function (comp) {
                    councilResultValue = comp.getValue();
                    if (councilResultValue) {
                        if (councilResultValue > 10) {
                            me.getMainComponent().down('#emptyResultLabel').hide();
                            me.getMainComponent().down('#councillorsGrid').setDisabled(false);
                            me.getMainComponent().down('#documentNum').setDisabled(false);
                            me.getMainComponent().down('#dateFrom').setDisabled(false);
                            me.getMainComponent().down('#file').setDisabled(false);
                            me.getMainComponent().down('#councillorsLabel').hide();

                        } else {
                            me.hideComponents();
                        }
                    } else {
                        me.hideComponents();
                    }
                },
                scope: me
            }
        };

        me.control(actions);
        me.callParent(arguments);
    },
    
    hideComponents: function () {
        var me = this;
        me.getMainComponent().down('#emptyResultLabel').show();
        me.getMainComponent().down('#councillorsGrid').setDisabled(true);
        me.getMainComponent().down('#documentNum').setDisabled(true);
        me.getMainComponent().down('#dateFrom').setDisabled(true);
        me.getMainComponent().down('#file').setDisabled(true);
        me.getMainComponent().down('#councillorsLabel').hide();
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('councilapartmenthouseeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.Councillors').load();
        me.getAspect('realityObjCouncilPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onMainViewAfterRender: function () {
        var me = this, objCons, obj,
            aspect = me.getAspect('councilApartmentHouseAspect');

        me.mask('Подождите', me.getMainComponent());
        
        B4.Ajax.request(B4.Url.action('IsShowConcillors', 'RealityObjectCouncillors', {
            realtyObjectId: me.getContextValue(me.getMainComponent(), 'realityObjectId')
        })).next(function (response) {
            objCons = Ext.JSON.decode(response.responseText);
            me.unmask();
            if (objCons.data.visible) {
                me.getMainComponent().down('#fsCouncilProtocol').setDisabled(false);
                me.getMainComponent().down('#councillorsGrid').setDisabled(false);
                me.getMainComponent().down('#councillorsGroupButton').show();

                me.getMainComponent().down('#councillorsLabel').hide();

                //Загрузка протоколов
                me.mask('Загрузка', me.getMainComponent());

                B4.Ajax.request(B4.Url.action('GetProtocolByRealityObjectId', 'RealityObjectProtocol', {
                    realtyObjectId: me.getContextValue(me.getMainComponent(), 'realityObjectId')
                })).next(function (response) {
                    obj = Ext.JSON.decode(response.responseText);
                    aspect.setData(obj.protocolId);
                    me.unmask();
                    return true;
                }, me)
                .error(function () {
                    Ext.Msg.alert('Сообщение', 'Произошла ошибка');
                    me.unmask();
                }, me);
                //конец

            } else {
                me.getMainComponent().down('#fsCouncilProtocol').setDisabled(true);
                me.getMainComponent().down('#councillorsGrid').setDisabled(true);
                me.getMainComponent().down('#councillorsGroupButton').hide();

                me.getMainComponent().down('#councillorsLabel').show();
                    
                if (objCons.data.apartamentsOnly) {
                    Ext.Msg.alert('Сообщение', 'Данный раздел не предназначен для заполнения при текущем количестве квартир');
                }
            }
            return true;
        }, me)
        .error(function () {
            me.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        }, me);
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.roId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});