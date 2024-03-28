Ext.define('B4.controller.dpkrdocument.DpkrDocument', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.dpkrdocument.DpkrDocument',
        'B4.form.ComboBox',
        'B4.aspects.GkhButtonImportAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'dpkrdocument.DpkrDocument',
        'dpkrdocument.DpkrDocumentRealityObject',
        'dpkrdocument.DpkrDocumentRealityObjectForSelect'
    ],

    stores: [
        'dpkrdocument.DpkrDocument',
        'dpkrdocument.IncludedDpkrDocumentRealityObject',
        'dpkrdocument.ExcludedDpkrDocumentRealityObject',
        'dpkrdocument.DpkrDocumentRealityObjectForSelect',
        'dpkrdocument.DpkrDocumentRealityObjectForSelected'
    ],

    views: [
        'dpkrdocument.Grid',
        'dpkrdocument.EditWindow',
        'dpkrdocument.DpkrDocumentRealityObject'
    ],

    mainView: 'dpkrdocument.Grid',
    mainViewSelector: 'dpkrdocumentgrid',

    aspects: [
        {
            xtype: 'dpkrdocumentpermissions'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'dpkrDocumentGridWindowAspect',
            gridSelector: 'dpkrdocumentgrid',
            editFormSelector: 'dpkrdocumenteditwindow',
            modelName: 'dpkrdocument.DpkrDocument',
            editWindowView: 'dpkrdocument.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.dpkrDocumentId = record.getId();
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.dpkrDocumentId = record.getId();
                    form.down('#includedDpkrDocumentRealityObject b4addbutton').setDisabled(!record.data.Id);
                    form.down('#excludedDpkrDocumentRealityObject b4addbutton').setDisabled(!record.data.Id);
                    form.down('#includedDpkrDocumentRealityObject').getStore().load();
                    form.down('#excludedDpkrDocumentRealityObject').getStore().load();
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'includedDpkrDocumentRealityObjectGjiAspect',
            gridSelector: '#includedDpkrDocumentRealityObject',
            storeName: 'dpkrdocument.IncludedDpkrDocumentRealityObject',
            modelName: 'dpkrdocument.DpkrDocumentRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#includedDpkrDocumentRealityObjectGjiAspectMultiSelectWindow',
            storeSelect: 'dpkrdocument.DpkrDocumentRealityObjectForSelect',
            storeSelected: 'dpkrdocument.DpkrDocumentRealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRealityObjects', 'DpkrDocumentRealityObject', {
                        ids: Ext.encode(recordIds),
                        isIncluded: true,
                        dpkrDocumentId: asp.controller.dpkrDocumentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранено!', 'Дома сохранены успешно');
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation.params.isIncluded = true;
            },
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'excludedDpkrDocumentRealityObjectGjiAspect',
            gridSelector: '#excludedDpkrDocumentRealityObject',
            storeName: 'dpkrdocument.ExcludedDpkrDocumentRealityObject',
            modelName: 'dpkrdocument.DpkrDocumentRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#excludedDpkrDocumentRealityObjectGjiAspectMultiSelectWindow',
            storeSelect: 'dpkrdocument.DpkrDocumentRealityObjectForSelect',
            storeSelected: 'dpkrdocument.DpkrDocumentRealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRealityObjects', 'DpkrDocumentRealityObject', {
                        ids: Ext.encode(recordIds),
                        isIncluded: false,
                        dpkrDocumentId: asp.controller.dpkrDocumentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранено!', 'Дома сохранены успешно');
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation.params.isIncluded = false;
            },
        },
        {
            xtype: 'gkhbuttonimportaspect',
            buttonSelector: 'dpkrdocumenteditwindow #ImportFileButton',
            windowImportView: 'dpkrdocument.FileUploadWindow',
            windowImportSelector: 'dpkrdocumentfileuploadwindow',
            loadImportList: false,
            getUserParams: function () {
                this.params = { DpkrDocumentId: this.controller.dpkrDocumentId };
            }
        }
    ],
    init: function () {
        var me = this;

        me.getStore('dpkrdocument.IncludedDpkrDocumentRealityObject').on('beforeload', me.onBeforeLoad, me, true);
        me.getStore('dpkrdocument.ExcludedDpkrDocumentRealityObject').on('beforeload', me.onBeforeLoad, me, false);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onBeforeLoad: function (store, operation, isIncluded) {
        operation.params.isIncluded = isIncluded;
        operation.params.dpkrDocumentId = this.dpkrDocumentId;
    }
});