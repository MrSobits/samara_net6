Ext.define('B4.controller.dict.OrganizationWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.OrganizationWork',
    'dict.ContentRepairMkdWork'],
    stores: ['dict.OrganizationWork',
        'dict.ContentRepairMkdWorkSelected',
        'dict.OrganizationWorkContentRepairMkdWork',
        'dict.ContentRepairMkdWorkForSelect'],
    views: [
        'dict.OrganizationWork.EditWindow',
        'dict.OrganizationWork.Grid',
        'dict.OrganizationWork.ContentRepairMkdWorkGrid',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'organizationWorkGrid',
            permissionPrefix: 'Gkh.Dictionaries.OrganizationWork'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'organizationWorkGridWindowAspect',
            gridSelector: 'organizationWorkGrid',
            editFormSelector: '#organizationWorkEditWindow',
            storeName: 'dict.OrganizationWork',
            modelName: 'dict.OrganizationWork',
            editWindowView: 'dict.OrganizationWork.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.organizationWorkId = record.getId();
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentId(record.getId());

                    var organizationWorkId = record.getId();
                    asp.controller.organizationWorkId = organizationWorkId;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'contentRepairMkdWorkGridAspect',
            gridSelector: '#contentRepairMkdWorkGrid',
            storeName: 'dict.OrganizationWorkContentRepairMkdWork',
            modelName: 'dict.ContentRepairMkdWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#contentRepairMkdWorkGridMultiSelectWindow',
            storeSelect: 'dict.ContentRepairMkdWorkForSelect',
            storeSelected: 'dict.ContentRepairMkdWorkSelected',
            titleSelectWindow: 'Выбор работ по содержанию и ремонту МКД',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    header: 'Код',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    header: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование'
                }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.excludeOrganizationWorkId = this.controller.organizationWorkId;
            },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(res) {
                    if (res == 'yes') {

                        B4.Ajax.request({
                            url: B4.Url.action('DeleteContentRepairMkdWork', 'Work'),
                            params: {
                                contentRepairMkdWorkId: Ext.encode(record.data.Id),
                                signedOrganizationWorkId: me.controller.organizationWorkId
                            },
                            timeout: 5 * 60 * 1000 // 5 минут
                        }).next(function (response) {
                            me.updateGrid();
                            return true;
                        }).error(function (result) {
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        });
                    }
                }, me);
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        B4.Ajax.request({
                            url: B4.Url.action('AddContentRepairMkdWorks', 'Work'),
                            params: {
                                contentRepairMkdWorkIds: Ext.encode(recordIds),
                                signedOrganizationWorkId: asp.controller.organizationWorkId
                            },
                            timeout: 5 * 60 * 1000 // 5 минут
                        }).next(function (response) {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранено!', 'Выбранные работы сохранены успешно');
                            return true;
                        }).error(function (result) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка сохранения работы!', (Ext.isString(result.responseData) ? result.responseData : result.responseData.message)
                                || 'При сохранении произошла ошибка');
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('dict.OrganizationWorkContentRepairMkdWork').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    mainView: 'dict.OrganizationWork.Grid',
    mainViewSelector: 'organizationWorkGrid',

    editWindowSelector: '#organizationWorkEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'organizationWorkGrid'
        }],

    index: function () {
        var view = this.getMainView() || Ext.widget('organizationWorkGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.OrganizationWork').load();
    },

    setCurrentId: function(id) {
        this.organizationWorkId = id;
        var store = this.getStore('dict.OrganizationWorkContentRepairMkdWork');
        store.removeAll();

        var editwindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        editwindow.down('#contentRepairMkdWorkGrid').setDisabled(!id);

        if (id) {
            store.load();
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.organizationWorkId = this.organizationWorkId;
    }
});