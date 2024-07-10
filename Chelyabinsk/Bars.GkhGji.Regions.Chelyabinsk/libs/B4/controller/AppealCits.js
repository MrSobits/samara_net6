//ToDo данный контроллер нельзя переводить на роуты поскольку у него форма редактирвоания открывается в модальном окне, и нельзя без реестра вызывать отдельно открытие карточки редактирвоания обращения
//ToDo необходимо данный контроллер переделать на отдельно открывающуюся панель а не модальное окно

Ext.define('B4.controller.AppealCits', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhBlobText',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.form.FileField',
        'B4.aspects.GkhBlobText',
        'B4.mixins.Context',
        'B4.data.Connection',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.permission.AppealCits',
        'B4.aspects.permission.AppealCitsAnswer',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.view.PreviewFileWindow',
        'B4.aspects.GkhGjiNestedDigitalSignatureGridAspect'
    ],

    appealCitizensId: null,
    answerId: null,
    appealCitsAdmonition: null,
    appealCitsAnswer: null,
    appealCitsDecision: null,
    appealCitsEmergencyHouse: null,

    currentPerson: null,
    author: null,
    executor: null,
    fileId: null,

    models: [
        'BaseStatement',
        'appealcits.Decision',
        'appealcits.Definition',
        'AppealCits',
        'dict.Subj',
        'appealcits.Request',
        'appealcits.RequestAnswer',
        'appealcits.AppealCitsExecutant',
        'administration.Operator',
        'appealcits.AppealCitsAdmonition',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitAdmonAnnex',
        'appealcits.AppealCitsEmergencyHouse'
    ],

    stores: [
        'AppealCits',
        'appealcits.Answer',
        'appealcits.Decision',
        'appealcits.Definition',
        'appealcits.Source',
        'appealcits.StatSubject',
        'appealcits.RealityObject',
        'appealcits.Request',
        'appealcits.RequestAnswer',
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
        'appealcits.AppealCitsExecutant',
        'dict.ApplicantCategory',
        'appealcits.AppealCitsEmergencyHouse',
        'appealcits.AppealCitsAdmonition',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitAdmonAnnex'
    ],

    views: [
        'appealcits.Grid',
        'appealcits.DecisionGrid',
        'appealcits.DecisionEditWindow',
        'appealcits.DefinitionGrid',
        'appealcits.DefinitionEditWindow',
        'appealcits.EditWindow',
        'appealcits.RealityObjectGrid',
        'appealcits.StatSubjectGrid',
        'appealcits.SourceGrid',
        'appealcits.SourceEditWindow',
        'appealcits.AnswerGrid',
        'appealcits.AnswerEditWindow',
        'appealcits.RequestGrid',
        'appealcits.RequestEditWindow',
        'appealcits.RequestAnswerGrid',
        'appealcits.RequestAnswerEditWindow',
        'appealcits.BaseStatementGrid',
        'appealcits.Panel',
        'appealcits.LtextEditWindow',
        'appealcits.FilterPanel',
        'appealcits.BaseStatementAddWindow',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'appealcits.AppealCitsExecutantGrid',
        'appealcits.ExecutantEditWindow',
        'appealcits.MultiSelectWindowExecutant',
        'appealcits.RelatedAppealCitsGrid',
        'appealcits.AppealCitsAttachmentEditWindow',
        'appealcits.AppealCitsAttachmentGrid',
        'appealcits.AppealCitsQuestionGrid',
        'appealcits.AppealCitsHeadInspectorGrid',
        'appealcits.AppealCitsAnswerAttachmentEditWindow',
        'appealcits.AppealCitsAnswerAttachmentGrid',
        'appealcits.AnswerStatSubjectGrid',
        'appealcits.AppealCitsAdmonitionGrid',
        'appealcits.AppealCitsAdmonitionEditWindow',
        'appealcits.AppealCitsEmergencyHouseGrid',
        'appealcits.AppealCitsEmergencyHouseEditWindow',
        'appealcits.AppCitAdmonVoilationGrid',
        'appealcits.AppCitAdmonVoilationEditWindow',
        'appealcits.AppCitAdmonAnnexGrid',
        'appealcits.AppCitAdmonAnnexEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
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
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsDecisionPrintAspect',
            buttonSelector: '#appealCitsDecisionEditWindow #btnPrint',
            codeForm: 'AppealDecision',
            getUserParams: function () {
                var param = { Id: this.controller.appealCitsDecision };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#appealCitsExecutantMultiSelectWindowExecutant' },
            requirements: [
                {
                    name: 'GkhGji.AppealCits.Executant.Field.PerformanceDate',
                    applyTo: 'datefield[name=PerformanceDate]',
                    selector: '#appealCitsExecutantMultiSelectWindowExecutant'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'appealCitsExecutantStatePerm',
            editFormAspectName: 'appealCitsExecutantGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Surety_Edit', applyTo: '[name=Author]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_Edit', applyTo: '[name=Resolution]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyDate_Edit', applyTo: '[name=OrderDate]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Executant_Edit', applyTo: '[name=Executant]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Tester_Edit', applyTo: '[name=Controller]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ExecuteDate_Edit', applyTo: '[name=PerformanceDate]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ZonalInspection_Edit', applyTo: '[name=ExecutantZji]', selector: '#appealCitsExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ApprovalContragent_View', applyTo: '[name=ApprovalContragent]', selector: '#appealCitsExecutantEditWindow' }
            ]
        },
        {
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#appealCitsRedirectExecutantSelectWindow' },
            requirements: [
                {
                    name: 'GkhGji.AppealCits.Executant.Field.PerformanceDate',
                    applyTo: 'datefield[name=PerformanceDate]',
                    selector: '#appealCitsRedirectExecutantSelectWindow'
                }
            ]
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
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
                    name: 'GkhGji.AppealCitizens.ShowOnlyFromEais', applyTo: 'checkbox[name=ShowOnlyFromEais]', selector: '#appealCitsGrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
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
        //{
        //    xtype: 'appealcitsanswerperm',
        //    name: 'appealCitsAnswerStatePerm',
        //    editFormAspectName: 'appealCitsAnswerGridWindowAspect',
        //    setPermissionEvent: 'aftersetformdata'
        //},
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsAdmonitionPrintAspect',
            buttonSelector: '#appealCitsAdmonitionEditWindow #btnPrint',
            codeForm: 'AppealCitsAdmonition',
            getUserParams: function () {
                var param = { Id: appealCitsAdmonition };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'appealCitsStateButtonAspect',
            stateButtonSelector: '#appealCitsEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var model = this.controller.getModel('AppealCits');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(rec);
                            this.controller.getAspect('appealCitizensWindowAspect').setFormData(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('appealCitsStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.AppealCits.Field.Department_Rqrd', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindow' }
            ]
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
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId(), record.get('NumberGji'));
            },
            otherActions: function (actions) {
                actions['#appealcitsFilterPanel #sfStatSubj'] = { 'change': { fn: this.onChangeStatSubject, scope: this } };
                actions['#appealcitsFilterPanel #sfStatSubsubjectGji'] = { 'change': { fn: this.onChangeStatSubsubjectGji, scope: this } };
                actions['#appealCitsEditWindow #cbTypeCorrespondent'] = { 'change': { fn: this.onChangeTypeCorrespondent, scope: this } };
                actions['#appealcitsFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#appealcitsFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#appealcitsFilterPanel #dfDateFromStart'] = { 'change': { fn: this.onChangeDateFromStart, scope: this } };
                actions['#appealcitsFilterPanel #dfDateFromEnd'] = { 'change': { fn: this.onChangeDateFromEnd, scope: this } };
                actions['#appealcitsFilterPanel #dfCheckTimeStart'] = { 'change': { fn: this.onChangeCheckTimeStart, scope: this } };
                actions['#appealcitsFilterPanel #dfCheckTimeEnd'] = { 'change': { fn: this.onChangeCheckTimeEnd, scope: this } };
                actions['#appealcitsFilterPanel #sfAuthor'] = { 'change': { fn: this.onChangeAuthor, scope: this } };
                actions['#appealcitsFilterPanel #sfExecutant'] = { 'change': { fn: this.onChangeExecutant, scope: this } };
                actions['#appealcitsFilterPanel #sfController'] = { 'change': { fn: this.onChangeController, scope: this } };
                actions[this.editFormSelector + ' #btnCopy'] = { 'click': { fn: this.onCopyClick, scope: this } };

                actions[this.editFormSelector + ' #previousAppealCitsSelectField'] = { 'beforeload': { fn: this.onBeforeLoadPreviousAppeal, scope: this } };
                actions[this.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: this.onRedtapeFlagChange, scope: this } };
                actions[this.editFormSelector + ' #appealCitsSuretySelectField'] = {
                    'change': { fn: this.onSuretyChange, scope: this },
                    'beforeload': { fn: this.onSuretyBeforeLoad, scope: this }
                };
                actions[this.editFormSelector + ' #appealCitsExecutantSelectField'] = { 'change': { fn: this.onExecutantChange, scope: this } };
                actions[this.editFormSelector + ' #btnCreateStatement'] = { 'click': { fn: this.onCreateStatement, scope: this } };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                actions[this.editFormSelector + ' button[actionName=checkTimeHistoryBtn]'] = { 'click': { fn: this.showCheckTimeHistory, scope: this } };
                actions[this.editFormSelector + ' [name=SuretyResolve]'] = { 'change': { fn: this.onSuretyResolveChange, scope: this } };
                actions[this.editFormSelector + ' #btnSOPR'] = { 'click': { fn: this.goToSOPR, scope: this } };

                actions['#appealcitsFilterPanel #clear'] = { 'click': { fn: this.clearAllFilters, scope: this } };
                actions[this.gridSelector + ' #cbShowExtensTimes'] = { 'change': { fn: this.onExtensTimesCheckbox, scope: this } };
                actions[this.gridSelector + ' checkbox[name=ShowOnlyFromEais]'] = { 'change': { fn: this.onShowOnlyFromEais, scope: this } };
                actions[this.gridSelector + ' checkbox[name=ShowFavorites]'] = { 'change': { fn: this.onShowFavorites, scope: this } };

                actions[this.editFormSelector + ' #sfQuestionStatus'] = { 'change': { fn: this.onChangeQuestionStatus, scope: this } };

                actions[this.editFormSelector + ' button[name=btnGetAttachmentArchive]'] = { 'click': { fn: this.onGetAttachmentArchiveClick, scope: this } };
            },

            onCopyClick: function (btn) {
                var asp = this,
                    rec = asp.getForm().getRecord();
                asp.controller.mask('Копирование входящего документа', asp.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('CopyAppeal', 'AppCitOperations'),
                    params: {
                        docId: rec.getId()
                    }
                }).next(function (response) {
                    var newId = Ext.decode(response.responseText);
                    B4.QuickMsg.msg('Копирование обращения', 'Обращение скопировано', 'success');
                    var model = asp.getModel();
                    model.load(newId, {
                        success: function (rec) {
                            asp.setFormData(rec);
                        }
                    });
                    asp.getGrid().getStore().load();
                    asp.controller.unmask();
                })
                    .error(function (resp) {
                        B4.QuickMsg.msg('Копирование входящего документа', resp.message || 'При копировании входящего документа возникла ошибка', 'error');
                        asp.controller.unmask();
                    });
            },

            onChangeQuestionStatus: function (field, newValue) {
                var form = this.getForm(),
                    sfSSTUTransferOrg = form.down('#sfSSTUTransferOrg');
                
                if (newValue == B4.enums.QuestionStatus.Transferred) {
                    sfSSTUTransferOrg.setDisabled(false);
                    sfSSTUTransferOrg.allowBlank = false;
                }
                else {
                    sfSSTUTransferOrg.setValue(null);
                    sfSSTUTransferOrg.setDisabled(true);
                    sfSSTUTransferOrg.allowBlank = true;
                }


            },

            goToSOPR: function (btn) {
                var asp = this,
                    portal = asp.controller.getController('PortalController'),
                    params = {},
                    sopr = null,
                    rec = asp.getForm().getRecord();
                var result = B4.Ajax.request(B4.Url.action('GetSOPRId', 'AppealCitsExecutant', {
                    recordId: rec.getId()
                }
                )).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data.soprId != null) {
                        sopr = data.data.soprId;
                        if (sopr != null) {
                            var controllerEditName = 'B4.controller.AppealOrder';
                            params.soprId = sopr;
                            portal.loadController(controllerEditName, params);
                        }
                    }
                }).error(function () {

                });

            },

            onChangeTypeCorrespondent: function (field, newValue) {

                var form = this.getForm(),
                    cbContragentCorrespondent = form.down('#cbContragentCorrespondent');

                if (newValue == B4.enums.TypeCorrespondent.IndividualEntrepreneur 
                   || newValue == B4.enums.TypeCorrespondent.LegalEntity
                   || newValue == B4.enums.TypeCorrespondent.LocalAuthorities
                   || newValue == B4.enums.TypeCorrespondent.PublicOrganization
                   || newValue == B4.enums.TypeCorrespondent.PublicAuthorities) {
                    cbContragentCorrespondent.setDisabled(false);
                    cbContragentCorrespondent.setValue(null);
                }
                else {
                    cbContragentCorrespondent.setDisabled(true);
                }
            },

            onSuretyResolveChange: function (sf, val) {
                var form = this.getForm(),
                    contragentField = form.down('[name=ApprovalContragent]');

                contragentField.setDisabled(!val || val.Code != "4");
            },

            onAfterSetFormData: function (aspect, rec, form) {
                // Изза того что в Gkh перекрыт аспект в нем 2 раза делается метод show у окна что приводит к повторному открытию окна
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта Executant и Surety
            // передать только Id на сохранение, поскольку если на сохранение уйдет Executant или Surety целиком,
            //то это поле тоже сохраниться и поля для записи Executant и Surety будут потеряны
            getRecordBeforeSave: function (record) {

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
            onBeforeLoadPreviousAppeal: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            },
            onRedtapeFlagChange: function (field, newValue) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0];

                var previousAppealCitizensGjiSelectField = wnd.down('#previousAppealCitsSelectField');
                //TODO! Переделать
                previousAppealCitizensGjiSelectField.setDisabled(newValue > 2 ? false : true);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var id = record.getId(),
                        numberGji = record.get('NumberGji'),
                        statusNames = ['В работе', 'Не принято в работу'],
                        showExecutantInfo = !!(id && statusNames.indexOf(record.get('State').Name) >= 0),
                        form = asp.getForm(),
                        categoryGrid = form.down('appealcitscategorygrid'),
                        attachmentGrid = form.down('appealcitsattachmentgrid'),
                        questionGrid = form.down('appealcitsquestiongrid'),
                        executantGrid = form.down('appealcitsexecutantgrid'),
                        headInspectorGrid = form.down('appealcitsheadinspectorgrid'),
                        appealCitsAdmonitionGrid = form.down('appealCitsAdmonitionGrid');
                    appealCitsEmergencyHouseGrid = form.down('appealCitsEmergencyHouseGrid')

                    //Передаем аспекту смены статуса необходимые параметры
                    asp.controller.getAspect('appealCitsStateButtonAspect').setStateData(id, record.get('State'));

                    asp.controller.setCurrentId(id, numberGji);

                    categoryGrid.tab.setDisabled(record.phantom);
                    categoryGrid.getStore().filter('appealCitizensId', id);

                    attachmentGrid.tab.setDisabled(record.phantom);
                    attachmentGrid.getStore().filter('appealCitizensId', id);

                    questionGrid.tab.setDisabled(record.phantom);
                    questionGrid.getStore().filter('appealCitizensId', id);

                    headInspectorGrid.tab.setDisabled(record.phantom);
                    headInspectorGrid.getStore().filter('appealCitizensId', id);

                    executantGrid.tab.setDisabled(record.phantom);
                },
                beforesaverequest: function () {
                    var checkTime = this.getForm().down('[name=CheckTime]').getValue(),
                        extensTime = this.getForm().down('[name=ExtensTime]').getValue();

                    if (extensTime != null && checkTime > extensTime) {
                        Ext.Msg.alert('Ошибка сохранения!', "Дата продленного контрольного срока не может быть меньше Даты контрольного срока");
                        return false;
                    }
                    return true;
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteAppealCitsStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
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

                if (fieldPosition) {
                    fieldPosition.setValue(data && data.Position);
                }
            },

            onSuretyBeforeLoad: function (field, options) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;
            },

            onExecutantChange: function (field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#appealCitsExecutantPositionTextField');
                if (fieldPosition) {
                    fieldPosition.setValue(data && data.Position);
                }

            },
            onChangeRealityObject: function (field, newValue) {
                this.controller.params.realityObjectId = newValue && newValue.Id;
            },
            onChangeStatSubject: function (field, newValue) {
                if (newValue) {
                    this.controller.params.statSubjectId = newValue.Id;
                } else {
                    this.controller.params.statSubjectId = null;
                }
            },
            onChangeStatSubsubjectGji: function (field, newValue) {
                if (newValue) {
                    this.controller.params.statSubsubjectGjiId = newValue.Id;
                } else {
                    this.controller.params.statSubsubjectGjiId = null;
                }
            },
            onChangeExecutant: function (field) {
                this.controller.params.executantId = field.getValue();
            },
            onChangeAuthor: function (field, newValue) {
                this.controller.params.authorId = newValue && newValue.Id;
            },
            onChangeController: function (field) {
                this.controller.params.controllerId = field.getValue();
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('AppealCits');
                str.currentPage = 1;
                str.load();
            },
            onCreateStatement: function (btn) {
                var me = this;
                me.controller
                    .getAspect("baseStatementAppCitEditWindowAspect")
                    .checkAppealCits(me.controller.appealCitizensId)
                    .next(function () {
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
                    .error(function (resp) {
                        Ext.Msg.alert('Невозможно сформировать проверку!', resp.message);
                    });
            },
            onChangeDateFromStart: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromStart = newValue;
                } else {
                    this.controller.params.dateFromStart = null;
                }
            },
            onChangeDateFromEnd: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromEnd = newValue;
                } else {
                    this.controller.params.dateFromEnd = null;
                }
            },
            onChangeCheckTimeStart: function (field, newValue, oldValue) {

                if (newValue) {
                    this.controller.params.checkTimeStart = newValue;
                } else {
                    this.controller.params.checkTimeStart = null;
                }
            },
            onChangeCheckTimeEnd: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.checkTimeEnd = newValue;
                } else {
                    this.controller.params.checkTimeEnd = null;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('AppealCits').load();
            },
            clearAllFilters: function (bt) {
                var panel = bt.up('appealcitsFilterPanel');
                panel.down('#dfDateFromStart').setValue();
                panel.down('#dfDateFromEnd').setValue();
                panel.down('#dfCheckTimeStart').setValue();
                panel.down('#dfCheckTimeEnd').setValue();
                panel.down('#sfRealityObject').setValue();
                panel.down('#sfAuthor').setValue();
                panel.down('#sfExecutant').setValue();
                panel.down('#sfController').setValue();
                this.controller.getStore('AppealCits').load();
            },
            onExtensTimesCheckbox: function (field, newValue) {
                this.controller.params.showExtensTimes = newValue;
                this.controller.getStore('AppealCits').load();
            },
            onShowOnlyFromEais: function (field, newValue) {
                this.controller.params.showOnlyFromEais = newValue;
                this.controller.getStore('AppealCits').load();
            },
            onShowFavorites: function (field, newValue) {
                this.controller.params.showFavorites = newValue;
                this.controller.getStore('AppealCits').load();
            },
            onGetAttachmentArchiveClick: function (btn) {
                var asp = this,
                    rec = asp.getForm().getRecord();

                asp.controller.mask('Создание архива', asp.controller.getMainComponent());
                B4.Ajax.request({
                    //method: 'POST',
                    url: B4.Url.action('GetAttachmentArchive', 'AppealCits'),
                    params: {
                        appealCitsId: rec.getId(),
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    asp.controller.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }

                    var id = resp.data ? resp.data : tryDecoded.data;

                    if (id > 0) {
                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: B4.Url.action('Download', 'FileUpload', { Id: id })
                        });

                        //me.fireEvent('onprintsucess', me);
                    }
                }).error(function (err) {
                    asp.controller.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err.message || err);
                });
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ Категории заявителя обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitsCategory/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealcitscategoryMultiselectWindowAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealcitscategorySelectWindow',
            storeSelect: 'dict.ApplicantCategory',
            storeSelected: 'dict.ApplicantCategory',
            gridSelector: 'appealcitscategorygrid',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование'
                }
            ],
            titleSelectWindow: 'Выбор категории заявителя',
            titleGridSelect: 'Категории заявителя для выбора',
            titleGridSelected: 'Выбранные категории заявителя',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCitsCategory', {
                            recordIds: Ext.encode(recordIds),
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Категории заявителя сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать категории заявителя');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitizensAttachmentsWindowAspect',
            gridSelector: 'appealcitsattachmentgrid',
            editFormSelector: 'appealcitsattachmenteditwindow',
            modelName: 'appealcits.AppealCitsAttachment',
            editWindowView: 'appealcits.AppealCitsAttachmentEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (record.phantom) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ Категории заявителя обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitsQuestion/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealcitsQuestionMultiselectWindowAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealcitsquestionSelectWindow',
            storeSelect: 'dict.QuestionKind',
            storeSelected: 'dict.QuestionKind',
            gridSelector: 'appealcitsquestiongrid',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование вида вопроса',
                    filter : { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'QuestionType',
                    flex: 1,
                    header: 'Наименование типа вопроса',
                    filter : { xtype: 'textfield', filterName: 'QuestionType.Name' },
                    renderer: function(val) { return val && val.Name; }
                },
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование'
                }
            ],
            titleSelectWindow: 'Выбор видов вопросов обращения',
            titleGridSelect: 'Виды вопросов обращения для выбора',
            titleGridSelected: 'Выбранные виды вопросов обращения',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCitsQuestion', {
                            recordIds: Ext.encode(recordIds),
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Виды вопросов обращения сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды вопросов');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ руководителя обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitsHeadInspector/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealcitsHeadInspectorMultiselectWindowAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealcitsheadinspectorSelectWindow',
            storeSelect: 'dict.Inspector',
            storeSelected: 'dict.Inspector',
            gridSelector: 'appealcitsheadinspectorgrid',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    header: 'ФИО',
                    filter : { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    header: 'Должность',
                    filter : { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    header: 'ФИО'
                }
            ],
            titleSelectWindow: 'Выбор руководителя',
            titleGridSelect: 'Руководители для выбора',
            titleGridSelected: 'Выбранные руководители',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCitsHeadInspector', {
                            recordIds: Ext.encode(recordIds),
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Руководители сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать руководителей');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
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
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCits', {
                            objectIds: recordIds,
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            Ext.ComponentQuery.query('#gridRelatedAppealCits')[0].store.load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Связанные/аналогичные обращения успешно сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ Связанные/Аналогичные обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitizensGJI/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'relatedAppealCitizensMultiSelectWindowAspectForTab',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#relatedAppCitSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            gridSelector: '#gridRelatedAppealCits',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    header: '№ обращения'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    header: 'Дата регистрации'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Subjects',
                    flex: 1,
                    header: 'Тематика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubSubjects',
                    flex: 1,
                    header: 'Подтематика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Features',
                    flex: 1,
                    header: 'Характеристика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    flex: 1,
                    header: 'Заявитель'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executants',
                    flex: 1,
                    header: 'Проверяющий инспектор'
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    header: '№ обращения'
                }
            ],
            titleSelectWindow: 'Выбор обращений граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCits', {
                            objectIds: recordIds,
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            Ext.ComponentQuery.query('#gridRelatedAppealCits')[0].store.load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Связанные/аналогичные обращения успешно сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        var controller = this.controller;
                        controller.mask('Удаление', controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('RemoveRelated', 'AppealCits', {
                            id: record.get('Id'),
                            parentId: this.controller.appealCitizensId
                        })).next(function (response) {
                            controller.unmask();
                            Ext.ComponentQuery.query('#gridRelatedAppealCits')[0].store.load();
                            return true;
                        }).error(function (response) {
                            Ext.Msg.alert('Ошибка удаления!', response.message);
                            controller.unmask();
                        });
                    }
                }, me);
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.relatesToId = this.controller.appealCitizensId;
                operation.params.matchRelated = true;

                this.getSelectedGrid().getStore().load();
            },

            onSelectedBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.relatesToId = this.controller.appealCitizensId;
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

            onBeforeLoad: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.appealCitizensId = this.controller.appealCitizensId;
            },

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'AppealCitsRealObject', {
                            objectIds: recordIds,
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();

                            return true;
                        }).error(function () {
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
                getdata: function (asp, record) {
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
                getdata: function (me, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0]) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddStatementSubject', 'AppealCitsStatSubject', {
                            objectIds: recordIds,
                            appealCitizensId: me.controller.appealCitizensId
                        })).next(function (response) {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать тематики обращений');
                        return false;
                    }
                    return true;
                }
            },
            onCheckRec: function (node, checked) {
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
            name: 'appealCitsDecisionGridWindowAspect',
            gridSelector: '#appealCitsDecisionGrid',
            editFormSelector: '#appealCitsDecisionEditWindow',
            storeName: 'appealcits.Decision',
            modelName: 'appealcits.Decision',
            editWindowView: 'appealcits.DecisionEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
             
                aftersetformdata: function (asp, record) {
                    var me = this,
                        form = me.getForm();
                    asp.controller.appealCitsDecision = record.get('Id');
                    this.controller.getAspect('appealCitsDecisionPrintAspect').loadReportStore();
                    this.controller.getAspect('appealCitsDecisionEstablishedAspect').doInjection();
                    this.controller.getAspect('appealCitsDecisionDecidedAspect').doInjection();
              
                },
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsDefinitionGridWindowAspect',
            gridSelector: 'appealcitsDefinitionGrid',
            editFormSelector: '#appealcitsDefinitionEditWindow',
            storeName: 'appealcits.Definition',
            modelName: 'appealcits.Definition',
            editWindowView: 'appealcits.DefinitionEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },

                aftersetformdata: function (asp, record) {
                    var me = this,
                        form = me.getForm();
                    asp.controller.appealCitsDecision = record.get('Id');
                    this.controller.getAspect('appealCitsDefinitionEstablishedAspect').doInjection();
                    this.controller.getAspect('appealCitsDefinitionDecidedAspect').doInjection();
                  //  this.controller.getAspect('appealCitsDecisionPrintAspect').loadReportStore();

                },
            },
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'appealCitsDecisionEstablishedAspect',
            fieldSelector: '[name=Established]',
            editPanelAspectName: 'appealCitsDecisionGridWindowAspect',
            controllerName: 'AppealCitsDecision',
            valueFieldName: 'Established',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'appealCitsDecisionDecidedAspect',
            fieldSelector: '[name=Decided]',
            editPanelAspectName: 'appealCitsDecisionGridWindowAspect',
            controllerName: 'AppealCitsDecision',
            valueFieldName: 'Decided',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'appealCitsDefinitionEstablishedAspect',
            fieldSelector: '[name=Established]',
            editPanelAspectName: 'appealCitsDefinitionGridWindowAspect',
            controllerName: 'AppealCits',
            valueFieldName: 'Established',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'appealCitsDefinitionDecidedAspect',
            fieldSelector: '[name=Decided]',
            editPanelAspectName: 'appealCitsDefinitionGridWindowAspect',
            controllerName: 'AppealCits',
            valueFieldName: 'Decided',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },

        //{
        //    xtype: 'gkhblobtextaspect',
        //    name: 'appealCitsDecisionEstablishedAspect',
        //    fieldSelector: '[name=Established]',
        //    editPanelAspectName: 'appealCitsDecisionGridWindowAspect',
        //    controllerName: 'AppealCitsDecision',
        //    valueFieldName: 'Established',
        //    previewLength: 2000,
        //    autoSavePreview: true,
        //    previewField: false
        //},
        //{
        //    xtype: 'gkhblobtextaspect',
        //    name: 'appealCitsDecisionDecidedAspect',
        //    fieldSelector: '[name=Decided]',
        //    editPanelAspectName: 'appealCitsDecisionGridWindowAspect',
        //    controllerName: 'AppealCitsDecision',
        //    valueFieldName: 'Decided',
        //    previewLength: 2000,
        //    autoSavePreview: true,
        //    previewField: false
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsAnswerGridWindowAspect',
            gridSelector: '#appealCitsAnswerGrid',
            editFormSelector: '#appealCitsAnswerEditWindow',
            storeName: 'appealcits.Answer',
            modelName: 'appealcits.Answer',
            editWindowView: 'appealcits.AnswerEditWindow',
            otherActions: function (actions) {

                actions['#appealCitsAnswerEditWindow #sendEmailButton'] = { 'click': { fn: this.sendEmail, scope: this } };
                actions['#appealCitsAnswerEditWindow #dfAnswerContent'] = { 'change': { fn: this.onChangeAnswerContent, scope: this } };
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
                beforesetformdata: function (asp, record) {
                    var me = this,
                    form = me.getForm(),
                    dfDocumentNumber = form.down('[name=DocumentNumber]'),
                    taDescription = form.down('[name=Description]'),
                    sfConcederationResult = form.down('[name=ConcederationResult]'),
                    state = record.get('State'),
                    recordStateIsFinal = state && state.FinalState;

                    dfDocumentNumber.allowBlank = !recordStateIsFinal;
                    taDescription.allowBlank = !recordStateIsFinal;
                    sfConcederationResult.allowBlank = !recordStateIsFinal;

                    if (!Gkh.config.HousingInspection.GeneralConfig.ExecutorIsCurrentOperator || !record.phantom) {
                        return;
                    }

                    B4.Ajax.request({
                        url: B4.Url.action('GetActiveOperatorId', 'Operator')
                    }).next(function (response) {
                        var resp = Ext.decode(response.responseText),
                            operatorId = resp && resp.data && resp.data.Id;

                        if (operatorId) {
                            B4.model.administration.Operator.load(operatorId, {
                                success: function (rec) {
                                    asp.getForm().down('[name=Executor]').setValue(rec.get('Inspector'));
                                }
                            });
                        }
                    });
                },
                aftersetformdata: function (asp, record) {
                    var me = this,
                    form = me.getForm(),
                    answerGrid = form.down('appealcitsanswerattachmentgrid'),
                    subjectsTab = form.down('appealcitsanswerstatsubjectgrid');
                    appealCitsAnswer = record.getId();
                    answerGrid.tab.setDisabled(record.phantom);
                    answerGrid.getStore().clearFilter(true);
                    answerGrid.getStore().filter('answerId', record.get('Id'));

                    subjectsTab.tab.setDisabled(record.phantom);
                    subjectsTab.getStore().clearFilter(true);
                    subjectsTab.getStore().filter('answerId', record.get('Id'));

                    if (!record.phantom) {
                        me.controller.answerId = record.getId();
                    }                  

                   // me.controller.getAspect('appealCitsAnswerEditWindowAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('descriptionBlobTextAspect').doInjection();
                    this.controller.getAspect('description2BlobTextAspect').doInjection();
                }
            },
            onChangeAnswerContent: function (selectionModel, selectedRecord, df, sdf) {
                var form = this.getForm();
                var dfRedirectContragent = form.down('#dfRedirectContragent');
                if (selectedRecord == null) {
                    dfRedirectContragent.hide();
                }
                else {
                    if (selectedRecord.Name == "Перенаправлено") {
                        dfRedirectContragent.show();
                    }
                }
            },
            sendEmail: function (record) {
                var me = this;
                var taskId = appealCitsAnswer;
               
                if (appealCitsAnswer == 0 || appealCitsAnswer == null) {
                    Ext.Msg.alert('Внимание!', 'Перед отправкой почты необходимо сохранить запись');
                }
                else {
                    me.mask('Отправка Email', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('SendEmail', 'SendEmail', {
                        taskId: taskId
                    }
                    )).next(function (response) {

                        me.unmask();
                        Ext.Msg.alert('Внимание!', 'Письмо отправлено');
                        var data = Ext.decode(response.responseText);
                        return true;
                    }).error(function (response) {
                        var data = response.exceptionText;
                        me.unmask();
                        Ext.Msg.alert('Внимание!', data);
                    });

                }
            }

        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitizensAnswerAttachmentsWindowAspect',
            gridSelector: 'appealcitsanswerattachmentgrid',
            editFormSelector: 'appealcitsanswerattachmenteditwindow',
            modelName: 'appealcits.AppealCitsAnswerAttachment',
            editWindowView: 'appealcits.AppealCitsAnswerAttachmentEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    record.set('AppealCitsAnswer', asp.controller.answerId);
                }
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ тематик ответов с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitsAnswerStatSubject/AddAnswer
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealCitizensAnswerStatementSubjectAspect',
            gridSelector: 'appealcitsanswerstatsubjectgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealCitsAnswerStatSubjectMultiSelectWindow',
            storeSelect: 'appealcits.StatSubject',
            storeSelected: 'dict.Subj',
            titleSelectWindow: 'Выбор тематик обращения',
            titleGridSelected: 'Выбранные тематики обращений',
            titleGridSelect: 'Виды тематик для выбора',
            columnsGridSelect: [
                { header: 'Тематика', xtype: 'gridcolumn', dataIndex: 'Subject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Подтематика', xtype: 'gridcolumn', dataIndex: 'Subsubject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Характеристика', xtype: 'gridcolumn', dataIndex: 'Feature', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Тематика', xtype: 'gridcolumn', dataIndex: 'Subject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Подтематика', xtype: 'gridcolumn', dataIndex: 'Subsubject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Характеристика', xtype: 'gridcolumn', dataIndex: 'Feature', flex: 1, filter: { xtype: 'textfield' } }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAnswer', 'AppealCitsAnswerStatSubject', {
                            recordIds: Ext.encode(recordIds),
                            answerId: asp.controller.answerId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Тематики ответов сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды вопросов');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.appealCitizensId = this.controller.appealCitizensId;
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
                getdata: function (asp, record) {
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
            xtype: 'gkhgrideditformaspect',
            name: 'baseStatementAppCitEditWindowAspect',
            gridSelector: '#baseStatementAppCitsGrid',
            storeName: 'appealcits.BaseStatement',
            modelName: 'appealcits.BaseStatement',
            editFormSelector: '#baseStatementAppCitsAddWindow',
            editWindowView: 'appealcits.BaseStatementAddWindow',
            controllerEditName: 'B4.controller.basestatement.Navigation',
            otherActions: function (actions) {
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
                            function (item) {
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
                        }).next(function (result) {
                            me.unmask();
                            me.updateGrid();
                            var res = Ext.decode(result.responseText);
                            var baseStatementId = res.data.Id;

                            var model = me.controller.getModel('BaseStatement');
                            model.load(baseStatementId, {
                                success: function (recBaseStatement) {
                                    me.fireEvent('savesuccess', me, recBaseStatement);
                                },
                                scope: me
                            });
                            return true;
                        }).error(function (result) {
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
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
                operation.params.roId = this.getForm().down('#sfRealityObject').getValue();
            },
            onBeforeLoadRealityObject: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                var realityObjIds = [];
                this.controller.getStore('appealcits.RealityObject').each(function (obj) {
                    realityObjIds.push(obj.get('RealityObjectId'));
                });
                operation.params.realityObjIds = realityObjIds.length == 0 ? -1 : realityObjIds;
            },
            onChangeType: function (field, newValue) {
                this.controller.params = this.controller.params || {};
                this.controller.params.typeJurOrg = newValue;
                this.getForm().down('#sfContragent').setValue(null);
                if (newValue == 10) {
                    this.controller.setManOrg();
                }
                this.getForm().down('#tfPhysicalPerson').setValue(null);
            },
            onChangePerson: function (field, newValue) {
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
                getdata: function (asp, records) {

                    //Id обращений
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Добавление', asp.controller.getMainComponent());
                        var store = asp.controller.getStore(asp.storeName);

                        var defRec = store.getAt(store.find('Id', asp.controller.appealCitizensId, 0, false, false, true));

                        store.removeAll();

                        store.add(defRec);

                        Ext.Array.each(records.items,
                            function (rec) {
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
            deleteRecord: function (record) {
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
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1 }
            ],
            otherActions: function (actions) {
               // actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions['#appealCitsExecutantEditWindow #sflAuthor'] = { 'change': { fn: this.onChangeAutor, scope: this } };
                actions['#appealCitsExecutantEditWindow #sflExecutant'] = { 'change': { fn: this.onChangeExecutant, scope: this } ,  'beforeload': { fn: this.onBeforeLoadAuthor, scope: this } };
                actions['#appealCitsExecutantEditWindow #sflController'] = { 'beforeload': { fn: this.onBeforeLoadExecutant, scope: this } };
            },
            onBeforeLoadExecutant: function (store, operation) {
                var me = this;
                operation.params.currentPerson = me.controller.Executant;
            },
            onBeforeLoadAuthor: function (store, operation) {
                var me = this;
                operation.params.currentPerson = me.controller.author;
            },
            onChangeAutor: function (field, newValue) {
                var me = this;
                if (newValue)
                    me.controller.author = newValue.Id;
            },
            onChangeExecutant: function (field, newValue) {
                var me = this;
                if (newValue)
                    me.controller.Executant = newValue.Id;
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.excludeInpectorId = this.controller.inpectorId;

                B4.Ajax.request(B4.Url.action('GetParamByKey', 'GjiParams', {
                    key: 'AutoSetSurety'
                }))
                    .next(function (resp) {
                        var win = me.getForm(),
                            fields = win.query('[name=Author]'),
                            field = fields ? fields[0] : null,
                            data = Ext.decode(resp.responseText);

                        field.setDisabled(data.data.toLowerCase() === 'true');
                    });
            },
            listeners: {
                beforesave: function (aspect, rec) { //Перекрываем для поддержки загрузки файла
                    var win = Ext.ComponentQuery.query('#appealCitsExecutantEditWindow')[0];
                    var frm = win.getForm();
                    win.mask('Сохранение', frm);
                    frm.submit({
                        url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                        params: {
                            records: Ext.encode([rec.getData()])
                        },
                        success: function (form, action) {
                            win.unmask();
                            aspect.updateGrid();

                            win.close();
                        },
                        failure: function (form, action) {
                            win.unmask();
                            win.fireEvent('savefailure', rec, action.result.message);
                            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                        }
                    });

                    return false;
                },
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var dateField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfPerformanceDate')[0];
                    if (!dateField.allowBlank && !dateField.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать срок исполнения');
                        return false;
                    }

                    var authorField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #sflAuthor')[0];
                    if (!authorField.isDisabled() && (!authorField.value || authorField.value.Id <= 0)) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать поручителя');
                        return false;
                    }
                    //var resolutionField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #ffResolution')[0];
                    //if (!resolutionField.isDisabled() && (!resolutionField.value || resolutionField.value.Id <= 0)) {
                    //    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать резолюцию');
                    //    return false;
                    //}
                    //var win = resolutionField.up(asp.multiSelectWindowSelector);
                    //form = win.down('form');
                    //var files = resolutionField.fileInputEl.dom.files;
                    //var me = this;
                    //
                   // var form = Ext.ComponentQuery.query(asp.multiSelectWindowSelector)[0];
                    //var file = files[0];
                    //var formData = new FormData();
                    //formData.append(file.name, file.data);
                    //form.files = form.files || [];
                    //for (var i = 0; i < files.length; i++) {    
                    //        form.files.push({
                    //            name: files[i].name,
                    //            extension: files[i].name.split('.')[1],
                    //            data: files[i]
                    //        });
                    //}
                    //var params = {
                    //    File: win.down('b4filefield[name=Resolution]').getValue(),                      
                    //};
                    //params.File = resolutionField.getValue();
                    //me.params = me.params || {};
                    //me.params.inspectorIds = Ext.encode(recordIds);
                    //me.params.appealCitizensId = asp.controller.appealCitizensId;
                    //me.params.performanceDate = dateField.value;
                    //me.params.authorId = authorField.value ? authorField.value.Id : 0;

                    if (recordIds[0] <= 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                        return false;
                    }
                    //if (form.getForm().isValid()) {
                    //    
                    //    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    //    form.submit({
                    //        url: B4.Url.action('AddExecutants', 'AppealCitsExecutant'),
                    //        params: params,
                    //        timeout: 9999999,
                    //        FormData: formData,
                    //        success: function () {
                    //            asp.controller.unmask();
                    //            Ext.Msg.alert('Сохранено!', 'Исполнители сохранены успешно');
                    //        },
                    //        failure: function (form, action) {
                    //            asp.controller.unmask();
                    //            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    //        }
                    //    });
                    //}

                    //var connection = Ext.create('Ext.data.Connection');
                    //connection.request({
                    //    url: B4.Url.action('/AppealCitsExecutant/AddExecutants'),
                    //    method: 'POST',
                    //    timeout: 120000,
                    //    params: me.params,
                    //    success: function (response) {
                    //        asp.controller.unmask();
                    //        Ext.Msg.alert('Сохранено!', 'Исполнители сохранены успешно');

                    //    },
                    //    failure: function (response) {
                    //        asp.controller.unmask();
                    //        Ext.Msg.alert('Ошибка', response.message ? response.message : 'Произошла ошибка');
                    //    }
                    //});


                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddExecutants', 'AppealCitsExecutant', {
                        inspectorIds: Ext.encode(recordIds),
                        appealCitizensId: asp.controller.appealCitizensId,
                        performanceDate: dateField.value,
                        authorId: authorField.value ? authorField.value.Id : 0
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Исполнители сохранены успешно');
                        return true;
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    this.controller.getAspect('appcitsExecutantStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                },
                panelrendered: function (asp, prm) {
                    var me = this,
                        autoPerformanceDate = Gkh.config.HousingInspection.SettingTheVerification.AutoPerformanceDate;

                    if (autoPerformanceDate) {
                        var performanceDateValue = me.controller.getStore('AppealCits').getById(me.controller.appealCitizensId).get('CheckTime'),
                            performanceDateEl = prm.window.down('#dfPerformanceDate');

                        performanceDateEl.setValue(performanceDateValue);
                        performanceDateEl.setDisabled(true);
                    }
                },
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

                    if (!dateField.allowBlank && !dateField.value) {
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
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'appealCitsAnswerEditWindowAspect',
            stateButtonSelector: '#appealCitsAnswerEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var model = this.controller.getModel('appealcits.Answer');
                    var store = this.controller.getStore('appealcits.Answer');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('appealCitsAnswerStatePerm').setPermissionsByRecord(rec);
                            this.controller.getAspect('appealCitsAnswerGridWindowAspect').setFormData(rec);
                            store.load();
                        },
                        scope: this
                    }) : this.controller.getAspect('appealCitsAnswerStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'appealCitsAnswerGridAspect',
            gridSelector: '#appealCitsAnswerGrid',
            stateType: 'gji_appeal_cits_answer',
            menuSelector: 'appealCitsAnswerGridStateMenu'
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'descriptionBlobTextAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'appealCitsAnswerGridWindowAspect',
            controllerName: 'AppealCits',
            getAction: 'GetAnswerDescription',
            saveAction: 'SaveAnswerDescription',
            valueFieldName: 'Description',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'description2BlobTextAspect',
            fieldSelector: '[name=Description2]',
            editPanelAspectName: 'appealCitsAnswerGridWindowAspect',
            controllerName: 'AppealCits',
            getAction: 'GetAnswerDescription',
            saveAction: 'SaveAnswerDescription',
            valueFieldName: 'Description2',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false
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
             name: 'appealCitsAdmonitionGridWindowAspect',
             gridSelector: '#appealCitsAdmonitionGrid',
             editFormSelector: '#appealCitsAdmonitionEditWindow',
             storeName: 'appealcits.AppealCitsAdmonition',
             modelName: 'appealcits.AppealCitsAdmonition',
             editWindowView: 'appealcits.AppealCitsAdmonitionEditWindow',
             otherActions: function (actions) {
                 actions['#appealCitsAdmonitionEditWindow #dfPayerType'] = { 'change': { fn: this.onChangePayerType, scope: this } };
             },
             onSaveSuccess: function () {
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },
             onChangePayerType: function (field, newValue) {
                 var form = this.getForm(),
                     //dfIdentifierType = form.down('#dfIdentifierType'),
                     //dfFLDocType = form.down('#dfFLDocType'),
                     dfINN = form.down('#dfINN'),
                     dfINN2 = form.down('#dfINN2'),
                     //dfDocumentSerial = form.down('#dfDocumentSerial'),
                     //dfKPP = form.down('#dfKPP'),
                     //dfDocumentNumber = form.down('#dfDocumentNumber'),
                     //dfKIO = form.down('#dfKIO')
                     fsUrParams = form.down('#fsUrParams'),
                     fsFizParams = form.down('#fsFizParams'),
                     fsIpParams = form.down('#fsIpParams'),
                     contragent = form.down('#contragent');

                 if (newValue == B4.enums.PayerType.Physical) {
                     fsFizParams.show();
                     fsFizParams.setDisabled(false);
                     fsUrParams.hide();
                     fsUrParams.setDisabled(true);
                     fsIpParams.hide();
                     fsIpParams.setDisabled(true);
                     contragent.hide();
                     contragent.setDisabled(true);
                 }
                 else if (newValue == B4.enums.PayerType.Juridical) {
                     fsFizParams.hide();
                     fsFizParams.setDisabled(true);
                     fsUrParams.show();
                     fsUrParams.setDisabled(false);
                     fsIpParams.hide();
                     fsIpParams.setDisabled(true);
                     contragent.show();
                     contragent.setDisabled(false);
                 }
                 else {
                     fsFizParams.hide();
                     fsFizParams.setDisabled(true);
                     fsUrParams.hide();
                     fsUrParams.setDisabled(true);
                     fsIpParams.show();
                     fsIpParams.setDisabled(false);
                     contragent.show();
                     contragent.setDisabled(false);

                     dfINN2.setValue(dfINN.getValue());
                 }
             },
             listeners: {
                 getdata: function (asp, record) {
                     if (!record.get('Id')) {
                         record.set('AppealCits', this.controller.appealCitizensId);
                     }
                 },
                 aftersetformdata: function (asp, record, form) {
                     this.controller.getAspect('appealCitsAdmonitionPrintAspect').loadReportStore();
                     var grid = form.down('appCitAdmonVoilationGrid'),
                         store = grid.getStore();
                     var grid_annex = form.down('appCitAdmonAnnexGrid'),
                         store_annex = grid_annex.getStore();
                     store.filter('AppealCitsAdmonition', record.getId());
                     store_annex.filter('AppealCitsAdmonition', record.getId());
                 }
                 //aftersetformdata: function (asp, record, form) {
                 //    appealCitsAdmonition = record.getId();
                 //}
             }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsEmergencyHouseGridWindowAspect',
            gridSelector: '#appealCitsEmergencyHouseGrid',
            editFormSelector: '#appealCitsEmergencyHouseEditWindow',
            storeName: 'appealcits.AppealCitsEmergencyHouse',
            modelName: 'appealcits.AppealCitsEmergencyHouse',
            editWindowView: 'appealcits.AppealCitsEmergencyHouseEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                }
            }
        },
          {
              xtype: 'gkhbuttonprintaspect',
              name: 'appealCitsAdmonitionPrintAspect',
              buttonSelector: '#appealCitsAdmonitionEditWindow #btnPrint',
              codeForm: 'AppealCitsAdmonition',
              getUserParams: function () {
                  var param = { Id: appealCitsAdmonition };
                  this.params.userParams = Ext.JSON.encode(param);
              }
          },
        {
            xtype: 'grideditwindowaspect',
            name: 'appCitAdmonVoilationGridWindowAspect',
            gridSelector: '#appCitAdmonVoilationGrid',
            editFormSelector: '#appCitAdmonVoilationEditWindow',
            storeName: 'appealcits.AppCitAdmonVoilation',
            modelName: 'appealcits.AppCitAdmonVoilation',
            editWindowView: 'appealcits.AppCitAdmonVoilationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsAdmonition', appealCitsAdmonition);
                    }
                },
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //    var grid = form.down('appCitAdmonVoilationGrid'),
                //    store = grid.getStore();
                //    store.filter('appealCitsAdmonitionId', appealCitsAdmonition);
                //}
            }
        },
             
        {
            xtype: 'grideditwindowaspect',
            name: 'appCitAdmonAnnexGridWindowAspect',
            gridSelector: '#appCitAdmonAnnexGrid',
            editFormSelector: '#appCitAdmonAnnexEditWindow',
            storeName: 'appealcits.AppCitAdmonAnnex',
            modelName: 'appealcits.AppCitAdmonAnnex',
            editWindowView: 'appealcits.AppCitAdmonAnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsAdmonition', appealCitsAdmonition);
                    }
                },
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //    var grid = form.down('appCitAdmonAnnexGrid'),
                //    store = grid.getStore();
                //    store.filter('appealCitsAdmonitionId', appealCitsAdmonition);
                //}
            }
        },
    ],

    setCurrentId: function (id, numberGji) {
        this.appealCitizensId = id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            fieldRelatedAppealCitizens = editWindow.down('#trigfRelatedAppealCitizens'),
            tabpanel = editWindow.down('.tabpanel'),
            btnCreateStatement = editWindow.down('#btnCreateStatement'),
            storeRo = this.getStore('appealcits.RealityObject'),
            storeStatement = this.getStore('appealcits.StatSubject'),
            sourceStore = this.getStore('appealcits.Source'),
            storeAnswer = this.getStore('appealcits.Answer'),
            storeDecision = this.getStore('appealcits.Decision'),
            storeDefinition = this.getStore('appealcits.Definition'),
            storeRequest = this.getStore('appealcits.Request'),
            storeBaseStatement = this.getStore('appealcits.BaseStatement'),
            storeAppCitsBaseStatement = this.getStore('appealcits.AppealCitsBaseStatement'),
            storeAppealCitsExecutant = this.getStore('appealcits.AppealCitsExecutant'),
            relatedAppealCits = editWindow.down('#gridRelatedAppealCits');
        var storeAppealCitsAdmonition = this.getStore('appealcits.AppealCitsAdmonition');
        var storeAppealCitsEmergencyHouse = this.getStore('appealcits.AppealCitsEmergencyHouse');
        btnCreateStatement.setDisabled(!id);
        relatedAppealCits.setRelatesToId(id);

        sourceStore.removeAll();
        storeRo.removeAll();
        storeStatement.removeAll();
        storeAnswer.removeAll();
        storeDecision.removeAll();
        storeDefinition.removeAll();
        storeRequest.removeAll();
        storeBaseStatement.removeAll();
        storeAppCitsBaseStatement.removeAll();
        storeAppealCitsExecutant.removeAll();
        storeAppealCitsAdmonition.removeAll();
        storeAppealCitsEmergencyHouse.removeAll();

        tabpanel.down('#tabLocationProblem').tab.setDisabled(!id);
        tabpanel.down('#tabSources').tab.setDisabled(!id);
        tabpanel.down('#tabApproval').tab.setDisabled(!id);
        tabpanel.down('#tabStatementSubject').tab.setDisabled(!id);
        tabpanel.down('#appealCitsAnswerGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsDecisionGrid').tab.setDisabled(!id);
        tabpanel.down('appealcitsDefinitionGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsRequestGrid').tab.setDisabled(!id);
        tabpanel.down('#baseStatementAppCitsGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsAdmonitionGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsEmergencyHouseGrid').tab.setDisabled(!id);
        tabpanel.setActiveTab(0);

        editWindow.down('#trigfRelatedAppealCitizens').setDisabled(!id);

        if (id > 0) {
            storeRo.load();
            storeStatement.load();
            sourceStore.load();
            storeAnswer.load();
            storeDecision.load();
            storeDefinition.load();
            storeRequest.load();
            storeBaseStatement.load();
            storeAppealCitsExecutant.load();
            storeAppealCitsAdmonition.load();
            storeAppealCitsEmergencyHouse.load();

            storeAppCitsBaseStatement.add({
                Id: this.appealCitizensId,
                NumberGji: numberGji
            });

            this.mask('Загрузка', this.getMainView());
            B4.Ajax.request(B4.Url.action('GetInfo', 'AppealCits', {
                appealCitizensId: this.appealCitizensId
            })).next(function (response) {
                this.unmask();
                var obj = Ext.JSON.decode(response.responseText);

                fieldRelatedAppealCitizens.updateDisplayedText(obj.relatedAppealNames);
                fieldRelatedAppealCitizens.setValue(obj.relatedAppealIds);
            }, this)
                .error(function () {
                    this.unmask();
                }, this);
        }
    },

    init: function () {
        this.getStore('AppealCits').on('beforeload', this.onBeforeLoadAppealCits, this);
        this.getStore('appealcits.RealityObject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.StatSubject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Source').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Answer').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Decision').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Definition').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Request').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.BaseStatement').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsExecutant').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsAdmonition').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsEmergencyHouse').on('beforeload', this.onBeforeLoad, this);

        var actions = {};
        this.control({

            'appealcitsexecutantgrid actioncolumn[action="previewfille"]': { click: { fn: this.onPreviewClick, scope: this } },
            'previewFileWindow button[name="Save"]': { click: { fn: this.downloadFile, scope: this } }
        });
        this.callParent(arguments);
    },

    onPreviewClick: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        var file = rec.get('Resolution');
        me.fileId = file.Id;
        if (me.fileId != null) {
            win = Ext.widget('previewFileWindow', {
                renderTo: B4.getBody().getActiveTab().getEl(),
                fileId: me.fileId,
            });
            win.show();
            var save = win.down('Save');
        }
    },
    downloadFile: function (params) {
        var me = this;
        var id = me.fileId;
        window.open(B4.Url.action('/FileUpload/Download?id=' + id));
    },
    deleteAllRelated: function (btn) {
        var me = this;
        me.mask('Загрузка', me.getMainView());
        B4.Ajax.request(B4.Url.action('RemoveAllRelated', 'AppealCits', {
            parentId: me.appealCitizensId
        })).next(function (response) {
            me.unmask();
            var grid = btn.up('relatedAppealCitsGrid'),
                store = grid.getStore();
            store.load();
        }, this)
            .error(function () {
                this.unmask();
            }, this);

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

    onBeforeLoad: function (store, operation) {
        operation.params.appealCitizensId = this.appealCitizensId;
    },

    onBeforeLoadAppealCits: function (store, operation) {
        if (this.params) {
            Ext.apply(operation.params, this.params);
        }
    },

    setManOrg: function () {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetJurOrgs', 'AppealCitsRealObject'),
            params: {
                appealCitizensId: this.appealCitizensId
            }
        }).next(function (resp) {

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