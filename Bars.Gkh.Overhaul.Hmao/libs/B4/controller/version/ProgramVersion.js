Ext.define('B4.controller.version.ProgramVersion', {
    extend: 'B4.base.Controller',
    alias: 'widget.programversioncontroller',
    requires: [
        'B4.form.SelectField',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhMultiSelectWindow',
<<<<<<< HEAD
        'B4.view.version.ActualizeSubProgramByFilters',
        'B4.view.version.YearFromWindow',
        'B4.view.version.ActualizePeriodWindow',
        'B4.view.version.ActualizeByFiltersAddGrid', 
        'B4.view.actualisedpkr.EditWindow',       
        'B4.enums.TypeUsage',
        'B4.aspects.StateContextMenu',
        'B4.controller.SubDPKR',
        'B4.enums.ChangeBasisType',
=======
        'B4.enums.TypeUsage',
        'B4.form.FileField',
        'B4.Url'
>>>>>>> net6
    ],

    versionProgram: null,
    versionRecord:null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['version.ProgramVersion'],

    stores:[
        'version.ForMassChangeSelect',
        'version.ForMassChangeSelected',
        'version.ActualizeByFiltersAdd',
    ],

    views: [
        'version.ActualizeDelWindow',
        'version.Panel',
        'version.Grid',
        'version.EditPanel',
        'version.OrderWindow',
        'version.ImportWindow',
        'version.RecordsEditWindow',
        'actualisedpkr.EditWindow',
        'version.RecordsGrid',
        'version.PublicationGrid',
        'version.SubsidyRecordGrid',
        'version.ActualizationLogGrid',
        'version.CorrectionGrid',
        'version.CopyWindow',
        'version.MassYearChangeWindow',
        'version.MakeKPKR',
        'subkpkr.MakeSubKPKR',
        'version.ActualizeProgramCrWindow',
        'version.ActualizeByFilters',
        'version.ActualizeByFiltersAddGrid',
        'version.ActualizeByFiltersDeleteGrid',
        'version.Add',
        'SelectWindow.MultiSelectWindow',
    ],

    refs: [
        { ref: 'mainPanel', selector: 'programversionpanel' },
        { ref: 'grid', selector: 'programversiongrid' },
        { ref: 'editPanel', selector: 'programversioneditpanel' },
        { ref: 'versionField', selector: 'programversioneditpanel b4selectfield[name="Version"]' },
        { ref: 'muField', selector: 'programversionpanel combobox[name=Municipality]' },
        { ref: 'versionRecordsEditWin', selector: 'versionrecordseditwin' },
        { ref: 'versionRecordsGrid', selector: 'versionrecordsgrid' },
        { ref: 'EditWindow', selector: '#actualisedpkrEditWindow' },
        { ref: 'ActualizeByFiltersAddGrid', selector: 'actualizedbyfilteraddgrid' },
        { ref: 'ActualizeByFiltersDeleteGrid', selector: 'actualizedbyfilterdeletegrid' },
        { ref: 'ActualizeSubProgramByFiltersAddGrid', selector: 'actualizesubprogrambyfilteraddgrid' },
        { ref: 'ActualizeSubProgramByFiltersDeleteGrid', selector: 'actualizesubprogrambyfilterdeletegrid' },
        { ref: 'copyWindow', selector: 'versioncopywindow' },
        { ref: 'orderWindow', selector: 'versionorderyearwindow' },
        { ref: 'subKPKRwin', selector: 'versmakesubkpkrwin' },
        { ref: 'maxcostgrid', selector: 'subkpkrmaxcostgrid' },
        { ref: 'kegrid', selector: 'subkpkrkkegrid' },
        { ref: 'addPanel', selector: 'versionadd' },
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'versionStateTransferAspect',
            gridSelector: 'programversiongrid',
            stateType: 'ovrhl_program_version',
            menuSelector: 'programversionstatecontextmenu'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'versionstatepermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.View', applyTo: '[dataIndex=Actualize]', selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.View', applyTo: '[dataIndex=StavrActualize]', selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },                
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View", 
                    applyTo: '[action=actualize]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View", 
                    applyTo: '[action=massyearchange]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[action=makeKPKR]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[action=makeSubKPKR]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[action=publish]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[action=calculatecosts]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[cmd=order]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.Delete',
                    applyTo: '[action=hide]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.Undo',
                    applyTo: '[action=restore]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.ToSubProgram',
                    applyTo: '[action=insubdpkr]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.UndoSubProgram',
                    applyTo: '[action=reinsubdpkr]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersion.ActualizeProgram.View',
                    applyTo: 'b4addbutton',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeNewRecords",
                    applyTo: '[action=AddNewRecords]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.CalculateSequence",
                    applyTo: '[action=ActualizePriority]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSum",
                    applyTo: '[action=ActualizeSum]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYear",
                    applyTo: '[action=ActualizeYear]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeDel",
                    applyTo: '[action=ActualizeDeletedEntries]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYearChange",
                    applyTo: '[action=ActualizeYearForStavropol]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeMainVersion",
                    applyTo: '[action=ActualizeByFilters]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSubProgram",
                    applyTo: '[action=ActualizeSubrecord]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeFromShortCr",
                    applyTo: '[action=ActualizeFromShortCr]',
                    selector: 'versionrecordsgrid'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'vesionExportAspect',
            gridSelector: 'versionrecordsgrid',
            buttonSelector: 'versionrecordsgrid button[action="export"]',
            controllerName: 'ProgramVersion',
            actionName: 'Export'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actualizationLogExportAspect',
            gridSelector: 'actualizationloggrid',
            buttonSelector: 'actualizationloggrid #btnExport',
            controllerName: 'VersionActualizeLogRecord',
            actionName: 'Export'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'versmassyearchangewinMultiselectwindowaspect',
            fieldSelector: 'versmassyearchangewin [name=versRecForChange]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#versmassyearchangewinMunicipalitySelectWindow',
            storeSelect: 'version.ForMassChangeSelect',
            storeSelected: 'version.ForMassChangeSelected',
            textProperty: 'IndexNumber',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 50
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 75,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.versionId = this.controller.getEditPanel().versionId;
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
<<<<<<< HEAD
                //{
                //    name: 'Ovrhl.ProgramVersions.Actualize',
                //    applyTo: '[dataIndex=Actualize]',
                //    selector: 'versionrecordsgrid',
                //    applyBy: function (component, allowed) {
                //        debugger;
                //        if (allowed) component.show();
                //        else component.hide();
                //    }
                //}
                //{
                //    name: 'Ovrhl.ProgramVersions.StavropolActualize',
                //    applyTo: '[dataIndex=StavrActualize]',
                //    selector: 'versionrecordsgrid',
                //    applyBy: function (component, allowed) {
                //        if (allowed) component.show();
                //        else component.hide();
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.ActualizeYearForStavropol',
                //    applyTo: '[action=actualize]',
                //    selector: 'versionrecordsgrid',
                //    applyBy: function (component, allowed) {
                //        Ext.each(component.menu.items.items, function (item) {
                //            if (item.action == 'ActualizeYearForStavropol') {
                //                if (allowed) item.show();
                //                else item.hide();
                //            }
                //        });
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.MassYearChange',
                //    applyTo: '[action=massyearchange]',
                //    selector: 'versionrecordsgrid',
                //    applyBy: function (component, allowed) {
                //        if (allowed) component.show();
                //        else component.hide();
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.Copy',
                //    applyTo: 'actioncolumn[type=copy]',
                //    selector: 'programversiongrid',
                //    applyBy: function (component, allowed) {
                //        if (allowed) component.show();
                //        else component.hide();
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.Tab.Subsidy',
                //    applyTo: 'tabpanel',
                //    selector: 'programversioneditpanel',
                //    applyBy: function (component, allowed) {
                //        var tab = component.down('panel[type = subsidy]');
                //        if (allowed) {
                //            component.showTab(tab);
                //        } else {
                //            component.hideTab(tab);
                //        }
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.Tab.Correction',
                //    applyTo: 'tabpanel',
                //    selector: 'programversioneditpanel',
                //    applyBy: function (component, allowed) {
                //        var tab = component.down('panel[type = correction]');
                //        if (allowed) {
                //            component.showTab(tab);
                //        } else {
                //            component.hideTab(tab);
                //        }
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.Tab.Publication',
                //    applyTo: 'tabpanel',
                //    selector: 'programversioneditpanel',
                //    applyBy: function (component, allowed) {
                //        var tab = component.down('panel[type = publication]');
                //        if (allowed) {
                //            component.showTab(tab);
                //        } else {
                //            component.hideTab(tab);
                //        }
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.OwnerDecision.View',
                //    applyTo: 'tabpanel',
                //    selector: 'programversioneditpanel',
                //    applyBy: function (component, allowed) {
                //        var tab = component.down('panel[type = ownerDecision]');
                //        if (allowed) {
                //            component.showTab(tab);
                //        } else {
                //            component.hideTab(tab);
                //        }
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.OwnerDecision.Create',
                //    applyTo: 'versionownerdecisionform',
                //    selector: 'versionrecordseditwin',
                //    applyBy: function (component, allowed) {
                //        component.setVisible(allowed);
                //    }
                //},
                //{
                //    name: 'Ovrhl.ProgramVersions.RedirectToParent',
                //    applyTo: '#btnredirect',
                //    selector: 'programversioneditpanel',
                //    applyBy: function (component, allowed) {
                //        component.setVisible(allowed);
                //    }
                //}
=======
                {
                    name: 'Ovrhl.ProgramVersions.Actualize',
                    applyTo: '[dataIndex=Actualize]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.StavropolActualize',
                    applyTo: '[dataIndex=StavrActualize]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.ActualizeYearForStavropol',
                    applyTo: '[action=actualize]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        Ext.each(component.menu.items.items, function (item) {
                            if (item.action == 'ActualizeYearForStavropol') {
                                if (allowed) item.show();
                                else item.hide();
                            }
                        });
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.MassYearChange',
                    applyTo: '[action=massyearchange]',
                    selector: 'versionrecordsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Copy',
                    applyTo: 'actioncolumn[type=copy]',
                    selector: 'programversiongrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Tab.Subsidy',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = subsidy]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Tab.Correction',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = correction]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Tab.Publication',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = publication]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Tab.ActualizationLog',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = actualizationlog]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.Tab.ActualizationFileLogging',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = actualizationfilelogging]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.OwnerDecision.View',
                    applyTo: 'tabpanel',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        var tab = component.down('panel[type = ownerDecision]');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.OwnerDecision.Create',
                    applyTo: 'versionownerdecisionform',
                    selector: 'versionrecordseditwin',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.RedirectToParent',
                    applyTo: '#btnredirect',
                    selector: 'programversioneditpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Ovrhl.ProgramVersions.SplitWork',
                    applyTo: '#btnSplitWork',
                    selector: 'versionrecordseditwin',
                    applyBy: function (component, allowed) {
                        this.setVisible(component, allowed);
                    }
                }
