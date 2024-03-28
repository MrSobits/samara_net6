Ext.define('B4.catalogs.ProgramCrSelectField', {
    extend: 'B4.form.SelectField',
    alias: 'widget.programcrselectfield',
    requires: [
        'B4.store.dict.ProgramCr',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeVisibilityProgramCr',
        'B4.enums.TypeProgramStateCr'
    ],

    titleWindow: 'Выбор программы капитального ремонта',
    store: 'B4.store.dict.ProgramCr',
    columns: [
        {
            xtype: 'gridcolumn',
            dataIndex: 'Name',
            flex: 3,
            text: 'Наименование',
            filter: { xtype: 'textfield' }
        },
        {
            xtype: 'gridcolumn',
            dataIndex: 'PeriodName',
            flex: 2,
            text: 'Период',
            filter: { xtype: 'textfield' }
        },
        {
            xtype: 'b4enumcolumn',
            dataIndex: 'TypeVisibilityProgramCr',
            width: 80,
            text: 'Видимость',
            enumName: 'B4.enums.TypeVisibilityProgramCr',
            filter: true
        },
        {
            xtype: 'b4enumcolumn',
            dataIndex: 'TypeProgramStateCr',
            width: 100,
            text: 'Состояние',
            enumName: 'B4.enums.TypeProgramStateCr',
            filter: true
        }
    ],
    editable: false
});