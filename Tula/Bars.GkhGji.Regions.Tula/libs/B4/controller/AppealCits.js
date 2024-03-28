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
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.permission.AppealCits',
        'B4.aspects.permission.AppealCitsAnswer',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.view.appealcits.CheckTimeHistory'
    ],

    appealCitizensId: null,
    
    models: [
        'BaseStatement',
        'AppealCits',
        'dict.Subj',
        'appealcits.Request',
        'appealcits.AppealCitsExecutant',
        'appealcits.Answer'
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
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'appealcits.AppealCitsExecutant'
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
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'appealcits.AppealCitsExecutantGrid',
        'appealcits.ExecutantEditWindow',
        'appealcits.MultiSelectWindowExecutant',
        'appealcits.CheckTimeHistory'
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
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.AppealCitizens.Create', applyTo: 'b4addbutton', selector: '#appealCitsGrid' },
                {
                    name: 'GkhGji.AppealCitizens.CheckBoxShowCloseApp',
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
                }
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
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'appealCitsAnswerGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentName_Edit', applyTo: '[name=DocumentName]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentNumber_Edit', applyTo: '[name=DocumentNumber]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentDate_Edit', applyTo: '[name=DocumentDate]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Addressee_Edit', applyTo: '[name=Addressee]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Executor_Edit', applyTo: '[name=Executor]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.AnswerContent_Edit', applyTo: '[name=AnswerContent]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.File_Edit', applyTo: '[name=File]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Description_Edit', applyTo: '[name=Description]', selector: '#appealCitsAnswerEditWindow', applyBy: function (component, allowed) {
                           if (component) {
                               component.setReadOnly(!allowed);
                           }
                       }
                   },

                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentName_View', applyTo: '[name=DocumentName]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentNumber_View', applyTo: '[name=DocumentNumber]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.DocumentDate_View', applyTo: '[name=DocumentDate]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Addressee_View', applyTo: '[name=Addressee]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Executor_View', applyTo: '[name=Executor]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.AnswerContent_View', applyTo: '[name=AnswerContent]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.File_View', applyTo: '[name=File]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   },
                   {
                       name: 'GkhGji.AppealCitizensState.Answer.Field.Description_View', applyTo: '[name=Description]', selector: '#appealCitsAnswerEditWindow',
                       applyBy: function (component, allowed) {
                           if (component) {
                               if (allowed) component.show();
                               else component.hide();
                           }
                       }
                   }
            ],
            name: 'appCitsAnswerStatePerm'
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
                    var me = this,
                        model = me.controller.getModel('AppealCits');
                    entityId ? model.load(entityId, {
                        success: function(rec) {
                            me.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(rec);
                            me.controller.getAspect('appealCitizensWindowAspect').setFormData(rec);
                        },
                        scope: me
                    }) : me.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'appealCitsStateAnswerButtonAspect',
            stateButtonSelector: '#appealCitsAnswerEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var me = this,
                        model = me.controller.getModel('appealcits.Answer');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            me.controller.getAspect('appCitsAnswerStatePerm').setPermissionsByRecord(rec);
                            me.controller.getAspect('appealCitsAnswerGridWindowAspect').setFormData(rec);
                        },
                        scope: me
                    }) : me.controller.getAspect('appCitsAnswerStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
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
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'appealCitsStateAnswerTransferAspect',
            gridSelector: '#appealCitsAnswerGrid',
            stateType: 'gji_appeal_cits_answer',
            menuSelector: 'appealCitsAnswerGridStateMenu'
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
            otherActions: function (actions) {
                var me = this;
                
                actions['#appealcitsFilterPanel #sfRealityObject'] = { 'change': { fn: me.onChangeRealityObject, scope: me } };
                actions['#appealcitsFilterPanel #updateGrid'] = { 'click': { fn: me.onUpdateGrid, scope: me } };
                actions['#appealcitsFilterPanel #dfDateFromStart'] = { 'change': { fn: me.onChangeDateFromStart, scope: me } };
                actions['#appealcitsFilterPanel #dfDateFromEnd'] = { 'change': { fn: me.onChangeDateFromEnd, scope: me } };
                actions['#appealcitsFilterPanel #dfCheckTimeStart'] = { 'change': { fn: me.onChangeCheckTimeStart, scope: me } };
                actions['#appealcitsFilterPanel #dfCheckTimeEnd'] = { 'change': { fn: me.onChangeCheckTimeEnd, scope: me } };
                actions['#appealcitsFilterPanel #sfAuthor'] = { 'change': { fn: me.onChangeAuthor, scope: me } };
                actions['#appealcitsFilterPanel #sfExecutant'] = { 'change': { fn: me.onChangeExecutant, scope: me } };

                actions[me.editFormSelector + ' #previousAppealCitsSelectField'] = { 'beforeload': { fn: me.onBeforeLoadPreviousAppeal, scope: me } };
                actions[me.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: me.onRedtapeFlagChange, scope: me } };
                actions[me.editFormSelector + ' #appealCitsSuretySelectField'] = {
                    'change': { fn: me.onSuretyChange, scope: me },
                    'beforeload': { fn: me.onSuretyBeforeLoad, scope: me }
                };
                actions[me.editFormSelector + ' #appealCitsExecutantSelectField'] = { 'change': { fn: me.onExecutantChange, scope: me } };
                actions[me.editFormSelector + ' #btnCreateStatement'] = { 'click': { fn: me.onCreateStatement, scope: me } };
                actions[me.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: me.onChangeCheckbox, scope: me } };
                actions[me.editFormSelector + ' button[actionName=checkTimeHistoryBtn]'] = { 'click': { fn: me.showCheckTimeHistory, scope: me } };
                actions['appealCitsCheckTimeHistory b4updatebutton'] = { 'click': { fn: me.showCheckTimeHistoryUpdate, scope: me } };
            },
            onAfterSetFormData: function (aspect, rec, form) {
                // Изза того что в Gkh перекрыт аспект в нем 2 раза делается метод show у окна что приводит к повторному открытию окна
                form.down('[name=isImported]').setValue(rec.get('IsImported'));
            },
            showCheckTimeHistoryUpdate: function (btn) {
                var grid = btn.up('grid');
                grid.getStore().load();
            },
            showCheckTimeHistory: function () {
                var me = this,
                    win = Ext.widget('appealCitsCheckTimeHistory', {
                        modal:true,
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy'
                    }),
                    store = win.down('grid').getStore();
                
                store.clearFilter(true);
                store.filter('appealCitizensId', me.controller.appealCitizensId);
                win.show();
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
            onChangeExecutant: function (field, newValue) {
                if (newValue) {
                    this.controller.params.executantId = newValue.Id;
                } else {
                    this.controller.params.executantId = null;
                }
            },
            onChangeAuthor: function (field, newValue) {
                if (newValue) {
                    this.controller.params.authorId = newValue.Id;
                } else {
                    this.controller.params.authorId = null;
                }
            },
            onUpdateGrid: function() {
                var str = this.controller.getStore('AppealCits');
                str.currentPage = 1;
                str.load();
            },
            onCreateStatement: function (btn) {
                var me = this;
                me.controller
                    .getAspect("baseStatementAppCitEditWindowAspect")
                    .checkAppealCits(me.controller.appealCitizensId)
                    .next(function() {
                        B4.Ajax.request({
                            url: B4.Url.action('GetRealityObjects', 'AppealCitsRealObject'),
                            params: {
                                appealCitizensId: me.controller.appealCitizensId
                            }
                        }).next(function (resp) {
                            me.controller.getAspect("baseStatementAppCitEditWindowAspect").editRecord();

                            var res = Ext.JSON.decode(resp.responseText);

                            var sfRealityObject = Ext.ComponentQuery.query(me.controller.baseStatementRealityObjectSelector)[0];
                            if (sfRealityObject) {
                                if (res.length == 1) {
                                    sfRealityObject.setValue(res[0].Id);
                                    sfRealityObject.setRawValue(res[0].Address);
                                    sfRealityObject.validate();
                                }
                            }
                        }).error(function (resp) {
                            me.controller.getAspect("baseStatementAppCitEditWindowAspect").editRecord();
                        });
                        
                    })
                    .error(function(resp) {
                        Ext.Msg.alert('Невозможно сформировать проверку!', resp.message);
                    });
            },
            onChangeDateFromStart: function(field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromStart = newValue;
                } else {
                    this.controller.params.dateFromStart = null;
                }
            },
            onChangeDateFromEnd: function(field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromEnd = newValue;
                } else {
                    this.controller.params.dateFromEnd = null;
                }
            },
            onChangeCheckTimeStart: function(field, newValue, oldValue) {

                if (newValue) {
                    this.controller.params.checkTimeStart = newValue;
                } else {
                    this.controller.params.checkTimeStart = null;
                }
            },
            onChangeCheckTimeEnd: function(field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.checkTimeEnd = newValue;
                } else {
                    this.controller.params.checkTimeEnd = null;
                }
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
                },
                aftersetformdata: function (asp, record) {

                    asp.controller.getAspect('appealCitsStateAnswerButtonAspect').setStateData(record.getId(), record.get('State'));
                }
            }
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
            /*
            * Аспект взаимодействия таблицы проверки и формы редактирования
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseStatementAppCitEditWindowAspect',
            gridSelector: '#baseStatementAppCitsGrid',
            storeName: 'appealcits.BaseStatement',
            modelName: 'appealcits.BaseStatement',
            editFormSelector: '#baseStatementAppCitsAddWindow',
            editWindowView: 'appealcits.BaseStatementAddWindow',
            controllerEditName: 'B4.controller.basestatement.Navigation',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions[this.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editFormSelector + ' #sfRealityObject'] = { 'beforeload': { fn: this.onBeforeLoadRealityObject, scope: this } };
            },
            saveRecord: function (rec) {
                var me = this;
                if (this.fireEvent('beforesave', this, rec) !== false) {
                    var frm = me.getForm();
                    me.mask('Сохранение', frm);

                    // Проверяем наличие тематик
                    me.checkAppealCits(me.controller.appealCitizensId).next(function () {
                        var realtyObjId = Ext.ComponentQuery.query(me.controller.baseStatementRealityObjectSelector)[0].getValue();
                        var contragentId = Ext.ComponentQuery.query(me.controller.baseStatementContragentSelector)[0].getValue();

                        var storeAppealCits = me.controller.getStore('appealcits.AppealCitsBaseStatement');

                        var appealCits = [];
                        Ext.Array.each(storeAppealCits.getRange(0, storeAppealCits.getCount()),
                            function(item) {
                                appealCits.push(item.get('Id'));
                            });

                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('CreateWithAppealCits', 'BaseStatement'),
                            params: {
                                realtyObjId: realtyObjId,
                                contragentId: contragentId,
                                baseStatement: Ext.encode(rec.data),
                                appealCits: Ext.encode(appealCits)
                            }
                        }).next(function(result) {
                            me.unmask();
                            me.updateGrid();
                            var res = Ext.decode(result.responseText);
                            var baseStatementId = res.data.Id;

                            var model = me.controller.getModel('BaseStatement');
                            model.load(baseStatementId, {
                                success: function(recBaseStatement) {
                                    me.fireEvent('savesuccess', me, recBaseStatement);
                                },
                                scope: me
                            });
                            return true;
                        }).error(function(result) {
                            me.unmask();
                            me.fireEvent('savefailure', result.record, result.responseData);

                            Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.message);
                        });
                    }).error(function (resp) {
                        me.unmask();
                        B4.QuickMsg.msg("Ошибка", resp.message, "error");
                    });
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = this.controller.getModel('BaseStatement');
                
                if (id) {
                    if (me.controllerEditName) {
                        var portal = me.controller.getController('PortalController');

                        if (!me.controller.hideMask) {
                            me.controller.hideMask = function () { me.controller.unmask(); };
                        }

                        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                        me.controller.mask('Загрузка', me.controller.getMainComponent());
                        portal.loadController(me.controllerEditName, record, portal.containerSelector, me.controller.hideMask);
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            }
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            onBeforeLoadContragent: function(store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
                operation.params.roId = this.getForm().down('#sfRealityObject').getValue();
            },
            onBeforeLoadRealityObject: function(store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                var realityObjIds = [];
                this.controller.getStore('appealcits.RealityObject').each(function(obj) {
                    realityObjIds.push(obj.get('RealityObjectId'));
                });
                operation.params.realityObjIds = realityObjIds.length == 0 ? -1 : realityObjIds;
            },
            onChangeType: function(field, newValue) {
                this.controller.params = this.controller.params || {};
                this.controller.params.typeJurOrg = newValue;
                this.getForm().down('#sfContragent').setValue(null);
                if (newValue == 10) {
                    this.controller.setManOrg();
                }
                this.getForm().down('#tfPhysicalPerson').setValue(null);
            },
            onChangePerson: function(field, newValue) {
                var form = this.getForm(),
                    sfContragent = form.down('#sfContragent'),
                    tfPhysicalPerson = form.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = form.down('#cbTypeJurPerson');
                sfContragent.setValue(null);
                tfPhysicalPerson.setValue(null);
                cbTypeJurPerson.setValue(10);
                
                switch (newValue) {
                    case 10:
                        //физлицо
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        break;
                    case 20:
                        //организацияы
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                    case 30:
                        //должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                }
            },
            
            checkAppealCits: function (appealCitizensId) {
                return B4.Ajax.request({
                    url: B4.Url.action('CheckAppealCits', 'BaseStatement'),
                    params: {
                        appealCitizensId: appealCitizensId
                    }
                });
            }
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
        },
        {
            /*
            Аспект смены статуса в гриде исполнителей обращения
            */
            xtype: 'b4_state_contextmenu',
            name: 'appcitsExecutantStateTransferAspect',
            gridSelector: '#appealCitsExecutantGrid',
            menuSelector: 'appcitsExecutantGridStateMenu',
            stateType: 'gji_appcits_executant'
        },
        {
            /*
            Аспект смены статуса в карточке редактирования исполнителя обращения
            */
            xtype: 'statebuttonaspect',
            name: 'appcitsExecutantStateButtonAspect',
            stateButtonSelector: '#appealCitsExecutantEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                    var editWindowAspect = asp.controller.getAspect('appealCitsExecutantGridWindowAspect');
                    editWindowAspect.updateGrid();

                    var model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /* 
               Аспект взаимодействия таблицы Исполнителей и грида с массовым доабавлением
            */
             xtype: 'gkhgridmultiselectwindowaspect',
             name: 'appealCitsExecutantGridWindowAspect',
             gridSelector: '#appealCitsExecutantGrid',
             storeName: 'appealcits.AppealCitsExecutant',
             modelName: 'appealcits.AppealCitsExecutant',
             multiSelectWindow: 'appealcits.MultiSelectWindowExecutant',
             multiSelectWindowSelector: '#appealCitsExecutantMultiSelectWindowExecutant',
             editFormSelector: '#appealCitsExecutantEditWindow',
             editWindowView: 'appealcits.ExecutantEditWindow',
             storeSelect: 'dict.InspectorForSelect',
             storeSelected: 'dict.InspectorForSelected',
             titleSelectWindow: 'Выбор исполнителей',
             titleGridSelect: 'Исполнители',
             titleGridSelected: 'Выбранные исполнители',
             columnsGridSelect: [
                 { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1, filter: { xtype: 'textfield' } }
             ],
             columnsGridSelected: [
                 { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                 { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1 }
             ],
             otherActions: function (actions) {
                 actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
             },
             onBeforeLoad: function (store, operation) {
                 operation.params.excludeInpectorId = this.controller.inpectorId;
             },
             listeners: {
                 getdata: function (asp, records) {
                     var recordIds = [];

                     records.each(function (rec) {
                         recordIds.push(rec.get('Id'));
                     });

                     var dateField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfPerformanceDate')[0];
                     if (dateField.value) {
                         if (recordIds[0] > 0) {
                             asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                             B4.Ajax.request(B4.Url.action('AddExecutants', 'AppealCitsExecutant', {
                                 inspectorIds: Ext.encode(recordIds),
                                 appealCitizensId: asp.controller.appealCitizensId,
                                 performanceDate: dateField.value
                             })).next(function () {
                                 asp.controller.getStore(asp.storeName).load();
                                 asp.controller.unmask();
                                 Ext.Msg.alert('Сохранено!', 'Исполнители сохранены успешно');
                                 return true;
                             }).error(function (result) {
                                 asp.controller.unmask();
                                 Ext.Msg.alert('Ошибка', result.message ? result.message : 'Произошла ошибка');
                             });
                         } else {
                             Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                             return false;
                         }
                     } else {
                         Ext.Msg.alert('Ошибка!', 'Необходимо выбрать срок исполнения');
                         return false;
                     }

                     return true;
                 },
                 aftersetformdata: function (asp, record) {
                     this.controller.getAspect('appcitsExecutantStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                 }
             }
         },
        {   /* 
               Аспект взаимодейсвтия кнопки Перенаравить с массовой формой выбора исполнителей
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'appealCitsRedirectExecutantAspect',
            buttonSelector: '#appealCitsExecutantEditWindow #btnRedirect',
            multiSelectWindowSelector: '#appealCitsRedirectExecutantSelectWindow',
            multiSelectWindow: 'appealcits.MultiSelectWindowExecutant',
            storeName: 'appealcits.AppealCitsExecutant',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор исполнителей',
            titleGridSelect: 'Исполнители',
            titleGridSelected: 'Выбранные исполнители',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.excludeInpectorId = this.controller.inpectorId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        btn = Ext.ComponentQuery.query(this.buttonSelector)[0],
                        form = btn.up('#appealCitsExecutantEditWindow').getForm(),
                        record = form.getRecord();

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var dateField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfPerformanceDate')[0];

                    if (!dateField.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать срок исполнения');
                        return false;
                    }

                    if (recordIds.length == 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                        return false;
                    }

                    asp.controller.mask('Перенаправленеи', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('RedirectExecutant', 'AppealCitsExecutant', {
                        objectIds: Ext.encode(recordIds),
                        executantId: record.getId(),
                        performanceDate: dateField.value
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Перенаправление выполнено успешно');
                        return true;
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка перенаправления', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                }
            }
        }
    ],

    setCurrentId: function(id, numberGji) {
        this.appealCitizensId = id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            fieldRelatedAppealCitizens = editWindow.down('#trigfRelatedAppealCitizens'),
            tabpanel = editWindow.down('.tabpanel'),
            btnCreateStatement = editWindow.down('#btnCreateStatement'),
            storeRo = this.getStore('appealcits.RealityObject'),
            storeStatement = this.getStore('appealcits.StatSubject'),
            sourceStore = this.getStore('appealcits.Source'),
            storeAnswer = this.getStore('appealcits.Answer'),
            storeRequest = this.getStore('appealcits.Request'),
            storeBaseStatement = this.getStore('appealcits.BaseStatement'),
            storeAppCitsBaseStatement = this.getStore('appealcits.AppealCitsBaseStatement'),
            storeAppealCitsExecutant = this.getStore('appealcits.AppealCitsExecutant');
            
        
        btnCreateStatement.setDisabled(!id);

        sourceStore.removeAll();
        storeRo.removeAll();
        storeStatement.removeAll();
        storeAnswer.removeAll();
        storeRequest.removeAll();
        storeBaseStatement.removeAll();
        storeAppCitsBaseStatement.removeAll();
        storeAppealCitsExecutant.removeAll();

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
            storeAppealCitsExecutant.load();

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
        this.getStore('appealcits.AppealCitsExecutant').on('beforeload', this.onBeforeLoad, this);
        
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
        if (this.params) {
            operation.params.authorId = this.params.authorId;
            operation.params.executantId = this.params.executantId;
            operation.params.realityObjectId = this.params.realityObjectId;
            operation.params.dateFromStart = this.params.dateFromStart;
            operation.params.dateFromEnd = this.params.dateFromEnd;
            operation.params.checkTimeStart = this.params.checkTimeStart;
            operation.params.checkTimeEnd = this.params.checkTimeEnd;
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
        }
    },
    
    setManOrg: function () {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetJurOrgs', 'AppealCitsRealObject'),
            params: {
                appealCitizensId: this.appealCitizensId
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