>>>>>>> net6
            ]
        },
        {
            xtype: 'gkhmultiselectwindowaspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#deletedentriesmultiselectwin',
            storeSelect: 'version.ActualizeDeletedEntries',
            storeSelected: 'version.ActualizeDeletedEntries',
            name: 'deletedentriesmultiselectwindowaspect',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранные элементы',
            otherActions: function (actions) {
                var me = this;
                actions[this.multiSelectWindowSelector + ' checkboxfield[name=selectUsingFilters]'] = {
                    change: function(comp, newValue) {
                        me.getSelectedGrid().setDisabled(newValue);
                        me.getSelectGrid().getSelectionModel().b4deselectAll();
                    }
                }
            }, 
            validateRowSelect: function () {
                var me = this,
                    checkBox = me.getForm().down('checkboxfield[name=selectUsingFilters]'),
                    selectUsingFilters = checkBox ? checkBox.getValue() : false;

                return !selectUsingFilters;
            },
            toolbarItems: [
                {
                    xtype: 'b4updatebutton'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'selectUsingFilters',
                    boxLabel: 'Выбрать все с учетом фильтров',
                    margin: '0 0 0 5'
                }
            ],
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsChangedYear',
                    flex: 1,
                    text: 'Плановый год изменен',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                this._onBeforeLoad.apply(this, arguments);
            },
            _onBeforeLoad: Ext.emptyFn,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'getdata': function (asp, records) {
                    var ids = [],
                        panel = asp.controller.getEditPanel(),
                        versionId = panel.versionId,
                        form = asp.getForm(),
                        checkBox = form.down('checkboxfield[name=selectUsingFilters]'),
                        selectUsingFilters = checkBox ? checkBox.getValue() : false,
                        params = {
                            versionId: versionId,
                            selectUsingFilters: selectUsingFilters,
                            yearStart: asp.controller.yearStart
                        },
                        selectGrid = asp.getSelectGrid(),
                        selectStore = selectGrid.getStore();

                    if (selectUsingFilters) {
                        Ext.apply(params, selectStore.lastOptions.params);
                    } else {
                        if (records.length === 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                            return false;
                        }

                        Ext.each(records.items, function(r) {
                            ids.push(r.get('Id'));
                        });

                        Ext.apply(params, { st3Ids: Ext.encode(ids) });
                    }

                    asp.controller.mask('Пожалуйста, подождите. Идет удаление записей', form);

                    B4.Ajax.request({
                        url: B4.Url.action('/ProgramVersion/ActualizeDeletedEntries'),
                        params: params,
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Успешно', 'Записи удалены', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Удаление невозможно!', 'error');
                    });

                    return false;
                }
            }
        },
        {
            xtype: 'gkhmultiselectwindowaspect',
            name: 'addentriesmultiselectwindowaspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#addentriesmultiselectwin',
            storeSelect: 'version.ActualizeAddEntries',
            storeSelected: 'version.ActualizeAddEntries',
            titleSelectWindow: 'Добавление новых записей',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранные элементы',
            otherActions: function (actions) {
                var me = this;
                actions[this.multiSelectWindowSelector + ' checkboxfield[name=selectUsingFilters]'] = {
                    change: function (comp, newValue) {
                        me.getSelectedGrid().setDisabled(newValue);
                        me.getSelectGrid().getSelectionModel().b4deselectAll();
                    }
                }
            },
            validateRowSelect: function () {
                var me = this,
                    checkBox = me.getForm().down('checkboxfield[name=selectUsingFilters]'),
                    selectUsingFilters = checkBox ? checkBox.getValue() : false;

                return !selectUsingFilters;
            },
            toolbarItems: [
                {
                    xtype: 'b4updatebutton'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'selectUsingFilters',
                    boxLabel: 'Выбрать все с учетом фильтров',
                    margin: '0 0 0 5'
                }
            ],
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    flex: 1,
                    text: 'Структурный элемент',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    flex: 1,
                    text: 'Структурный элемент',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                this._onBeforeLoad.apply(this, arguments);
            },
            _onBeforeLoad: Ext.emptyFn,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'getdata': function (asp, records) {
                    var ids = [],
                        panel = asp.controller.getEditPanel(),
                        versionId = panel.versionId,
                        form = asp.getForm(),
                        checkBox = form.down('checkboxfield[name=selectUsingFilters]'),
                        selectUsingFilters = checkBox ? checkBox.getValue() : false,
                        params = {
                            versionId: versionId,
                            selectUsingFilters: selectUsingFilters,
                            yearStart: asp.controller.yearStart
                        },
                        selectGrid = asp.getSelectGrid(),
                        selectStore = selectGrid.getStore();

                    if (selectUsingFilters) {
                        Ext.apply(params, selectStore.lastOptions.params);
                    } else {
                        if (records.length === 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                            return false;
                        }

                        Ext.each(records.items, function (r) {
                            ids.push(r.get('Id'));
                        });

                        Ext.apply(params, { roStructElIds: Ext.encode(ids) });
                    }

                    asp.controller.mask('Пожалуйста, подождите. Идет добавление записей', form);

                    B4.Ajax.request({
                        url: B4.Url.action('/ProgramVersion/AddNewRecords'),
                        params: params,
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Успешно', 'Записи добавлены', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Добавление невозможно!', 'error');
                    });

                    return false;
                }
            }
        },
        {
            xtype: 'gkhmultiselectwindowaspect',
            name: 'sumentriesmultiselectwindowaspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#sumentriesmultiselectwin',
            storeSelect: 'version.ActualizeSumEntries',
            storeSelected: 'version.ActualizeSumEntries',
            titleSelectWindow: 'Актуализировать стоимость',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранные элементы',
            otherActions: function (actions) {
                var me = this;
                actions[this.multiSelectWindowSelector + ' checkboxfield[name=selectUsingFilters]'] = {
                    change: function (comp, newValue) {
                        me.getSelectedGrid().setDisabled(newValue);
                        me.getSelectGrid().getSelectionModel().b4deselectAll();
                    }
                }
            },
            validateRowSelect: function () {
                var me = this,
                    checkBox = me.getForm().down('checkboxfield[name=selectUsingFilters]'),
                    selectUsingFilters = checkBox ? checkBox.getValue() : false;

                return !selectUsingFilters;
            },
            toolbarItems: [
                {
                    xtype: 'b4updatebutton'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'selectUsingFilters',
                    boxLabel: 'Выбрать все с учетом фильтров',
                    margin: '0 0 0 5'
                }
            ],
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                this._onBeforeLoad.apply(this, arguments);
            },
            _onBeforeLoad: Ext.emptyFn,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'getdata': function (asp, records) {
                    var me = this,
                        ids = [],
                        panel = asp.controller.getEditPanel(),
                        versionId = panel.versionId,
                        form = asp.getForm(),
                        checkBox = form.down('checkboxfield[name=selectUsingFilters]'),
                        selectUsingFilters = checkBox ? checkBox.getValue() : false,
                        params = {
                            versionId: versionId,
                            selectUsingFilters: selectUsingFilters,
                            yearStart: me.controller.yearStart,
                            yearEnd: me.controller.yearEnd
                        },
                        selectGrid = asp.getSelectGrid(),
                        selectStore = selectGrid.getStore();

                    if (selectUsingFilters) {
                        Ext.apply(params, selectStore.lastOptions.params);
                    } else {
                        if (records.length === 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                            return false;
                        }

                        Ext.each(records.items, function (r) {
                            ids.push(r.get('Id'));
                        });

                        Ext.apply(params, { st3Ids: Ext.encode(ids) });
                    }

                    asp.controller.mask('Пожалуйста, подождите. Идет актуализация стоимости', form);

                    B4.Ajax.request({
                        url: B4.Url.action('/ProgramVersion/ActualizeSum'),
                        params: params,
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Успешно', 'Записи актуализированы', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Актуализация невозможна!', 'error');
                    });

                    return false;
                }
            }
        },
        {
            xtype: 'gkhmultiselectwindowaspect',
            name: 'yearentriesmultiselectwindowaspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#yearentriesmultiselectwin',
            storeSelect: 'version.ActualizeYearEntries',
            storeSelected: 'version.ActualizeYearEntries',
            titleSelectWindow: 'Актуализация года',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранные элементы',
            otherActions: function (actions) {
                var me = this;
                actions[this.multiSelectWindowSelector + ' checkboxfield[name=selectUsingFilters]'] = {
                    change: function (comp, newValue) {
                        me.getSelectedGrid().setDisabled(newValue);
                        me.getSelectGrid().getSelectionModel().b4deselectAll();
                    }
                }
            },
            validateRowSelect: function () {
                var me = this,
                    checkBox = me.getForm().down('checkboxfield[name=selectUsingFilters]'),
                    selectUsingFilters = checkBox ? checkBox.getValue() : false;

                return !selectUsingFilters;
            },
            toolbarItems: [
                {
                    xtype: 'b4updatebutton'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'selectUsingFilters',
                    boxLabel: 'Выбрать все с учетом фильтров',
                    margin: '0 0 0 5'
                }
            ],
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 2,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 2,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Changes',
                    flex: 2,
                    text: 'Изменения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsChangedYear',
                    flex: 1,
                    text: 'Плановый год изменен',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],

            onBeforeLoad: function (store, operation) {
                this._onBeforeLoad.apply(this, arguments);
            },
            _onBeforeLoad: Ext.emptyFn,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'getdata': function (asp, records) {
                    var ids = [],
                        panel = asp.controller.getEditPanel(),
                        versionId = panel.versionId,
                        form = asp.getForm(),
                        checkBox = form.down('checkboxfield[name=selectUsingFilters]'),
                        selectUsingFilters = checkBox ? checkBox.getValue() : false,
                        params = {
                            versionId: versionId,
                            selectUsingFilters: selectUsingFilters,
                            yearStart: asp.controller.yearStart
                        },
                        selectGrid = asp.getSelectGrid(),
                        selectStore = selectGrid.getStore();

                    if (selectUsingFilters) {
                        Ext.apply(params, selectStore.lastOptions.params);
                    } else {
                        if (records.length === 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                            return false;
                        }

                        Ext.each(records.items, function (r) {
                            ids.push(r.get('Id'));
                        });

                        Ext.apply(params, { st3Ids: Ext.encode(ids) });
                    }

                    asp.controller.mask('Пожалуйста, подождите. Идет актуализация', form);

                    B4.Ajax.request({
                        url: B4.Url.action('/ProgramVersion/ActualizeYear'),
                        params: params,
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Успешно', 'Записи актуализированы', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Актуализация невозможна!', 'error');
                    });

                    return false;
                }
            }
        },
        {
            xtype: 'gkhmultiselectwindowaspect',
            name: 'yearchangeentriesmultiselectwindowaspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#yearchangeentriesmultiselectwin',
            storeSelect: 'version.ActualizeYearChangeEntries',
            storeSelected: 'version.ActualizeYearChangeEntries',
            titleSelectWindow: 'Актуализация изменения года',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранные элементы',
            otherActions: function (actions) {
                var me = this;
                actions[this.multiSelectWindowSelector + ' checkboxfield[name=selectUsingFilters]'] = {
                    change: function (comp, newValue) {
                        me.getSelectedGrid().setDisabled(newValue);
                        me.getSelectGrid().getSelectionModel().b4deselectAll();
                    }
                }
            },
            validateRowSelect: function () {
                var me = this,
                    checkBox = me.getForm().down('checkboxfield[name=selectUsingFilters]'),
                    selectUsingFilters = checkBox ? checkBox.getValue() : false;

                return !selectUsingFilters;
            },
            toolbarItems: [
                {
                    xtype: 'b4updatebutton'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'selectUsingFilters',
                    boxLabel: 'Выбрать все с учетом фильтров',
                    margin: '0 0 0 5'
                }
            ],
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 2,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 2,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Changes',
                    flex: 2,
                    text: 'Изменения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsChangedYear',
                    flex: 1,
                    text: 'Плановый год изменен',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                this._onBeforeLoad.apply(this, arguments);
            },
            _onBeforeLoad: Ext.emptyFn,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'getdata': function (asp, records) {
                    var ids = [],
                        panel = asp.controller.getEditPanel(),
                        versionId = panel.versionId,
                        form = asp.getForm(),
                        checkBox = form.down('checkboxfield[name=selectUsingFilters]'),
                        selectUsingFilters = checkBox ? checkBox.getValue() : false,
                        params = {
                            versionId: versionId,
                            selectUsingFilters: selectUsingFilters,
                            yearStart: asp.controller.yearStart,
                            yearEnd: asp.controller.yearEnd
                        },
                        selectGrid = asp.getSelectGrid(),
                        selectStore = selectGrid.getStore();

                    if (selectUsingFilters) {
                        Ext.apply(params, selectStore.lastOptions.params);
                    } else {
                        if (records.length === 0) {
                            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                            return false;
                        }

                        Ext.each(records.items, function (r) {
                            ids.push(r.get('Id'));
                        });

                        Ext.apply(params, { st3Ids: Ext.encode(ids) });
                    }

                    asp.controller.mask('Пожалуйста, подождите. Идет актуализация', form);

                    B4.Ajax.request({
                        url: B4.Url.action('/ProgramVersion/ActualizeYearForStavropol'),
                        params: params,
                        timeout: 9999999
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Успешно', 'Записи актуализированы', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        asp.getForm().destroy();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Актуализация невозможна!', 'error');
                    });

                    return false;
                }
            }
        },
    ],

    init: function() {
        var me = this;

        this.control({
            'actualizedbyfilteraddgrid [action=Actualize]': { click: { fn: me.ActualizeFromAddGrid, scope: me } },  
            'actualizedbyfilterdeletegrid [action=Actualize]': { click: { fn: me.ActualizeFromDeleteGrid, scope: me } },  
            'versionactualizebyfilterswindow [action=Update]': { click: { fn: me.onClickUpdate, scope: me } },    
            'versionactualizebyfilterswindow [action=RemoveSelected]': { click: { fn: me.DPKRRemoveSelected, scope: me } },   
            'versionactualizesubprogrambyfilterswindow [action=RemoveSelected]': { click: { fn: me.SubDPKRRemoveSelected, scope: me } },   
            'versionactualizebyfilterswindow b4closebutton': { 'click': { fn: me.closeFilter, scope: me } },
            'versmakesubkpkrwin #nfStartYear': { 'change': { fn: me.updateKEGrid, scope: me } },
            'versmakesubkpkrwin #nfYearCount': { 'change': { fn: me.updateKEGrid, scope: me } },
            'versionactualizesubprogrambyfilterswindow [action=Update]': { click: { fn: me.onClickUpdateSubProgram, scope: me } },
            'versionactualizesubprogrambyfilterswindow b4closebutton': { 'click': { fn: me.closeFilterSubProgram, scope: me } },
            'programversioneditpanel menuitem[action=redirecttoparent]': { 'click': me.goToParent },
            'programversiongrid b4updatebutton': { 'click': { fn: me.updateGrid, scope: me } },
            'programversiongrid #cbShowNotActualProgramm': { 'change': { fn: me.updateGridByChecker, scope: me } },
            'versionrecordsgrid b4addbutton': { 'click': { fn: me.addRecords, scope: me } },
            'versionrecordsgrid b4updatebutton': { 'click': { fn: me.updateRecords, scope: me } },
<<<<<<< HEAD
          //  'versionrecordsgrid button[cmd="order"]': { click: { fn: me.onCalculateYearsClick, scope: me } },  
            'versionrecordsgrid button[cmd="order"]': { click: { fn: me.showCalculateYearsWindow, scope: me } },  
=======
            'versionrecordsgrid button[cmd="order"]': { click: { fn: me.showOrder, scope: me } },
            'versionrecordsgrid button[cmd="import"]': { click: { fn: me.showImport, scope: me } },
>>>>>>> net6
            'programversiongrid': { rowaction: { fn: me.rowaction, scope: me } },
            'subkpkrkkegrid': { 'selectionchange': { fn: me.onKESelectionChange, scope: this } },  
            'versionorderwin b4closebutton': { 'click': { fn: me.closeOrderWin, scope: me } },
<<<<<<< HEAD
            'versionadd b4closebutton': { 'click': { fn: me.closeAdd, scope: me } },
            'versionadd b4savebutton': { 'click': { fn: me.Add, scope: me } },
            'versmakekpkrwin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('versmakekpkrwin').close();
                    },
                    scope: me
                }
            },
            'versmakesubkpkrwin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('versmakesubkpkrwin').close();
                    },
                    scope: me
                }
            },
