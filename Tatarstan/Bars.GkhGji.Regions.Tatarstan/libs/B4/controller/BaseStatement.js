Ext.define('B4.controller.BaseStatement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.BaseStatementRequestType'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'BaseStatement'
    ],
    stores: [
        'BaseStatement',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'motivationconclusion.ForBaseStatement'
    ],
    views: [
        'basestatement.MainPanel',
        'basestatement.Grid',
        'basestatement.AddWindow',
        'basestatement.FilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'basestatement.MainPanel',
    mainViewSelector: 'baseStatementPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseStatementPanel'
        }
    ],

    basisDocIds: [],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'requirementAspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseStatement.Create', applyTo: 'b4addbutton', selector: '#baseStatementGrid' },
                { name: 'GkhGji.Inspection.BaseStatement.Edit', applyTo: 'b4savebutton', selector: '#baseStatementEditPanel' },
                { name: 'GkhGji.Inspection.BaseStatement.Delete', applyTo: 'b4deletecolumn', selector: '#baseStatementGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseStatement.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: '#baseStatementGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            this.controller.params.showCloseInspections = false;
                            component.show();
                        } else {
                            this.controller.params.showCloseInspections = true;
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseStatementStateTransferAspect',
            gridSelector: '#baseStatementGrid',
            menuSelector: 'baseStatementStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'baseStatementExportAspect',
            gridSelector: '#baseStatementGrid',
            buttonSelector: '#baseStatementGrid #btnExport',
            controllerName: 'BaseStatement',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок по обращениям граждан, формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseStatementGridWindowAspect',
            gridSelector: '#baseStatementGrid',
            editFormSelector: '#baseStatementAddWindow',
            storeName: 'BaseStatement',
            modelName: 'BaseStatement',
            editWindowView: 'basestatement.AddWindow',
            controllerEditName: 'B4.controller.basestatement.Navigation',   
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                var asp = this;

                actions['#baseStatementFilterPanel #sfRealityObject'] = { 'change': { fn: asp.onChangeRealityObject, scope: asp } };
                actions['#baseStatementFilterPanel #updateGrid'] = { 'click': { fn: asp.onUpdateGrid, scope: asp } };
                actions['#baseStatementGrid #cbShowCloseInspections'] = { 'change': { fn: asp.onChangeCheckbox, scope: asp } };

                actions[asp.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: asp.onChangeType, scope: asp } };
                actions[asp.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: asp.onChangePerson, scope: asp } };
                actions[asp.editFormSelector + ' b4enumcombo[name=RequestType]'] = { 'change': { fn: asp.onChangeRequestType, scope: asp } };
                actions[asp.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: asp.onBeforeLoadContragent, scope: asp } };
                actions[asp.editFormSelector + ' #cbShowCloseInspections'] = { 'change': { fn: asp.onChangeCheckbox, scope: asp } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseStatement');
                str.currentPage = 1;
                str.load();
            },
            onChangeRealityObject: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },
            onChangeType: function (field, newValue, oldValue) {
                this.controller.params = this.controller.params || {};
                this.controller.params.typeJurOrg = newValue;
                this.getForm().down('#sfContragent').setValue(null);
                this.getForm().down('#tfPhysicalPerson').setValue(null);
            },
            onChangePerson: function (field, newValue, oldValue) {
                var form = this.getForm(),
                    sfContragent = form.down('#sfContragent'),
                    tfPhysicalPerson = form.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = form.down('#cbTypeJurPerson'),
                    innField = form.down('[name=Inn]');
                
                sfContragent.setValue(null);
                tfPhysicalPerson.setValue(null);
                cbTypeJurPerson.setValue(10);
                
                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        innField.setDisabled(false);
                        innField.show();
                        break;
                    case 20://организация
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(false);
                        innField.setDisabled(true);
                        innField.hide();
                        break;
                    case 30://должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(false);
                        innField.setDisabled(false);
                        innField.show();
                        break;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseInspections = newValue;
                this.controller.getStore('BaseStatement').load();
            },
            onChangeRequestType: function (field, newValue, oldValue) {
                var asp = this,
                    form = asp.getForm(),
                    appealCitizens = form.down('gkhtriggerfield[name=appealCitizens]'),
                    motivationConclusions = form.down('gkhtriggerfield[name=MotivationConclusions]'),
                    visibility = function(form, isVisible) {
                        form.setDisabled(!isVisible);
                        form.setVisible(isVisible);
                    };
                appealCitizens.setValue(null);
                motivationConclusions.setValue(null);

                switch (newValue) {
                    case B4.enums.BaseStatementRequestType.AppealCits:
                        visibility(appealCitizens, true);
                        visibility(motivationConclusions, false);
                        break;
                    case B4.enums.BaseStatementRequestType.MotivationConclusion:
                        visibility(appealCitizens, false);
                        visibility(motivationConclusions, true);
                        break;
                }
            },
            saveRecord: function (rec) {
                if (this.fireEvent('beforesave', this, rec) !== false) {
                    
                    var me = this;
                    var frm = me.getForm(), model;
                    me.mask('Сохранение', frm);

                    B4.Ajax.request({
                        url: B4.Url.action('CreateWithBasis', 'BaseStatement'),
                        params: {
                            basisDocIds: Ext.encode(this.controller.basisDocIds),
                            baseStatement: Ext.encode(rec.data),
                            contragentId: rec.get('Contragent')
                        },
                        timeout: 9999999
                    }).next(function (response) {
                        var res = Ext.JSON.decode(response.responseText);
                        me.unmask();
                        
                        model = me.getModel(rec);

                        model.load(res.Id, {
                            success: function (record) {
                                me.fireEvent('savesuccess', me, record);
                            }
                        });
                    }).error(function (result) {
                        me.unmask();
                        me.fireEvent('savefailure');

                        Ext.Msg.alert('Невозможно сформировать проверку!', result.message);
                    });
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealCitizensAddMultiSelectWindowAspect',
            fieldSelector: '#baseStatementAddWindow #trigfAppealCitizens',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementAppCitAddSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            textProperty: 'Name',
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
            titleSelectWindow: 'Выбор обращения граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        controller = me.controller,
                        form = controller.getAspect('baseStatementGridWindowAspect').getForm(),
                        contragentField = form.down('b4selectfield[name=Contragent]'),
                        contragent,
                        recordIds = [],
                        moArr = [],
                        moIds = [],
                        uniqArr = [];

                    contragentField.setValue('');

                    records.each(function (rec) {
                        contragent = rec.get('Contragent');
                        recordIds.push(rec.get('Id'));
                        moArr.push(contragent);
                        moIds.push(contragent.Id);
                    });

                    uniqArr = Ext.Array.unique(moIds);
                    if (uniqArr.length === 1) {
                        contragentField.setValue(moArr[0]);
                    }
                    
                    me.controller.basisDocIds = recordIds;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'motivationConclusionAddMultiSelectWindowAspect',
            fieldSelector: '#baseStatementAddWindow gkhtriggerfield[name=MotivationConclusions]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementMotivConclAddSelectWindow',
            storeSelect: 'motivationconclusion.ForBaseStatement',
            storeSelected: 'motivationconclusion.ForBaseStatement',
            textProperty: 'DocumentNumber',
            columnsGridSelect: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DocumentDate', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
            ],
            columnsGridSelected: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, sortable: false },
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор мотивировочного заключения',
            titleGridSelect: 'Мотивировочные заключения для выбора',
            titleGridSelected: 'Мотивировочные заключения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        controller = me.controller,
                        form = controller.getAspect('baseStatementGridWindowAspect').getForm(),
                        contragentField = form.down('b4selectfield[name=Contragent]'),
                        contragent,
                        recordIds = [],
                        moArr = [],
                        moIds = [],
                        uniqArr = [];

                    contragentField.setValue('');

                    records.each(function (rec) {
                        contragent = rec.get('Contragent');
                        recordIds.push(rec.get('Id'));
                        moArr.push(contragent);
                        moIds.push(contragent.Id);
                    });
                    
                    uniqArr = Ext.Array.unique(moIds);
                    if (uniqArr.length === 1) {
                        contragentField.setValue(moArr[0]);
                    }

                    me.controller.basisDocIds = recordIds;
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        me.params = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);

        this.getStore('BaseStatement').on('beforeload', this.onBeforeLoad, this);
    },

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('baseStatementPanel'),
            requestTypeColumn = view.down('gridcolumn[dataIndex=RequestType]');

        if (requestTypeColumn) {
            requestTypeColumn.setVisible(true);
        }
        me.bindContext(view);
        me.application.deployView(view);
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            mainView = me.getMainView(),
            params = operation.params;

        if (me.params) {
            Ext.apply(operation.params, me.params);
        }
        
        params.showCloseInspections = mainView.down('#baseStatementGrid #cbShowCloseInspections').checked;
    },

    onMainViewAfterRender: function () {
        this.getStore('BaseStatement').load();
    }
});