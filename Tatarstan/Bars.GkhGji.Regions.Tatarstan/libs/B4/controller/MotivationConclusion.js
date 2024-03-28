Ext.define('B4.controller.MotivationConclusion', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.permission.MotivationConclusion',
        'B4.form.SelectWindow',
        'B4.view.motivationconclusion.EditPanel'
    ],

    models: [
        'MotivationConclusion'
    ],

    stores: [
        'MotivationConclusion'
    ],

    views: [
        'motivationconclusion.EditPanel',
        'motivationconclusion.AnnexEditWindow'
    ],

    mainView: 'motivationconclusion.EditPanel',
    mainViewSelector: '#motivationconclusioneditpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'motivationconclusionperm',
            editFormAspectName: 'editPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'printAspect',
            buttonSelector: '#motivationconclusioneditpanel #btnPrint',
            codeForm: 'MotivationConclusion',
            getUserParams: function(reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'stateButtonAspect',
            stateButtonSelector: '#motivationconclusioneditpanel #btnState',
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
            editPanelSelector: '#motivationconclusioneditpanel',
            modelName: 'MotivationConclusion',
            otherActions: function(actions) {
                var asp = this;
                actions[asp.editPanelSelector + ' b4selectfield[name=Autor]'] = {
                    'beforeload': { fn: asp.onBeforeLoadInspectorManager, scope: asp }
                };
            },
            onBeforeLoadInspectorManager: function(field, options, store) {
                options.params = options.params || {};
                options.params.headOnly = true;

                return true;
            },
            disableButtons: function(value) {
                var asp = this,
                    buttons = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');

                Ext.each(buttons, function(b) {
                    b.setDisabled(value);
                });
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = asp.controller,
                    id = rec.get('Id'),
                    violationStore = panel.down('grid[name=Violations]').getStore(),
                    title;
                me.params = asp.controller.params || {};

                var callbackUnMask = me.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title у вкладки
                title = 'Мотивировочное заключение';

                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('tabpanel').setActiveTab(0);
                asp.disableButtons(false);
                violationStore.on('beforeload', function (store, operation) {
                        Ext.apply(operation.params, {
                            documentId: rec.get('BaseDocumentId')
                        });
                    });

                this.controller.getAspect('stateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                me.getAspect('printAspect').loadReportStore();
                violationStore.load();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'annexAspect',
            gridSelector: '#motivationconclusioneditpanel motivationconclusionannexgrid',
            editFormSelector: '#motivationconclusionAnnexEditWindow',
            modelName: 'motivationconclusion.Annex',
            editWindowView: 'motivationconclusion.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MotivationConclusion', this.controller.params.documentId);
                    }
                }
            }
        },
    ],

    init: function() {
        var me = this;

        me.callParent(arguments);
    },

    onLaunch: function() {
        var me = this,
            editPanelAsp = me.getAspect('editPanelAspect'),
            annexGrid = editPanelAsp.getPanel().down('motivationconclusionannexgrid');

        if (me.params) {
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