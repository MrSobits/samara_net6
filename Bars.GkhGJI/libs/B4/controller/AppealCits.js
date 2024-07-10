//ToDo данный контроллер нельзя переводить на роуты поскольку у него форма редактирвоания открывается в модальном окне, и нельзя без реестра вызывать отдельно открытие карточки редактирвоания обращения
//ToDo необходимо данный контроллер переделать на отдельно открывающуюся панель а не модальное окно

Ext.define('B4.controller.AppealCits', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.BaseStatementAppCitEditWindow',
        'B4.aspects.WarningInspectionAppCitEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.permission.AppealCits',
        'B4.aspects.permission.AppealCitsAnswer',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.StateContextMenu',
        'B4.enums.PersonInspection',
        'B4.enums.TypeJurPerson',
        'B4.aspects.GkhGjiNestedDigitalSignatureGridAspect'
    ],

    appealCitizensId: null,
    
    models: [
        'BaseStatement',
        'AppealCits',
        'dict.Subj',
        'appealcits.Request',
        'appealcits.WarningInspection'
    ],
    
    stores: [
        'AppealCits',
        'appealcits.Answer',
        'appealcits.Source',
        'appealcits.StatSubject',
        'appealcits.RealityObject',
        'appealcits.Request',
        'appealcits.BaseStatement',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'dict.statsubjectgji.Select',
        'dict.statsubjectgji.Selected',
        'dict.StatSubjectTreeSelect',
        'dict.Subj',
        'dict.Inspector',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'appealcits.AppealCitsBaseStatement',
        'appealcits.WarningInspection'
    ],
    
    views: [
        'appealcits.Grid',
        'appealcits.EditWindow',
        'appealcits.RealityObjectGrid',
        'appealcits.StatSubjectGrid',
        'appealcits.SourceGrid',
        'appealcits.SourceEditWindow',
        'appealcits.AnswerGrid',
        'appealcits.AnswerEditWindow',
        'appealcits.RequestGrid',
        'appealcits.RequestEditWindow',
        'appealcits.BaseStatementGrid',
        'appealcits.Panel',
        'appealcits.FilterPanel',
        'appealcits.BaseStatementAddWindow',
        'appealcits.WarningInspectionAddWindow',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
        //ToDo Пока невозможно перевести реестр обращения на роуты
        /* Закоментировал в связи с невозможностью перевода на роутинг
        ,
        context: 'B4.mixins.Context'*/
    },

    mainView: 'appealcits.Panel',
    mainViewSelector: 'appealCitsPanel',
    
    editWindowSelector: '#appealCitsEditWindow',
    baseStatementRealityObjectSelector: '#baseStatementAppCitsAddWindow #sfRealityObject',
    baseStatementContragentSelector: '#baseStatementAppCitsAddWindow #sfContragent',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealCitsPanel'
        },
        {
            ref: 'filterPanel',
            selector: '#appealcitsFilterPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.Inspection.WarningInspection.Create',
                    applyTo: '#btnCreateWarningInspection',
                    selector: '#appealCitsEditWindow',
                    applyBy: function(component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                { name: 'GkhGji.AppealCitizens.Create', applyTo: 'b4addbutton', selector: '#appealCitsGrid' },
                {
                    name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowClosedAppeals',
                    applyTo: '#cbShowCloseAppeals',
                    selector: '#appealCitsGrid',
                    applyBy: function (component, allowed) {
                        var me = this;

                        me.controller.params = me.controller.params || {};
                        if (allowed) {
                            component.show();
                        }
                        else {
                            component.hide();
                        }

                        // Проверка на выполнение функции preDisable в аспекте 
                        if (!component.wasPreDisabled) {
                            component.wasPreDisabled = true;
                        } else {
                            me.controller.params.showCloseAppeals = !allowed;
                            me.controller.getStore('AppealCits').load();
                        }
                    }
                },
                {
                    name: 'GkhGji.AppealCitizensState.Field.DateFrom_Edit',
                    applyTo: '[name=DateFrom]',
                    selector: '#appealCitsEditWindow',
                    applyBy: function (component, allowed) {
                        
                        component.setDisabled(!allowed);
                    }
                },
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.AppealCits.Field.Department_Rqrd', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindow' },
                { name: 'GkhGji.AppealCits.Field.IsPrelimentaryCheck', applyTo: '[name=IsPrelimentaryCheck]', selector: '#appealCitsEditWindow' },
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhGji.AppealCitizensState.Delete' }],
            name: 'deleteAppealCitsStatePerm'
        },
        {
            xtype: 'appealcitsperm',
            name: 'appealCitsStatePerm',
            editFormAspectName: 'appealCitizensWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'appealcitsanswerperm',
            name: 'appealCitsAnswerStatePerm',
            editFormAspectName: 'appealCitsAnswerGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'appealCitsStateButtonAspect',
            stateButtonSelector: '#appealCitsEditWindow #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var model = this.controller.getModel('AppealCits');
                    entityId ? model.load(entityId, {
                        success: function(rec) {
                            this.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(rec);
                            this.controller.getAspect('appealCitizensWindowAspect').setFormData(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'appealCitsStateTransferAspect',
            gridSelector: '#appealCitsGrid',
            stateType: 'gji_appeal_citizens',
            menuSelector: 'appealCitsGridStateMenu'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitizensWindowAspect',
            gridSelector: '#appealCitsGrid',
            editFormSelector: '#appealCitsEditWindow',
            storeName: 'AppealCits',
            modelName: 'AppealCits',
            editWindowView: 'appealcits.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.getId(), record.get('NumberGji'));
            },
            otherActions: function(actions) {
                actions['#appealcitsFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#appealcitsFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };

                actions[this.editFormSelector + ' #previousAppealCitsSelectField'] = { 'beforeload': { fn: this.onBeforeLoadPreviousAppeal, scope: this } };
                actions[this.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: this.onRedtapeFlagChange, scope: this } };
                actions[this.editFormSelector + ' #appealCitsSuretySelectField'] = {
                    'change': { fn: this.onSuretyChange, scope: this },
                    'beforeload': { fn: this.onSuretyBeforeLoad, scope: this }
                };
                actions[this.editFormSelector + ' #appealCitsExecutantSelectField'] = { 'change': { fn: this.onExecutantChange, scope: this } };
                actions[this.editFormSelector + ' #btnCreateStatement'] = { 'click': { fn: this.onCreateStatement, scope: this } };
                actions[this.editFormSelector + ' #btnCreateWarningInspection'] = { 'click': { fn: this.onCreateStatement, scope: this } };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onAfterSetFormData: function (aspect, rec, form) {
                // Изза того что в Gkh перекрыт аспект в нем 2 раза делается метод show у окна что приводит к повторному открытию окна
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта Executant и Surety
            // передать только Id на сохранение, поскольку если на сохранение уйдет Executant или Surety целиком,
            //то это поле тоже сохраниться и поля для записи Executant и Surety будут потеряны
            getRecordBeforeSave: function(record) {

                if (record && record.data) {
                    var executant = record.data.Executant;
                    if (executant && executant.Id > 0) {
                        record.data.Executant = executant.Id;
                    }

                    var surety = record.data.Surety;
                    if (surety && surety.Id > 0) {
                        record.data.Surety = surety.Id;
                    }
                }

                return record;
            },
            onBeforeLoadPreviousAppeal: function(store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            },
            onRedtapeFlagChange: function(field, newValue) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0];

                var previousAppealCitizensGjiSelectField = wnd.down('#previousAppealCitsSelectField');
                //TODO! Переделать
                previousAppealCitizensGjiSelectField.setDisabled(newValue > 2 ? false : true);
            },
            listeners: {
                aftersetformdata: function(asp, record, form) {
                    var id = record.getId();
                    var numberGji = record.get('NumberGji');
                    //Передаем аспекту смены статуса необходимые параметры
                    asp.controller.getAspect('appealCitsStateButtonAspect').setStateData(id, record.get('State'));

                    asp.controller.setCurrentId(id, numberGji);
                }
            },
            deleteRecord: function(record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteAppealCitsStatePerm').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
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
            },
            onSuretyChange: function (field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#appealCitsSuretyPositionTextField');
                if (data) {
                    fieldPosition.setValue(data.Position);
                } else {
                    fieldPosition.setValue(null);
                }
            },

            onSuretyBeforeLoad: function(field, options) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;
            },

            onExecutantChange: function(field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#appealCitsExecutantPositionTextField');
                if (data) {
                    fieldPosition.setValue(data.Position);
                } else {
                    fieldPosition.setValue(null);
                }
            },
            onChangeRealityObject: function(field, newValue) {
                if (newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
            onUpdateGrid: function() {
                var str = this.controller.getStore('AppealCits');
                str.currentPage = 1;
                str.load();
            },
            onCreateStatement: function (btn) {
                var me = this,
                    aspectName = btn.aspectName || 'baseStatementAppCitEditWindowAspect',
                    editAspect = me.controller.getAspect(aspectName);

                editAspect
                    .checkAppealCits(me.controller.appealCitizensId)
                    .next(function () {
                        B4.Ajax.request({
                            url: B4.Url.action('GetRealityObjects', 'AppealCitsRealObject'),
                            params: {
                                appealCitizensId: me.controller.appealCitizensId
                            }
                        }).next(function (resp) {
                            var roSelector = editAspect.editFormSelector + ' b4selectfield[name=Address]';
                            editAspect.editRecord();
                            
                            var res = Ext.JSON.decode(resp.responseText);

                            var sfRealityObject = Ext.ComponentQuery.query(roSelector)[0];
                            if (sfRealityObject) {
                                if (res.length == 1) {
                                    sfRealityObject.setValue(res[0].Id);
                                    sfRealityObject.setRawValue(res[0].Address);
                                    sfRealityObject.validate();
                                }
                            }
                        }).error(function (resp) {
                            editAspect.editRecord();
                        });

                    })
                    .error(function (resp) {
                        Ext.Msg.alert('Невозможно сформировать предостережение!', resp.message);
                    });
            },
         
            onChangeCheckbox: function(field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('AppealCits').load();
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля Связанные/Аналогичные обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitizensGJI/AddAppealCitizens
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'relatedAppealCitizensMultiSelectWindowAspect',
            fieldSelector: '#appealCitsEditWindow #trigfRelatedAppealCitizens',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#relatedAppCitSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            textProperty: 'NumberGji',
            columnsGridSelect: [
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата обращения', xtype: 'datecolumn', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, sortable: false },
                { header: 'Дата обращения', xtype: 'datecolumn', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
            ],
            titleSelectWindow: 'Выбор обращений граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCits', {
                            objectIds: recordIds,
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function(response) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Связанные/аналогичные обращения успешно сохранены');
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function(store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            }
        },
        {
            /*
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealCitsRealityObjectAspect',
            gridSelector: '#appealCitsRealObjGrid',
            storeName: 'appealcits.RealityObject',
            modelName: 'appealcits.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealCitizensRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function(store, operation) {
                operation.params = operation.params || {};
                operation.params.appealCitizensId = this.controller.appealCitizensId;
            },

            listeners: {
                getdata: function(asp, records) {

                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'AppealCitsRealObject', {
                            objectIds: recordIds,
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();

                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealSourcesGridWindowAspect',
            gridSelector: '#appealCitsSourceGrid',
            editFormSelector: '#appealCitsSourceEditWindow',
            storeName: 'appealcits.Source',
            modelName: 'appealcits.Source',
            editWindowView: 'appealcits.SourceEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'appealCitsButtonExportAspect',
            gridSelector: '#appealCitsGrid',
            buttonSelector: '#appealCitsGrid #btnExport',
            controllerName: 'AppealCits',
            actionName: 'Export'
        },
        {
            /**
            *Аспект взаимодействия таблицы тематик обращений проверки с формой массового выбора тематик
            */
            xtype: 'gkhmultiselectwindowtreeaspect',
            name: 'appealCitizensStatementSubjectAspect',
            gridSelector: '#appealCitsStatSubjectGrid',
            storeName: 'appealcits.StatSubject',
            modelName: 'appealcits.StatSubject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindowTree',
            multiSelectWindowSelector: '#appealCitsStatSubjectMultiSelectWindow',
            storeSelect: 'dict.StatSubjectTreeSelect',
            storeSelected: 'dict.Subj',
            titleSelectWindow: 'Выбор тематик обращения',
            titleGridSelected: 'Выбранные тематики обращений',
            columnsGridSelected: [
                { header: 'Тематика', xtype: 'gridcolumn', dataIndex: 'Subject', flex: 1, sortable: false },
                { header: 'Подтематика', xtype: 'gridcolumn', dataIndex: 'Subsubject', flex: 1, sortable: false },
                { header: 'Характеристика', xtype: 'gridcolumn', dataIndex: 'Feature', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function(me, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0]) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddStatementSubject', 'AppealCitsStatSubject', {
                            objectIds: recordIds,
                            appealCitizensId: me.controller.appealCitizensId
                        })).next(function(response) {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function() {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать тематики обращений');
                        return false;
                    }
                    return true;
                }
            },
            onCheckRec: function(node, checked) {
                var grid = this.getSelectedGrid(),
                    storeSelected = grid.getStore(),
                    model = this.controller.getModel('dict.Subj'),
                    id = node.get('id'),
                    arr;

                //если элемент конечный то добавляем в стор выбранных
                if (node.get('leaf')) {
                    if (checked) {
                        if (storeSelected.find('Id', id, 0, false, false, true) == -1) {

                            var newRec = new model();
                            arr = id.split('/');

                            newRec.set('Id', id);
                            if (arr[2]) {
                                newRec.set('Subject', node.parentNode.parentNode.get('text'));
                                newRec.set('Subsubject', node.parentNode.get('text'));
                                newRec.set('Feature', node.get('text'));
                            } else if (arr[1]) {
                                newRec.set('Subject', node.parentNode.get('text'));
                                newRec.set('Subsubject', node.get('text'));
                            } else {
                                newRec.set('Subject', node.get('text'));
                            }

                            storeSelected.add(newRec);
                        }
                    } else {
                        storeSelected.remove(storeSelected.getById(node.get('id')));
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsAnswerGridWindowAspect',
            gridSelector: '#appealCitsAnswerGrid',
            editFormSelector: '#appealCitsAnswerEditWindow',
            storeName: 'appealcits.Answer',
            modelName: 'appealcits.Answer',
            editWindowView: 'appealcits.AnswerEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                }
            }
        },
        {
            xtype: 'gkhgjinesteddigitalsignaturegridaspect',
            gridSelector: '#appealCitsAnswerGrid',
            controllerName: 'AppealCitsAnswer',
            name: 'appCitsAnswerNestedSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'requestGridWindowAspect',
            gridSelector: '#appealCitsRequestGrid',
            editFormSelector: '#appealCitsRequestEditWindow',
            storeName: 'appealcits.Request',
            modelName: 'appealcits.Request',
            editWindowView: 'appealcits.RequestEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                }
            }
        },
        {
            xtype: 'gkhgjinesteddigitalsignaturegridaspect',
            gridSelector: '#appealCitsRequestGrid',
            controllerName: 'AppealCitsRequest',
            name: 'appCitsRequestNestedSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * Аспект взаимодействия таблицы проверки и формы редактирования
            */
            xtype: 'basestatementappciteditwindowaspect',
            name: 'baseStatementAppCitEditWindowAspect',
        },
        {
            /*
            * Аспект взаимодействия таблицы проверки и формы редактирования
            */
            xtype: 'warninginspectionappciteditwindowaspect',
            name: 'warningInspectionAppCitEditWindowAspect',
            otherActions: function (actions) {
                actions['#appealcitsFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
            },
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'baseStatementnAppCitsAspect',
            gridSelector: '#appealCitsBaseStatGrid',
            storeName: 'appealcits.AppealCitsBaseStatement',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementnAppCitsMultiSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            titleSelectWindow: 'Выбор обращений граждан',
            titleGridSelect: 'Обращения граждан для отбора',
            titleGridSelected: 'Выбранные обращения граждан',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата обращения', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Количество вопросов', xtype: 'gridcolumn', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, sortable: false },
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } }
            ],

            listeners: {
                getdata: function(asp, records) {

                    //Id обращений
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Добавление', asp.controller.getMainComponent());
                        var store = asp.controller.getStore(asp.storeName);

                        var defRec = store.getAt(store.find('Id', asp.controller.appealCitizensId, 0, false, false, true));

                        store.removeAll();

                        store.add(defRec);

                        Ext.Array.each(records.items,
                            function(rec) {
                                store.add({
                                    Id: rec.get('Id'),
                                    NumberGji: rec.get('NumberGji')
                                });
                            }, this);
                        asp.controller.unmask();
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            },
            deleteRecord: function(record) {
                if (record.get('Id') != this.controller.appealCitizensId) {
                    this.controller.getStore('appealcits.AppealCitsBaseStatement').remove(record);
                }
            }
        }
    ],

    setCurrentId: function(id, numberGji) {
        this.appealCitizensId = id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            fieldRelatedAppealCitizens = editWindow.down('#trigfRelatedAppealCitizens'),
            tabpanel = editWindow.down('.tabpanel');

        var btnCreateStatement = editWindow.down('#btnCreateStatement');
        btnCreateStatement.setDisabled(!id);

        //грид во вкладке место возникновения проблемы
        var storeRo = this.getStore('appealcits.RealityObject');
        var storeStatement = this.getStore('appealcits.StatSubject');
        var sourceStore = this.getStore('appealcits.Source');
        var storeAnswer = this.getStore('appealcits.Answer');
        var storeRequest = this.getStore('appealcits.Request');
        var storeBaseStatement = this.getStore('appealcits.BaseStatement');
        var storeWarningInspection = this.getStore('appealcits.WarningInspection');
        var storeAppCitsBaseStatement = this.getStore('appealcits.AppealCitsBaseStatement');

        sourceStore.removeAll();
        storeRo.removeAll();
        storeStatement.removeAll();
        storeAnswer.removeAll();
        storeRequest.removeAll();
        storeBaseStatement.removeAll();
        storeWarningInspection.removeAll();
        storeAppCitsBaseStatement.removeAll();

        tabpanel.down('#tabLocationProblem').tab.setDisabled(!id);
        tabpanel.down('#tabSources').tab.setDisabled(!id);
        tabpanel.down('#tabApproval').tab.setDisabled(!id);
        tabpanel.down('#tabStatementSubject').tab.setDisabled(!id);
        tabpanel.down('#appealCitsAnswerGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsRequestGrid').tab.setDisabled(!id);
        tabpanel.down('#baseStatementAppCitsGrid').tab.setDisabled(!id);
        tabpanel.setActiveTab(0);

        editWindow.down('#trigfRelatedAppealCitizens').setDisabled(!id);

        if (id > 0) {
            storeRo.load();
            storeStatement.load();
            sourceStore.load();
            storeAnswer.load();
            storeRequest.load();
            storeBaseStatement.load();
            storeWarningInspection.load();

            storeAppCitsBaseStatement.add({
                Id: this.appealCitizensId,
                NumberGji: numberGji
            });

            this.mask('Загрузка', this.getMainView());
            B4.Ajax.request(B4.Url.action('GetInfo', 'AppealCits', {
                appealCitizensId: this.appealCitizensId
            })).next(function(response) {
                this.unmask();
                var obj = Ext.JSON.decode(response.responseText);

                fieldRelatedAppealCitizens.updateDisplayedText(obj.relatedAppealNames);
                fieldRelatedAppealCitizens.setValue(obj.relatedAppealIds);
            }, this)
                .error(function() {
                    this.unmask();
                }, this);
        }
    },
    
    init: function() {
        this.getStore('AppealCits').on('beforeload', this.onBeforeLoadAppealCits, this);
        this.getStore('appealcits.RealityObject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.StatSubject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Source').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Answer').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Request').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.BaseStatement').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.WarningInspection').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },
    //ToDo Пока невозможно перевести реестр обращения на роуты
    /*Закомментировал в связи с невозможностью перевода на роутинг
    index: function () {
        var view = this.getMainView() || Ext.widget('appealCitsPanel');
        this.bindContext(view);
        this.application.deployView(view);

        this.params = {};
        this.params.dateFromStart = null;
        this.params.dateFromEnd = null;
        this.params.checkTimeStart = null;
        this.params.checkTimeEnd = null;

        this.params.realityObjectId = null;
    },

    edit: function (id) {
        var view = this.getMainView() || Ext.widget('appealCitsPanel');
        
        if (view && !view.rendered) {
            this.bindContext(view);
            this.application.deployView(view);

            this.params = {};
            this.params.dateFromStart = null;
            this.params.dateFromEnd = null;
            this.params.checkTimeStart = null;
            this.params.checkTimeEnd = null;

            this.params.realityObjectId = null;
        }

        var model = this.getModel('AppealCits');
        this.getAspect('appealCitizensWindowAspect').editRecord(new model({ Id: id }));
    },
    */
    //ToDo Убрать метод только в случае переывода на роутинг когда окно будет октрываться отдельной вклдкой а не модальныйм окном
    onLaunch: function () {
        if (this.params && this.params.appealId > 0) {
            var model = this.getModel('AppealCits');
            this.getAspect('appealCitizensWindowAspect').editRecord(new model({ Id: this.params.appealId }));
            this.params.appealId = 0;
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.appealCitizensId = this.appealCitizensId;
    },

    onBeforeLoadAppealCits: function(store, operation) {     
        var filterPanel = this.getFilterPanel();
        if (filterPanel) {
            operation.params.dateFromStart = filterPanel.down('#dfDateFromStart').getValue();
            operation.params.dateFromEnd = filterPanel.down('#dfDateFromEnd').getValue();
            operation.params.checkTimeStart = filterPanel.down('#dfCheckTimeStart').getValue();
            operation.params.checkTimeEnd = filterPanel.down('#dfCheckTimeEnd').getValue();                      
        }
        if (this.params) {
            operation.params.realityObjectId = this.params.realityObjectId;           
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
        }
    },
    
    setManOrg: function (roId) {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetJurOrgs', 'AppealCitsRealObject'),
            params: {
                appealCitizensId: this.appealCitizensId,
                realityObjectId: roId
            }
        }).next(function(resp) {

            var res = Ext.JSON.decode(resp.responseText);

            if (res != null && res.length > 0) {
                var sfContragent = Ext.ComponentQuery.query(me.baseStatementContragentSelector)[0];
                if (sfContragent) {
                    sfContragent.setValue(res[0].Id);
                    sfContragent.setRawValue(res[0].Name);
                    sfContragent.validate();
                }
            }
        });
    }
});