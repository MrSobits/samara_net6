Ext.define('B4.controller.preventiveaction.Visit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.permission.preventiveaction.Visit',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet',
        'B4.form.ComboBox'
    ],

    models: [
        'preventiveaction.visit.Annex',
        'preventiveaction.visit.Visit',
        'preventiveaction.visit.InfoProvided'
    ],

    stores: [
        'preventiveaction.visit.Annex',
        'preventiveaction.visit.Visit',
        'preventiveaction.visit.Violation',
        'preventiveaction.visit.ViolationInfo',
        'preventiveaction.visit.InfoProvided',
        'preventiveaction.visit.RealityObjectForSelect'
    ],

    views: [
        'preventiveaction.visit.AnnexEditWindow',
        'preventiveaction.visit.ViolationInfoEditWindow',
        'preventiveaction.visit.EditPanel',
        'preventiveaction.visit.InfoProvidedGrid'
    ],

    mainView: 'preventiveaction.visit.EditPanel',
    mainViewSelector: '#visitEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
    },

    aspects: [
        {
            xtype: 'preventiveactionvisitpermissions',
            editFormAspectName: 'visitPanelAspect'
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'visitCreateButtonAspect',
            buttonSelector: '#visitEditPanel gjidocumentcreatebutton',
            containerSelector: '#visitEditPanel',
            typeDocument: 210, // VisitSheet
            onValidateUserParams: function (params) {
                if (params.ruleId === 'VisitSheetToMotivatedPresentationRule') {
                    return false;
                }
                return true;
            },
            setData: function (id) {
                var me = this;
                me.parentId = id;

                var btn = Ext.ComponentQuery.query(this.buttonSelector)[0];
                btn.setDisabled(true);

                if (btn && me.controller.params.hasViolThreats) {
                    me.rulesStore.load();
                }
            },
            onLoadStore: function (store) {
                var btn = Ext.ComponentQuery.query(this.buttonSelector)[0];
                btn.menu.removeAll();

                store.each(function (rec) {
                    btn.menu.add({
                        xtype: 'menuitem',
                        text: rec.data.Name,
                        textAlign: 'left',
                        ruleId: rec.data.Id,
                        actionUrl: rec.data.ActionUrl,
                        iconCls: 'icon-report'
                    });
                });

                btn.setDisabled(false);
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'visitPanelAspect',
            editPanelSelector: '#visitEditPanel',
            modelName: 'preventiveaction.visit.Visit',
            otherActions: function (actions) {
            },
            onAfterSetPanelData: function (asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};
                asp.controller.params.hasViolThreats = Boolean(rec.get('HasViolThreats'));
                
                this.disableButtons(false);

                asp.controller.getAspect('visitStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем кнопку Сформирвоать
                asp.controller.getAspect('visitCreateButtonAspect').setData(rec.get('Id'));

                var callbackUnMask = asp.controller.params?.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
            },
            disableButtons: function (value) {
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'infoProvidedGridAspect',
            storeName: 'preventiveaction.visit.InfoProvided',
            modelName: 'preventiveaction.visit.InfoProvided',
            gridSelector: 'infoprovidedgrid',
            saveButtonSelector: 'infoprovidedgrid #btnSave',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.set('VisitSheet', asp.controller.params.documentId);
                }
            }
        },
        {
            xtype: 'statebuttonaspect',
            name: 'visitStateButtonAspect',
            stateButtonSelector: '#visitEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    asp.controller.getAspect('visitPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#preventiveActionNavigationPanel').getComponent('preventiveActionMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'visitPrintAspect',
            buttonSelector: '#visitEditPanel #btnPrint',
            codeForm: 'PreventiveActionVisitSheet',
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'visitAnnexGridWindowAspect',
            gridSelector: '#visitAnnexGrid',
            editFormSelector: '#visitAnnexEditWindow',
            storeName: 'preventiveaction.visit.Annex',
            modelName: 'preventiveaction.visit.Annex',
            editWindowView: 'preventiveaction.visit.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('VisitSheet', asp.controller.params.documentId);
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'visitViolationInfoAspect',
            gridSelector: '#visitViolationInfoGrid',
            editFormSelector: '#visitViolationInfoEditWindow',
            storeName: 'preventiveaction.visit.ViolationInfo',
            modelName: 'preventiveaction.visit.ViolationInfo',
            editWindowView: 'preventiveaction.visit.ViolationInfoEditWindow',
            listeners: {
                beforesetformdata: function(asp, rec) {
                    var form = asp.getForm(),
                        grid = form.down('#visitViolationGrid');

                    form.currentViolInfoId = rec.getId();
                    grid.getStore().load();
                },
                beforerowaction: function(asp, grid, action, rec) {
                    switch (action.toLowerCase()) {
                        case 'doubleclick':
                            return false;
                    }

                    return true;
                },
                beforesave: function (asp, rec) {
                    rec.set('VisitSheet', asp.controller.params.documentId);
                }
            },
            onBeforeLoad: function(store, operation){
                operation.params.documentId = this.controller.params.documentId;
            },
            onSaveSuccess: function (asp, rec) {
                var editWindow = asp.getForm(),
                    grid = editWindow.down('#visitViolationGrid'),
                    violationStore = grid.getStore(),
                    modifiedRecs = violationStore.getModifiedRecords(),
                    removedRecs = violationStore.getRemovedRecords(),
                    violations = [],
                    deletedViolationIds = [];

                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    Ext.Array.each(modifiedRecs, function (record) {
                        var data = record.getData();
                        violations.push({
                            Id: data.Id || 0,
                            ViolationId: data.ViolationId,
                            IsThreatToLegalProtectedValues: data.IsThreatToLegalProtectedValues
                        });
                    });

                    Ext.Array.each(removedRecs, function (record) {
                        deletedViolationIds.push(record.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddViolations', 'VisitSheetViolation'),
                        params: {
                            violationInfoId: rec.getId(),
                            violations: Ext.encode(violations),
                            deletedViolationId: deletedViolationIds
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getGrid().getStore().load();
                        editWindow.close();
                    }).error(function (e) {
                        Ext.Msg.alert('Ошибка!', e.message);
                        asp.controller.unmask();
                    });
                }
                else {
                    editWindow.close();
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'visitViolationAspect',
            gridSelector: '#visitViolationGrid',
            storeName: 'preventiveaction.visit.Violation',
            modelName: 'preventiveaction.visit.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#visitViolationMultiSelectWindow',
            multiSelectWindowWidth: 1024,
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Пункт НПА', xtype: 'gridcolumn', dataIndex: 'NormDocNum', width: 80, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Группы нарушений', xtype: 'gridcolumn', dataIndex: 'FeatViol', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Мероприятия по устранению нарушений', xtype: 'gridcolumn', dataIndex: 'ActRemViol', flex: 0.75, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var store = asp.controller.getStore(asp.storeName);

                    records.each(function (rec) {
                        var violationModel = asp.controller.getModel('preventiveaction.visit.Violation').create();
                        violationModel.set('NormativeDocNames', rec.get('NormDocNum'));
                        violationModel.set('Name', rec.get('Name'));
                        violationModel.set('ViolationId', rec.getId());

                        store.insert(0, violationModel);
                    });
                    return true;
                }
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'visitSheetToMotivatedPresentationAspect',
            buttonSelector: '#visitEditPanel [ruleId=VisitSheetToMotivatedPresentationRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#visitSheetToMotivatedPresentationRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'preventiveaction.visit.RealityObjectForSelect',
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
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',
            onBeforeLoad: function (store, operation) {
                var me = this,
                    record = me.controller.getAspect('visitPanelAspect').getRecord();

                if (record) {
                    operation.params.documentId = record.get('Id');
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('visitCreateButtonAspect');
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        me.getStore('preventiveaction.visit.Violation').on('beforeload', me.onViolationBeforeLoad, me);
        me.getStore('preventiveaction.visit.ViolationInfo').on('beforeload', me.onBeforeStoreLoad, me);

        actions['#visitViolationInfoGrid'] = { 'afterrender': { fn: me.onGridRender, scope: me } };
        actions['#visitViolationInfoEditWindow #sfRealityObject'] = { 'beforeload': { fn: me.onRealityObjectBeforeLoad, scope: me } };

        me.control(actions);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('visitPanelAspect').setData(me.params.documentId);
            me.getAspect('visitPrintAspect').loadReportStore();

            [
                'preventiveaction.visit.InfoProvided',
                'preventiveaction.visit.Annex'
            ].forEach(function (storeName) {
                var store = me.getStore(storeName);

                store.on('beforeload', me.onBeforeStoreLoad, me);
                store.removeAll();
                store.load();
            });
        }
    },

    onGridRender: function (grid) {
        grid.getStore().load();
    },

    onBeforeStoreLoad: function (store, operation) {
        operation.params.Id = this.params.documentId;
    },

    onViolationBeforeLoad: function (store, operation) {
        var me = this,
            aspect = me.getAspect('visitViolationInfoAspect'),
            form = aspect.getForm();

        if (form.currentViolInfoId > 0)
            operation.params.objectId = form.currentViolInfoId;
    },

    onRealityObjectBeforeLoad: function (field, operation) {
        var me = this,
            panel = me.getMainComponent(),
            grid = panel.down('#visitViolationInfoGrid'),
            store = grid.getStore(),
            existsRoIds = [];

        store.each(function (rec) {
            var roId = rec.get('RealityObjectId');
            
            if (roId > 0)
            {
                existsRoIds.push(roId);
            }
        });

        if (!panel.visitMunicipalityId) {
            me.mask('Получение муниципального образования');
            Ext.Ajax.request({
                url: B4.Url.action('GetVisitSheetMunicipality', 'VisitSheet'),
                params: {
                    documentId: me.params.documentId
                },
                async: false,
                success: function (response) {
                    panel.visitMunicipalityId = Ext.decode(response.responseText);
                    me.unmask();
                },
                failure: function (response) {
                    var resp = Ext.decode(response.responseText);
                    Ext.Msg.alert('Ошибка', resp.message || 'Не удалось определить муниципальное образование');
                    me.unmask();
                    return false;
                }
            });
        }

        operation.params.municipalityId = panel.visitMunicipalityId;
        operation.params.existsRoIds = existsRoIds;
    }
});