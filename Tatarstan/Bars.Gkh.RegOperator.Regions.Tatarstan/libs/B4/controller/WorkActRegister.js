Ext.define('B4.controller.WorkActRegister', {
    /*
    * Контроллер реестра Актов выполненных работ
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.PerformedWorkActDetails',

        'B4.form.SelectField',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'objectcr.PerformedWorkAct',
        'objectcr.PerformedWorkActRegister',
        'objectcr.performedworkact.Record',
        'ObjectCr'
    ],

    stores: [
        'WorkActRegister',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'RealityObject',
        'workactregister.Details'
    ],
    
    views: ['workactregister.Panel', 

        'workactregister.Grid',
        'workactregister.FilterPanel',
        'workactregister.AddWindow',
        'workactregister.DetailsPanel',

        'SelectWindow.MultiSelectWindow',
        'realityobj.Grid',
        
        'dict.period.Grid'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'workactregister.Panel',
    mainViewSelector: 'workActRegisterPanel',

    refs: [
      {
          ref: 'WorkActFilterPanel',
          selector: '#workActFilterPanel'
      },
      {
          ref: 'WorkActRegisterAddWindow',
          selector: '#workActRegisterAddWindow'
      },
      {
          ref: 'mainView',
          selector: 'workActRegisterPanel'
      }
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'WorkActRegisterStateTransferAspect',
            gridSelector: 'workActRegisterPanel workactreggrid',
            stateType: 'cr_obj_performed_work_act',
            menuSelector: 'workActRegisterGridStateMenu'
        },
         {
             xtype: 'b4buttondataexportaspect',
             name: 'PerformedWorkActButtonExportAspect',
             gridSelector: 'workactreggrid',
             buttonSelector: 'workactreggrid #btnPerformedWorkActExport',
             controllerName: 'PerformedWorkAct',
             actionName: 'Export'
         },
         {
             /*
             аспект взаимодействия триггер-поля фильтрации мун. образований и таблицы объектов КР
             */
             xtype: 'gkhtriggerfieldmultiselectwindowaspect',
             name: 'workactfieldmultiselectwindowaspect',
             fieldSelector: '#workActFilterPanel #tfMunicipality',
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
                         url: '/Municipality/ListWithoutPaging'
                     }
                 },
                 { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
             ],
             columnsGridSelected: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
             ],
             titleSelectWindow: 'Выбор записи',
             titleGridSelect: 'Записи для отбора',
             titleGridSelected: 'Выбранная запись',
             listeners: {
                 getdata: function (asp, records) {
                     var str = '';

                     records.each(function (rec) {
                         if (str)
                             str += ';';
                         str += rec.getId();
                     });

                     this.controller.params.municipalities = str;
                 }
             },
             onTrigger2Click: function () {
                 this.setValue(null);
                 this.updateDisplayedText();
                 this.controller.params.municipalities = null;
             }
         },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'workActRegisterGridEditWindowAspect',
            gridSelector: 'workactreggrid',
            storeName: 'WorkActRegister',
            modelName: 'objectcr.PerformedWorkActRegister',
            editFormSelector: '#workActRegisterAddWindow',
            editWindowView: 'workactregister.AddWindow',
            listeners: {
                'beforesetformdata': function (_, __, form) {
                    var manual = Gkh.config.GkhCr.General.TypeWorkActNumeration == 10,
                        num = form.down('[name=DocumentNum]');

                    num.setVisible(manual);
                    num.setDisabled(!manual);
                }
            },
            otherActions: function (actions) {
                actions['workActRegisterPanel' + ' #sfProgram'] = { 'change': { fn: this.onChangeProgram, scope: this } };
                actions['workActRegisterPanel' + ' #tfMunicipality'] = { 'triggerClear': { fn: this.onClearMunicipalities, scope: this } };
                actions['workActRegisterPanel' + ' #sfRealityObj'] = { 'change': { fn: this.onChangeRealityObj, scope: this } };
                actions['#workActFilterPanel' + ' #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#workActFilterPanel #sfRealityObj'] = { 'beforeload': { fn: this.onBeforeLoadRealObj, scope: this } };
                actions['#workActFilterPanel #sfProgram'] = { 'beforeload': { fn: this.onBeforeLoadProgram, scope: this } };
                actions['#workActRegisterAddWindow' + ' #sfProgramCr'] = {
                    'beforeload': { fn: this.onBeforeLoadProgram, scope: this },
                    'change': { fn: this.onChangeProgramCr, scope: this }
                };
                actions['#workActRegisterAddWindow' + ' #sfObjectCr'] = {
                    'beforeload': { fn: this.onBeforeLoadWorkActRegister, scope: this },
                    'change': { fn: this.onChangeAddWindowObjectCr, scope: this }
                };
                actions['#workActRegisterAddWindow' + ' #sfTypeWorkCr'] = { 'beforeload': { fn: this.onBeforeLoadTypeWorkCr, scope: this } };
            },
            
            onSaveSuccess: function (aspect, rec) {
                var me = aspect;
                aspect.getForm().close();
                var objCrId = !Ext.isEmpty(rec.get('ObjectCr').Id) ? rec.get('ObjectCr').Id : rec.get('ObjectCr');

                if (objCrId) {
                    var model = this.controller.getModel('ObjectCr');

                    var params = new model({ Id: objCrId, RealityObjName: rec.get('Address') });
                    params.childController = 'B4.controller.objectcr.PerformedWorkActRo';
                    params.record = rec;

                    var portal = this.controller.getController('PortalController');
                    //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                    if (!me.controller.hideMask) {
                        me.controller.hideMask = function () { me.controller.unmask(); };
                    }
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    portal.loadController('B4.controller.objectcr.Navigation', params, portal.containerSelector, me.controller.hideMask);
                }
            },
            editRecord: function (rec) {
                var me = this;

                if (!Ext.isEmpty(rec)) {
                    var objCrId = !Ext.isEmpty(rec.get('ObjectCrId').Id) ? rec.get('ObjectCrId').Id : rec.get('ObjectCrId');

                    if (objCrId) {
                        Ext.History.add('objectcredit/' + objCrId + '/performedworkact');
                    }
                }
                else {
                    var model = me.controller.getModel(me.modelName);
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('WorkActRegister');
                str.currentPage = 1;
                str.load();
            },
            onClearMunicipalities: function () {
                if (this.controller.params) {
                    this.controller.params.municipalities = null;
                }
            },
            onChangeRealityObj: function (field, newValue) {
                if (this.controller.params && newValue) {
                    this.controller.params.realityObjId = newValue.Id;
                } else {
                    this.controller.params.realityObjId = null;
                }
            },
            onBeforeLoadRealObj: function (field, options) {
                options.params = {};
                options.params.municipalities = this.controller.params.municipalities;
            },
            onBeforeLoadProgram: function (field, options) {
                options.params = {};
                options.params.onlyFull = true;
            },
            onChangeProgramCr: function (field, newValue) {
                var sfObjectCr = field.up('#workActRegisterAddWindow').down('#sfObjectCr');
                if (newValue) {
                    if (this.controller.params) {
                        this.controller.params.programId = newValue.Id;
                        sfObjectCr.setDisabled(false);
                    }
                } else {
                    sfObjectCr.setDisabled(true);
                }
            },
            onChangeAddWindowObjectCr: function (field, newValue) {
                var sfTypeWorkCr = field.up('#workActRegisterAddWindow').down('#sfTypeWorkCr'); 
                if (newValue) {
                    this.controller.objectCrId = newValue.Id;
                    sfTypeWorkCr.setDisabled(false);
                } else {
                    sfTypeWorkCr.setValue(null);
                    sfTypeWorkCr.setDisabled(true);
                }
            },
            onBeforeLoadWorkActRegister: function (field, options) {
                options.params = {};
                options.params.programId = this.controller.params.programId;
            },
            onBeforeLoadTypeWorkCr: function (field, options) {
                options.params = {};
                options.params.objectCrId = this.controller.objectCrId;
            },
            onChangeProgram: function (field, newValue) {
                if (this.controller.params && newValue) {
                    this.controller.params.programFilterId = newValue.Id;
                } else {
                    this.controller.params.programFilterId = null;
                }
            }
        },
        {
            xtype: 'performedworkactdetails',
            name: 'performedWorkActDetailsAspect',
            showDetailsButtonSelector: 'workactreggrid button[action=ShowDetails]',
            listeners: {
                aftersetformdata: function(asp, detailsPanel) {
                    var workActPanel = Ext.ComponentQuery.query('workActRegisterPanel')[0],
                        filterPanel = workActPanel.down('workactregfilterpnl'),
                        programField = detailsPanel.down('[name=Program]'),
                        municipalityField = detailsPanel.down('[name=Municipality]'),
                        addressField = detailsPanel.down('[name=Address]');

                    programField.setValue(filterPanel.down('[name=Program]').getRawValue() || '-');
                    municipalityField.setValue(filterPanel.down('[name=Municipality]').getRawValue() || '-');
                    addressField.setValue(filterPanel.down('[name=Address]').getRawValue() || '-');
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                if (me.controller.params) {
                    operation.params.programFilterId = me.controller.params.programFilterId;
                    operation.params.realityObjId = me.controller.params.realityObjId;
                    operation.params.municipalities = me.controller.params.municipalities;
                }
            },
            getParams: function () {
                var me = this,
                    params = {};

                if (me.controller.params) {
                    params.programFilterId = me.controller.params.programFilterId;
                    params.realityObjId = me.controller.params.realityObjId;
                    params.municipalities = me.controller.params.municipalities;
                }

                return params;
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('WorkActRegister').on('beforeload', me.onBeforeLoad, me);
        me.control({
            'workactregfilterpnl [name=Program]': { 'change': { fn: me.onChangeProgram } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('workActRegisterPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.params = this.params || {};
        this.getStore('WorkActRegister').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.programFilterId = this.params.programFilterId;
            operation.params.realityObjId = this.params.realityObjId;
            operation.params.municipalities = this.params.municipalities;
        }
    },

    onChangeProgram: function (field, newValue) {
        var grid = field.up('workActRegisterPanel').down('gridpanel');
        var btnShowDetails = grid.down('button[action=ShowDetails]');

        btnShowDetails.setDisabled(!newValue);
    }

});