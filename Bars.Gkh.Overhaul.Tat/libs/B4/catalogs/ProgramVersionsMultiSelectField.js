Ext.define('B4.catalogs.ProgramVersionsMultiSelectField', {
    extend: 'B4.view.Control.GkhTriggerField',
    alias: 'widget.programversionsmultiselectfield', 

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'],

    aspects: [
        {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'programRecordsMultiselectwindowaspect',
        fieldSelector: 'programversionsmultiselectfield',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#programRecordsMultiselectwindow',
        storeSelect: 'dict.ProgramVersionsForSelect',
        storeSelected: 'dict.ProgramVersionsForSelected',
        columnsGridSelect: [
            {
                text: 'Наименование', dataIndex: 'Name', flex: 2, filter: { xtype: 'textfield' }
            },
            { text: 'Период', dataIndex: 'PeriodName', flex: 1.2, filter: { xtype: 'textfield' } },
            { xtype: 'b4enumcolumn', enumName: 'B4.enums.TypeVisibilityProgramCr', text: 'Видимость', dataIndex: 'TypeVisibilityProgramCr', flex: 1, filter: true },
            { xtype: 'b4enumcolumn', enumName: 'B4.enums.TypeProgramStateCr', text: 'Состояние', dataIndex: 'TypeProgramStateCr', flex: 1, filter: true }
        ],
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
        ],
        titleSelectWindow: 'Выбор записи',
        titleGridSelect: 'Записи для отбора',
        titleGridSelected: 'Выбранная запись'
    }]
});