=======
            'versionimportwin b4closebutton': { 'click': { fn: me.closeImportWin, scope: me } },
            'versionimportwin b4savebutton': { 'click': { fn: me.saveImportWin, scope: me } },
>>>>>>> net6
            'programversioneditpanel b4selectfield[name="Version"]': {
                'beforeload': { fn: me.onVersionStoreBeforeLoad, scope: me },
                'change': { fn: me.onVersionChange, scope: me }
            },
            'versionactualizebyfilterswindow numberfield[name="StartYear"]': {
                'change': { fn: me.onStartYearChange, scope: me }
            },
            'programversioneditpanel checkbox[name="IsMain"]': {
                'change': { fn: me.onIsMainCheckBoxChange, scope: me }
            },
            'programversioneditpanel checkbox[name="ShowNoShowing"]': {
                'change': { fn: me.onShowNoShowingCheckBoxChange, scope: me }
            },
            'programversioneditpanel checkbox[name="ShowMainVersion"]': {
                'change': { fn: me.onShowMainVersionCheckBoxChange, scope: me }
            },
            'programversioneditpanel checkbox[name="ShowSubVersion"]': {
                'change': { fn: me.onShowSubVersionCheckBoxChange, scope: me }
            },
            'programversionpanel combobox[name=Municipality]': {
                render: { fn: me.onRenderMunicipality, scope: me },
                select: { fn: me.onSelectMunicipality, scope: me }
            },
            'versionrecordsgrid': {
                itemdblclick: {
                    fn: function (view, record) {
                        me.editRecord(record);
                    },
                    scope: me
                },
                rowaction: {
                    fn: function (view, action, record) {
                        if (action.toLowerCase() === 'edit') {
                            me.editRecord(record);
                        }
                    },
                    scope: me
                },

            },
            'versionrecordseditwin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('versionrecordseditwin').close();
                    },
                    scope: me
                }
            },
            'versionrecordseditwin b4savebutton': {
                click: {
                    fn: me.onClickSave,
                    scope: me
                }
            },
            'versionrecordseditwin #btnSplitWork': {
                click: {
                    fn: me.onBtnSplitWorkClick,
                    scope: me
                }
            },
            'versionrecordsgrid [dataIndex=Actualize] menuitem': {
                 click: {
                     fn: me.onActualizeMenuItemClick,
                     scope: me
                 }
            },
            'versionrecordsgrid [dataIndex=StavrActualize] menuitem': {
                click: {
                    fn: me.onStavrActualizeMenuItemClick,
                    scope: me
                }
            },
            'versionactualizedelgrid b4updatebutton': {
                click: {
                    fn: me.onUpdateGrid,
                    scope: me
                }
            },
            'versionactualizeperiodwindow b4savebutton': {
                click: {
                    fn: me.onActualizeParamsApply,
                    scope: me
                }
            },
            'versionactualizeperiodwindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        var win = btn.up('versionactualizeperiodwindow');
                        win.destroy();
                    },
                    scope: me
                }
            },
            'versionactualizeperiodwindow': {
                afterrender: {
                    fn: function (view) {
                        var fldYearEnd = view.down('numberfield[name=YearEnd]');

                        // Для удаления и добавления должен быть только 1 параметр для остальных 2
                        if (view.action == 'AddNewRecords' || view.action == 'ActualizeDel'
                            || view.action == 'ActualizeYear' || view.action == 'ActualizePriority') {
                            fldYearEnd.allowBlank = true;
                            fldYearEnd.hide();
                        } else {
                            fldYearEnd.allowBlank = false;
                            fldYearEnd.show();
                        }
                    },
                    scope: me
                }
            },
            'versionactualizedelgrid button[cmd=apply]': {
                click: {
                    fn: me.onActualizeDelApply,
                    scope: me
                }
            },
            'programversioneditpanel versioncorrectiongrid b4updatebutton': {
                click: { fn: me.onUpdateGrid, scope: me }
            },
            'programversioneditpanel versionpublicationgrid b4updatebutton': {
                click: { fn: me.onUpdateGrid, scope: me }
            },
            'programversioneditpanel actualizationloggrid b4updatebutton': {
                click: { fn: me.onUpdateGrid, scope: me }
            },
            'programversioneditpanel actualizationfilelogginggrid b4updatebutton': {
                click: { fn: me.onUpdateGrid, scope: me }
            },
            'versioncopywindow b4closebutton': {
                click: { fn: function (btn) {
                        btn.up('window').close();
                    }, scope: me }
            },
            'versioncopywindow button[action=copy]': {
                click: { fn: me.saveCopyVersion, scope: me }
            },

            'versionorderyearwindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('window').close();
                    }, scope: me
                }
            },
            'versionorderyearwindow button[action=copy]': {
                click: { fn: me.onCalculateYearsClick, scope: me }
            },

            'versionrecordsgrid button[action=massyearchange]': {
                click: {
                    fn: me.onMassYearChangeItemClick,
                    scope: me
                }
            },
            'versionrecordsgrid button[action=makeKPKR]': {
                click: {
                    fn: me.onMakeKPKRItemClick,
                    scope: me
                }
            },
            'versionrecordsgrid button[action=makeSubKPKR]': {
                click: {
                    fn: me.onMakeSubKPKRItemClick,
                    scope: me
                }
            },
            'versionrecordsgrid button[action=publish]': {
                click: {
                    fn: me.onPublishClick,
                    scope: me
                }
            },
            'versionrecordsgrid button[action=calculatecosts]': {
                click: {
                    fn: me.onCalculateCostsClick,
                    scope: me
                }
            },
            'versmassyearchangewin button[action=changeyear]': {
                click: { fn: me.massChangeYear, scope: me }
            },
            'versmakekpkrwin button[action=makeKPKR]': {
                click: { fn: me.makeKPKR, scope: me }
            },
            'versmakesubkpkrwin button[action=makeSubKPKR]': {
                click: { fn: me.makeSubKPKR, scope: me }
            },
            'versmassyearchangewin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('window').close();
                    }, scope: me
                }
            },
            'versionactualizeprogramwindow b4savebutton': {
                click: {
                    fn: me.onActualizeProgramCrApply,
                    scope: me
                }
            },
            'versionactualizebyfilterswindow b4savebutton': {
                click: {
                    fn: me.onFilterProgram,
                    scope: me
                }
            },
            'versionactualizesubprogrambyfilterswindow b4savebutton': {
                click: {
                    fn: me.onFilterSubProgram,
                    scope: me
                }
            },
            'versionactualizeprogramwindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        var win = btn.up('versionactualizeprogramwindow');
                        win.destroy();
                    },
                    scope: me
                }
            },
            'versionactualizeprogramwindow b4selectfield[name=ProgramCr]': {
                'beforeload': { fn: me.onProgramCrBeforeLoad, scope: me }
            },
            'versionrecordsgrid actioncolumn[action="hide"]': { click: { fn: this.hideRecord, scope: this } },
            'versionrecordsgrid actioncolumn[action="restore"]': { click: { fn: this.restoreRecord, scope: this } },
            'versionrecordsgrid actioncolumn[action="insubdpkr"]': { click: { fn: this.insubdpkrRecord, scope: this } },
            'versionrecordsgrid actioncolumn[action="reinsubdpkr"]': { click: { fn: this.reinsubdpkrRecord, scope: this } },
            'actualizedbyfilteraddgrid actioncolumn[action="remove"]': { click: { fn: this.removeHouseForAdd, scope: this } },
            'actualizedbyfilterdeletegrid actioncolumn[action="remove"]': { click: { fn: this.removeHouseForDelete, scope: this } },
            'actualizesubprogrambyfilteraddgrid actioncolumn[action="remove"]': { click: { fn: this.removeHouseForAddSubProgram, scope: this } },
            'actualizesubprogrambyfilterdeletegrid actioncolumn[action="remove"]': { click: { fn: this.removeHouseForDeleteSubProgram, scope: this } }    
        });

        this.callParent(arguments);
    },

   index: function() {
        var view = this.getMainPanel() || Ext.widget('programversionpanel');

        this.bindContext(view);
        this.application.deployView(view);
       //asp.controller.getAspect('overhaulproposestatepermissionaspect').setPermissionsByRecord(rec); 
        //this.getStore('version.ActualizeByFiltersAdd').load();
        //this.getStore('version.ActualizeByFiltersDelete').load();
    },

    onRenderMunicipality: function(cmp) {
        var store = cmp.getStore(),
            storeVersion = this.getGrid().getStore();

        store.on('load', this.onLoadMunicipality, this);
        store.load();
        storeVersion.on('beforeload', this.onBeforeLoad, this);
    },

    onBeforeLoad: function (store, operation) {
      
        var form = this.getGrid();
        var cbox = form.down('#cbShowNotActualProgramm');
        var isChecked = cbox.checked;
        operation.params.isChecked = isChecked;
        operation.params.muId = this.getMuField().getValue();
    },

    onSelectMunicipality: function (field) {
        field.store.clearFilter();
        this.getGrid().getStore().clearFilter();
    },

    onLoadMunicipality: function(store, records) {
        var me = this,
            cmb = me.getMuField();

        if (records[0]) {
            cmb.setValue(records[0]);
            me.onSelectMunicipality(cmb);
        }
    },
    
    updateGrid: function (btn, operation) {
        //
        var win = btn.up('programversiongrid');
        var checkboxSNAP = win.down('#cbShowNotActualProgramm');
        var isChecked = checkboxSNAP.checked;
        btn.up('programversiongrid').getStore().load({
            params: {

                isChecked: isChecked
            }
        });
    },

    updateGridByChecker: function (cbx, operation) {
        //
     
        var win = cbx.up('programversiongrid');
        var isChecked = cbx.checked;
        var store =  win.getStore();      
        store.load({
            params: {
                
                isChecked: isChecked
            }
        });
      
    },

    rowaction: function (grid, action, record) {
        var me = this;
        
        switch (action.toLowerCase()) {
        case 'edit':
            me.showVersion(record);
            break;
        case 'delete':
            me.deleteRecord(record);
            break;
       case 'copy':
            me.copyRecord(record);
            break;
        }
    },

    goToParent: function (menuitem) {
        var me = this,
            parentId = menuitem.parentVersionId;
        if (parentId > 0) {
            b4app.getRouter().redirectTo('show_version/' + parentId);

        }
    },

    copyRecord: function (record) {
        var me = this,
            view,
            name = record.get('Name'),
            versDate = record.get('VersionDate');

        view = me.getCopyWindow() || Ext.widget('versioncopywindow', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy',
            ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
            versionId: record.getId()
        });
       
        
        if (B4.getBody().getActiveTab()) {
            B4.getBody().getActiveTab().add(view);
        } else {
            B4.getBody().add(view);
        }

        view.down('[name=Name]').setValue(name);
        view.down('[name=VersionDate]').setValue(versDate);
        view.down('[name=NewName]').setValue('Копия ' + name);

        view.show();
    },

    saveCopyVersion: function (btn) {
        var me = this,
            view = btn.up('window'),
            newName = view.down('[name=NewName]').getValue(),
            newDate = view.down('[name=NewDate]').getValue(),
            versionId = view.versionId;
        
        me.mask('Копирование версии...', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('CopyVersion', 'ProgramVersion'),
            params: {
                newName: newName,
                newDate: newDate,
                versionId: versionId
            },
            timeout: 86400000 // сутки
        }).next(function (response) {
            me.getGrid().getStore().load();
            view.close();
            B4.QuickMsg.msg('Сообщение', 'Версия успешно скопирована', 'success');
            me.unmask();
        }).error(function () {
            B4.QuickMsg.msg('Ошибка!', 'При копировании версии произошла ошибка', 'error');
            me.unmask();
        });

    },

    deleteRecord: function(record) {
        var me = this,
            panel = me.getMainPanel(),
            grid = me.getGrid(),
            model,
            rec;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
            if (result == 'yes') {
                model = this.getModel('version.ProgramVersion');

                rec = new model({ Id: record.getId() });
                me.mask('Удаление версии...', panel);

                rec.destroy()
                    .next(function(sResult) {
                        me.unmask();
                        grid.getStore().load();
                    }, this)
                    .error(function(e) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка удаления!', (e.responseData.message));
                    }, this);
            }
        }, me);
    },

    showVersion: function (rec) {
        versionRecord = rec;
        Ext.History.add('show_version/' + rec.getId());
    },
    
    showOrder: function() {
        var versionId = this.getEditPanel().versionId,
            win = Ext.widget('versionorderwin'),
            grid = win.down('versionparamsgrid'),
            store = grid.getStore();

        store.filter('version', versionId);

        win.show();
    },

    closeOrderWin: function(btn) {       

        btn.up('versionorderwin').close();
    },
    closeFilter: function (btn) {
        var me = this;

        btn.up('versionactualizebyfilterswindow').destroy();

        var view = me.getEditPanel();
        var versRecStore = view.down('versionrecordsgrid').getStore();
        versRecStore.load();
    },
    closeFilterSubProgram: function (btn) {
        var me = this;

<<<<<<< HEAD
        btn.up('versionactualizesubprogrambyfilterswindow').destroy();

        var  view = me.getEditPanel();
        var versRecStore = view.down('versionrecordsgrid').getStore();
        versRecStore.load();
    },
