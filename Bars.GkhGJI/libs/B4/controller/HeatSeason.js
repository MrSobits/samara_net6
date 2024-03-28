Ext.define('B4.controller.HeatSeason', {
    extend: 'B4.base.Controller',
    seasonId: 0,
    inspectionId: 0,
    periodId: 0,
    showIndividual: false,
    showBlocked: false,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateButton',
        'B4.aspects.permission.HeatSeason',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.heatseason.Document',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'HeatSeason',
        'BaseHeatSeason',
        'heatseason.Document',
        'RealityObject'
    ],

    stores: [
        'HeatSeason',
        'heatseason.Document',
        'BaseHeatSeason',
        'dict.HeatSeasonPeriodGji',
        'view.HeatSeason'
    ],

    views: [
        'heatseason.Grid',
        'heatseason.EditWindow',
        'heatseason.DocumentGrid',
        'heatseason.DocumentEditWindow',
        'heatseason.InspectionGrid'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'heatseason.Grid',
    mainViewSelector: 'heatSeasonGrid',

    heatSeasonEditWindowSelector: '#heatSeasonEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'heatSeasonGrid'
        }
    ],

    aspects: [
        {
            /* Вешаем аспект смены статуса */
            xtype: 'b4_state_contextmenu',
            name: 'heatSeasonStateTransferAspect',
            gridSelector: 'heatingseasondocgrid',
            stateType: 'gji_heatseason_document',
            menuSelector: 'heatSeasonDocGridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'heatSeasonButtonExportAspect',
            gridSelector: 'heatSeasonGrid',
            buttonSelector: 'heatSeasonGrid #btnExport',
            controllerName: 'HeatSeason',
            actionName: 'Export'
        },
        {
            xtype: 'heatseasonperm'
        },
        {
            xtype: 'heatseasondocumentperm',
            name: 'heatSeasonDocumentPermissionAspect',
            editFormAspectName: 'heatSeasonDocumentGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'heatseasondocdeletepermissionaspect',
            permissions: [{ name: 'GkhGji.HeatSeason.Register.Document.Delete' }]
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'heatSeasonStateButtonAspect',
            stateButtonSelector: '#heatSeasonDocEditWindow #btnState',
            listeners: {
                transfersuccess: function(asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('heatSeasonDocumentGridWindowAspect');

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();

                    var model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function(rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы подготовок к отапливаемому сезону и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'heatSeasonGridWindowAspect',
            gridSelector: 'heatSeasonGrid',
            editFormSelector: '#heatSeasonEditWindow',
            storeName: 'view.HeatSeason',
            modelName: 'HeatSeason',
            editWindowView: 'heatseason.EditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #btnEditRealityObj'] = { 'click': { fn: this.onClickBtnEditRealityObject, scope: this } };

                actions[this.gridSelector + ' #heatPeriodSelectField'] = { 'change': { fn: this.onChangeSelectFieldHeatingSeasonPeriod, scope: this } };
                actions[this.gridSelector + ' #cbIndividualHeatingSystem'] = { 'change': { fn: this.onChangeCheckboxIndividualHeatSys, scope: this } };
                actions[this.gridSelector + ' #cbBlockedObjects'] = { 'change': { fn: this.onChangeCheckboxBlockedObjects, scope: this } };
                actions[this.gridSelector + ' #cbShowIndividual'] = { 'change': { fn: this.cbShowIndividualChange, scope: this } };

                actions[this.gridSelector] = {
                    'rowaction': { fn: this.rowAction, scope: this },
                    'itemdblclick': { fn: this.rowDblClick, scope: this },
                    'gridaction': { fn: this.gridAction, scope: this }
                };
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта RealityObject
            // передать только Id на сохранение, поскольку если на сохранение уйдет RealityObject целиком,
            //то это поле тоже сохраниться и поля для записи RealityObject будут потеряны
            getRecordBeforeSave: function(record) {
                var ro = record.get('RealityObject');
                if (ro && ro.Id > 0) {
                    record.set('RealityObject', ro.Id);
                }

                return record;
            },

            saveRecord: function(rec) {
                var me = this;
                if (!rec.getData().Id) {
                    Ext.Msg.confirm('Cохранение записи',
                        'Проверьте Тип системы отопления! Продолжить сохранение?',
                        function(result) {
                            if (result == 'yes') {
                                me.saveRecordHasUpload(rec);
                            }
                        });
                } else {
                    me.saveRecordHasUpload(rec);
                }
            },

            cbShowIndividualChange: function(field, newValue) {
                this.controller.showIndividualRealObj = newValue;
                this.controller.getStore('view.HeatSeason').load();
            },

            editRecord: function(record) {
                var me = this,
                    id = record ? record.get('HeatingSeasonId') : null,
                    model;

                var cloneRecord = record.copy();

                var roId = cloneRecord.get('Id');
                cloneRecord.set('Id', id);

                model = this.getModel(cloneRecord);

                id ? model.load(id, {
                    success: function(rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                }) : this.setFormData(new model(
                    {
                        Id: null,
                        RealityObject: {
                            Id: roId,
                            Address: cloneRecord.get('Address'),
                            Floors: cloneRecord.get('MinFl'),
                            MaximumFloors: cloneRecord.get('MaxFl'),
                            TypeHouse: cloneRecord.get('Type'),
                            NumberEntrances: cloneRecord.get('NumEntr')
                        },
                        HeatingSystem: cloneRecord.get('HeatSys'),
                        Floors: cloneRecord.get('MinFl'),
                        MaximumFloors: cloneRecord.get('MaxFl'),
                        AreaMkd: cloneRecord.get('AreaMkd'),
                        NumberEntrances: cloneRecord.get('NumEntr'),
                        TypeHouse: cloneRecord.get('Type'),
                        Period: cloneRecord.get('Period')
                    }
                ));

                this.getForm().getForm().isValid();
            },
            onSaveSuccess: function(asp, record) {
                asp.controller.setSeasonId(record.getId());
            },
            onChangeSelectFieldHeatingSeasonPeriod: function(field, newValue) {
                if (newValue) {
                    this.controller.periodId = newValue.Id;
                } else {
                    this.controller.periodId = 0;
                }
                this.controller.getStore('view.HeatSeason').load();
            },
            onChangeCheckboxIndividualHeatSys: function(field, newValue) {
                this.controller.showIndividual = newValue;
                this.controller.getStore('view.HeatSeason').load();
            },
            onChangeCheckboxBlockedObjects: function(field, newValue) {
                this.controller.showBlocked = newValue;
                this.controller.getStore('view.HeatSeason').load();
            },
            onClickBtnEditRealityObject: function() {
                var me = this,
                    editWindow = Ext.ComponentQuery.query(me.controller.heatSeasonEditWindowSelector)[0],
                    realityObj = editWindow.down('#realityObjectSelectField').getValue();

                if (realityObj) {
                    Ext.History.add('realityobjectedit/' + realityObj.Id);
                }
            },
            deleteRecord: function(record) {
                var me = this;

                if (!record.get('HeatingSeasonId')) {
                    Ext.Msg.alert('Ошибка удаления!', 'Запись подготовки к отопительному сезону не существует');
                    return;
                }

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                    if (result == 'yes') {
                        var model = this.getModel(record),
                            rec = new model({ Id: record.get('HeatingSeasonId') });
                        me.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function(result) {
                                this.fireEvent('deletesuccess', this);
                                me.updateGrid();
                                me.unmask();
                            }, this)
                            .error(function(result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                me.unmask();
                            }, this);
                    }
                }, me);
            },
            listeners: {
                aftersetformdata: function(asp, record, form) {
                    asp.controller.setSeasonId(record.get('Id'));

                    var ro = record.get('RealityObject');

                    if (ro && record.get('Id')) {
                        form.down('#tfFloors').setValue(ro.Floors);
                        form.down('#tfMaximumFloors').setValue(ro.MaximumFloors);
                        form.down('#tfNumberEntrances').setValue(ro.NumberEntrances);
                        form.down('#tfAreaMkd').setValue(ro.AreaMkd);
                        form.down('#cbTypeHouse').setValue(ro.TypeHouse);
                    }
                }
            }
        },
        {
            /**
            * Аспект взаимодействия таблицы документов и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'heatSeasonDocumentGridWindowAspect',
            gridSelector: '#heatSeasonDocGrid',
            editFormSelector: '#heatSeasonDocEditWindow',
            storeName: 'heatseason.Document',
            modelName: 'heatseason.Document',
            editWindowView: 'heatseason.DocumentEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('HeatingSeason', this.controller.seasonId);
                    }
                },
                aftersetformdata: function(asp, record, form) {
                    this.controller.getAspect('heatSeasonStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            },
            deleteRecord: function(record) {
                if (record.getId()) {
                    this.controller.getAspect('heatseasondocdeletepermissionaspect').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы обследований

            editRecord тут перекрыт потомучто при создании мы сами формируем модель и создаем Проверку,
            а затем открываем форму проверки

            а при редактировании сразу открываем форму проверки
            */
            xtype: 'grideditwindowaspect',
            name: 'heatSeasonInspectionGridWindowAspect',
            gridSelector: '#heatSeasonInspectionGrid',
            storeName: 'BaseHeatSeason',
            modelName: 'BaseHeatSeason',

            editRecord: function(record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (id > 0) {
                    me.showInspectionForm(record);
                } else {
                    //Тут мы сами создаем модель проверки и открываем форму редактирования
                    model = this.getModel(record);
                    var newRec = new model({ Id: 0 });
                    newRec.set('HeatingSeason', this.controller.seasonId);
                    newRec.set('TypeBase', 80);

                    newRec.save({ Id: newRec.getId() })
                        .next(function(result) {
                            me.controller.getStore('BaseHeatSeason').load();
                            me.showInspectionForm(result.record);
                        }, this)
                        .error(function(result) {
                            Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        }, this);
                }
            },
            
            showInspectionForm: function (record) {
                var me = this,
                    inspectionId = record.get('Id'),
                    portal = me.controller.getController('PortalController'),
                    controllerEditName = 'B4.controller.baseheatseason.Navigation',
                    model = me.controller.getModel('InspectionGji'),
                    inspection = new model({ Id: inspectionId });

                portal.loadController(controllerEditName, inspection, portal.containerSelector);
            },
            
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('HeatingSeason', this.controller.seasonId);
                    }
                }
            }
        }
    ],

    init: function() {
        this.getStore('heatseason.Document').on('beforeload', this.onBeforeLoad, this);
        this.getStore('BaseHeatSeason').on('beforeload', this.onBeforeLoad, this);
        this.getStore('view.HeatSeason').on('beforeload', this.onBeforeLoadHeatSeason, this);

        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView(),
            firstTime = view === null;
        view = view || Ext.widget(this.mainViewSelector);
        this.bindContext(view);
        this.application.deployView(view);
       if (firstTime)
        {
            this.showIndividual = null;
            this.showBlocked = null;
            this.showIndividualRealObj = null;
            this.periodId = null;
        }

    },

    onMainViewAfterRender: function() {
        var me = this,
            grid = me.getMainView() || Ext.widget('heatSeasonGrid');

        me.mask('Загрузка', grid);
        B4.Ajax.request(B4.Url.action('GetCurrentPeriod', 'HeatSeasonPeriodGji'))
            .next(function(response) {
                me.unmask();
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText),
                    periodId = obj.periodId,
                    periodName = obj.periodName;

                grid.down('#heatPeriodSelectField').setValue({ Id: periodId });
                grid.down('#heatPeriodSelectField').updateDisplayedText(periodName);

                me.periodId = periodId;
            })
            .error(function() {
                me.unmask();
            });
    },

    setSeasonId: function(id) {
        this.seasonId = id;

        var editWindow = Ext.ComponentQuery.query(this.heatSeasonEditWindowSelector)[0];

        var docStore = this.getStore('heatseason.Document');
        var insStore = this.getStore('BaseHeatSeason');
        insStore.removeAll();
        docStore.removeAll();

        if (id > 0) {
            editWindow.down('#heatSeasonDocGrid').setDisabled(false);
            editWindow.down('#heatSeasonInspectionGrid').setDisabled(false);
            editWindow.down('#btnEditRealityObj').setDisabled(false);
            docStore.load();
            insStore.load();
        } else {
            editWindow.down('#heatSeasonDocGrid').setDisabled(true);
            editWindow.down('#heatSeasonInspectionGrid').setDisabled(true);
            editWindow.down('#btnEditRealityObj').setDisabled(true);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.seasonId = this.seasonId;
    },

    onBeforeLoadHeatSeason: function(store, operation) {
        if (this.periodId) {
            operation.params.periodId = this.periodId;
        }
        operation.params.showIndividual = this.showIndividual;
        operation.params.showBlocked = this.showBlocked;
        operation.params.showIndividualRealObj = this.showIndividualRealObj;
    }
});