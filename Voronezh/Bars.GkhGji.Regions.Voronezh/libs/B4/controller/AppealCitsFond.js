//ToDo данный контроллер нельзя переводить на роуты поскольку у него форма редактирвоания открывается в модальном окне, и нельзя без реестра вызывать отдельно открытие карточки редактирвоания обращения
//ToDo необходимо данный контроллер переделать на отдельно открывающуюся панель а не модальное окно //Voronezh

Ext.define('B4.controller.AppealCitsFond', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonPrintAspect',
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
        'B4.store.AppealCits',
        'B4.aspects.FieldRequirementAspect',
        'B4.model.administration.Operator'
    ],

    appealCitizensId: null,
    appealCitsAnswer: null,
    appealCitsOrder: null,
    appcitRequestId: null,
    appealCitsAdmonition: null,
    appealCitsPrescriptionFond: null,
    appealCitsAnswerId: null,
    bcId: null,

    models: [
        'BaseStatement',
        'AppealCits',
        'appealcits.Admonition',
        'appealcits.PrescriptionFond',
        'dict.Subj',
        'appealcits.Request',
        'appealcits.RequestAnswer',
        'dict.AppealExecutionType',
        'appealcits.AppealCitsExecutant',
        'appealcits.AppealCitsExecutionType',
        'appealcits.AppealCitsAdmonition',
        'appealcits.AppealCitsPrescriptionFond',
        'appealcits.AppealCitsResolution',
        'appealcits.AppealCitsResolutionExecutor',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitPrFondVoilation',
        'appealcits.AppCitPrFondObjectCr',
        'PreliminaryCheck'
    ],

    stores: [
        'AppealCits',
        'appealcits.Answer',
        'appealcits.Source',
        'appealcits.StatSubject',
        'appealcits.RealityObject',
        'appealcits.Admonition',
        'appealcits.PrescriptionFond',
        'appealcits.Request',
        'dict.AppealExecutionType',
        'appealcits.AppealCitsExecutionType',
        'appealcits.BaseStatement',
        'appealcits.RequestAnswer',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'dict.statsubjectgji.Select',
        'dict.statsubjectgji.Selected',
        'dict.StatSubjectTreeSelect',
        'dict.Subj',
        'PreliminaryCheck',
        'dict.Inspector',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'appealcits.AppealCitsBaseStatement',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'appealcits.AppealCitsAdmonition',
        'appealcits.AppealCitsPrescriptionFond',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitPrFondVoilation',
        'appealcits.AppCitPrFondObjectCr',
        'appealcits.AppealCitsExecutant',
        'appealcits.AppealCitsResolution',
        'appealcits.AppealCitsResolutionExecutor'
    ],

    views: [
        'appealcits.GridFond',
        'appealcits.EditWindowFond',
        'appealcits.RealityObjectGrid',
        'appealcits.StatSubjectGrid',
        'appealcits.SourceGrid',
        'appealcits.SourceEditWindow',
        'appealcits.AppealCitsExecutionTypeGrid',
        'appealcits.AnswerGrid',
        'appealcits.AnswerEditWindow',
        'appealcits.RequestGrid',
        'appealcits.RequestEditWindow',
        'appealcits.RequestAnswerGrid',
        'appealcits.RequestAnswerEditWindow',
        'appealcits.BaseStatementGrid',
        'appealcits.PanelFond',
        'appealcits.FilterPanelFond',
        'appealcits.BaseStatementAddWindow',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'appealcits.AppealCitsExecutantGrid',
        'appealcits.ExecutantEditWindow',
        'appealcits.MultiSelectWindowExecutant',
        'B4.view.appealcits.DetailsEditWindow',
        'appealcits.RelatedAppealCitsGrid',
        'appealcits.AppealCitsAdmonitionGrid',
        'appealcits.AppealCitsAdmonitionEditWindow',
        'appealcits.AppCitAdmonVoilationGrid',
        'appealcits.AppCitAdmonVoilationEditWindow',
        'appealcits.AppealCitsPrescriptionFondGrid',
        'appealcits.AppealCitsPrescriptionFondEditWindow',
        'appealcits.AppCitPrFondVoilationGrid',
        'appealcits.AppCitPrFondVoilationEditWindow',
        'appealcits.AppCitPrFondObjectCrGrid',
        'appealcits.AppCitPrFondObjectCrEditWindow',
        'B4.view.appealcits.AnotherRelatedAppealCitsGrid',
        'appealcits.PreliminaryCheckGrid',
        'appealcits.PreliminaryCheckEditWindow',
        'appealcits.AppealCitsAttachmentEditWindow',
        'appealcits.AppealCitsAttachmentGrid',
        'appealcits.AppealCitsResolutionGrid',
        'appealcits.AppealCitsResolutionEditWindow',
        'appealcits.AppealCitsResolutionExecutorGrid',
        'appealcits.AppealCitsResolutionExecutorEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        //ToDo Пока невозможно перевести реестр обращения на роуты
        /* Закоментировал в связи с невозможностью перевода на роутинг
        ,
        context: 'B4.mixins.Context'*/
    },

    mainView: 'appealcits.PanelFond',
    mainViewSelector: 'appealCitsPanelFond',

    editWindowSelector: '#appealCitsEditWindowFond',
    baseStatementRealityObjectSelector: '#baseStatementAppCitsAddWindow #sfRealityObject',
    baseStatementContragentSelector: '#baseStatementAppCitsAddWindow #sfContragent',

    refs: [
        {
            ref: 'relAppealCitsGrid',
            selector: 'relatedAppealCitsGrid'
        },
        {
            ref: 'mainView',
            selector: 'appealCitsPanelFond'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#appealCitsRequestGrid',
            controllerName: 'AppealCitsRequest',
            name: 'appealCitsRequestSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#appealCitsAdmonitionGrid',
            controllerName: 'AppealCitsAdmonitionSign',
            name: 'appealCitsAdmonitionSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsLiteAdmonitionPrintAspect',
            buttonSelector: '#appealCitsAdmonitionEditWindow #btnPrint',
            codeForm: 'AppealCitsAdmonition',
            getUserParams: function () {
                var param = { Id: appealCitsAdmonition };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsLitePrescriptionFondPrintAspect',
            buttonSelector: '#appealCitsPrescriptionFondEditWindow #btnPrint',
            codeForm: 'AppealCitsPrescriptionFond',
            getUserParams: function () {
                var param = { Id: appealCitsPrescriptionFond };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsFKRPrintAspect',
            buttonSelector: '#appealCitsEditWindowFond #btnPrint',
            codeForm: 'AppealResolutionReport',
            getUserParams: function () {
                var param = { Id: appealCitizensId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'appealCitsAnswerPrintAspect',
            buttonSelector: '#appealCitsAnswerEditWindow #btnPrint',
            codeForm: 'AppealAnswer1',
            getUserParams: function () {
                var param = { Id: this.controller.appealCitsAnswerId };
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
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.AppealCits.Field.Department_Rqrd', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindowFond' }
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
                { name: 'GkhGji.AppealCitizens.Create', applyTo: 'b4addbutton', selector: '#appealCitsGridFond' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Create', applyTo: 'b4addbutton', selector: '#appealCitsExecutantGrid' },
                {
                    name: 'GkhGji.AppealCitizensState.Field.Consideration.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: '#appealCitsExecutantGrid',
                    applyBy: function (component, allowed) {
                        var me = this;
                        me.controller.params = me.controller.params || {};
                        if (allowed) {
                            component.show();
                        }
                        else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhGji.AppealCitizens.CheckBoxShowCloseApp',
                    applyTo: '#cbShowCloseAppeals',
                    selector: 'appealCitsGridFond',
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
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'appealCitsStateButtonAspect',
            stateButtonSelector: '#appealCitsEditWindowFond #btnState',
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
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'relatedAppealCitsGridStateTransferAspect',
            gridSelector: '#gridRelatedAppealCits',
            stateType: 'gji_appeal_citizens',
            menuSelector: 'appealCitsGridStateMenu'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'appealCitsStateTransferAspect',
            gridSelector: '#appealCitsGridFond',
            stateType: 'gji_appeal_citizens',
            menuSelector: 'appealCitsGridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'appcitExecutantButtonExportAspect',
            gridSelector: '#appealCitsExecutantGrid',
            buttonSelector: '#appealCitsExecutantGrid #btnExport',
            controllerName: 'AppealCitsExecutant',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitizensWindowAspect',
            gridSelector: 'appealCitsGridFond',
            editFormSelector: '#appealCitsEditWindowFond',
            storeName: 'AppealCits',
            modelName: 'AppealCits',
            editWindowView: 'appealcits.EditWindowFond',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId(), record.get('NumberGji'));
            },
            otherActions: function (actions) {

// новые поля фильтра - тематика и подтематика
                actions['#appealcitsFilterPanelFond #sfStatSubj'] = { 'change': { fn: this.onChangeStatSubject, scope: this } };
                actions['#appealcitsFilterPanelFond #sfStatSubsubjectGji'] = { 'change': { fn: this.onChangeStatSubsubjectGji, scope: this } };

                actions['#appealcitsFilterPanelFond #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#appealcitsFilterPanelFond #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#appealcitsFilterPanelFond #dfDateFromStart'] = { 'change': { fn: this.onChangeDateFromStart, scope: this } };
                actions['#appealcitsFilterPanelFond #dfDateFromEnd'] = { 'change': { fn: this.onChangeDateFromEnd, scope: this } };
                actions['#appealcitsFilterPanelFond #dfCheckTimeStart'] = { 'change': { fn: this.onChangeCheckTimeStart, scope: this } };
                actions['#appealcitsFilterPanelFond #dfCheckTimeEnd'] = { 'change': { fn: this.onChangeCheckTimeEnd, scope: this } };
                actions['#appealcitsFilterPanelFond #sfAuthor'] = { 'change': { fn: this.onChangeAuthor, scope: this } };
                actions['#appealcitsFilterPanelFond #sfExecutant'] = { 'change': { fn: this.onChangeExecutant, scope: this } };
                actions['#appealcitsFilterPanelFond #sfController'] = { 'change': { fn: this.onChangeController, scope: this } };

               // actions['appealcitsexecutantgrid #btnExport'] = { 'click': { fn: this.runExportToExcel, scope: this } };
             
                actions[this.editFormSelector + ' [name=Description]'] = { 'focus': { fn: this.showBigDescription, scope: this } };

                actions[this.editFormSelector + ' #previousAppealCitsSelectField'] = { 'beforeload': { fn: this.onBeforeLoadPreviousAppeal, scope: this } };
                actions[this.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: this.onRedtapeFlagChange, scope: this } };
                actions[this.editFormSelector + ' #btnSOPR'] = { 'click': { fn: this.goToSOPR, scope: this } };
                actions[this.editFormSelector + ' #appealCitsSuretySelectField'] = {
                    'change': { fn: this.onSuretyChange, scope: this },
                    'beforeload': { fn: this.onSuretyBeforeLoad, scope: this }
                };
                actions[this.editFormSelector + ' #appealCitsExecutantSelectField'] = { 'change': { fn: this.onExecutantChange, scope: this } };
                actions[this.editFormSelector + ' #btnCreateStatement'] = { 'click': { fn: this.onCreateStatement, scope: this } };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                actions[this.editFormSelector + ' button[actionName=checkTimeHistoryBtn]'] = { 'click': { fn: this.showCheckTimeHistory, scope: this } };
                actions[this.editFormSelector + ' [name=SuretyResolve]'] = { 'change': { fn: this.onSuretyResolveChange, scope: this } };
                actions[this.editFormSelector + ' #kindStatementSelectField'] = { 'change': { fn: this.onKindStatementChange, scope: this } };
                actions[this.editFormSelector + ' [name=Year]'] = { 'change': { fn: this.onYearChange, scope: this } };
                actions[this.editFormSelector + ' #sfQuestionStatus'] = { 'change': { fn: this.onChangeQuestionStatus, scope: this } };
                actions['#appealcitsFilterPanelFond #clear'] = { 'click': { fn: this.clearAllFilters, scope: this } };
                actions[this.gridSelector + ' #cbShowExtensTimes'] = { 'change': { fn: this.onExtensTimesCheckbox, scope: this } };
            },
            goToSOPR: function (btn) {
                var asp = this,
                    portal = asp.controller.getController('PortalController'),
                    params = {},
                    sopr = null,
                    rec = asp.getForm().getRecord();
                var result = B4.Ajax.request(B4.Url.action('GetSOPRId', 'AppealCitsExecutant', {
                   recordId : rec.getId()
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

            onChangeQuestionStatus: function (field, newValue) {
                var form = this.getForm(),
                    sfSSTUTransferOrg = form.down('#sfSSTUTransferOrg');

                if (newValue == B4.enums.QuestionStatus.Transferred) {
                    sfSSTUTransferOrg.show();
                    //sfSSTUTransferOrg.setDisabled(false);
                    sfSSTUTransferOrg.allowBlank = false;
                }
                else {
                    sfSSTUTransferOrg.setValue(null);
                    sfSSTUTransferOrg.hide();
                   // sfSSTUTransferOrg.setDisabled(true);
                    sfSSTUTransferOrg.allowBlank = true;
                }


            },

            onSuretyResolveChange: function (sf, val) {
                var form = this.getForm(),
                    contragentField = form.down('[name=ApprovalContragent]');

                contragentField.setDisabled(!val || val.Code != "4");
            },

            onAfterSetFormData: function (aspect, rec, form) {
                appealCitizensId = rec.get('Id');
                this.controller.getAspect('appealCitsFKRPrintAspect').loadReportStore();
                var sflZonalInspection = form.down('#sflZonalInspection');
                if (sflZonalInspection.value == null)
                {
                    var newZonal = {
                        Id: 1,
                        ZoneName: 'ФКР Воронеж'
                    };
                    sflZonalInspection.setValue(newZonal);
                }
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
                    var id = record.getId();
                    var numberGji = record.get('NumberGji');
                    var number = record.get('Number');
                    var attachmentGrid = form.down('appealcitsattachmentgrid');
                   // var categoryGrid = form.down('appealcitscategorygrid');
                    //Передаем аспекту смены статуса необходимые параметры
                    asp.controller.getAspect('appealCitsStateButtonAspect').setStateData(id, record.get('State'));
                   // categoryGrid.getStore().filter('appealCitizensId', id);
                    asp.controller.setCurrentId(id, numberGji, number);

                    attachmentGrid.tab.setDisabled(record.phantom);
                    attachmentGrid.getStore().filter('appealCitizensId', id);
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
                if (data) {
                    fieldPosition.setValue(data.Position);
                } else {
                    fieldPosition.setValue(null);
                }
            },

            showBigDescription: function (field, data, record) {            
                var currentDescriptonText = field.getValue();            
                win = Ext.create('B4.view.appealcits.DetailsEditWindow');              
                var valuefield = win.down('#dfDescription');
                var closebutton = win.down('b4closebutton');
                var savebutton = win.down('b4savebutton');
                valuefield.setValue(currentDescriptonText);              
                win.show();
                closebutton.addListener('click', function () {
                    win.close();
                });
                savebutton.addListener('click', function () {
                    currentDescriptonText = valuefield.getValue(); 
                    field.setValue(currentDescriptonText);
                    win.close();
                });   
               
            },

            onSuretyBeforeLoad: function (field, options) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;
            },

            onExecutantChange: function (field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#appealCitsExecutantPositionTextField');
                if (data) {
                    fieldPosition.setValue(data.Position);
                } else {
                    fieldPosition.setValue(null);
                }
            },
            onChangeRealityObject: function (field, newValue) {
                if (newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
//новые фильтры тематики и подтематики
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
            onChangeExecutant: function (field, newValue) {
                if (newValue) {
                    var ids = [];
                    Ext.Array.each(newValue, function (elem) { ids.push(elem.Id); });
                    this.controller.params.executantId = ids;
                } else {
                    this.controller.params.executantId = null;
                }
            },
            onChangeAuthor: function (field, newValue) {
                if (newValue) {
                    var ids = [];
                    Ext.Array.each(newValue, function (elem) { ids.push(elem.Id); });
                    this.controller.params.authorId = ids;
                } else {
                    this.controller.params.authorId = null;
                }
            },
            onChangeController: function (field, newValue) {
                if (newValue) {
                    var ids = [];
                    Ext.Array.each(newValue, function (elem) { ids.push(elem.Id); });
                    this.controller.params.controllerId = ids;
                } else {
                    this.controller.params.controllerId = null;
                }
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('AppealCits');
                str.currentPage = 1;
                str.load();
            },

            runExportToExcel: function (btn) {
                var currApp = appealCitizensId;
                B4.Ajax.request(B4.Url.action('Export', 'AppealCitsExecutant', {
                    appealCitizensId: appealCitizensId
                }));
              
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
            onKindStatementChange: function (field, data, oldValue) {
                var docField = field.up(this.editFormSelector).down('[name=DocumentNumber]'),
                    newValue = data ? data.Id : null;
                newValuePostfix = data ? data.Postfix : null;
                oldValuePostfix = null;

                Ext.Array.each(field.getStore().data.items, function (item) {
                    if (item.data.Id == oldValue) {
                        oldValuePostfix = item.data.Postfix;
                    }
                });

                if (oldValue !== undefined && newValue !== oldValue && newValuePostfix != oldValuePostfix) {
               //     docField.setValue();
                }
            },
            onYearChange: function (field, newValue, oldValue) {
                var me = this,
                    docNumberField = field.up(me.editFormSelector).down('[name=DocumentNumber]');

                if (oldValue) {
                  //  docNumberField.setValue();
                }
            },
            clearAllFilters: function (bt) {
                var panel = bt.up('appealcitsFilterPanelFond');
                panel.down('#dfDateFromStart').setValue();
                panel.down('#dfDateFromEnd').setValue();
                panel.down('#dfCheckTimeStart').setValue();
                panel.down('#dfCheckTimeEnd').setValue();
                panel.down('#sfRealityObject').setValue();
                panel.down('#sfAuthor').setValue();
                panel.down('#sfExecutant').setValue();
				panel.down('#sfController').setValue();

				panel.down('#sfStatSubsubjectGji').setValue();
				panel.down('#sfStatSubj').setValue();
                this.controller.getStore('AppealCits').load();
            },
            onExtensTimesCheckbox: function (field, newValue) {
                this.controller.params.showExtensTimes = newValue;
                this.controller.getStore('AppealCits').load();
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
            и сохраняем обращения граждан через серверный метод /AppealCitsCategory/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appealcitsexecutiontypegridAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealcitsexecutiontypeSelectWindow',
            storeName: 'appealcits.AppealCitsExecutionType',
            modelName: 'appealcits.AppealCitsExecutionType',
            storeSelect: 'dict.AppealExecutionType',
            storeSelected: 'dict.AppealExecutionType',
            gridSelector: 'appealcitsexecutiontypegrid',
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
            titleSelectWindow: 'Выбор показателей исполнения',
            titleGridSelect: 'Показатели для выбора',
            titleGridSelected: 'Выбранные показатели',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'AppealCitsExecutionType', {
                            recordIds: Ext.encode(recordIds),
                            appealCitizensId: asp.controller.appealCitizensId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Показатели сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать показатели');
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
            fieldSelector: '#appealCitsEditWindowFond #trigfRelatedAppealCitizens',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#relatedAppCitSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            textProperty: 'NumberGji',
            columnsGridSelect: [
                { header: 'Номер ФКР', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата обращения', xtype: 'datecolumn', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер ФКР', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, sortable: false },
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
                    header: 'Проверяющий сотрудник'
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
              xtype: 'gkhinlinegridaspect',
              name: 'relatedAppealCitizensGridInlineAspect',
              storeName: 'AppealCits',
              modelName: 'AppealCits',
              gridSelector: '#gridAnotherRelatedAppealCits',
              onBeforeLoad: function (store, operation) {
                  operation = operation || {};
                  operation.params = operation.params || {};
                  operation.params.anotherrelatesToId = this.controller.appealCitizensId;
                  operation.params.anothermatchRelated = true;

                  this.getSelectedGrid().getStore().load();
              },
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
            xtype: 'grideditwindowaspect',
            name: 'preliminaryCheckGridWindowAspect',
            gridSelector: 'preliminaryCheckGrid',
            editFormSelector: '#preliminaryCheckEditWindow',
            storeName: 'PreliminaryCheck',
            modelName: 'PreliminaryCheck',
            editWindowView: 'appealcits.PreliminaryCheckEditWindow',
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
            gridSelector: 'appealCitsGridFond',
            buttonSelector: 'appealCitsGridFond #btnExport',
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
            isTbar: true,
            tbarCmp: [
                {
                    xtype: 'textfield',
                    ident: 'searchfield',
                    width: 350,
                    emptyText: 'Поиск',
                    enableKeyEvents: true
                },
                {
                    xtype: 'button',
                    text: 'Искать',
                    iconCls: 'icon-page-white-magnify',
                    ident: 'searchbtn'
                }
            ],
            otherActions: function (actions) {
                var me = this;

                actions[me.multiSelectWindowSelector + ' [ident=searchbtn]'] = { 'click': { fn: me.goFilter, scope: me } };
                actions[me.multiSelectWindowSelector + ' [ident=searchfield]'] = {
                    'keypress': {
                        fn: function (scope, e) {
                            if (e.getKey() == 13) {
                                me.goFilter(scope);
                            }
                        }, scope: me
                    }
                };
            },
            goFilter: function (btn) {
                debugger;
                var filterData = btn.up('#appealCitsStatSubjectMultiSelectWindow').down('[ident=searchfield]').getValue(),
                    treepanel = btn.up('#appealCitsStatSubjectMultiSelectWindow').down('treepanel');
                treepanel.getStore().reload({
                    params: { filter: filterData }
                });
            },
            //columnsGridSelect: [
            //    { header: 'Тематика', xtype: 'gridcolumn', dataIndex: 'text', flex: 1, filter: { xtype: 'textfield' } }
            //],
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
            getForm: function () {
                var me = this,
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0],
                    stSelected,
                    stSelect;

                if (win && !win.getBox().width) {
                    win = win.destroy();
                }

                if (!win) {
                    stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);
                    stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

                    stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
                    stSelect.on('beforeload', me.onBeforeLoad, me);
                    stSelect.on('load', me.onLoad, me);

                    win = me.controller.getView(me.multiSelectWindow).create({
                        itemId: me.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: me.columnsGridSelect,
                        columnsGridSelected: me.columnsGridSelected,
                        title: me.titleSelectWindow,
                        titleGridSelect: me.titleGridSelect,
                        titleGridSelected: me.titleGridSelected,
                        selModelMode: me.selModelMode,
                        isTbar: me.isTbar,
                        tbarCmp: me.tbarCmp,
                        constrain: true,
                        modal: false,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    });

                    win.on('afterrender', me.formAfterrender, me);

                    if (Ext.isNumber(me.multiSelectWindowWidth) && win.setWidth) {
                        win.setWidth(me.multiSelectWindowWidth);
                    }

                    stSelected.sorters.clear();
                    stSelect.sorters.clear();
                }

                return win;
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
            name: 'appealCitsAnswerGridWindowAspect',
            gridSelector: '#appealCitsAnswerGrid',
            editFormSelector: '#appealCitsAnswerEditWindow',
            storeName: 'appealcits.Answer',
            modelName: 'appealcits.Answer',
            editWindowView: 'appealcits.AnswerEditWindow',
            otherActions: function (actions) {

                actions['#appealCitsAnswerEditWindow #sendEmailButton'] = { 'click': { fn: this.sendEmail, scope: this } };
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
                beforesetformdata: function (asp, record) {
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
                    asp.controller.appealCitsAnswerId = record.get('Id');
                    appealCitsAnswer = record.getId();
                    this.controller.getAspect('appealCitsAnswerPrintAspect').loadReportStore();
                    this.controller.getAspect('appealCitsAnswerEditWindowAspect').setStateData(record.get('Id'), record.get('State'));
                },
             
            },
            sendEmail: function (record) {
                var me = this;
                var taskId = appealCitsAnswer;

                if (appealCitsAnswer == 0 || appealCitsAnswer == null) {
                    Ext.Msg.alert('Внимание!', 'Перед отправкой почты необходимо сохранить запись');
                }
                else {
                    me.mask('Отправка Email', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('SendEmailFKR', 'SendEmail', {
                        taskId: taskId
                    }
                    )).next(function (response) {

                        me.unmask();
                        Ext.Msg.alert('Внимание!', 'Письмо отправлено');
                        var data = Ext.decode(response.responseText);
                        return true;
                    }).error(function (response) {
                        var data = response.message;
                        me.unmask();
                        Ext.Msg.alert('Внимание!', data);
                    });

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
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    } 
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    appcitRequestId = record.getId();
                    var grid = form.down('appcitrequestanswergrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.appcitRequestId = appcitRequestId;
                        },
                        me);
                    store.load();                 

                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'requestAnswerGridWindowAspect',
            gridSelector: 'appcitrequestanswergrid',
            editFormSelector: '#appealCitsRequestAnswerEditWindow',
            storeName: 'appealcits.RequestAnswer',
            modelName: 'appealcits.RequestAnswer',
            editWindowView: 'appealcits.RequestAnswerEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsRequest', appcitRequestId);
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
                { header: 'Номер ФКР', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Количество вопросов', xtype: 'gridcolumn', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, sortable: false },
                { header: 'Номер ФКР', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } }
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
                                    Number: rec.get('Number'),
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
                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.excludeInpectorId = this.controller.inpectorId;
                operation.params.onlyActive = true;

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

                    var dfOrderDate = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfOrderDate')[0];
                    if (!dfOrderDate.allowBlank && !dfOrderDate.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дату поручения');
                        return false;
                    }

                    var taDescription = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #taDescription')[0];
                    if (!taDescription.allowBlank && !taDescription.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо заполнить резолюцию');
                        return false;
                    }
                    var cbIsResponsible = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #cbIsResponsible')[0];

                    var authorField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #sflAuthor')[0];
                    if (!authorField.isDisabled() && (!authorField.value || authorField.value.Id <= 0)) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать поручителя');
                        return false;
                    }

                    if (recordIds[0] <= 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddExecutants', 'AppealCitsExecutant', {
                        inspectorIds: Ext.encode(recordIds),
                        appealCitizensId: asp.controller.appealCitizensId,
                        performanceDate: dateField.value,
                        dfOrderDate: dfOrderDate.value,
                        cbIsResponsible: cbIsResponsible.value,
                        taDescription: taDescription.value,
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
            xtype: 'grideditwindowaspect',
            name: 'appealCitsLiteAdmonitionGridWindowAspect',
            gridSelector: '#appealCitsAdmonitionGrid',
            editFormSelector: '#appealCitsAdmonitionEditWindow',
            storeName: 'appealcits.AppealCitsAdmonition',
            modelName: 'appealcits.AppealCitsAdmonition',
            editWindowView: 'appealcits.AppealCitsAdmonitionEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
                aftersetformdata: function (asp, record, form) {

                    this.controller.getAspect('appealCitsLiteAdmonitionPrintAspect').loadReportStore();
                    appealCitsAdmonition = record.getId();

                    var grid = form.down('appCitAdmonVoilationGrid'),
                        store = grid.getStore();
                    store.filter('AppealCitsAdmonition', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsLitePrescriptionFondGridWindowAspect',
            gridSelector: '#appealCitsPrescriptionFondGrid',
            editFormSelector: '#appealCitsPrescriptionFondEditWindow',
            storeName: 'appealcits.AppealCitsPrescriptionFond',
            modelName: 'appealcits.AppealCitsPrescriptionFond',
            editWindowView: 'appealcits.AppealCitsPrescriptionFondEditWindow',
            otherActions: function (actions) {
                actions['#appealCitsPrescriptionFondEditWindow #sfBuildContract'] = { 'change': { fn: this.onChangeDoc, scope: this } };

            },
            onChangeDoc: function (field, newValue) {
                debugger;
                if (newValue != null) {
                    bcId = newValue.Id;
                }
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
                aftersetformdata: function (asp, record, form) {

                    this.controller.getAspect('appealCitsLitePrescriptionFondPrintAspect').loadReportStore();
                    appealCitsPrescriptionFond = record.getId();

                    var grid = form.down('appCitPrFondVoilationGrid'),
                        store = grid.getStore();
                    var grid2 = form.down('appCitPrFondObjectCrGrid'),
                        store2 = grid2.getStore();
                    store.filter('AppealCitsPrescriptionFond', record.getId());
                    store2.filter('AppealCitsPrescriptionFond', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitsLiteResolutionGridWindowAspect',
            gridSelector: '#appealCitsResolutionGrid',
            editFormSelector: '#appealCitsResolutionEditWindow',
            storeName: 'appealcits.AppealCitsResolution',
            modelName: 'appealcits.AppealCitsResolution',
            editWindowView: 'appealcits.AppealCitsResolutionEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCits', this.controller.appealCitizensId);
                    }
                },
                aftersetformdata: function (asp, record, form) {

                    //this.controller.getAspect('appealCitsLiteAdmonitionPrintAspect').loadReportStore();
                    appealCitsResolution = record.getId();

                    var grid = form.down('appealCitsResolutionExecutorGrid'),
                        store = grid.getStore();
                    store.filter('AppealCitsResolution', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appCitLiteAdmonVoilationGridWindowAspect',
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
            name: 'appCitLitePrFondVoilationGridWindowAspect',
            gridSelector: '#appCitPrFondVoilationGrid',
            editFormSelector: '#appCitPrFondVoilationEditWindow',
            storeName: 'appealcits.AppCitPrFondVoilation',
            modelName: 'appealcits.AppCitPrFondVoilation',
            editWindowView: 'appealcits.AppCitPrFondVoilationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsPrescriptionFond', appealCitsPrescriptionFond);
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
            name: 'appCitLitePrFondObjectCrGridWindowAspect',
            gridSelector: '#appCitPrFondObjectCrGrid',
            editFormSelector: '#appCitPrFondObjectCrEditWindow',
            storeName: 'appealcits.AppCitPrFondObjectCr',
            modelName: 'appealcits.AppCitPrFondObjectCr',
            editWindowView: 'appealcits.AppCitPrFondObjectCrEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    debugger;
                    if (!record.get('Id')) {
                        record.set('AppealCitsPrescriptionFond', appealCitsPrescriptionFond);
                    }
                },

                aftersetformdata: function (asp, record, form) {
                    debugger;
                    var sfObjCr = this.getForm().down('#sfObjCr');
                    sfObjCr.getStore().filter('bcId', bcId);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealCitLiteResolutionExecutorGridWindowAspect',
            gridSelector: '#appealCitsResolutionExecutorGrid',
            editFormSelector: '#appealCitsResolutionExecutorEditWindow',
            storeName: 'appealcits.AppealCitsResolutionExecutor',
            modelName: 'appealcits.AppealCitsResolutionExecutor',
            editWindowView: 'appealcits.AppealCitsResolutionExecutorEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsResolution', appealCitsResolution);
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
    ],

    setCurrentId: function (id, numberGji, number) {
        this.appealCitizensId = id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            fieldRelatedAppealCitizens = editWindow.down('#trigfRelatedAppealCitizens'),
            kindStatementField = editWindow.down('#kindStatementSelectField'),
            tabpanel = editWindow.down('.tabpanel'),
            btnCreateStatement = editWindow.down('#btnCreateStatement'),
            storeRo = this.getStore('appealcits.RealityObject'),
            storeStatement = this.getStore('appealcits.StatSubject'),
            sourceStore = this.getStore('appealcits.Source'),
            storeAnswer = this.getStore('appealcits.Answer'),
            storeRequest = this.getStore('appealcits.Request'),
            storeBaseStatement = this.getStore('appealcits.BaseStatement'),
            //storeAppealCitsAdmonition = this.getStore('appealcits.AppealCitsAdmonition');
        storeAppealCitsPrescriptionFond = this.getStore('appealcits.AppealCitsPrescriptionFond');
        storeAppealCitsResolution = this.getStore('appealcits.AppealCitsResolution');
        storeAppealCitsTypeExec = this.getStore('appealcits.AppealCitsExecutionType');
            storeAppCitsBaseStatement = this.getStore('appealcits.AppealCitsBaseStatement'),
            storeAppCitsPreCheck = this.getStore('PreliminaryCheck'),
            storeAppealCitsExecutant = this.getStore('appealcits.AppealCitsExecutant'),
            relatedAppealCits = editWindow.down('#gridRelatedAppealCits');
        anotherrelatedAppealCits = editWindow.down('#gridAnotherRelatedAppealCits');

        btnCreateStatement.setDisabled(!id);
        relatedAppealCits.setRelatesToId(id);
        anotherrelatedAppealCits.setAnotherRelatesToId(id);

        sourceStore.removeAll();
        storeRo.removeAll();
        storeAppCitsPreCheck.removeAll();
        storeStatement.removeAll();
        //storeAppealCitsAdmonition.removeAll();
        storeAppealCitsPrescriptionFond.removeAll();
        storeAppealCitsResolution.removeAll();
        storeAppealCitsTypeExec.removeAll();
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
        //tabpanel.down('#appealCitsAdmonitionGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsPrescriptionFondGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsResolutionGrid').tab.setDisabled(!id);
        tabpanel.down('#appealCitsRequestGrid').tab.setDisabled(!id);
        tabpanel.down('#baseStatementAppCitsGrid').tab.setDisabled(!id);
        tabpanel.setActiveTab(0);

        editWindow.down('#trigfRelatedAppealCitizens').setDisabled(!id);
        if (id > 0) {
            storeRo.load();
            storeStatement.load();
            storeAppCitsPreCheck.load();
            sourceStore.load();
            //storeAppealCitsAdmonition.load();
            storeAppealCitsPrescriptionFond.load();
            storeAppealCitsResolution.load();
            storeAppealCitsTypeExec.load();
            storeAnswer.load();
            storeRequest.load();
            storeBaseStatement.load();
            storeAppealCitsExecutant.load();

            storeAppCitsBaseStatement.add({
                Id: this.appealCitizensId,
                NumberGji: numberGji,
                Number: number
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
        this.getStore('PreliminaryCheck').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Answer').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.Request').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.BaseStatement').on('beforeload', this.onBeforeLoad, this);
        //this.getStore('appealcits.AppealCitsAdmonition').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsPrescriptionFond').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsResolution').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsExecutionType').on('beforeload', this.onBeforeLoad, this);
        this.getStore('appealcits.AppealCitsExecutant').on('beforeload', this.onBeforeLoad, this);

        var actions = {};
        
      //  actions['#gridRelatedAppealCits'] = { 'rowaction': { fn: this.rowActionRelApp, scope: this } };
        actions['relatedAppealCitsGridFond b4editcolumn'] = { click: { fn: this.rowActionRelApp, scope: this } };
        actions['#gridAnotherRelatedAppealCits'] = { 'rowaction': { fn: this.rowActionAnotherRelApp, scope: this } };

        this.control(actions);

        this.callParent(arguments);
    },

    rowActionRelApp: function(a, b, t, y, r, rec) {
        var me = this,
            portal = me.getController('PortalController'),
            controllerEditName = 'B4.controller.AppealCitsFond',
            params = {};

        params.appealId = rec.get('Id');
        portal.loadController(controllerEditName, params);
    },

    rowActionAnotherRelApp: function (grid, action, record) {
        var me = this,
            portal = me.getController('PortalController'),
            controllerEditName = 'B4.controller.AppealCitsFond',
            params = {};

        params.appealId = record.get('Id');
        portal.loadController(controllerEditName, params);
    },

    showDetails: function (fld) {
        var me = this,
            win = Ext.widget('personalaccountfielddetailswindow', {
                listeners: {
                    show: function () {
                        Ext.select('.x-mask').addListener('click', function () {
                            win.close();
                        });
                    }
                }
            }),
            store = win.down('gridpanel').getStore();

        store.on('beforeload',
            function (store, operation) {
                operation.params.fieldName = fld.name;
                operation.params.accId = me.getMainView().params.Id;
            },
            me);

        store.on('load', function () { win.show(); });
        store.load();
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
        this.params.statSubsubjectGjiId = null;
        this.params.statSubjectId = null;
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
        this.params.statSubsubjectGjiId = null;
        this.params.statSubjectId = null;


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
            operation.params.authorId = this.params.authorId;
            operation.params.executantId = this.params.executantId;
            operation.params.controllerId = this.params.controllerId;
            operation.params.realityObjectId = this.params.realityObjectId;
         operation.params.statSubsubjectGjiId = this.params.statSubsubjectGjiId;
         operation.params.statSubjectId = this.params.statSubjectId;
            operation.params.dateFromStart = this.params.dateFromStart;
            operation.params.dateFromEnd = this.params.dateFromEnd;
            operation.params.checkTimeStart = this.params.checkTimeStart;
            operation.params.checkTimeEnd = this.params.checkTimeEnd;
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
            operation.params.showExtensTimes = this.params.showExtensTimes;
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