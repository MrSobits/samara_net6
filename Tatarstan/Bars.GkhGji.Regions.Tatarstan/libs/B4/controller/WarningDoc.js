Ext.define('B4.controller.WarningDoc', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.permission.WarningDoc',

        'B4.form.SelectWindow',
        'B4.view.warningdoc.EditPanel',
        'B4.enums.TypeBase'
    ],

    models: [
        'WarningDoc',
        'dict.BaseDict'
    ],

    stores: [
        'WarningDoc',
        'dict.BaseDict'
    ],

    views: [
        'warningdoc.EditPanel',
        'warningdoc.ViolationsEditWindow',
        'warningdoc.AnnexEditWindow',
        'SelectWindow.MultiSelectWindow',
    ],

    mainView: 'warningdoc.EditPanel',
    mainViewSelector: '#warningdoceditpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    getCurrentContextKey: function() {
        return 'B4.controller.WarningDoc';
    },

    aspects: [
        {
            xtype: 'warningdocperm',
            editFormAspectName: 'editPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.Inspection.WarningInspection.Basis.Field.BaseWarning', applyTo: '[name=BaseWarning]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.Field.TakingDate', applyTo: '[name=TakingDate]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.Field.File', applyTo: '[name=File]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.Field.ResultText', applyTo: '[name=ResultText]', selector: 'warningdocrequisitepanel' },

                { name: 'GkhGji.Inspection.WarningInspection.Basis.Officials.Field.Autor', applyTo: '[name=Autor]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.Officials.Field.Executant', applyTo: '[name=Executant]', selector: 'warningdocrequisitepanel' },

                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutDate', applyTo: '[name=NcOutDate]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutNum', applyTo: '[name=NcOutNum]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutDateLatter', applyTo: '[name=NcOutDateLatter]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutNumLatter', applyTo: '[name=NcOutNumLatter]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutSent', applyTo: '[name=NcOutSent]', selector: 'warningdocrequisitepanel' },

                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInDate', applyTo: '[name=NcInDate]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInNum', applyTo: '[name=NcInNum]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInDateLatter', applyTo: '[name=NcInDateLatter]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInNumLatter', applyTo: '[name=NcInNumLatter]', selector: 'warningdocrequisitepanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInRecived', applyTo: '[name=NcInRecived]', selector: 'warningdocrequisitepanel' }
            ]
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'printAspect',
            buttonSelector: '#warningdoceditpanel #btnPrint',
            codeForm: 'WarningDoc',
            getUserParams: function(reportId) {
                var asp = this.controller,
                    view = asp.getMainView(),
                    param = {
                        DocumentId: asp.params.documentId,
                        TypeBase: asp.getContextValue(view, 'typeBase')
                    };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'warningDocStateButtonAspect',
            stateButtonSelector: '#warningdoceditpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('editPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'editPanelAspect',
            editPanelSelector: '#warningdoceditpanel',
            modelName: 'WarningDoc',
            enableRelatedObjectDeletingWarning: true,
            otherActions: function(actions) {
                var asp = this;
                actions[asp.editPanelSelector + ' b4selectfield[name=Autor]'] = {
                    'beforeload': { fn: asp.onBeforeLoadAuthorInspector, scope: asp }
                };
                actions[asp.editPanelSelector + ' b4selectfield[name=Executant]'] = {
                    'beforeload': { fn: asp.onBeforeLoadExecutantInspector, scope: asp }
                };
            },
            onBeforeLoadAuthorInspector: function(field, options, store) {
                options.params = options.params || {};
                options.params.headOnly = true;

                return true;
            },
            onBeforeLoadExecutantInspector: function(field, options, store) {
                options.params = options.params || {};
                options.params.notHeadOnly = true;

                return true;
            },
            disableButtons: function(value) {
                var buttons = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');

                Ext.each(buttons, function(b) {
                    b.setDisabled(value);
                });
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = asp.controller;
                me.params = asp.controller.params || {};

                var callbackUnMask = me.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                panel.down('tabpanel').setActiveTab(0);
                asp.disableButtons(false);

                me.getAspect('warningDocStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                var typeBase = panel.getRecord().getData().TypeBase,
                    view = me.getMainView(),
                    typeBases = [
                        B4.enums.TypeBase.ActionIsolated,
                        B4.enums.TypeBase.PlanJuridicalPerson,
                        B4.enums.TypeBase.ProsecutorsClaim,
                        B4.enums.TypeBase.DisposalHead,
                        B4.enums.TypeBase.CitizenStatement,
                        B4.enums.TypeBase.InspectionActionIsolated,
                        B4.enums.TypeBase.GjiWarning
                    ];

                me.setContextValue(view, 'typeBase', typeBase);

                if (typeBases.includes(typeBase)) {
                    asp.controller.getAspect('printAspect').loadReportStore();
                    panel.down('#btnPrint').enable();
                }
                else {
                    panel.down('#btnPrint').disable();
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'warningDocBasisAspect',
            gridSelector: '#warningdoceditpanel warningdocbasisgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#warningDocBasisSelectWindow',
            storeSelect: Ext.create('B4.store.dict.BaseDict', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'warningbasis'
                }
            }),
            storeSelected: 'dict.BaseDict',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 3, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор оснований для предостережения',
            titleGridSelect: 'Основание для предостережения',
            titleGridSelected: 'Выбранные основания для предостережения',
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddWarningsBasis', 'WarningDoc', {
                        warningIds: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function(response) {
                        asp.controller.unmask();
                        asp.updateGrid();
                        Ext.Msg.alert('Сохранение', 'Основания для предостережения сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'violationsAspect',
            gridSelector: '#warningdoceditpanel warningdocviolationsgrid',
            editFormSelector: '#warningdocViolationsEditWindow',
            modelName: 'warningdoc.Violations',
            editWindowView: 'warningdoc.ViolationsEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('WarningDoc', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function(asp, rec, form) {
                    var params = asp.controller.params,
                        realityObjectField = form.down('[name=RealityObject]'),
                        normDocField = form.down('[name=NormativeDoc]'),
                        violationsField = form.down('[name=Violations]');

                    realityObjectField.getStore()
                        .on('beforeload', function(store, operation) {
                            operation.params = operation.params || {};
                            Ext.apply(operation.params, {
                                inspectionId: params.inspectionId
                            })
                        });
                    violationsField.getStore()
                        .on('beforeload', function(store, operation) {
                            operation.params = operation.params || {};
                            Ext.apply(operation.params, {
                                NormativeDocId: normDocField.getValue() || -1
                            })
                        });
                    normDocField.on('change', function() {
                        violationsField.setValue(null);
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'annexAspect',
            gridSelector: '#warningdoceditpanel warningdocannexgrid',
            editFormSelector: '#warningdocAnnexEditWindow',
            modelName: 'warningdoc.Annex',
            editWindowView: 'warningdoc.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('WarningDoc', this.controller.params.documentId);
                    }
                }
            }
        }
    ],

    init: function() {
        var me = this;

        me.callParent(arguments);
    },

    onLaunch: function() {
        var me = this,
            editPanelAsp = me.getAspect('editPanelAspect'),
            basisGrid = editPanelAsp.getPanel().down('warningdocbasisgrid'),
            violationsGrid = editPanelAsp.getPanel().down('warningdocviolationsgrid'),
            annexGrid = editPanelAsp.getPanel().down('warningdocannexgrid');

        me.bindContext(me.getMainView());

        if (me.params) {
            me.subscribeGrid(basisGrid);
            me.subscribeGrid(violationsGrid);
            me.subscribeGrid(annexGrid);
            editPanelAsp.setData(me.params.documentId);
        }
    },

    subscribeGrid: function(grid) {
        var me = this,
            store;
        if (grid) {
            store = grid.getStore();
            store.on('beforeload', me.onBeforeLoad, me);
            store.load();
        }
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        if (me.params && me.params.documentId)
            operation.params.documentId = me.params.documentId;
    },
});