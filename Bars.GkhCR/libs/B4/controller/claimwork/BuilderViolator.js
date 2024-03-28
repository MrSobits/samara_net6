Ext.define('B4.controller.claimwork.BuilderViolator', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.claimwork.buildcontract.BuilderViolatorGrid',
        'B4.view.claimwork.buildcontract.BuilderViolatorAddWindow',
        'B4.view.claimwork.buildcontract.BuilderViolatorEditWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.BuilderViolator',
        'claimwork.BuilderViolatorViol',
        'ObjectCr',
        'objectcr.BuildContract'
    ],
    
    stores: [
         'claimwork.BuilderViolator',
         'claimwork.BuilderViolatorViol',
         'claimwork.ViolClaimWork'
    ],
    
    views: [
        'claimwork.buildcontract.BuilderViolatorGrid',
        'claimwork.buildcontract.BuilderViolatorAddWindow',
        'claimwork.buildcontract.BuilderViolatorEditWindow',
        'claimwork.buildcontract.BuilderViolatorViolGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    
    refs: [
       {
           ref: 'mainView',
           selector: 'builderviolatorgrid'
       },
        {
            ref: 'editWindow',
            selector: 'builderviolatoreditwindow'
        }
    ],

    aspects: [

        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhCr.BuilderViolator.ClaimWorkForm', applyTo: 'button[action="CreateClaimWork"]', selector: 'builderviolatorgrid',
                    applyBy: function (component, allowed) {
                        var grid = component.up('builderviolatorgrid');
                        if (allowed) {
                            component.show();
                            grid.headerCt.child('gridcolumn[isCheckerHd]').show();
                        } else {
                            component.hide();
                            grid.headerCt.child('gridcolumn[isCheckerHd]').hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.BuilderViolator.MakeNew', applyTo: 'button[action="Create"]', selector: 'builderviolatorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.BuilderViolator.Clear', applyTo: 'button[action="Clear"]', selector: 'builderviolatorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.BuilderViolator.Add', applyTo: 'button[actionName=builderViolatorAdd]', selector: 'builderviolatorgrid'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'builderViolatorButtonExportAspect',
            gridSelector: 'builderviolatorgrid',
            buttonSelector: 'builderviolatorgrid #btnExport',
            controllerName: 'BuilderViolator',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'builderViolatorGridWindowAspect',
            gridSelector: 'builderviolatorgrid',
            editFormSelector: 'builderviolatoreditwindow',
            editWindowView: 'claimwork.buildcontract.BuilderViolatorEditWindow',
            modelName: 'claimwork.BuilderViolator',
            otherActions: function (actions) {
                var me = this;

                actions['builderviolatoraddwindow [name=BuildContract]'] = {
                    'beforeload': { fn: me.onBuilderContractBeforeLoad, scope: me }
                };
                
                actions['builderviolatoraddwindow [name=ProgramCr]'] = {
                    'beforeload': { fn: me.onProgramBeforeLoad, scope: me }
                };

                actions['builderviolatorgrid button[actionName=builderViolatorAdd]'] = {
                    'click': { fn: me.getAddForm, scope: me }
                };
                
                actions['builderviolatoreditwindow button[action=goContract]'] = {
                    'click': { fn: me.goContract, scope: me }
                };
            },
            onBuilderContractBeforeLoad: function (field, options, store) {
                var programId = field.up('builderviolatoraddwindow').down('[name=ProgramCr]').getValue();
                options = options || {};
                options.params = {};

                options.params.programCrId = programId;

                return true;
            },
            onProgramBeforeLoad: function (field, options) {
                options.params = {};
                options.params.onlyFull = true;

                return true;
            },
            goContract: function () {
                var me = this,
                    form = me.getForm(),
                    record = form.getRecord();
                
                if (record.getId() && record.get('ObjCrId')) {
                    Ext.History.add('objectcredit/' + record.get('ObjCrId') + '/contractcr');
                }

            },
            getAddForm: function() {
                var me = this,
                    renderTo,
                    addWindow,
                    addView,
                    model,
                    rec,
                    btnSave,
                    btnClose,
                    grid = me.getGrid();

                addWindow = me.componentQuery('builderviolatoraddwindow');

                if (!addWindow) {
                    renderTo = grid;
                    if (Ext.isString(me.editWindowContainerSelector)) {
                        renderTo = me.componentQuery(me.editWindowContainerSelector);
                        if (!renderTo)
                            throw "Не удалось найти контейнер для формы редактирования по селектору " + me.editWindowContainerSelector;
                    }

                    addView = me.controller.getView('claimwork.buildcontract.BuilderViolatorAddWindow');
                    if (!addView)
                        throw "Не удалось найти вьюшку контроллера claimwork.buildcontract.BuilderViolatorAddWindow";

                    addWindow = addView.create({ constrain: true, renderTo: renderTo.getEl() });

                    model = me.controller.getModel(me.modelName);

                    rec = new model();
                    addWindow.loadRecord(rec);
                    addWindow.getForm().updateRecord();
                    addWindow.getForm().isValid();
                    
                    grid.add(addWindow);

                    btnSave = addWindow.down('b4savebutton');
                    btnClose = addWindow.down('b4closebutton');

                    btnSave.on('click', me.saveNewRecord, me);
                    btnClose.on('click', function() {
                        addWindow.destroy();
                    }, me);
                    
                    addWindow.show();
                    addWindow.center();
                }

                return addWindow;
            },
            saveNewRecord:function(btn) {
                var me = this,
                    rec,
                    fields,
                    invalidFields,
                    from = btn.up('builderviolatoraddwindow');
                
                from.getForm().updateRecord();
                rec = from.getRecord();

                if (from.getForm().isValid()) {
                    if (me.fireEvent('validate', this)) {
                        me.mask('Сохранение', from);
                        rec.save({ id: 0 })
                            .next(function (result) {
                                me.unmask();
                                from.destroy();
                                me.updateGrid();
                                me.editRecord(result.record);
                            }, me)
                            .error(function (result) {
                                me.unmask();
                                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                    }
                } else {
                    //получаем все поля формы
                    fields = from.getForm().getFields();

                    invalidFields = '';

                    //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<br>' + field.fieldLabel;
                        }
                    });

                    //выводим сообщение
                    Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
                }
                
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        violGrid = form.down('builderviolatorviolgrid'),
                        violStore = violGrid.getStore();


                    violStore.clearFilter(true);
                    violStore.filter('builderId', record.getId());

                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы ДЛ с массовой формой выбора ДЛ
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'builderViolatorViolAspect',
            gridSelector: 'builderviolatorviolgrid',
            saveButtonSelector: 'builderviolatorviolgrid #builderViolationSaveButton',
            modelName: 'claimwork.BuilderViolatorViol',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#builderviolatorViolMultiSelectWindow',
            storeSelect: 'claimwork.ViolClaimWork',
            storeSelected: 'claimwork.ViolClaimWork',
            titleSelectWindow: 'Выбор нарушений условий договора',
            titleGridSelect: 'Нарушения условий договора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        editWindow = me.controller.getEditWindow(),
                        record = editWindow.getRecord(),
                        violGrid = asp.getGrid(),
                        form = me.getForm();

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', form);
                        B4.Ajax.request(B4.Url.action('AddViolations', 'BuilderViolator', {
                            violIds: Ext.encode(recordIds),
                            violatorId: record.getId()
                        })).next(function (response) {
                            asp.controller.unmask();
                            violGrid.getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],
    
    init: function () {
        var me = this;

        me.control({
            'builderviolatorgrid button[action="Create"]': { 'click': { fn: me.create } },
            'builderviolatorgrid button[action="Clear"]': { 'click': { fn: me.clear } },
            'builderviolatorgrid button[action="CreateClaimWork"]': { 'click': { fn: me.validateClaimWork } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            settlementCol,
            json,
            view = me.getMainView() || Ext.widget('builderviolatorgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
        
        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            settlementCol = view.down('[dataIndex=Settlement]');
            json = Ext.JSON.decode(response.responseText);

            if (settlementCol) {
                if (json.ShowStlBuildContractGrid) {
                    settlementCol.show();
                } else {
                    settlementCol.hide();
                }
            }

        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'Ошибка получения параметров приложения');
        });
    },
    
    clear: function () {
        var me = this,
            json;

        Ext.Msg.confirm('Внимание', 'Удалить все записи из реtстра, сформированные автоматически? ', function (result) {
            if (result == 'yes') {
                me.mask("Удаление...");
                B4.Ajax.request({
                    url: B4.Url.action('Clear', 'BuilderViolator')
                }).next(function (response) {
                    me.unmask();
                    json = Ext.JSON.decode(response.responseText);
                    if (json && json.message) {
                        Ext.Msg.alert('Ошибка!', ' Ошибка при удалении!');
                        
                    } else {
                        Ext.Msg.alert('Внимание!', json.message);
                        me.getMainView().getStore().load();
                    }
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message||'Ошибка при удалении!');
                });
            }
        });
    },
    
    create: function () {
        var me = this,
            json;

        Ext.Msg.confirm('Внимание', 'Сформировать реестр подрядчиков, нарушивших условие договора? ', function (result) {
            if (result == 'yes') {
                me.mask("Формрование...");
                B4.Ajax.request({
                    url: B4.Url.action('MakeNew', 'BuilderViolator'),
                    timeout: 9999999
                }).next(function (response) {
                    me.unmask();
                    json = Ext.JSON.decode(response.responseText);
                    if (json && (json.message || (json.data && json.data.message ))) {
                        Ext.Msg.alert('Ошибка!', ' Ошибка при формировании реестра!');
                    } else {
                        Ext.Msg.alert('Внимание!', json.message || (json.data && json.data.message) || 'Формирование реестра успешно выполнено');
                        me.getMainView().getStore().load();
                    }
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message || 'Ошибка при формировании реестра!');
                });
            }
        });
    },
    
    validateClaimWork: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            selection = grid.getSelectionModel().getSelection(),
            ids = [];

        selection.forEach(function (entry) {
            ids.push(entry.getId());
        });

        if (ids.length == 0) {
            Ext.Msg.confirm('Внимание', "Записи невыбраны! Нажмите 'Да', если необходимо начать претензионную работу по всем договорам реестра.", function (result) {
                if (result == 'yes') {
                    me.validationExecute(ids);
                }
            });
        } else {
            Ext.Msg.confirm('Внимание', "Количество выбранных записей " + ids.length + ". Начать претензионную работу по выбранным записям?", function (result) {
                if (result == 'yes') {
                    me.validationExecute(ids);
                }
            });
        }
    },
    
    validationExecute:function(ids) {
        var me = this,
            json,
            mainView = me.getMainView();
            
        me.mask("Проверка...", mainView);
        B4.Ajax.request({
            url: B4.Url.action('ValidateToCreateClaimWorks', 'BuilderViolator', { ids: ids.join(',') })
        }).next(function (response) {
            me.unmask();
            json = Ext.JSON.decode(response.responseText);
            
            if (json.message) {
                Ext.Msg.confirm('Внимание', json.message, function (result) {
                    if (result == 'yes') {
                        me.createClaimWork(ids);
                    }
                });
            } else {
                me.createClaimWork(ids);
            }
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка создания претензионной работы');
        });
    },

    createClaimWork: function (ids) {
        var me = this,
            mainView = me.getMainView();

        me.mask("Формирование...", mainView);
        B4.Ajax.request({
            url: B4.Url.action('CreateClaimWorks', 'BuilderViolator', { ids: ids.join(',') }),
            timeout: 9999999
        }).next(function () {
            me.unmask();
            Ext.Msg.alert('Успешно!', ' Претензионные работы успешно созданы!');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'Ошибка при формировании!');
        });
    }
});