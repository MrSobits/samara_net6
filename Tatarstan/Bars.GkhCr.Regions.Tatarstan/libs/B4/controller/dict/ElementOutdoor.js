Ext.define('B4.controller.dict.ElementOutdoor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.ElementOutdoor',
        'B4.aspects.fieldrequirement.ElementOutdoor'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'dict.ElementOutdoor',
        'dict.WorkRealityObjectOutdoor',
        'dict.WorksElementOutdoorSelect',
        'dict.WorksElementOutdoorSelected'
    ],

    models: [
        'dict.ElementOutdoor',
        'dict.WorkRealityObjectOutdoor'
    ],

    views: [
        'dict.elementoutdoor.Grid',
        'dict.elementoutdoor.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dict.elementoutdoor.Grid',
    mainViewSelector: 'elementoutdoorpanel',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'elementoutdoorGridWindowAspect',
            gridSelector: 'elementoutdoorpanel',
            editFormSelector: 'elementoutdoorwindow',
            modelName: 'dict.ElementOutdoor',
            editWindowView: 'dict.elementoutdoor.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record, panel) {
                    var me = this,
                        id = record.getId(),
                        form = panel.down('workselementoutdoorgrid'),
                        editFormStore = form.getStore(),
                        isEdit = id > 0;

                    form.setDisabled(!isEdit);
                    if (isEdit) {
                        asp.controller.setContextValue(asp.controller.getMainView(), 'elementOutdoorId', id);
                        editFormStore.on('beforeload', me.onBeforeLoad, me);
                        editFormStore.load();
                    }
                }
            },

            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.elementOutdoorId = me.controller.getContextValue(me.controller.getMainView(), 'elementOutdoorId');
            }
        },
        {
            xtype: 'elementoutdoorpermission'
        },
        {
            xtype: 'elementoutdoorfieldrequirement'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'workselementOutdoorGridAspect',
            gridSelector: 'workselementoutdoorgrid',
            storeName: 'WorksElementOutdoor',
            modelName: 'WorksElementOutdoor',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#elementOutdoorMultiSelectWindow',
            storeSelect: 'dict.WorksElementOutdoorSelect',
            storeSelected: 'dict.WorksElementOutdoorSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы по благоустройству двора',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],

            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (res) {
                    if (res === 'yes') {
                        me.controller.mask('Удаление', me.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('DeleteWork', 'ElementOutdoor'),
                            params: {
                                workId: record.getId()
                            },
                            timeout: 9999999
                        }).next(function () {
                            me.controller.unmask();
                            me.updateGrid();
                        }).error(function (result) {
                            me.controller.unmask();
                            Ext.Msg.alert('Ошибка удаления!',
                                Ext.isString(result.responseData)
                                    ? result.responseData
                                    : result.responseData.message);
                        });
                    }
                }, me);
            },

            listeners: {
                getdata: function (asp, records) {
                    if (records.items.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }

                    var recordIds = [];

                    Ext.each(records.items, function (item) {
                        recordIds.push(item.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    B4.Ajax.request({
                        url: B4.Url.action('AddWorks', 'ElementOutdoor'),
                        params: {
                            workIds: recordIds,
                            elementOutdoorId: asp.controller.getContextValue(asp.controller.getMainView(), 'elementOutdoorId')
                        },
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', 'Ошибка при сохранении работ');
                        return false;
                    });
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});