Ext.define('B4.controller.VDGOViolators', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    models: [
        'vdgoviolators.VDGOViolators',
    ],
    stores: [
        'vdgoviolators.VDGOViolators'

    ],
    views: [
        'vdgoviolators.Grid',
        'vdgoviolators.EditWindow'
    ],
    mainView: 'vdgoviolators.Grid',
    mainViewSelector: '#vdgoviolatorsgrid',
    refs: [
        {
            ref: 'mainView',
            selector: '#vdgoviolatorsgrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'vdgoviolatorsGridAspect',
        //    storeName: 'vdgoviolators.VDGOViolators',
        //    modelName: 'vdgoviolators.VDGOViolators',
        //    gridSelector: '#vdgoviolatorsgrid'
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'vdgoviolatorsGridWindowAspect',
            gridSelector: 'vdgoviolatorsgrid',
            editFormSelector: '#vdgoviolatorsEditWindow',
            storeName: 'vdgoviolators.VDGOViolators',
            modelName: 'vdgoviolators.VDGOViolators',
            editWindowView: 'vdgoviolators.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: this.onRedtapeFlagChange, scope: this } };                
            },
            onRedtapeFlagChange: function (field, newValue) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0];

                var VDGOViolatorsRO = wnd.down('#VDGOViolatorsROId');
                //TODO! Переделать
                VDGOViolatorsRO.setDisabled(newValue > 2 ? false : true);
            },

        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'VDGOViolatorsButtonExportAspect',
            gridSelector: 'vdgoviolatorsgrid',
            buttonSelector: 'vdgoviolatorsgrid #btnExport',
            controllerName: 'VDGOViolators',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.VDGOViolators.Edit.Contragent_Edit', applyTo: '[name=Contragent]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.Address_Edit', applyTo: '[name=Address]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.MinOrgContragent_Edit', applyTo: '[name=MinOrgContragent]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.NotificationNumber_Edit', applyTo: '[name=NotificationNumber]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.NotificationDate_Edit', applyTo: '[name=NotificationDate]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.Description_Edit', applyTo: '[name=Description]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.Email_Edit', applyTo: '[name=Email]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.FIO_Edit', applyTo: '[name=FIO]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.PhoneNumber_Edit', applyTo: '[name=PhoneNumber]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.DateExecution_Edit', applyTo: '[name=DateExecution]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.MarkOfExecution_Edit', applyTo: '[name=MarkOfExecution]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Edit.File_Edit', applyTo: '[name=File]', selector: '#vdgoviolatorsEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Create', applyTo: '#addVDGOViolators', selector: '#vdgoviolatorsGrid',
                    applyBy: function (component, allowed) {
                        
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhGji.VDGOViolators.Delete', applyTo: '#deleteVDGOViolators', selector: '#vdgoviolatorsGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
            ]
        },
    ],
    
    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('vdgoviolatorsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('vdgoviolators.VDGOViolators').load();
    }
});