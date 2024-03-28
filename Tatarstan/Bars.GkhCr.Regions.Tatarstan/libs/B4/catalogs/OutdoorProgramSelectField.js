Ext.define('B4.catalogs.OutdoorProgramSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор программы благоустройства дворов',
    store: 'B4.store.dict.RealityObjectOutdoorProgramSelected',
    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    editable: false
});