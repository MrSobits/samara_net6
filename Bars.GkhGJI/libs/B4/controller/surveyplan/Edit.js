Ext.define('B4.controller.surveyplan.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.StateContextButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditCtxWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'SurveyPlan',
        'surveyplan.Contragent',
        'surveyplan.ContragentAttachment'
    ],

    stores: [
        'SurveyPlan',
        'surveyplan.Contragent',
        'surveyplan.ContragentAttachment'
    ],

    views: [
        'surveyplan.EditPanel',
        'surveyplan.CandidateSelectWindow',
        'surveyplan.ContragentEditWindow',
        'surveyplan.ContragentAttachmentEditWindow'
    ],

    mainView: 'surveyplan.EditPanel',
    mainViewSelector: 'surveyPlanEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveyPlanEditPanel'
        },
        {
            ref: 'contragentGrid',
            selector: 'surveyPlanContragentGrid'
        },
        {
            ref: 'contragentEditWindow',
            selector: 'surveyPlanContragentEditWindow'
        }
    ],
    
    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'surveyPlanContragentStateTransferAspect',
            gridSelector: 'surveyPlanContragentGrid',
            menuSelector: 'surveyPlanContragentGridStateMenu',
            stateType: 'gji_survey_plan_contragent'
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'surveyPlanStateButtonAspect',
            stateButtonSelector: 'surveyPlanEditPanel [action=ChangeState]',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    asp.controller.getAspect('surveyPlanEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'surveyPlanEditPanelAspect',
            editPanelSelector: 'surveyPlanEditPanel',
            modelName: 'SurveyPlan',
            listeners: {
                savesuccess: function (asp, rec) {
                    asp.setData(rec.getId());
                },
                aftersetpaneldata: function (asp, rec) {
                    var grid = asp.controller.getContragentGrid(),
                        store = grid.getStore();

                    store.load();
                    asp.controller.getAspect('surveyPlanStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'surveyPlanContragentGridAspect',
            gridSelector: 'surveyPlanContragentGrid',
            storeName: 'surveyplan.Contragent',
            modelName: 'surveyplan.Contragent',
            editFormSelector: 'surveyPlanContragentEditWindow',
            editWindowView: 'surveyplan.ContragentEditWindow',
            multiSelectWindow: 'surveyplan.CandidateSelectWindow',
            multiSelectWindowSelector: 'surveyPlanCandidateSelectWindow',
            storeSelect: 'surveyplan.Candidate',
            storeSelected: 'surveyplan.CandidateForSelected',
            titleSelectWindow: 'Выбор контрагентов для включения в план',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты',
            leftGridConfig: {
                minWidth: 1200
            },
            rightGridConfig: {
                minWidth: 1200
            },
            columnsGridSelect: [
                { xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, header: 'Муниципальный район', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'JuridicalAddress', flex: 1, header: 'Юридический адрес', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, header: 'Наименование', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Phone', width: 100, header: 'Телефон', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Inn', width: 100, header: 'ИНН', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Kpp', width: 100, header: 'КПП', filter: { xtype: 'textfield' } },
                { xtype: 'b4enumcolumn', dataIndex: 'PlanMonth', header: 'Месяц проверки', enumName: 'B4.enums.Month', width: 100, filter: true },
                { xtype: 'gridcolumn', dataIndex: 'PlanYear', header: 'Год проверки', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'AuditPurpose', flex: 1, header: 'Цель проверки', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Reason', flex: 1, header: 'Основание включения в план', filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, header: 'Муниципальный район', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'JuridicalAddress', flex: 1, header: 'Юридический адрес', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, header: 'Наименование', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Phone', width: 100, header: 'Телефон', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Inn', width: 100, header: 'ИНН', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Kpp', width: 100, header: 'КПП', filter: { xtype: 'textfield' } },
                { xtype: 'b4enumcolumn', dataIndex: 'PlanMonth', header: 'Месяц проверки', enumName: 'B4.enums.Month', width: 100, filter: true },
                { xtype: 'gridcolumn', dataIndex: 'PlanYear', header: 'Год проверки', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'AuditPurpose', flex: 1, header: 'Цель проверки', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Reason', flex: 1, header: 'Основание включения в план', filter: { xtype: 'textfield' } }
            ],
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds.length > 0 && recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddCandidates', 'SurveyPlan', {
                            recordIds: recordIds,
                            surveyPlanId: asp.controller.getSurveyPlanId()
                        })).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function (response) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', response.message);
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать контрагентов');
                        return false;
                    }
                    return true;
                },
                aftersetformdata: function (asp, rec, form) {
                    form.id = rec.get('Id');
                    asp.controller.getAspect('surveyPlanContragentAttachmentWindowAspect').getGrid().getStore().load();
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=IsExcluded]'] = { 'change': { fn: me.onExcludedChange, scope: me } };
                actions[me.multiSelectWindowSelector + ' [name=Okopf]'] = { 'beforeload': { fn: me.onBeforeOrganizationFormLoad, scope: me } };
            },
            
            onExcludedChange: function(cb, val) {
                var w = this.getEditForm(),
                    reason = w.down('[name=ExclusionReason]');

                reason.setDisabled(!val);
            },
            onBeforeOrganizationFormLoad: function (store, operation) {
                var me = this,
                    form = me.getForm(),
                    field = form.down('[name=AuditPurpose]');

                operation.params.filterPlan = true;

                if (field && field.value) {
                    operation.params.goal = field.value.Code;
                }
            },
            onBeforeLoad: function (_, operation) {
                var w = this.getForm(),
                    auditPurpose = w.down('[name=AuditPurpose]'),
                    year = w.down('[name=Year]'),
                    municipality = w.down('[name=Municipality]'),
                    okopf = w.down('[name=Okopf]');

                operation.params.purposeId = auditPurpose.getValue();
                operation.params.moId = municipality.getValue();
                operation.params.okopfId = okopf.getValue();
                operation.params.year = year.getValue();
                operation.params.surveyPlanId = this.controller.getSurveyPlanId();
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'surveyPlanContragentAttachmentWindowAspect',
            gridSelector: 'surveyPlanContragentAttachmentGrid',
            editFormSelector: 'surveyPlanContragentAttachmentEditWindow',
            storeName: 'surveyplan.ContragentAttachment',
            modelName: 'surveyplan.ContragentAttachment',
            editWindowView: 'surveyplan.ContragentAttachmentEditWindow',
            listeners : {
                getdata: function(asp, rec) {
                    rec.set('SurveyPlanContragent', asp.controller.getContragentEditWindow().id);
                }
            }
        }
    ],

    getSurveyPlanId: function() {
        return this.getContextValue(this.getMainView(), 'surveyPlanId');
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('surveyPlanEditPanel');

        me.bindContext(view);
        view.params = {};
        view.params.surveyPlanId = id;
        me.setContextValue(view, 'surveyPlanId', id);
        me.application.deployView(view);

        me.getStore('surveyplan.ContragentAttachment').on('beforeload', me.onBeforeContragentAttachmentLoad, me);
        me.getContragentGrid().getStore().on('beforeload', me.onContragentStoreBeforeLoad, me);
        me.getAspect('surveyPlanEditPanelAspect').setData(id);
    },

    init: function () {
        var me = this,
            actions = {};

        actions['surveyPlanEditPanel [action=CreateCandidates]'] = { 'click': { fn: me.onCreateCandidates, scope: me } };
        me.control(actions);

        me.callParent(arguments);
    },

    onContragentStoreBeforeLoad: function(_, operation) {
        operation.params.surveyPlanId = this.getSurveyPlanId();
    },
    onBeforeContragentAttachmentLoad: function (_, operation) {
        operation.params.contragentId = this.getContragentEditWindow().id;
    },

    onCreateCandidates: function () {
        var me = this;
        me.mask('Сохранение', me.getMainComponent());
        B4.Ajax.request(B4.Url.action('CreateCandidates', 'SurveyPlan')).next(function () {
            me.unmask();
            B4.QuickMsg.msg('Расчет плановых дат проверок', 'Задача поставлена в очередь на обработку', 'success');
            return true;
        }).error(function (response) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', response.message);
        });
    }
});