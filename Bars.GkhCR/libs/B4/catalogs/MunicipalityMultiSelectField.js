Ext.define('B4.catalogs.MunicipalityMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор муниципального района',
    store: 'B4.store.dict.Municipality',
    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    selectionMode: 'MULTI',
    editable: false
});