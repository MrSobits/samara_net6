Ext.define('B4.controller.report.PhotoArchiveReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PhotoArchivePanel',
    mainViewSelector: '#photoArchivePanel',
    programCrId: 0,
    municipalities: "",
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'objectcr.RealityObjectsByProgramm',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#photoArchivePanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#photoArchivePanel #sfProgramCr'
        },
        {
            ref: 'RealityObjectTriggerField',
            selector: '#photoArchivePanel #tfRealityObject'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'photoArchiveMultiselectwindowaspect',
            fieldSelector: '#photoArchivePanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrReportPanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
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
                getdata: function(asp, records) {
                    var str = '';

                    records.each(function(rec) {
                        if (str)
                            str += ';';
                        str += rec.getId();
                    });

                    this.controller.municipalities = str;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'realityObjectsMultiselectwindowaspect',
            fieldSelector: '#photoArchivePanel #tfRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#PhotoArchRealObjSelectWindow',
            storeSelect: 'objectcr.RealityObjectsByProgramm',
            storeSelected: 'realityobj.RealityObjectForSelected',
            textProperty: 'Address',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function(store, operation) {
                operation.params = {};
                operation.params.programCrId = this.controller.programCrId;
                operation.params.municipalities = this.controller.municipalities;
            }
        }
    ],

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var municipalitiesField = this.getMunicipalityTriggerField();
        var programField = this.getProgramCrSelectField();
        var realityObjectField = this.getRealityObjectTriggerField();

        return {
            ProgramCr: programField ? programField.getValue() : null,
            Municipalities: municipalitiesField ? municipalitiesField.getValue() : null,
            RealityObjs: realityObjectField ? realityObjectField.getValue() : null
        };
    },

    init: function() {
        var actions = {};
        actions['#photoArchivePanel #sfProgramCr'] = {
            'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this },
            'change': { fn: this.onChangeProgrammCr, scope: this }
        };
        this.control(actions);

        this.callParent(arguments);
    },

    onChangeProgrammCr: function(field, newValue) {
        if (newValue) {
            this.programCrId = newValue.Id;
        } else {
            this.programCrId = 0;
        }

        var panel = this.getMainView();
        var tfRealityObject = panel.down('#tfRealityObject');
        tfRealityObject.setValue('');
        tfRealityObject.updateDisplayedText();
    },

    onBeforeLoadProgramCr: function(field, options) {
        options.params = {};
        options.params.onPrintorFull = true;
    }
});