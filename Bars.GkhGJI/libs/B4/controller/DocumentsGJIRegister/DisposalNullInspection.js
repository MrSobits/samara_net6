Ext.define('B4.controller.documentsgjiregister.DisposalNullInspection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.DisposalNullInsp',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    models: ['Disposal', 'disposal.Annex'],

    stores: [
        'view.DisposalNullInspection',
        'documentsgjiregister.DisposalNullInspAnnex'
    ],

    views: [
        'documentsgjiregister.NullInspGrid',
        'documentsgjiregister.NullInspEditWindow',
        'documentsgjiregister.NullInspAnnexEditWindow',
        'documentsgjiregister.NullInspAnnexGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'documentsgjiregister.NullInspGrid',
    mainViewSelector: '#disposalNullInspectionGrid',

    aspects: [
        {
            xtype: 'disposalnullinspperm',
            editFormAspectName: 'dispnullinspaspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'disposalNullInspGridEditFormAspect',
            gridSelector: '#disposalNullInspectionGrid',
            storeName: 'view.DisposalNullInspection',
            modelName: 'Disposal',
            editRecord: function () { }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'disposalGjiButtonExportAspect',
            gridSelector: '#disposalNullInspectionGrid',
            buttonSelector: '#disposalNullInspectionGrid #btnExport',
            controllerName: 'Disposal',
            actionName: 'ExportNullInspection'
        },
        {
            xtype: 'statebuttonaspect',
            name: 'disposalNullInspStateButtonAspect',
            stateButtonSelector: '#disposalNullInspEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус

                    var aspGridEdit = asp.controller.getAspect('dispnullinspaspect');
                    aspGridEdit.updateGrid();

                    var model = this.controller.getModel('Disposal');
                    model.load(entityId, { success: function (rec) { aspGridEdit.setFormData(rec); } });

                    asp.setStateData(entityId, newState);
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'disposalNullInsStateTransferAspect',
            gridSelector: '#disposalNullInspectionGrid',
            stateType: 'gji_document_disp',
            menuSelector: 'disposalNullInspectionGridStateMenu'
        },
        {
            /*
            Аспект взаимодействия грида распоряжений без проверки и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'dispnullinspaspect',
            gridSelector: '#disposalNullInspectionGrid',
            editFormSelector: '#disposalNullInspEditWindow',
            storeName: 'view.DisposalNullInspection',
            modelName: 'Disposal',
            editWindowView: 'documentsgjiregister.NullInspEditWindow',
            onSaveSuccess: function (asp, newRec) {

                var id = newRec.getId();
                asp.setDisposalId(id);

                var model = this.controller.getModel('Disposal');
                model.load(id, {
                    success: function (rec) {
                        asp.controller.getAspect('disposalNullInspStateButtonAspect').setStateData(id, rec.get('State'));
                    }
                });
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    asp.setDisposalId(rec.getId());

                    var aspStateButton = this.controller.getAspect('disposalNullInspStateButtonAspect');

                    aspStateButton.setStateData(rec.get('Id'), rec.get('State'));

                    if (!rec.get('Id')) {
                        aspStateButton.getStateButton().setText('Статус');
                    }
                },
                getdata: function (asp, rec) {
                    rec.set('TypeDisposal', 30);
                }
            },
            setDisposalId: function (id) {

                this.controller.params = this.controller.params || {};

                this.controller.params.disposalId = id;

                this.getForm().down('.tabpanel').setActiveTab(0);

                var storeAnnex = this.controller.getStore('documentsgjiregister.DisposalNullInspAnnex');
                var gridAnnex = this.getForm().down('#disposalNullInspAnnexGrid');

                if (id) {
                    gridAnnex.setDisabled(false);
                    storeAnnex.load();
                } else {
                    gridAnnex.setDisabled(true);
                }
            }
        },
        {
            /*
            Аспект взаимодействия грида приложений распоряжения и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'disposalNullInspAnnexGridWindowAspect',
            gridSelector: '#disposalNullInspAnnexGrid',
            editFormSelector: '#disposalNullInspAnnexEditWindow',
            storeName: 'documentsgjiregister.DisposalNullInspAnnex',
            modelName: 'disposal.Annex',
            editWindowView: 'documentsgjiregister.NullInspAnnexEditWindow',
            listeners: {
                getdata: function (asp, rec) {
                    if (this.controller.params && !rec.getId()) {
                        rec.set('Disposal', asp.controller.params.disposalId);
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('view.DisposalNullInspection').on('beforeload', this.onBeforeLoad, this);
        this.getStore('documentsgjiregister.DisposalNullInspAnnex').on('beforeload', this.onBeforeLoadAnnex, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.DisposalNullInspection').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
        }
    },

    onBeforeLoadAnnex: function (store, operation) {
        if (this.params) {
            operation.params.documentId = this.params.disposalId;
        }
    }
});