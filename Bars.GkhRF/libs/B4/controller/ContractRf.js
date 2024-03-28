Ext.define('B4.controller.ContractRf', {
    /*
    * Контроллер раздела договоры объекта рег. фонда
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.ButtonDataExport'
    ],

    models: ['ContractRf',
             'contractrf.Object'],

    stores: [
        'ContractRf',
        'contractrf.ObjectIn',
        'contractrf.ObjectOut',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'contractrf.ActualRealObjForSelect'
    ],

    views: [
        'contractrf.Grid',
        'contractrf.EditWindow',
        'contractrf.ObjectGridOut',
        'contractrf.ObjectGridIn',
        'SelectWindow.MultiSelectWindow'
    ],
    

    aspects: [
    {
        xtype: 'gkhpermissionaspect',
        permissions: [
            { name: 'GkhRf.ContractRf.Create', applyTo: 'b4addbutton', selector: 'contractRfGrid' },
            { name: 'GkhRf.ContractRf.Edit', applyTo: 'b4savebutton', selector: '#contractRfEditWindow' },
            { name: 'GkhRf.ContractRf.Delete', applyTo: 'b4deletecolumn', selector: 'contractRfGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'GkhRf.ContractRf.Edit', applyTo: 'b4addbutton', selector: '#contractRfEditWindow #objectGridIn' },
            { name: 'GkhRf.ContractRf.Edit', applyTo: '#contractRfInSaveButton', selector: '#contractRfEditWindow #objectGridIn' },
            { name: 'GkhRf.ContractRf.Edit', applyTo: 'b4deletecolumn', selector: '#contractRfEditWindow #objectGridIn',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'GkhRf.ContractRf.Edit', applyTo: '#contractRfOutSaveButton', selector: '#contractRfEditWindow #objectGridOut' },
            { name: 'GkhRf.ContractRf.Edit', applyTo: 'b4deletecolumn', selector: '#contractRfEditWindow #objectGridOut',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'GkhRf.ContractRf.Column.SumAreaMkd', applyTo: '[dataIndex=SumAreaMkd]', selector: 'contractRfGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'GkhRf.ContractRf.Column.SumAreaLivingOwned', applyTo: '[dataIndex=SumAreaLivingOwned]', selector: 'contractRfGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'GkhRf.ContractRf.WithArchiveRecs', applyTo: 'checkbox[cmd=withArchiveRecs]', selector: 'contractRfGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        ]      
    },
    {
        /*
        * Аспект взаимодействия таблицы и формы редактирования раздела договоры объекта рег. фонда
        */
        xtype: 'grideditwindowaspect',
        name: 'contractRfGridEditWindowAspect',
        gridSelector: 'contractRfGrid',
        editFormSelector: '#contractRfEditWindow',
        modelName: 'ContractRf',
        editWindowView: 'contractrf.EditWindow',
        otherActions: function (actions) {
            actions[this.gridSelector + ' checkbox[cmd=withArchiveRecs]'] = { 'change': { fn: this.onShowWithArchiveRecs, scope: this } };
        },
        onSaveSuccess: function (asp, record) {
            asp.controller.setCurrentId(record.getId());
        },
        listeners: {
            aftersetformdata: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            }
        },
        onShowWithArchiveRecs: function (fld, newValue) {
            var store = this.getGrid().getStore();
            store.clearFilter(true);
            store.filter('withArchiveRecs', newValue);
        }
    },
    {
        /*
        * Аспект взаимодействия таблицы и формы массового выбора элементов вкладки включенные в договор дома
        */
        xtype: 'gkhinlinegridmultiselectwindowaspect',
        name: 'contractObjGkhInlineGridMultiSelectWindowAspect',
        gridSelector: '#objectGridIn',
        saveButtonSelector: '#objectGridIn #contractRfInSaveButton',
        storeName: 'contractrf.ObjectIn',
        modelName: 'contractrf.Object',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#contractRfMultiSelectWindow',
        storeSelect: 'contractrf.ActualRealObjForSelect',
        storeSelected: 'realityobj.RealityObjectForSelected',
        titleSelectWindow: 'Выбор домов',
        titleGridSelect: 'Дома для выбора',
        titleGridSelected: 'Выбранные дома',
        columnsGridSelect: [
            {
                header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
            { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
            { header: 'Код МЖФ', xtype: 'gridcolumn', dataIndex: 'GkhCode', flex: 1, filter: { xtype: 'textfield' } },
            { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManOrgNames', flex: 1, filter: { xtype: 'textfield' } }
        ],
        otherActions: function (actions) {
            var me = this;

            actions[me.gridSelector] = { 'edit': { 'fn': me.onAreaEdit, 'scope': me } };
        },
        columnsGridSelected: [
            { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
        ],
        listeners: {
            getdata: function (asp, records) {
                var recordIds = [];
                records.each(function (rec) {
                    recordIds.push(rec.get('Id'));
                });

                if (recordIds[0] > 0) {
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddContractObjects', 'ContractRfObject', {
                        objectIds: recordIds,
                        contractRfId: asp.controller.contractRfId
                    })).next(function () {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
                else {
                    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                    return false;
                }
                return true;
            }
        },
        //перекрываем метод сохранения
        onSelectRequestHandler: function () {
            var grid = this.getSelectedGrid();
            if (grid) {
                //в качестве данных возвращаются records из выбранных
                var storeSelected = grid.getStore();
                
                var recordIds = [];
                storeSelected.data.each(function (rec) {
                    recordIds.push(rec.get('Id'));
                });

                this.controller.mask('Сохранение', this.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('CheckAvailableRealObj', 'ContractRf', {
                    recordIds: recordIds
                })).next(function (response) {
                    var obj = Ext.JSON.decode(response.responseText);
                    if (obj.success) {
                        if (this.fireEvent('getdata', this, storeSelected.data)) {
                            this.getForm().close();
                        }
                    }
                    this.controller.unmask();
                }, this).error(function (result) {
                    Ext.Msg.alert('Cooбщение', result.message);
                    this.controller.unmask();
                }, this);  
            }          
        },

        onAreaEdit: function(cellEditPlugin, eventArgs) {
            if (eventArgs.field == 'AreaLiving' || eventArgs.field == 'AreaNotLiving') {
                var rec = eventArgs.record,
                    areaLiving = rec.get('AreaLiving'),
                    areaNotLiving = rec.get('AreaNotLiving'),
                    totalArea = null;

                if (areaLiving || areaNotLiving) {
                    totalArea = (areaLiving || 0) + (areaNotLiving || 0);
                }

                rec.set('TotalArea', totalArea);
            }
        },

        /* переопределен метод deleteRecord. Удаление записи 
        * заменяем сохранением с другим типом TypeCondition
        */
        deleteRecord: function (record) {
            var me = this;

            Ext.Msg.confirm('Исключение из договора!', 'Вы действительно хотите исключить дом из договора?', function (result) {
                if (result == 'yes') {
                    record.set('TypeCondition', 20);
                    record.set('ExcludeDate', new Date());
                    record.save({ id: record.getId() })
                    .next(function () {
                        Ext.Msg.alert('Исключение из договора!', 'Объект перенесен в исключенные договора');
                        this.controller.getStore('contractrf.ObjectIn').load();
                        this.controller.getStore('contractrf.ObjectOut').load();
                    }, this)
                    .error(function (res) {
                        Ext.Msg.alert('Исключение из договора!', Ext.isString(res.responseData) ? res.responseData : res.responseData.message);
                    }, this);
                }
            }, me);
        }
    },
    {
        /*
        * Аспект инлайн редактирования таблицы
        */
        xtype: 'gkhinlinegridaspect',
        name: 'contractObjGkhInlineGridAspect',
        gridSelector: '#objectGridOut',
        storeName: 'contractrf.ObjectOut',
        modelName: 'contractrf.Object',
        saveButtonSelector: '#objectGridOut #contractRfOutSaveButton',
        rowAction: function (grid, action, record) {
            if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                switch (action.toLowerCase()) {
                    case 'delete':
                        this.deleteRecord(record);
                        break;
                    case 'custom':
                        this.backRecord(record);
                        break;
                }
            }
        },
        //меняем тип TypeCondition на противоположный, сохраняем и перезагружаем сторе
        backRecord: function (rec) {
            Ext.Msg.confirm('Включение в договора!', 'Вы действительно хотите включить дом в договор?', function (result) {
                if (result == 'yes') {
                    rec.set('TypeCondition', 10);
                    rec.set('ExcludeDate', null);
                    rec.save({ id: rec.getId() })
                       .next(function () {
                           this.controller.getStore('contractrf.ObjectIn').load();
                           this.controller.getStore('contractrf.ObjectOut').load();
                           Ext.Msg.alert('Включение в договор!', 'Объект перенесен во включенные договора');
                       }, this)
                       .error(function (res) {
                           Ext.Msg.alert('Включение в договор!', Ext.isString(res.responseData) ? res.responseData : res.responseData.message);
                       }, this);
                }
            }, this);
        }
    },
    {
        xtype: 'b4buttondataexportaspect',
        name: 'ContractRfButtonExportAspect',
        gridSelector: 'contractRfGrid',
        buttonSelector: 'contractRfGrid #btnContractRfExport',
        controllerName: 'ContractRf',
        actionName: 'Export'
    },
    {
        xtype: 'b4buttondataexportaspect',
        name: 'ContractRfIncludeObjsButtonExportAspect',
        gridSelector: '#objectGridIn',
        buttonSelector: '#objectGridIn #btnIncludeContractRfObjExport',
        controllerName: 'ContractRfObject',
        actionName: 'Export'
    },
    {
        xtype: 'b4buttondataexportaspect',
        name: 'ContractRfExcludeObjsButtonExportAspect',
        gridSelector: '#objectGridOut',
        buttonSelector: '#objectGridOut #btnExcludeContractRfObjExport',
        controllerName: 'ContractRfObject',
        actionName: 'Export'
    }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'contractrf.Grid',
    mainViewSelector: 'contractRfGrid',

    contractRfId: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'contractRfGrid'
        }
    ],

    editWindowSelector: '#contractRfEditWindow',

    init: function () {
        this.getStore('contractrf.ObjectIn').on('beforeload', this.onBeforeLoad, this, 'In');
        this.getStore('contractrf.ObjectOut').on('beforeload', this.onBeforeLoad, this, 'Out');

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('contractRfGrid'),
            withArchiveRecs = view.down('checkbox[cmd=withArchiveRecs]').getValue(),
            contractRfStore = view.getStore();

        this.bindContext(view);
        this.application.deployView(view);
        
        contractRfStore.clearFilter(true);
        contractRfStore.filter('withArchiveRecs', withArchiveRecs);
    },

    onBeforeLoad: function (store, operation, type) {
        if (!Ext.isEmpty(type)) {
            if (this.contractRfId) {
                operation.params.contractRfId = this.contractRfId;
                operation.params.typeCondition = type;
            }
        }
    },

    setCurrentId: function (id) {
        this.contractRfId = id;

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        editWindow.down('.tabpanel').setActiveTab(0);

        var outStore = this.getStore('contractrf.ObjectOut');
        var inStore = this.getStore('contractrf.ObjectIn');
        if (id > 0) {
            editWindow.down('#objectGridIn').setDisabled(false);
            editWindow.down('#objectGridOut').setDisabled(false);
            inStore.load();
            outStore.load();
        } else {
            editWindow.down('#objectGridIn').setDisabled(true);
            editWindow.down('#objectGridOut').setDisabled(true);
        }
    }
});