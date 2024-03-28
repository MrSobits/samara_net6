Ext.define('B4.controller.WarningInspection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.WarningInspection',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.enums.InspectionCreationBasis',
        'B4.enums.TypeBase',
        'B4.enums.InspectionCreationBasis'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['WarningInspection'],
    stores: ['WarningInspection'],
    views: [
        'warninginspection.MainPanel',
        'warninginspection.AddWindow',
        'warninginspection.Grid',
        'warninginspection.FilterPanel'
    ],

    mainView: 'warninginspection.MainPanel',
    mainViewSelector: 'warninginspectionmainpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'warninginspectionmainpanel'
        }
    ],

    disabledFields: [],
    aspects: [
        {
            xtype: 'warninginspectionperm',
            editFormAspectName: 'editPanelAspect'
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'permissionAspect',
            permissions: [
                {
                    name: 'GkhGji.Inspection.WarningInspection.Create',
                    applyTo: 'b4addbutton',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhGji.Inspection.WarningInspection.ShowCloseInspections',
                    applyTo: 'checkbox[name=ShowCloseInspections]',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhGji.Inspection.WarningInspection.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhGji.DocumentsGji.WarningInspection.Create',
                    applyTo: 'b4addbutton',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhGji.DocumentsGji.WarningInspection.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: "GkhGji.DocumentsGji.WarningInspection.Edit",
                    applyTo: 'b4editcolumn',
                    selector: 'warninginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                            //Записываем в отдельную переменную,разрешение на редактирование записей
                            this.canEditRecord = allowed
                        }
                    }
                }
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.Inspection.WarningInspection.Field.Date', applyTo: '[name=Date]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.InspectionNumber', applyTo: '[name=InspectionNumber]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.TypeJurPerson', applyTo: '[name=TypeJurPerson]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.PersonInspection', applyTo: '[name=PersonInspection]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.Contragent', applyTo: '[name=Contragent]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.PhysicalPerson', applyTo: '[name=PhysicalPerson]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumber', applyTo: '[name=RegistrationNumber]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate', applyTo: '[name=RegistrationNumberDate]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.CheckDayCount', applyTo: '[name=CheckDayCount]', selector: 'warninginspectioneditpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.CheckDate', applyTo: '[name=CheckDate]', selector: 'warninginspectioneditpanel' },

                { name: 'GkhGji.Inspection.WarningInspection.Field.Inspectors', applyTo: '[name=Inspectors]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.InspectionBasis', applyTo: '[name=InspectionBasis]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.SourceFormType', applyTo: '[name=SourceFormType]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Field.ControlType', applyTo: '[name=WarningInspectionControlType]', selector: 'warninginspectionmaininfotabpanel' },

                { name: 'GkhGji.Inspection.WarningInspection.Document.Field.DocumentName', applyTo: '[name=DocumentName]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Document.Field.DocumentNumber', applyTo: '[name=DocumentNumber]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Document.Field.DocumentDate', applyTo: '[name=DocumentDate]', selector: 'warninginspectionmaininfotabpanel' },
                { name: 'GkhGji.Inspection.WarningInspection.Document.Field.File', applyTo: '[name=File]', selector: 'warninginspectionmaininfotabpanel' }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'stateAspect',
            gridSelector: 'warninginspectiongrid',
            menuSelector: 'baseDispHeadStateMenu',
            stateType: 'gji_inspection'
        },
        {
            /*
            * аспект кнопки экспорта
            */
            xtype: 'b4buttondataexportaspect',
            name: 'exportAspect',
            gridSelector: 'warninginspectiongrid',
            buttonSelector: 'warninginspectiongrid button[action=Export]',
            controllerName: 'WarningInspection',
            actionName: 'Export'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'editPanelAspect',
            gridSelector: 'warninginspectiongrid',
            editFormSelector: 'warninginspectionaddwindow',
            modelName: 'WarningInspection',
            editWindowView: 'warninginspection.AddWindow',
            controllerEditName: 'B4.controller.warninginspection.Navigation',
            otherActions: function (actions) {
                var asp = this;
                actions[asp.editFormSelector + ' [name=InspectionBasis]'] = { 'change': { fn: asp.onChangeBaseWarningType, scope: asp } };
                actions[asp.editFormSelector + ' [name=TypeJurPerson]'] = { 'change': { fn: asp.onChangeType, scope: asp } };
                actions[asp.editFormSelector + ' [name=PersonInspection]'] = { 'change': { fn: asp.onChangePerson, scope: asp } };
                actions[asp.editFormSelector + ' [name=Contragent]'] = { 'beforeload': { fn: asp.onBeforeLoadContragent, scope: asp } };
                actions[asp.editFormSelector + ' [name=Head]'] = { 'beforeload': { fn: asp.onBeforeLoadHead, scope: asp } };

                actions['warninginspectionfiterpanel b4updatebutton'] = { 'click': { fn: asp.onUpdateGrid, scope: asp } };
                actions['warninginspectiongrid [name=ShowCloseInspections]'] = { 'change': { fn: asp.onChangeCheckbox, scope: asp } };
            },
            rowAction: function (grid, action, record) {
                const permAspect = this.controller.getAspect("permissionAspect");
                
                if (!grid || grid.isDestroyed) return;
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false && permAspect.canEditRecord) {
                    switch (action.toLowerCase()) {
                        case 'doubleclick':
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            saveRecord: function (rec) {
                rec.set('SourceFormType', B4.enums.TypeBase.DisposalHead);
                this.saveRecordHasUpload(rec);
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('WarningInspection');
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
                var me = this.controller;
                if (newValue) {
                    me.params.realityObjectId = newValue.Id;
                } else {
                    me.params.realityObjectId = null;
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
                var asp = this,
                    me = asp.controller;
                me.params = me.params || {};
                me.params.typeJurOrg = newValue;
                asp.getForm().down('[name=Contragent]').setValue(null);
                asp.getForm().down('[name=PhysicalPerson]').setValue(null);
            },
            onChangeBaseWarningType: function (field, newValue, oldValue) {
                var asp = this,
                    form = asp.getForm(),
                    appealCits = form.down('[name=AppealCits]');
                
                switch (newValue) {
                    case B4.enums.InspectionCreationBasis.AppealCitizens:
                        //Обращение граждан
                        appealCits.setDisabled(false);
                        appealCits.show();
                        return;
                    case B4.enums.InspectionCreationBasis.AnotherBasis: 
                        //Иное основание
                        appealCits.setValue(null);
                        appealCits.setDisabled(true);
                        appealCits.hide();
                        break;
                }
            },
            onChangePerson: function (field, newValue, oldValue) {
                var asp = this,
                    form = asp.getForm(),
                    sfContragent = form.down('[name=Contragent]'),
                    tfPhysicalPerson = form.down('[name=PhysicalPerson]'),
                    cbTypeJurPerson = form.down('[name=TypeJurPerson]'),
                    innField = form.down('[name=Inn]');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setValue(null);
                        cbTypeJurPerson.setValue(null);
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        innField.setDisabled(false);
                        innField.show();
                        return;
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
                tfPhysicalPerson.setValue(null);

                asp.controller.disabledFields['Contragent'] = sfContragent.disabled
                asp.controller.disabledFields['PhysicalPerson'] = tfPhysicalPerson.disabled
                asp.controller.disabledFields['TypeJurPerson'] = cbTypeJurPerson.disabled
            },
            onChangeCheckbox: function (field, newValue) {
                var me = this.controller;
                me.getStore('WarningInspection').load();
            }

        }
    ],

    init: function () {
        var me = this;

        me.getStore('WarningInspection').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('WarningInspection').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            grid = me.getMainView(),
            filterPanel = grid.up().down('warninginspectionfiterpanel'),
            params = {};

        params.dateStart = filterPanel.down('[name=DateStart]').getValue();
        params.dateEnd = filterPanel.down('[name=DateEnd]').getValue();
        params.realityObjectId = filterPanel.down('[name=RealityObject]').getValue();
        params.showCloseInspections = grid.down('[name=ShowCloseInspections]').getValue();

        Ext.apply(operation.params, params);
    }
});