Ext.define('B4.controller.BaseDispHead', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.BaseDispHead',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['BaseDispHead'],
    stores: ['BaseDispHead'],
    views: [
        'basedisphead.MainPanel',
        'basedisphead.AddWindow',
        'basedisphead.Grid',
        'basedisphead.FilterPanel'
    ],

    mainView: 'basedisphead.MainPanel',
    mainViewSelector: 'baseDispHeadPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseDispHeadPanel'
        }
    ],

    aspects: [
         {
             xtype: 'gkhpermissionaspect',
             permissions: [
                { name: 'GkhGji.Inspection.BaseDispHead.Create', applyTo: 'b4addbutton', selector: '#baseDispHeadGrid' },
                { name: 'GkhGji.Inspection.BaseDispHead.Edit', applyTo: 'b4savebutton', selector: '#baseDispHeadEditPanel' },
                { name: 'GkhGji.Inspection.BaseDispHead.Delete', applyTo: 'b4deletecolumn', selector: '#baseDispHeadGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
               { name: 'GkhGji.Inspection.BaseDispHead.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: '#baseDispHeadGrid',
                   applyBy: function (component, allowed) {
                       if (allowed) {
                           if (this.controller.params) {
                               this.controller.params.showCloseInspections = false;
                               this.controller.getStore('BaseDispHead').load();
                           }
                           component.show();
                       } else {
                           if (this.controller.params) {
                               this.controller.params.showCloseInspections = true;
                               this.controller.getStore('BaseDispHead').load();
                           }
                           component.hide();
                       }
                   }
               }
             ]
         },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseDispHeadStateTransferAspect',
            gridSelector: '#baseDispHeadGrid',
            menuSelector: 'baseDispHeadStateMenu',
            stateType: 'gji_inspection'
        },
        {
            /*
            * аспект кнопки экспорта
            */
            xtype: 'b4buttondataexportaspect',
            name: 'baseDispHeadButtonExportAspect',
            gridSelector: '#baseDispHeadGrid',
            buttonSelector: '#baseDispHeadGrid #btnExport',
            controllerName: 'BaseDispHead',
            actionName: 'Export'
        },
        {
            /**
            аспект взаимодействия таблицы проверок по распоряжению руководства формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseDispHeadGridWindowAspect',
            gridSelector: '#baseDispHeadGrid',
            editFormSelector: '#baseDispHeadAddWindow',
            storeName: 'BaseDispHead',
            modelName: 'BaseDispHead',
            editWindowView: 'basedisphead.AddWindow',
            controllerEditName: 'B4.controller.basedisphead.Navigation',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions[this.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editFormSelector + ' #sflHead'] = { 'beforeload': { fn: this.onBeforeLoadHead, scope: this } };

                actions['#baseDispHeadFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#baseDispHeadFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#baseDispHeadFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#baseDispHeadFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#baseDispHeadGrid #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            saveRecord: function (rec) {
                this.saveRecordHasUpload(rec);
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseDispHead');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateStart: function (field, newValue) {
                this.controller.params.dateStart = newValue;
            },
            onChangeDateEnd: function (field, newValue) {
                this.controller.params.dateEnd = newValue;
            },
            onChangeRealityObject: function (field, newValue) {
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
            onBeforeLoadHead: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.headOnly = true;
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
                    cbTypeJurPerson = form.down('#cbTypeJurPerson');
                sfContragent.setValue(null);
                tfPhysicalPerson.setValue(null);
                cbTypeJurPerson.setValue(10);
                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        break;
                    case 20://организацияы
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                    case 30://должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                    case 40://жилой дом
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(true);
                        break;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseInspections = newValue;
                this.controller.getStore('BaseDispHead').load();
            }
        }
    ],

    init: function () {
        this.getStore('BaseDispHead').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('baseDispHeadPanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date;
        me.params.realityObjectId = null;
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.realityObjectId = this.params.realityObjectId;
            operation.params.showCloseInspections = this.params.showCloseInspections;
        }
    }
});