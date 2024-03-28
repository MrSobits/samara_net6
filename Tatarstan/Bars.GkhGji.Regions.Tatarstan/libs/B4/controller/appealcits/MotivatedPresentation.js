Ext.define('B4.controller.appealcits.MotivatedPresentation', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.permission.appealcits.MotivatedPresentation',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'appealcits.MotivatedPresentation',
        'appealcits.motivatedpresentation.Annex'
    ],

    stores: [
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'appealcits.motivatedpresentation.Annex'
    ],

    views: [
        'appealcits.motivatedpresentation.AnnexEditWindow',
        'appealcits.motivatedpresentation.EditPanel'
    ],

    mainView: 'appealcits.motivatedpresentation.EditPanel',
    mainViewSelector: '#motivatedPresentationAppealCitsEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'motivatedpresentationappealcitsperm',
            editFormAspectName: 'motivatedPresentationAppealCitsEditPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.DocumentDate', applyTo: '[name=DocumentDate]', selector: '#motivatedPresentationAppealCitsEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.DocumentNumber', applyTo: '[name=DocumentNumber]', selector: '#motivatedPresentationAppealCitsEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.PresentationType', applyTo: '[name=PresentationType]', selector: '#motivatedPresentationAppealCitsEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.Inspectors', applyTo: '#tfInspectors', selector: '#motivatedPresentationAppealCitsEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.Official', applyTo: '[name=Official]', selector: '#motivatedPresentationAppealCitsEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.ResultType', applyTo: '[name=ResultType]', selector: '#motivatedPresentationAppealCitsEditPanel' }
            ]
        },
        {
            xtype: 'statebuttonaspect',
            name: 'motivatedPresentationAppealCitsStateButtonAspect',
            stateButtonSelector: '#motivatedPresentationAppealCitsEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('motivatedPresentationAppealCitsEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#actionIsolatedNavigationPanel').getComponent('actionisolatedMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'motivatedPresentationAppealCitsEditPanelAspect',
            editPanelSelector: '#motivatedPresentationAppealCitsEditPanel',
            modelName: 'appealcits.MotivatedPresentation',
            listeners: {
                beforesave: function (asp, rec) {
                    var panel = asp.getPanel(),
                        tfInspectors = panel.down('#tfInspectors');

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'DocumentGjiInspector'),
                        method: 'POST',
                        params: {
                            inspectorIds: tfInspectors.getValue(),
                            documentId: asp.controller.params.documentId,
                            clearingEnabled: true
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            },
            otherActions: function(actions) {
            },
            onSaveSuccess: function (asp, rec) {
                //исключение изменения заголовка панели
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                var callbackUnMask = asp.controller.params?.callbackUnMask;

                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                panel.down('#motivatedPresentationTabPanel').setActiveTab(0);

                this.disableButtons(false);
                this.updateFieldValue(panel, rec);

                this.controller.getAspect('motivatedPresentationAppealCitsStateButtonAspect').setStateData(rec.getId(), rec.get('State'));
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            updateFieldValue: function (panel, rec) {
                var fieldInspectors = panel.down('#tfInspectors');

                fieldInspectors.updateDisplayedText(rec.data.Inspectors);
                fieldInspectors.setValue(rec.data.InspectorIds);
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'motivatedPresentationAppealCitsInspectorMultiSelectWindowAspect',
            fieldSelector: '#motivatedPresentationAppealCitsEditPanel #tfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#motivatedPresentationInspectorsSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'motivatedPresentationAppealCitsAnnexGridWindowAspect',
            gridSelector: '#motivatedPresentationAppealCitsAnnexGrid',
            editFormSelector: '#motivatedPresentationAppealCitsAnnexEditWindow',
            storeName: 'appealcits.motivatedpresentation.Annex',
            modelName: 'appealcits.motivatedpresentation.Annex',
            editWindowView: 'appealcits.motivatedpresentation.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MotivatedPresentation', asp.controller.params.documentId);
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('appealcits.motivatedpresentation.Annex').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#motivatedPresentationAppealCitsAnnexGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('motivatedPresentationAppealCitsEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});