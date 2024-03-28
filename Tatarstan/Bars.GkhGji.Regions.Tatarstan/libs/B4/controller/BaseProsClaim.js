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

    models: [
        'BaseProsClaim'
    ],

    stores: [
        'BaseProsClaim'
    ],

    views: [
        'baseprosclaim.MainPanel',
        'baseprosclaim.AddWindow'
    ],

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
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.Create',
                    applyTo: 'b4addbutton',
                    selector: 'baseProsClaimPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'baseProsClaimPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'baseProsClaimPanel',
                    applyBy: function (component, allowed) {
                        var me = this;

                        if (!me.controller.params) {
                            me.controller.params = {};
                        }

                        if (allowed) {
                            me.controller.params.showCloseInspections = false;
                            me.controller.getStore('BaseProsClaim').load();
                            component.show();
                        } else {
                            me.controller.params.showCloseInspections = true;
                            me.controller.getStore('BaseProsClaim').load();
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.CheckBoxShowCloseInsp',
                    applyTo: '#cbShowCloseInspections',
                    selector: 'baseProsClaimPanel',
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
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: me.onChangeType, scope: me } };
                actions[me.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: me.onChangePerson, scope: me } };
                actions[me.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };

                actions['baseProsClaimPanel #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
                actions['baseProsClaimPanel #dfDateEnd'] = { 'change': { fn: me.onChangeDateEnd, scope: me } };
                actions['baseProsClaimPanel #updateGrid'] = { 'click': { fn: me.onUpdateGrid, scope: me } };
                actions['baseProsClaimPanel #cbShowCloseInspections'] = { 'change': { fn: me.onChangeCheckbox, scope: me } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseProsClaim');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                var me = this;

                if (me.controller.params) {
                    me.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                var me = this;

                if (me.controller.params) {
                    me.controller.params.dateEnd = newValue;
                }
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },
            onChangeType: function (field, newValue, oldValue) {
                var me = this;

                me.controller.params = me.controller.params || {};
                me.controller.params.typeJurOrg = newValue;
                me.getForm().down('#sfContragent').setValue(null);
                me.getForm().down('#tfPhysicalPerson').setValue(null);
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
                var me = this;

                me.controller.params.showCloseInspections = newValue;
                me.controller.getStore('BaseProsClaim').load();
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('BaseProsClaim').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
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
        var me = this;

        if (me.params) {
            operation.params.dateStart = me.params.dateStart;
            operation.params.dateEnd = me.params.dateEnd;
            operation.params.showCloseInspections = me.params.showCloseInspections;
        }
    }
});