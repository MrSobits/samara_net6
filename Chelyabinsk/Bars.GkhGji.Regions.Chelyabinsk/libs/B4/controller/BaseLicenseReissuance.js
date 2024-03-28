﻿Ext.define('B4.controller.BaseLicenseReissuance', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['BaseLicenseReissuance'],
    stores: [
        'BaseLicenseReissuance'
    ],
    views: [
        'baselicensereissuance.MainPanel',
        'baselicensereissuance.Grid',
        'baselicensereissuance.AddWindow',
        'baselicensereissuance.FilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'baselicensereissuance.MainPanel',
    mainViewSelector: 'baselicensereissuancepanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baselicensereissuancepanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseLicApplicants.Create', applyTo: 'b4addbutton', selector: 'baselicensereissuancegrid' },
                { name: 'GkhGji.Inspection.BaseLicApplicants.Delete', applyTo: 'b4deletecolumn', selector: 'baselicensereissuancegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {  name: 'GkhGji.Inspection.BaseLicApplicants.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: 'baselicensereissuancegrid',
                   applyBy: function (component, allowed) {
                       if (allowed) {
                           this.controller.params.showCloseInspections = false;
                           this.controller.getStore('BaseLicenseReissuance').load();
                           component.show();
                       } else {
                           this.controller.params.showCloseInspections = true;
                           this.controller.getStore('BaseLicenseReissuance').load();
                           component.hide();
                       }
                   }
               }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseLicenseApplicantsStateTransferAspect',
            gridSelector: 'baselicenseapplicantsgrid',
            menuSelector: 'baseLicenseApplicantsStateMenu',
            stateType: 'gji_inspection'
        },
        //{
        //    xtype: 'b4buttondataexportaspect',
        //    name: 'baseLicenseApplicantsExportAspect',
        //    gridSelector: 'baselicenseapplicantsgrid',
        //    buttonSelector: 'baselicenseapplicantsgrid button[action=Export]',
        //    controllerName: 'BaseLicenseApplicants',
        //    actionName: 'Export'
        //},
        {
            xtype: 'gkhgrideditformaspect',
            name: 'baseLicenseReissuanceGridWindowAspect',
            gridSelector: 'baselicensereissuancegrid',
            editFormSelector: 'baselicensereissuanceaddwin',
            storeName: 'BaseLicenseReissuance',
            modelName: 'BaseLicenseReissuance',
            editWindowView: 'baselicensereissuance.AddWindow',
            controllerEditName: 'B4.controller.baselicensereissuance.Navigation',
            otherActions: function (actions) {
                var me = this;

                actions['baselicensereissuancefilterpanel [name=Contragent]'] = { 'change': { fn: me.onChangeContragent, scope: me } };
                actions['baselicensereissuancefilterpanel [buttonType=updateGrid]'] = { 'click': { fn: me.onUpdateGrid, scope: me } };

                actions['baselicensereissuancegrid #cbShowCloseInspections'] = { 'change': { fn: me.onChangeCheckbox, scope: me } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseLicenseReissuance');
                str.currentPage = 1;
                str.load();
            },
            onChangeContragent: function (field, newValue, oldValue) {
                var me = this;

                if (!me.controller.params) {
                    me.controller.params = {};
                }

                if (newValue) {
                    me.controller.params.contragentId = newValue.Id;
                } else {
                    me.controller.params.contragentId = null;
                }
            },          
            onChangeCheckbox: function (field, newValue) {
                var me = this;

                me.controller.params.showCloseInspections = newValue;
                me.controller.getStore('BaseLicenseReissuance').load();
            }
        }
    ],

    init: function () {
        var me = this;

        me.params = {};
        me.getStore('BaseLicenseReissuance').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('baselicensereissuancepanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('BaseLicenseApplicants').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params) {
            operation.params.subjIds = me.params.subjIds;
            operation.params.contragentId = me.params.contragentId;
            operation.params.showCloseInspections = me.params.showCloseInspections;
        }
    }
});