=======
    showImport: function (btn) {
        var win = Ext.widget('versionimportwin'),
            view = btn.up('programversioneditpanel');

        win.versionId = view.down('[name=Version]').getValue();
        win.show();
    },

    closeImportWin: function (btn) {
        btn.up('versionimportwin').close();
    },

    saveImportWin: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.getForm();

        me.mask('Загрузка файла');
        
        form.submit({
            url: B4.Url.action('Import', 'ProgramVersion'),
            params: {
                versionId: win.versionId
            },
            timeout: 9999999,
            success: function () {
                me.unmask();
                win.close();
                Ext.Msg.alert('Файл загружен', 'Успешно загружено!');
            },
            failure: function (f, action) {
                me.unmask();
                win.close();
                Ext.Msg.alert({
                    title: 'Внимание!',
                    msg: 'Во время загрузки файла произошла ошибка. Для ознакомления перейдите в Логи загрузок.' +
                        ' Выберите тип импорта: "Импорт сведений о сроках проведения капитального ремонта", далее скачайте лог',
                    buttons: 1,
                    buttonText: { ok: 'Перейти в Логи загрузок' },
                    fn: function (btn) {
                        if (btn === 'ok') {
                            var router = b4app.getRouter();
                            router.redirectTo('importlog');
                        }
                    }
                });
            }
        });
    },

