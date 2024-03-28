Ext.define('B4.controller.OverhaulProposal', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.enums.TypeWork'
    ],

    proposalId: null,
    selectedProgram: null,
    objectCrId: null,


    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'OverhaulProposal',
        'OverhaulProposalWork'
    ],

    models: [
        'OverhaulProposal'
    ],

    views: [
        'overhaulpropose.EditWindow',
        'overhaulpropose.FilterPanel',
        'overhaulpropose.Grid',
        'overhaulpropose.MainPanel',
        'overhaulpropose.WorkGrid'
    ],

    mainView: 'overhaulpropose.MainPanel',
    mainViewSelector: 'overhaulproposeMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'overhaulproposemainpanel'
        },
        {
            ref: 'OverhaulProposeFilterPanel',
            selector: 'overhaulproposefilterpanel'
        },
        {
            ref: 'overhaulproposeeditwindow',
            selector: 'overhaulproposeeditwindow'
        }
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'overhaulproposeStateTransferAspect',
            gridSelector: 'overhaulproposegrid',
            stateType: 'ovrhl_proposal',
            menuSelector: 'overhaulproposestatecontextmenu'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.OverhaulProposal.Create', applyTo: 'b4addbutton', selector: 'overhaulproposegrid' },
                { name: 'GkhCr.OverhaulProposal.Register.Works.Create', applyTo: 'b4savebutton', selector: 'overhaulproposeeditwindow' },
                {
                    name: 'GkhCr.OverhaulProposal.Delete', applyTo: 'b4deletecolumn', selector: 'overhaulproposegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'overhaulproposeButtonExportAspect',
            gridSelector: 'overhaulproposegrid',
            buttonSelector: 'overhaulproposegrid #btnExport',
            controllerName: 'ObjectCr',
            actionName: 'ExportProposalList'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'overhaulproposestatepermissionaspect',
            permissions: [
                { name: 'GkhCr.OverhaulProposal.Edit', applyTo: 'b4savebutton', selector: 'overhaulproposeeditwindow' },
                { name: 'GkhCr.OverhaulProposal.Register.Works.Create', applyTo: 'b4addbutton', selector: 'overhaulproposeworkgrid' },
                {
                    name: 'GkhCr.OverhaulProposal.Register.Works.Delete', applyTo: 'b4deletecolumn', selector: 'overhaulproposeworkgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'overhaulProposalPrintAspect',
            buttonSelector: '#overhaulproposeEditWindow #btnPrint',
            codeForm: 'OverhaulProposal',
            getUserParams: function () {
                var param = { OverhaulProposalId: proposalId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
         {
             xtype: 'grideditwindowaspect',
             name: 'overhaulproposeGridWindowAspect',
             gridSelector: 'overhaulproposegrid',
             editFormSelector: 'overhaulproposeeditwindow',
             modelName: 'OverhaulProposal',
             storeName: 'OverhaulProposal',
             editWindowView: 'overhaulpropose.EditWindow',
             otherActions: function (actions) {
 
                 actions['overhaulproposeeditwindow #sfProgramCr'] = { 'change': { fn: this.onSelectProgram, scope: this } };
                 actions['overhaulproposeeditwindow #sfObjectCr'] = { 'beforeload': { fn: this.onBeforeLoadObjectCr, scope: this } };
             },  
             onSaveSuccess: function () {
                 // перекрыл метод потому что ненужно чтобы форма закрывалась                 

             },
             onSelectProgram: function (field, newValue) {
                 var form = this.getForm();
                 var thisrecord = form.getRecord();
                 if (thisrecord.data.Id == null || thisrecord.data.Id == "")
                {
                    if (newValue != null && newValue != "") {
                        selectedProgram = newValue.Id;
                        form.down('#sfObjectCr').setValue(null);
                        form.down('#sfObjectCr').setDisabled(false);
                    }
                    else {
                        form.down('#sfObjectCr').setDisabled(true);
                    }
                }
                else
                 {  
                     objectCrId = thisrecord.data.ObjectCr.Id;
                     form.down('#sfProgramCr').setDisabled(true);
                     form.down('#sfObjectCr').setDisabled(true);
                }
               
             },         

             onBeforeLoadObjectCr: function (store, operation) {
                 operation = operation || {};
                 operation.params = operation.params || {};

                 operation.params.programId = selectedProgram;
             },
            
             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     isChanges = false;
                     proposalId = rec.getId();
                     if (rec.getId()) {
                         var grid = form.down('overhaulproposeworkgrid'),
                             store = grid.getStore();
                         grid.setDisabled(false);
                         store.filter('proposalId', rec.getId());
                         form.down('#btnPrint').setDisabled(false);
                         me.controller.getAspect('overhaulProposalPrintAspect').loadReportStore();
                         asp.controller.getAspect('overhaulproposestatepermissionaspect').setPermissionsByRecord(rec); 

                     }
                     else
                     {
                         var grid = form.down('overhaulproposeworkgrid');
                         grid.setDisabled(true);
                         form.down('#btnPrint').setDisabled(true);
                     }
                 }
             }
        },
         {
             /* 
             Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
             По нажатию на Добавить открывается форма выбора предоставляемых документов.
             По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
             И сохранение предоставляемых документов
             */
             xtype: 'gkhinlinegridmultiselectwindowaspect',
             name: 'overhaulproposeWorksAspect',
             gridSelector: 'overhaulproposeworkgrid',
             storeName: 'OverhaulProposalWork',
             modelName: 'OverhaulProposalWork',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#CrObjectWorksByIdForSelectMultiSelectWindow',
             storeSelect: 'overhaulpropose.CrObjectWorksForSelect',
             storeSelected: 'overhaulpropose.CrObjectWorksForSelected',
             titleSelectWindow: 'Выбор работ',
             titleGridSelect: 'Работы',
             titleGridSelected: 'Выбранные работы',
             columnsGridSelect: [
                 { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 2 },
                 { header: 'Объем', xtype: 'gridcolumn', dataIndex: 'Volume', flex: 1 },
                 { header: 'Единица измерения', xtype: 'gridcolumn', dataIndex: 'UnitMeasure', flex: 1 },
                 { header: 'Сумма', xtype: 'gridcolumn', dataIndex: 'Sum', flex: 1 }
             ],
             columnsGridSelected: [
                 { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
             ],
             onBeforeLoad: function (store, operation) {              
                 operation.params.objectCrId = objectCrId;
             },
           
             listeners: {

                 getdata: function (asp, records) {
                     var recordIds = [];
                     records.each(function (rec, index) {
                         recordIds.push(rec.get('Id'));
                     });
                     if (recordIds[0] > 0) {
                         asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                         B4.Ajax.request(B4.Url.action('AddOverhaulProposalWork', 'ObjectCr', {
                             objectCrWorkIds: recordIds,
                             parentId: proposalId
                         })).next(function (response) {
                             asp.controller.unmask();
                             asp.controller.getStore(asp.storeName).load();
                             return true;
                         }).error(function () {
                             asp.controller.unmask();
                         });
                     } else {
                         Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                         return false;
                     }
                     return true;
                 }
             }
         },
         {
             /*
              * аспект взаимодействия триггер-поля фильтрации програм КР и таблицы объектов КР
              */
             xtype: 'gkhtriggerfieldmultiselectwindowaspect',
             name: 'overhaulproposefieldmultiselectwindowaspect',
             fieldSelector: 'overhaulproposefilterpanel #tfProgramCr',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#overhaulproposeSelectWindow',
             storeSelect: 'dict.ProgramCrObj',
             storeSelected: 'dict.ProgramCrForSelected',
             columnsGridSelect: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                 {
                     header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1,
                     filter: {
                         xtype: 'b4combobox',
                         operand: CondExpr.operands.eq,
                         storeAutoLoad: false,
                         hideLabel: true,
                         editable: false,
                         valueField: 'Name',
                         emptyItem: { Name: '-' },
                         url: '/Period/List'
                     }
                 }
             ],
             columnsGridSelected: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                 { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, sortable: false }
             ],
             titleSelectWindow: 'Выбор записи',
             titleGridSelect: 'Записи для отбора',
             titleGridSelected: 'Выбранные записи',
             listeners: {
                 getdata: function (asp, records) {
                     if (records.length > 0) {
                         var filterPanel = asp.controller.getOverhaulProposeFilterPanel(),
                             selprg = filterPanel.down('#tfProgramCr');

                         selprg.setValue(asp.parseRecord(records, asp.valueProperty));
                         selprg.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                     } else {
                         Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одну или несколько програм КР');
                         return false;
                     }
                     return true;
                 }
             }
         },
         {
             /*
              * аспект взаимодействия триггер-поля фильтрации мун. районов и таблицы объектов КР
              */
             xtype: 'gkhtriggerfieldmultiselectwindowaspect',
             name: 'overhaulproposeMunfieldmultiselectwindowaspect',
             fieldSelector: 'overhaulproposefilterpanel #tfMunicipality',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#municipalitySelectWindow',
             storeSelect: 'dict.MunicipalityForSelect',
             storeSelected: 'dict.MunicipalityForSelected',
             columnsGridSelect: [
                 {
                     header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                     filter: {
                         xtype: 'b4combobox',
                         operand: CondExpr.operands.eq,
                         storeAutoLoad: false,
                         hideLabel: true,
                         editable: false,
                         valueField: 'Name',
                         emptyItem: { Name: '-' },
                         url: '/Municipality/ListMoAreaWithoutPaging'
                     }
                 },
                 { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } }
             ],
             columnsGridSelected: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
             ],
             titleSelectWindow: 'Выбор записи',
             titleGridSelect: 'Записи для отбора',
             titleGridSelected: 'Выбранные записи',
             listeners: {
                 getdata: function (asp, records) {
                     if (records.length > 0) {
                         var filterPanel = asp.controller.getOverhaulProposeFilterPanel(),
                             municipality = filterPanel.down('#tfMunicipality');

                         municipality.setValue(asp.parseRecord(records, asp.valueProperty));
                         municipality.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                     } else {
                         Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одно или несколько муниципальных образования');
                         return false;
                     }
                     return true;
                 }
             }
         },
    ],


    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('overhaulproposemainpanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('OverhaulProposal').load();
    },

    init: function () {
        var me = this;
        this.getStore('OverhaulProposalWork').on('beforeload', this.onBeforeLoadSub, this);
        this.getStore('OverhaulProposal').on('beforeload', this.onBeforeLoad, this);
      //  this.getStore('OverhaulProposal').load();
        me.callParent(arguments);
    },

    onBeforeLoadSub: function (store, operation) {
        operation.params.proposalId = proposalId;
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getOverhaulProposeFilterPanel();
        if (filterPanel) {
            operation.params.programId = filterPanel.down('#tfProgramCr').getValue();
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
        }
    }
});