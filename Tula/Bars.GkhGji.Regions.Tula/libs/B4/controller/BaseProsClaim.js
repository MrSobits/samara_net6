Ext.define('B4.controller.BaseProsClaim', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.BaseProsClaim'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['BaseProsClaim'],
    stores: ['BaseProsClaim'],
    views: [
        'baseprosclaim.MainPanel',
        'baseprosclaim.AddWindow',
        'baseprosclaim.Grid',
        'baseprosclaim.FilterPanel'
    ],

    mainView: 'baseprosclaim.MainPanel',
    mainViewSelector: 'baseProsClaimPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseProsClaimPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseProsClaim.Create', applyTo: 'b4addbutton', selector: 'baseProsClaimPanel' },
                { name: 'GkhGji.Inspection.BaseProsClaim.Edit', applyTo: 'b4savebutton', selector: 'baseProsClaimPanel' },
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.Delete', applyTo: 'b4deletecolumn', selector: 'baseProsClaimPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            this.controller.params.showCloseInspections = false;
                            this.controller.getStore('BaseProsClaim').load();
                            component.show();
                        } else {
                            this.controller.params.showCloseInspections = true;
                            this.controller.getStore('BaseProsClaim').load();
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: 'baseProsClaimPanel',
                   applyBy: function (component, allowed) {
                       if (allowed) component.show();
                       else component.hide();
                   }
               }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseProsClaimStateTransferAspect',
            gridSelector: 'baseProsClaimPanel',
            menuSelector: 'baseProsClaimStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'baseProsClaimButtonExportAspect',
            gridSelector: 'baseProsClaimPanel',
            buttonSelector: 'baseProsClaimPanel #btnExport',
            controllerName: 'BaseProsClaim',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок по требованию прокуратуры , формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseProsClaimGridWindowAspect',
            gridSelector: 'baseProsClaimPanel',
            editFormSelector: '#baseProsClaimAddWindow',
            storeName: 'BaseProsClaim',
            modelName: 'BaseProsClaim',
            editWindowView: 'baseprosclaim.AddWindow',
            controllerEditName: 'B4.controller.baseprosclaim.Navigation',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions[this.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };

                actions['baseProsClaimPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['baseProsClaimPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['baseProsClaimPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['baseProsClaimPanel #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseProsClaim');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
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
                this.controller.getStore('BaseProsClaim').load();
            }
        }
    ],

    init: function () {
        this.getStore('BaseProsClaim').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('baseProsClaimPanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date;
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.showCloseInspections = this.params.showCloseInspections;
        }
    }
});