>>>>>>> net6
    version: function(id) {
        var me = this,
            versRecStore,
            correctionStore,
            subsidyStore,
            publicationStore,
            logStore,
            fileLogStore,
            decisionStore,
            view;

        B4.Ajax.request({
            url: B4.Url.action('Get', 'ProgramVersion'),
            method: 'GET',
            params: {
                id: id
            }
        }).next(function(response) {
            var resp = Ext.decode(response.responseText),
                data = resp.data;

            view = me.getEditPanel() || Ext.widget('programversioneditpanel', {
                versionId: id
            });

            view.down('[name=Version]').setValue(data);
            view.down('[name=IsMain]').setValue(data.IsMain);

            me.bindContext(view);
            me.application.deployView(view);

            versRecStore = view.down('versionrecordsgrid').getStore();
            correctionStore = view.down('versionsubsidyrecordgrid').getStore();
            subsidyStore = view.down('versioncorrectiongrid').getStore();
            publicationStore = view.down('versionpublicationgrid').getStore();
            logStore = view.down('actualizationloggrid').getStore();
            fileLogStore = view.down('actualizationfilelogginggrid').getStore();
            decisionStore = view.down('versionownerdecisiongrid').getStore();

            logStore.on('beforeload', me.onActualizationLogStoreBeforeLoad, me);
            logStore.load();

            versRecStore.clearFilter(true);
            correctionStore.clearFilter(true);
            subsidyStore.clearFilter(true);
            publicationStore.clearFilter(true);
            fileLogStore.clearFilter(true);
            decisionStore.clearFilter(true);

            versRecStore.filter('version', id);
            correctionStore.filter('versionId', id);
            subsidyStore.filter('versionId', id);
            publicationStore.filter('versionId', id);
<<<<<<< HEAD
            logStore.filter('versionId', id);
            decisionStore.filter('versionId', id);  
            var editPanel = me.getEditPanel();     
            try {
                      
                var recordPv = versionRecord;
                var asp = me.getAspect('versionstatepermissionaspect');
                asp.setPermissionsByRecord(recordPv);               
               
            }
            catch (e) {
                var asp = me.getAspect('versionstatepermissionaspect');
                asp.setPermissionsByRecord({ getId: function () { return id; } });                 
            }
=======
            fileLogStore.filter('versionId', id);
            decisionStore.filter('versionId', id);
>>>>>>> net6

        }).error(function() {
            B4.QuickMsg.msg('Ошибка!', 'При получении версии произошла ошибка', 'error');
        });
    },

    updateRecords: function (btn) {
        btn.up('versionrecordsgrid').getStore().load();
    },

    onVersionStoreBeforeLoad: function (field, opts, store) {
        //var grid = this.getEditPanel();
        //if (grid && grid.municipalityId) {
        //    Ext.apply(store.getProxy().extraParams, {
        //        municipalityId: grid.municipalityId
        //    });
        //}
    },

    onVersionChange: function(selectfield, newVal, oldVal) {
        var id = newVal && newVal.Id ? newVal.Id : 0,
            view = selectfield.up('panel'),
            viewPanel = selectfield.up('programversioneditpanel'),
            menuitem = view.down('buttongroup menu menuitem[action=redirecttoparent]'),
            hasParent = !Ext.isEmpty(newVal.ParentVersion),
            store;
       
        menuitem.parentVersionId = hasParent ? newVal.ParentVersion.Id : null;

        if (!hasParent) {
            menuitem.setDisabled(true);
            menuitem.tooltip = 'Родительской версии не существует';
        }

        if (oldVal) {
            view.versionId = id;
            store = view.down('versionrecordsgrid').getStore();

            store.clearFilter(true);
            store.filter('version', id);

            view.down('checkbox[name="IsMain"]').setValue(newVal.IsMain);
        }
    },

    onIsMainCheckBoxChange: function(cbx, newValue, oldValue) {
        var me = this,
            view = cbx.up(),
            selectfield = view.down('b4selectfield[name="Version"]'),
            value = selectfield.value;

        if (value.IsMain != undefined && value.IsMain != newValue) {
            value.IsMain = newValue;

            me.mask('Cохранение', view);
            B4.Ajax.request({
                url: B4.Url.action('Update', 'ProgramVersion'),
                params: {
                    id: value.Id,
                    records: Ext.encode([value])
                }
            }).next(function() {
                me.unmask();
                B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
                return true;
            }).error(function(result) {
                me.unmask();
                var message = "Ошибка при сохранении";
                if (!Ext.isEmpty(result) && !Ext.isEmpty(result.message)) {
                    message = result.message;
                }

                Ext.Msg.alert('Ошибка сохранения', message);

                value.IsMain = oldValue;
                cbx.setValue(oldValue);
                return true;
            });
        }
    },

    onShowNoShowingCheckBoxChange: function (cbx, newValue, oldValue) {
        var me = this,            
            view = cbx.up('programversioneditpanel'),
            grid = view.down('versionrecordsgrid'),
            showMainField = view.down('checkbox[name="ShowMainVersion"]'),
            showSubField = view.down('checkbox[name="ShowSubVersion"]'),
            showHiddenField = view.down('checkbox[name="ShowNoShowing"]'),
            showHiddenValue = showHiddenField.value,
            showMainValue = showMainField.value,
            showSubValue = showSubField.value;
            grid.getStore().getProxy().extraParams = { 'ShowMainVersion': showMainValue, 'ShowSubVersion': showSubValue, 'showHidden': showHiddenValue };
        grid.getStore().load();
    },

    onShowMainVersionCheckBoxChange: function (cbx, newValue, oldValue) {
        var me = this,
            view = cbx.up('programversioneditpanel'),
            grid = view.down('versionrecordsgrid'),
            showMainField = view.down('checkbox[name="ShowMainVersion"]'),
            showSubField = view.down('checkbox[name="ShowSubVersion"]'),
            showHiddenField = view.down('checkbox[name="ShowNoShowing"]'),
            showHiddenValue = showHiddenField.value,
            showMainValue = showMainField.value,
            showSubValue = showSubField.value;
        grid.getStore().getProxy().extraParams = { 'ShowMainVersion': showMainValue, 'ShowSubVersion': showSubValue, 'showHidden': showHiddenValue };
        grid.getStore().load();
    },

    onShowSubVersionCheckBoxChange: function (cbx, newValue, oldValue) {
        var me = this,
            view = cbx.up('programversioneditpanel'),
            grid = view.down('versionrecordsgrid'),
            showMainField = view.down('checkbox[name="ShowMainVersion"]'),
            showSubField = view.down('checkbox[name="ShowSubVersion"]'),
            showHiddenField = view.down('checkbox[name="ShowNoShowing"]'),
            showHiddenValue = showHiddenField.value,
            showMainValue = showMainField.value,
            showSubValue = showSubField.value;
        grid.getStore().getProxy().extraParams = { 'ShowMainVersion': showMainValue, 'ShowSubVersion': showSubValue, 'showHidden': showHiddenValue };
        grid.getStore().load();
    },

    editRecord: function (record) {
        var me = this,
            editView = me.getVersionRecordsEditWin() || Ext.widget('versionrecordseditwin'),
            grid = editView.down('stage1recordsgrid'),
            store = grid.getStore(),
            btnSplitWork = editView.down('#btnSplitWork'),
            // Колонка с checkBox от selModel
            selModelColumn = grid.columns[0],
            showSeGrid = Gkh.config.Overhaul.OverhaulHmao.ActualizeConfig.TransferSingleStructEl === 10;

        editView.down('[name=IndexNumber]').setValue(record.get('IndexNumber'));
        editView.down('[name=Year]').setValue(record.get('Year'));
        editView.down('[name=Remark]').setValue(record.get('Remark'));
        editView.down('[name=ChangeBasisType]').setValue(record.get('ChangeBasisType'));
        editView.params = {};
        editView.params.versionRecId = record.getId();

        me.bindContext(editView);

        editView.show();

        if (showSeGrid) {
            btnSplitWork.hide();
            selModelColumn.hide();

            store.on('load', function (store, records) {
                if (records.length > 1 && btnSplitWork.viewPermissionAllowed) {
                    var selModel = grid.getSelectionModel(),
                        selected = selModel.getSelection();

                    if (selected.length > 0) {
                        selModel.b4deselectAll();
                    }

                    btnSplitWork.show();
                    selModelColumn.show();
                }
            }, me);

            store.filter('versionRecId', record.getId());
            grid.show();
        } else {
            grid.hide();
        }
    },

    onClickSave: function (btn) {
        var me = this,
            grid = me.getVersionRecordsGrid(),
            store = grid.getStore(),
            win = btn.up('versionrecordseditwin'),
            decisionForm = win.down('versionownerdecisionform'),
            newPlanYearStore = win.down('stage1recordsgrid').getStore(),
            newPlanYearRecs = newPlanYearStore.getModifiedRecords(),
            reCalcSum = win.down('[name=ReCalcSum]').getValue(),
            newIndex = win.down('[name=NewIndexNumber]').getValue(),
            newYear = win.down('[name=Year]').getValue(),
            changeBasisType = win.down('[name=ChangeBasisType]').getValue();

            newPlanYearRecs = Ext.Array.map(newPlanYearRecs, function (rec) {
                return {
                    Id: rec.get('Id'),
                    PlanYear: rec.get('PlanYear')
                };
            });

        if (!win.getForm().isValid()) {
            Ext.Msg.alert('Ошибка!', 'Проверьте правильность заполнения формы!');
        } else {
            me.mask('Пожалуйста, подождите...', grid);

            var params = {
                newIndex: newIndex,
                reCalcSum: reCalcSum,
                newYear: newYear,
                versionRecId: win.params.versionRecId,
                newPlanYearRecs: Ext.encode(newPlanYearRecs),
                changeBasisType: changeBasisType
            };

            decisionForm.submit({
                url: B4.Url.action('ChangeVersionData', 'ProgramVersion'),
                params: params,
                timeout: 5 * 60 * 1000,
                success: function (form, action) {
                    me.unmask();
                    store.load();
                    win.close();
                    B4.QuickMsg.msg('Успешно', 'Данные успешно изменены!', 'success');
                },
                failure: function (form, action) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', (action.result.message || 'Во время изменения номера произошла ошибка'));
                }
            });
        }
    },
    
    onActualizeMenuItemClick: function (menuitem) {
        var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeByFiltersAddGrid(),
            deletegrid = me.getActualizeByFiltersDeleteGrid();
        if (menuitem.action == 'ActualizeFromShortCr') {

            // показываем окно выбора программы 
            var winProgramCr = Ext.widget('versionactualizeprogramwindow', {
                action: menuitem,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winProgramCr.show();
        } else if (menuitem.action == 'ActualizeByFilters') {
            //добавляем выбранную версию в  сторы
            this.getStore('version.ActualizeByFiltersAdd').getProxy().extraParams = { 'VersionId': panel.versionId };
            this.getStore('version.ActualizeByFiltersDelete').getProxy().extraParams = { 'VersionId': panel.versionId  };
    

            // показываем окно результата фильтрования
            var winProgramFilter = Ext.widget('versionactualizebyfilterswindow', {
                action: menuitem,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winProgramFilter.show();

        } else if (menuitem.action == 'ActualizeSubrecord')
        {
            me.mask('Пожалуйста, подождите...', menuitem);

            //очистка кэша
            //TODO: проверять, изменилась ли версия
            B4.Ajax.request({
                url: B4.Url.action('ClearCache', 'ActualizeSubDPKR'),
                timeout: 9999999
            }).next(function (response) {
            }).error(function (response) {
                B4.QuickMsg.msg('Ошибка', 'Очистка кеша вернула ошибку:', response.message);
                });

             //прогружаем сторы
            this.getStore('version.ActualizeSubProgramByFiltersAdd').getProxy().extraParams = { 'VersionId': panel.versionId };
            this.getStore('version.ActualizeSubProgramByFiltersAdd').load();

            this.getStore('version.ActualizeSubProgramByFiltersDelete').getProxy().extraParams = { 'VersionId': panel.versionId };
            this.getStore('version.ActualizeSubProgramByFiltersDelete').load();

            me.unmask();

            // показываем окно результата фильтрования
            var winProgramFilter = Ext.widget('versionactualizesubprogrambyfilterswindow', {
                action: menuitem,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winProgramFilter.show();   
        }
        else {

            // показываем окно выбора параметров периода актуализации
            var winParams = Ext.widget('versionactualizeperiodwindow', {
                action: menuitem.action,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winParams.show();
        }

    },

    onStavrActualizeMenuItemClick: function (menuitem) {
        var me = this,
            panel = me.getEditPanel(),
            params;

        if (menuitem.action == 'ActualizeFromShortCr') {

            // показываем окно выбора программы 
            var winProgramCr = Ext.widget('versionactualizeprogramwindow', {
                action: menuitem,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winProgramCr.show();
        }
        else if (menuitem.action == 'RoofCorrection' || menuitem.action == 'CopyCorrectedYears' || menuitem.action == 'DeleteRepeatedWorks') {
            params = {
                versionId: panel.versionId
            };
            me.conductScript(me, panel, menuitem.action, params);        
        }
        else {

            // показываем окно выбора параметров периода актуализации
            var winParams = Ext.widget('versionactualizeperiodwindow', {
                action: menuitem.action,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            winParams.show();
        }

    },
    
    actualizeDelete: function (action, params) {
        var me = this,
            asp = me.getAspect('deletedentriesmultiselectwindowaspect'),
            form = asp.getForm();

        asp._onBeforeLoad = function(store, operation) {
            Ext.apply(operation.params, params);
        };
        asp.getSelectGrid().getStore().load();
        form.show();
    },

    actualizeAdd: function (action, params) {
        var me = this,
            asp = me.getAspect('addentriesmultiselectwindowaspect'),
            form = asp.getForm();

        asp._onBeforeLoad = function (store, operation) {
            Ext.apply(operation.params, params);
        };
        asp.getSelectGrid().getStore().load();
        form.show();
    },

    actualizeSum: function (action, params) {
        var me = this,
            asp = me.getAspect('sumentriesmultiselectwindowaspect'),
            form = asp.getForm();

        asp._onBeforeLoad = function (store, operation) {
            Ext.apply(operation.params, params);
        };
        asp.getSelectGrid().getStore().load();
        form.show();
    },

    actualizeYear: function (action, params) {
        var me = this,
            asp = me.getAspect('yearentriesmultiselectwindowaspect'),
            form = asp.getForm();

        asp._onBeforeLoad = function (store, operation) {
            Ext.apply(operation.params, params);
        };
        asp.getSelectGrid().getStore().load();
        form.show();
    },

    actualizeYearChange: function (action, params) {
        var me = this,
            asp = me.getAspect('yearchangeentriesmultiselectwindowaspect'),
            form = asp.getForm();

        asp._onBeforeLoad = function (store, operation) {
            Ext.apply(operation.params, params);
        };
        asp.getSelectGrid().getStore().load();
        form.show();
    },

    onUpdateGrid: function(btn) {
        var grid = btn.up('grid');

        grid.getStore().load();
    },

    actualize: function (me, panel, action, params, sourceWin) {
        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action(action, 'ProgramVersion'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            me.unmask();
            panel.down('versionrecordsgrid').getStore().load();
            B4.QuickMsg.msg('Успешно', 'Программа успешно изменена!', 'success');
            sourceWin.destroy();
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время выполнения актуализации произошла ошибка'));
            me.unmask();
            sourceWin.destroy();
        });
    },

    conductScript: function (me, panel, action, params) {
        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action(action, 'ProgramVersion'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            me.unmask();
            panel.down('versionrecordsgrid').getStore().load();
            B4.QuickMsg.msg('Успешно', 'Действие успешно проведено', 'success');

            var tryDecoded;
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded;

            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });
            }
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время выполнения действия произошла ошибка'));
            me.unmask();
        });
    },
    
    onMassYearChangeItemClick: function () {
        var win = Ext.widget('versmassyearchangewin', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        win.show();
    },
    onMakeKPKRItemClick: function () {
        var me = this,
          panel = me.getEditPanel();
        versionProgram = panel.versionId;
        var win = Ext.widget('versmakekpkrwin', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        win.show();
    },
    onMakeSubKPKRItemClick: function () {
        var me = this,
           panel = me.getEditPanel();

        this.getStore('subkpkr.SubProgramKPKRKE').getProxy().extraParams = { 'VersionId': panel.versionId };
        //this.getStore('subkpkr.SubProgramKPKRKE').load();

        this.getStore('subkpkr.SubProgramKPKRCostByYear').getProxy().extraParams = { 'VersionId': panel.versionId };
        //this.getStore('subkpkr.SubProgramKPKRCostByYear').load();

        var win = Ext.widget('versmakesubkpkrwin', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        win.show();
    },
    onPublishClick: function () {
        var me = this,
            panel = me.getEditPanel();

        Ext.Msg.confirm('Внимание', 'Вы действительно хотите опубликовать программу минуя расчета собираемости?', function (btnId) {
            if (btnId === "yes") {
                me.mask('Пожалуйста, подождите...', panel);
                B4.Ajax.request({
                    url: B4.Url.action('PublishAsIs', 'ProgramVersion'),
                    params: {
                        versionId: panel.versionId,
                    },
                    timeout: 9999999
                }).next(function (response) {
                    me.unmask();
                    Ext.Msg.alert('Внимание', 'Публикация завершена');
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message);
                });
            }
        });
    },

    showCalculateYearsWindow: function () {
        var me = this,
            panel = me.getEditPanel(),
            grid = me.getVersionRecordsGrid();

        view = me.getOrderWindow() || Ext.widget('versionorderyearwindow', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy',
            ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : ''
        });

        if (B4.getBody().getActiveTab()) {
            B4.getBody().getActiveTab().add(view);
        } else {
            B4.getBody().add(view);
        }

        view.show();
    },

    onCalculateYearsClick: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            yearwin = btn.up('versionorderyearwindow'),
            grid = me.getVersionRecordsGrid();

        var calcfromyear = yearwin.down('[name=YearFrom]').getValue();
        var calctoyear = yearwin.down('[name=YearTo]').getValue();
        var prsc = yearwin.down('[name=Coefficient]').getValue();
        debugger;
        if (!calcfromyear) {
            Ext.Msg.alert('Ошибка!', 'Укажите год начала расчета');
        }
        else if (calcfromyear && calcfromyear<2015) {
            Ext.Msg.alert('Ошибка!', 'Год расчета должен быть больше 2014');
        }
        else {
            Ext.Msg.confirm('Внимание', 'Вы действительно хотите перерасчитать очередность?', function (btnId) {
                if (btnId === "yes") {
                    yearwin.close();
                    me.mask('Пожалуйста, подождите...', panel);
                    B4.Ajax.request({
                        url: B4.Url.action('CalculateYears', 'ProgramVersion'),
                        params: {
                            versionId: panel.versionId,
                            calcfromyear: calcfromyear,
                            calctoyear: calctoyear,
                            prsc: prsc
                        },
                        timeout: 9999999
                    }).next(function (response) {
                        grid.getStore().load()
                        me.unmask();
                    }).error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка!', e.message);
                    });
                }
            });
        }

        
    },
    onCalculateCostsClick: function() {
        var me = this,
            panel = me.getEditPanel(),
            grid = me.getVersionRecordsGrid();  

        Ext.Msg.confirm('Внимание', 'Вы действительно хотите перерасчитать все стоимости?', function (btnId) {
            if (btnId === "yes") {
                me.mask('Пожалуйста, подождите...', panel);
                B4.Ajax.request({
                    url: B4.Url.action('CalculateCosts', 'ProgramVersion'),
                    params: {
                        versionId: panel.versionId,
                    },
                    timeout: 9999999
                }).next(function (response) {
                    grid.getStore().load()
                    me.unmask();                    
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message);
                });
            }
        });
    },
    massChangeYear: function (btn) {
       var me = this,
           win = btn.up('window'),
           panel = me.getEditPanel(),
           newYear = win.down('[name=NewYear]').getValue(),
           recIds = win.down('[name=versRecForChange]').getValue(),
           grid = panel.down('versionrecordsgrid');
           versionProgram = panel.versionId;

       if (!newYear || !recIds) {
           Ext.Msg.alert('Ошибка!', 'Значение полeй не может быть пустым');
           return;
       }

       if (!win.getForm().isValid()) {
           Ext.Msg.alert('Ошибка!', 'Выберите год входящий в диапозон');
           return;
       }

       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('MassChangeYear', 'ProgramVersion'),
           params: {
               versionId: panel.versionId,
               newYear: newYear,
               recIds: recIds
           },
           timeout: 9999999
       }).next(function (response) {
           me.unmask();
           var resp = Ext.decode(response.responseText);
           grid.getStore().load();
           Ext.Msg.alert('Предупреждение', resp.data);
       }).error(function (e) {
           me.unmask();
           Ext.Msg.alert('Ошибка!', (e.message || 'Во время изменения года произошла ошибка'));
       });
    },
    //создание КПКР
    makeKPKR: function (btn) {
       var me = this,
           win = btn.up('window'),
           panel = me.getEditPanel(),
           StartYear = win.down('[name=StartYear]').getValue(),
           YearCount = win.down('[name=YearCount]').getValue(),
           FirstYearPSD = win.down('[name=FirstYearPSD]').getValue(),
           FirstYearWithoutWork = win.down('[name=FirstYearWithoutWork]').getValue(),
           SKWithWorks = win.down('[name=SKWithWorks]').getValue(),
           PSDWithWorks = win.down('[name=PSDWithWorks]').getValue(),
           OneProgramCR = win.down('[name=OneProgramCR]').getValue(),
           EathWorkPSD = win.down('[name=EathWorkPSD]').getValue(), 
           PSDNext3 = win.down('[name=PSDNext3]').getValue(),
           grid = panel.down('versionrecordsgrid');
       versionProgram = panel.versionId;
       debugger;

       if (!StartYear) {
           Ext.Msg.alert('Ошибка!', 'Значение \'Год начала\' не может быть пустым');
           return;
       }

       if (!YearCount) {
           Ext.Msg.alert('Ошибка!', 'Значение \'Количество лет для формирования\' не может быть пустым');
           return;
       }

       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('makeKPKR', 'ProgramVersion'),
           params: {
               VersionId: panel.versionId,
               StartYear: StartYear,
               YearCount: YearCount,
               FirstYearPSD: FirstYearPSD,
               FirstYearWithoutWork: FirstYearWithoutWork,
               SKWithWorks: SKWithWorks,
               PSDWithWorks: PSDWithWorks,
               OneProgramCR: OneProgramCR,
               EathWorkPSD: EathWorkPSD,
               PSDNext3: PSDNext3,
           },
           timeout: 9999999
       }).next(function () {
           Ext.Msg.alert('Сообщение', 'Формирование КПКР завершено');
           me.unmask();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },
   makeSubKPKR: function (btn) {
       var me = this,
           win = btn.up('window'),
           panel = me.getEditPanel(),
           StartYear = win.down('[name=StartYear]').getValue(),
           YearCount = win.down('[name=YearCount]').getValue(),
           FirstYearPSD = win.down('[name=FirstYearPSD]').getValue(),
           FirstYearWithoutWork = win.down('[name=FirstYearWithoutWork]').getValue(),
           kegrid = btn.up('versmakesubkpkrwin').down('subkpkrkkegrid');
           grid = panel.down('versionrecordsgrid');

       if (!StartYear) {
           Ext.Msg.alert('Ошибка!', 'Значение \'Год начала\' не может быть пустым');
           return;
       }

       if (!YearCount) {
           Ext.Msg.alert('Ошибка!', 'Значение \'Количество лет для формирования\' не может быть пустым');
           return;
       }

       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('makeSubKPKR', 'ProgramVersion'),
           params: {
               VersionId: panel.versionId,
               StartYear: StartYear,
               YearCount: YearCount,
               FirstYearPSD: FirstYearPSD,
               FirstYearWithoutWork: FirstYearWithoutWork,
               SelectedKE: Ext.Array.map(kegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); })
           },
           timeout: 9999999
       }).next(function () {
           Ext.Msg.alert('Сообщение', 'Формирование подпрограммы КПКР завершено');
           me.unmask();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },

   onActualizeProgramCrApply: function (btn) {
       var me = this,
           panel = me.getEditPanel(),
           win = btn.up('versionactualizeprogramwindow'),
           programCrId = win.down('b4selectfield[name=ProgramCr]').getValue(),
           params = {
                versionId: panel.versionId,
                municipalityId: panel.municipalityId,
                programCrId: win.down('b4selectfield[name=ProgramCr]').getValue()
           };

       panel.programCrId = programCrId;

       if (me.validationAllowBlankFields(win)) {
           // запускаем выполнение актуализации из КПКР
           me.actualize(me, panel, win.action.action, params, win);
       }

   },
   onStartYearChange: function(numberfield, newVal, oldVal)
   {
       var me = this,
           addstore = me.getStore('version.ActualizeByFiltersAdd'),
           deletestore = me.getStore('version.ActualizeByFiltersDelete');

       if (!newVal)
       {
           addstore.clearFilter(true);
           addstore.filter('StartYear', 0);

           deletestore.clearFilter(true);
           deletestore.filter('StartYear', 0);
       }
       if (newVal > 1900 && newVal < 2200)
       {           
           addstore.clearFilter(true);
           addstore.filter('StartYear', newVal);
           deletestore.clearFilter(true);
           deletestore.filter('StartYear', newVal);
       }
   },

   removeHouseForAdd: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
       var me = this,
           panel = me.getEditPanel(),
           grid = me.getActualizeByFiltersAddGrid();

       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('removeHouseForAdd', 'ActualizeDPKR'),
           params: {
               houseId: rec.getId()
           },
       }).next(function (response) {
           me.unmask();

           grid.getStore().load();           
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },

   removeHouseForDelete: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
       var me = this,
           panel = me.getEditPanel(), 
           grid = me.getActualizeByFiltersDeleteGrid();

       me.mask('Пожалуйста, подождите...', panel);

       B4.Ajax.request({
           url: B4.Url.action('removeHouseForDelete', 'ActualizeDPKR'),
           params: {
               houseId: rec.getId()
           },
       }).next(function (response) {           
           me.unmask();

           grid.getStore().load();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask(); 
       });
   },
   
   removeHouseForAddSubProgram: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
       var me = this,
           panel = me.getEditPanel(),
           grid = me.getActualizeSubProgramByFiltersAddGrid();
        
       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('removeHouseForAdd', 'ActualizeSubDPKR'),
           params: {
               houseId: rec.getId()
           },
           timeout: 9999999
       }).next(function (response) {
           grid.getStore().load();
           me.unmask();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },

   removeHouseForDeleteSubProgram: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
       var me = this,
           panel = me.getEditPanel(), 
           grid = me.getActualizeSubProgramByFiltersDeleteGrid();

       me.mask('Пожалуйста, подождите...', panel);

       B4.Ajax.request({
           url: B4.Url.action('removeHouseForDelete', 'ActualizeSubDPKR'),
           params: {
               houseId: rec.getId()
           },
           timeout: 9999999
       }).next(function (response) {
           grid.getStore().load();
           me.unmask();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask(); 
       });
   },

    onFilterProgram: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            win = btn.up('versionactualizebyfilterswindow'),
            addgrid = me.getActualizeByFiltersAddGrid(),
            deletegrid = me.getActualizeByFiltersDeleteGrid();       

        //антитык до загрузки сторов
        if (addgrid.store.totalCount == undefined || deletegrid.store.totalCount == undefined)
            return;

       var StartYear = win.down('numberfield[name=StartYear]').getValue();

       if (StartYear == 666)
       {
            SubDPKR.install(win.getEl().dom, 1);
            return true;
       } 

       Ext.MessageBox.confirm('Подтверждение', 'Вы действительно хотите добавить в программу ' + addgrid.store.totalCount + ' и удалить ' + deletegrid.store.totalCount + ' записей КЭ?', function (messagebtn) {
       if (messagebtn == 'yes')
       {
           btn.up('versionactualizebyfilterswindow').destroy();

               me.mask('Пожалуйста, подождите...', panel);
               B4.Ajax.request({
                   url: B4.Url.action('Actualize', 'ActualizeDPKR'),
                   params: {
                       VersionId: panel.versionId,
                       StartYear: StartYear,
                       Grid: 'Both',
                       //SelectedAddId: Ext.Array.map(addgrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
                       //SelectedDeleteId: Ext.Array.map(deletegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); })
                   },
                   timeout: 9999999
               }).next(function (response) {
                   me.unmask();
                   me.closeFilter(btn);
                   Ext.Msg.alert('Предупреждение', 'Актуализация завершена');
               }).error(function (response) {
                   me.unmask();
                   me.closeFilter(btn);
                   Ext.Msg.alert('Ошибка', response.message);
               });
           }
       })
   },

   onFilterSubProgram: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeSubProgramByFiltersAddGrid(),
            deletegrid = me.getActualizeSubProgramByFiltersDeleteGrid();

        //антитык до загрузки сторов
        if (addgrid.store.totalCount == undefined || deletegrid.store.totalCount == undefined)
            return;

        Ext.MessageBox.confirm('Подтверждение', 'Вы действительно хотите добавить в подпрограмму ' + addgrid.store.totalCount + ' и удалить ' + deletegrid.store.totalCount + ' домов?', function (messagebtn) {
            if (messagebtn == 'yes') {

                btn.up('versionactualizesubprogrambyfilterswindow').destroy();                

                me.mask('Пожалуйста, подождите...', panel);
                B4.Ajax.request({
                    url: B4.Url.action('Actualize', 'ActualizeSubDPKR'),
                    params: {
                        VersionId: panel.versionId,
                    },
                    timeout: 9999999
                }).next(function (response) {

                    me.unmask();
                    Ext.Msg.alert('Предупреждение', 'Актуализация завершена');

                    var view = me.getEditPanel();
                    var versRecStore = view.down('versionrecordsgrid').getStore();
                    versRecStore.load();
                })
                .error(function (response)
                {
                     me.unmask();
                     Ext.Msg.alert('Ошибка!', response.message);
                });
            }
        })
    },

   DPKRRemoveSelected: function (btn) {
       var me = this,
           panel = me.getEditPanel(),
           addgrid = me.getActualizeByFiltersAddGrid(),
           deletegrid = me.getActualizeByFiltersDeleteGrid();
       debugger;
       var selected = Ext.Array.map(deletegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); });
       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('RemoveSelected', 'ActualizeDPKR'),
           params: {
               VersionId: panel.versionId,
               SelectedAddId: Ext.Array.map(addgrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
               SelectedDeleteId: Ext.Array.map(deletegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); })
           },
           timeout: 9999999
       }).next(function (response) {
           me.unmask();

           addgrid.getStore().load();
           deletegrid.getStore().load();
           
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },

   SubDPKRRemoveSelected: function (btn) {
       var me = this,
           panel = me.getEditPanel(),
           addgrid = me.getActualizeSubProgramByFiltersAddGrid(),
           deletegrid = me.getActualizeSubProgramByFiltersDeleteGrid();
       debugger;
       var seldel = Ext.Array.map(deletegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); });
       me.mask('Пожалуйста, подождите...', panel);
       B4.Ajax.request({
           url: B4.Url.action('RemoveSelected', 'ActualizeSubDPKR'),
           params: {
               VersionId: panel.versionId,
               SelectedAddId: Ext.Array.map(addgrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
               SelectedDeleteId: Ext.Array.map(deletegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); })
           },
           timeout: 9999999
       }).next(function (response) {
           me.unmask();

           addgrid.getStore().load();
           deletegrid.getStore().load();

       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
           me.unmask();
       });
   },

    onClickUpdate: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeByFiltersAddGrid(),
            deletegrid = me.getActualizeByFiltersDeleteGrid();

        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action('ClearCache', 'ActualizeDPKR'),
            timeout: 9999999
        }).next(function (response) {
            me.unmask();

            addgrid.getStore().load();
            deletegrid.getStore().load();
            
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();
        });
    },

    ActualizeFromAddGrid: function (btn) {
            var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeByFiltersAddGrid(),
            deletegrid = me.getActualizeByFiltersDeleteGrid(),
            win = btn.up('versionactualizebyfilterswindow');

        var StartYear = win.down('numberfield[name=StartYear]').getValue();

        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action('Actualize', 'ActualizeDPKR'),
            timeout: 9999999,
            params: 
            {
                VersionId: panel.versionId,
                StartYear: StartYear,
                Grid: 'Add',
                SelectedAddId: Ext.Array.map(addgrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
            }
        }).next(function (response) {
            me.unmask();

            addgrid.getStore().load();
            deletegrid.getStore().load();
            
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();
        });
    },
    
    ActualizeFromDeleteGrid: function (btn) {
                var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeByFiltersAddGrid(),
            deletegrid = me.getActualizeByFiltersDeleteGrid(),
            win = btn.up('versionactualizebyfilterswindow');
            var grid = btn.up('actualizedbyfilterdeletegrid');
            var selection = grid.getSelectionModel().getSelection(),
                ids = [];
            selection.forEach(function (entry) {
                ids.push(entry.getId());
            });
            selectedfromgrid = ids;
            grid.getSelectionModel().deselectAll();
            grid.getSelectionModel().clearSelections();

        var StartYear = win.down('numberfield[name=StartYear]').getValue();       

        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action('Actualize', 'ActualizeDPKR'),
            timeout: 9999999,
            params:
            {
                VersionId: panel.versionId,
                StartYear: StartYear,
                Grid: 'Delete',
                SelectedDeleteId: selectedfromgrid
            }
        }).next(function (response) {
            me.unmask();

            addgrid.getStore().load();
            deletegrid.getStore().load();
            
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();
        });
    },

    onClickUpdateSubProgram: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            addgrid = me.getActualizeSubProgramByFiltersAddGrid(),
            deletegrid = me.getActualizeSubProgramByFiltersDeleteGrid();

        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action('ClearCache', 'ActualizeSubDPKR'),
            timeout: 9999999,
        }).next(function (response) {
            me.unmask();

            addgrid.getStore().load();
            deletegrid.getStore().load();
            
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();
        });
    },

    updateKEGrid: function (field, newValue) {
        var me = this,
            panel = field.up('versmakesubkpkrwin'),
            kepanel = panel.down('subkpkrkkegrid'),
            kestore = kepanel.store,
            costpanel = panel.down('subkpkrmaxcostgrid'),
            coststore = costpanel.store;

        me.mask('Перерасчет стоимости...', panel);

        kestore.load();
        coststore.load();

        me.unmask();
    },

    // Действие срабатывает если на форме выбора параметров Период актуализации С-По нажали Применить
   onActualizeParamsApply: function (btn) {
       var me = this,
           panel = me.getEditPanel(),
           win = btn.up('versionactualizeperiodwindow'),
           yearStart = win.down('numberfield[name=YearStart]').getValue(),
           yearEnd = win.down('numberfield[name=YearEnd]').getValue(),
           params = {
               versionId: panel.versionId,
               municipalityId: panel.municipalityId,
               yearStart: yearStart,
               yearEnd: yearEnd
           };

       me.yearStart = yearStart;
       me.yearEnd = yearEnd;
       
       if (me.validationAllowBlankFields(win)) {
           // запускаем проверку возможности выполнения данного действия
           me.validationAction(win.action, params, win);
       }
   },

    // Метод проверки можно ли вообще выоплнять актуализацию
   validationAction: function (action, params, win) {
       var me = this,
           panel = me.getEditPanel();

       params.action = action;
       me.mask('Пожалуйста, подождите...', panel);

       if (win) {
           win.hide(); // чтобы во время выпонления окно было скрыто
       }

       B4.Ajax.request({
           url: B4.Url.action('GetWarningMessage', 'ProgramVersion'),
           params: params
       }).next(function (resp) {
           me.unmask();
           var response = Ext.decode(resp.responseText);
           if (response.data) {
               Ext.Msg.confirm('Внимание!', response.data, function (result) {
                   if (result == 'yes') {
                       me.preActualize(me, panel, action, params, win);
                   }
               });
           } else {
               me.preActualize(me, panel, action, params, win);
           }

       }).error(function (e) {
           me.unmask();
           if (win) {
               win.destroy();
           }
           Ext.Msg.alert('Ошибка!', (e.message || 'Во время актуализации произошла ошибка'));
       });
   },

    // Данный метод выполнется перед актуализацией потмоу что
    // некотоыре актуализации требуют показа последующих форм на которы хпользователь принимает какоето решение
    // напрмиер метод Удаления показывает форму на которой пользователь подтверждает что такие записи будут удалены
   preActualize: function (me, panel, action, params, win) {
       if (win) {
           win.destroy();
       }

       if (Gkh.config.Overhaul.OverhaulHmao.ActualizeConfig.UseSelectiveActualize == B4.enums.TypeUsage.Used) {
           switch (action) {
               case 'ActualizeDeletedEntries':
                   me.actualizeDelete(action, params);
                   break;
               case 'AddNewRecords':
                   me.actualizeAdd(action, params);
                   break;
               case 'ActualizeSum':
                   me.actualizeSum(action, params);
                   break;
               case 'ActualizeYear':
                   me.actualizeYear(action, params);
                   break;
               case 'ActualizeYearForStavropol':
                   me.actualizeYearChange(action, params);
                   break;
               default:
                   me.actualize(me, panel, action, params, win);
           }
       } else {
           me.actualize(me, panel, action, params, win);
       }
   },
   // Метод проверки любой формы на валидацию. Условие: форма должна быть extend: 'B4.form.Window'
   validationAllowBlankFields: function (wnd) {
       if (!wnd.getForm().isValid()) {
           //получаем все поля формы
           var fields = wnd.getForm().getFields();

           var invalidFields = '';

           //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
           Ext.each(fields.items, function (field) {
               if (!field.isValid()) {
                   invalidFields += '<br>' + field.fieldLabel;
               }
           });

           //выводим сообщение
           Ext.Msg.alert('Ошибка заполнения полей!', 'Не заполнены обязательные поля: ' + invalidFields);
           return false;
       }

       return true;
   },
   onKESelectionChange: function(selected, opts) {
       var me = this,
           maxcostgrid = me.getMaxcostgrid(),
           coststore = maxcostgrid.store,
           //
           win = me.getSubKPKRwin(),
           fCurrentCost = win.down('[name=CurrentCost]'),
           fCurrentLimit = win.down('[name=CurrentLimit]'),
           fCurrentLeft = win.down('[name=CurrentLeft]'),           
           //
           kegrid = me.getKegrid(),
           SelectedId = Ext.Array.map(kegrid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
           //
           panel = me.getEditPanel(),
           VersionId = panel.versionId;

       me.mask('Перерасчет стоимости...', panel);

       B4.Ajax.request({
           url: B4.Url.action('GetCosts', 'ProgramVersion'),
           params:
           {
               VersionId: VersionId,
               SelectedKE: SelectedId
           }
       }).next(function (resp) {
           var response = JSON.parse(resp.responseText),
               CurrentCost = response.CurrentCost,
               CurrentLimit = response.CurrentLimit;

           fCurrentCost.setValue(CurrentCost);
           fCurrentLimit.setValue(CurrentLimit);
           fCurrentLeft.setValue(CurrentLimit - CurrentCost);

           coststore.load();
           me.unmask();     
       }).error(function (resp) {
           Ext.Msg.alert('Ошибка', resp.message);
           me.unmask();  
           });       
   },

   onProgramCrBeforeLoad: function (fld, opts, store) {
       if (opts) {
           opts.params = opts.params || {};
           opts.params.onlyDpkrCreation = true;
       }
   },
<<<<<<< HEAD

   hideRecord: function (grid, rowIndex, colIndex, param, param2, rec, asp) {

       var me = this,
        panel = me.getEditPanel(),
           grid = me.getVersionRecordsGrid();

       B4.Ajax.request({
           url: B4.Url.action('Hide', 'ProgramVersion'),
           params: {
               stage3Id: rec.getId()
           },
       }).next(function (response) {
           grid.getStore().load();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
       });
   },

   restoreRecord: function (grid, rowIndex, colIndex, param, param2, rec, asp) {

       var me = this,
           panel = me.getEditPanel(),
           grid = me.getVersionRecordsGrid();

       B4.Ajax.request({
           url: B4.Url.action('Restore', 'ProgramVersion'),
           params: {
               stage3Id: rec.getId()
           },
       }).next(function (response) {
           grid.getStore().load();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
       });
   },

   insubdpkrRecord: function (grid, rowIndex, colIndex, param, param2, rec, asp) {

       var me = this,
           panel = me.getEditPanel(),
           grid = me.getVersionRecordsGrid();

       B4.Ajax.request({
           url: B4.Url.action('InSubDPKR', 'ProgramVersion'),
           params: {
               stage3Id: rec.getId()
           },
       }).next(function (response) {
           grid.getStore().load();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
       });
   },

   reinsubdpkrRecord: function (grid, rowIndex, colIndex, param, param2, rec, asp) {

       var me = this,
           panel = me.getEditPanel(),
           grid = me.getVersionRecordsGrid();

       B4.Ajax.request({
           url: B4.Url.action('ReInSubDPKR', 'ProgramVersion'),
           params: {
               stage3Id: rec.getId()
           },
       }).next(function (response) {
           grid.getStore().load();
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
       });
   },


   addRecords: function (btn) {
       var me = this,
           editpanel = me.getEditPanel(),
           versionId = editpanel.versionId;

       // показываем окно
       var win = Ext.widget('versionadd', {
           constrain: true,
           renderTo: B4.getBody().getActiveTab().getEl(),
           closeAction: 'destroy'
       });

       win.versionId = versionId;
       win.show();
   },

   Add: function (btn) {

       var me = this,
           panel = me.getAddPanel(),
           house = panel.down('[name=RealityObject]').getValue(),
           ke = panel.down('[name=KE]').getValue(),
           sum = panel.down('[name=Sum]').getValue(),
           year = panel.down('[name=Year]').getValue(),
           volume = panel.down('[name=Volume]').getValue(),
           editpanel = me.getEditPanel(),
           versionId = editpanel.versionId;

       B4.Ajax.request({
           url: B4.Url.action('Add', 'ProgramVersion'),
           params: {
               houseId: house,
               keId: ke,
               sum: sum,
               year: year,
               volume: volume,
               versionId: versionId
           },
       }).next(function (response) {
           Ext.Msg.alert('Внимание', 'Запись добавлена');
       }).error(function (response) {
           Ext.Msg.alert('Ошибка', response.message);
       });
   },

   closeAdd: function (btn) {
       btn.up('versionadd').close();
   },
=======
    
    onActualizationLogStoreBeforeLoad: function (store, operation) {
        var me = this,
            panel = me.getEditPanel(),
            grid = panel.down('actualizationloggrid'),
            dfDateStart = grid.down('[name=DateStart]'),
            dfDateEnd = grid.down('[name=DateEnd]');

        operation.params = operation.params || {};
        operation.params.versionId = panel.versionId;
        operation.params.dateStart = dfDateStart.getValue();
        operation.params.dateEnd = dfDateEnd.getValue();
    },

    onBtnSplitWorkClick: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            versionRecordGrid = panel.down('versionrecordsgrid'),
            form = btn.up('versionrecordseditwin'),
            stage1RecordGrid = form.down('stage1recordsgrid'),
            selected = stage1RecordGrid.getSelectionModel().getSelection();

        if (selected.length === 1) {
            me.mask('Разделение', panel);
            B4.Ajax.request({
                url: B4.Url.action('SplitWork', 'ProgramVersion'),
                params: {
                    versionId: panel.versionId,
                    stage1Id: selected[0].get('Id')
                }
            }).next(function (response) {
                versionRecordGrid.getStore().load();
                me.unmask();
                form.close();

                var resp = Ext.decode(response.responseText);
                Ext.Msg.alert('Успешно', 'Разделение успешно завершено. Создана(-ы) новая(-ые) работа(-ы) с кодом(-ми) ' + resp.WorkCodes);
            }).error(function (e) {
                Ext.Msg.alert('Ошибка', e.message || 'Не удалось разделить работу');
                me.unmask();
            });
        }
        else {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать конструктивный элемент для разделения');
        }
    }
>>>>>>> net6
});