Ext.define('B4.controller.report.AppealCitsWorkingReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AppealCitsWorkingReportPanel',
    mainViewSelector: '#appealCitsWorkingReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'reportDateStartField',
            selector: '#appealCitsWorkingReportPanel #dfReportDateStart'
        },
        {
            ref: 'reportDateEndField',
            selector: '#appealCitsWorkingReportPanel #dfReportDateEnd'
        },
        {
            ref: 'inspectorsField',
            selector: '#appealCitsWorkingReportPanel #tfInspectors'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealCitsWorkingReportInspectorMultiSelectWindowAspect',
            fieldSelector: '#appealCitsWorkingReportPanel #tfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы'
        }
    ],

    getParams: function() {

        var reportDateStartField = this.getReportDateStartField();
        var reportDateEndField = this.getReportDateEndField();
        var inspectorsField = this.getInspectorsField();

        return {
            dateStart: (reportDateStartField ? reportDateStartField.getValue() : null),
            dateEnd: (reportDateEndField ? reportDateEndField.getValue() : null),
            inspectors: (inspectorsField ? inspectorsField.getValue() : null)
        };
    }
});