Ext.define('B4.controller.realityobj.realityobjectoutdoor.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.fieldrequirement.RealityObjectOutdoor',
        'B4.aspects.permission.realityobj.RealityObjectOutdoorFields'
        ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'realityobj.RealityObjectOutdoor',
        'dict.MunicipalityFiasOktmo'
    ],

    views: [
        'realityobj.realityobjectoutdoor.EditPanel',
        'realityobj.realityobjectoutdoor.RealityObjectsInOutdoorGrid'
    ],

    stores: [
        'realityobj.RealityObjectOutdoor',
        'RealityObject',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    mainView: 'realityobj.realityobjectoutdoor.EditPanel',
    mainViewSelector: 'realityobjectoutdooreditpanel',

    aspects: [
        {
            xtype: 'realityobjectoutdoorfieldsperm',
            name: 'realityobjectoutdoorfieldsperm'
        },
        {
            xtype: 'realityobjectoutdoorfieldrequirement'
        },
        {
            xtype: 'gkheditpanel',
            name: 'realityobjectOutdoorEditPanelAspect',
            editPanelSelector: 'realityobjectoutdooreditpanel',
            modelName: 'realityobj.RealityObjectOutdoor',

            listeners: {
                aftersetpaneldata: function (asp, record, panel) {
                    var me = this,
                        store = panel.down('realityobjectsinoutdoorgrid').getStore();
                    store.on('beforeload', me.onBeforeLoad, me);
                    store.load();
                }
            },

            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.outdoorId = me.controller.getContextValue(me.controller.getMainView(), 'outdoorId');
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'realityobjectOutdoorGridAspect',
            gridSelector: 'realityobjectsinoutdoorgrid',
            storeName: 'RealityObject',
            modelName: 'RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityobjectOutdoorMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],

            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (res) {
                    if (res == 'yes') {
                        me.controller.mask('Удаление', me.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('DeleteOutdoorFromRealityObject', 'RealityObjectOutdoor'),
                            params: {
                                realityObjectId: record.getId(),
                            },
                            timeout: 9999999
                        }).next(function () {
                            me.controller.unmask();
                            me.updateGrid();
                        }).error(function (result) {
                            me.controller.unmask();
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        });
                    }
                }, me);
            },

            onBeforeLoad: function(store, operation) {
                operation.params.needOnlyWithoutOutdoor = true;
            },

            listeners: {
                getdata: function(asp, records) {
                    if (records.items.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }

                    var recordIds = [];

                    Ext.each(records.items, function(item) {
                        recordIds.push(item.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    B4.Ajax.request({
                        url: B4.Url.action('UpdateOutdoorInRealityObjects', 'RealityObjectOutdoor'),
                        params: {
                            realityObjectIds: recordIds,
                            outdoorId: asp.controller.getContextValue(asp.controller.getMainView(), 'outdoorId')
                        },
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', 'Ошибка при сохранении домов');
                        return false;
                    });
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector),
            fieldspermaspect = me.getAspect('realityobjectoutdoorfieldsperm');
        me.bindContext(view);
        me.application.deployView(view, 'realityobject_outdoor_info');
        me.setContextValue(view, 'outdoorId', id);
        me.getAspect('realityobjectOutdoorEditPanelAspect').setData(id);
        fieldspermaspect.setPermissionsByRecord({ getId: function () { return id; } });
    }
});