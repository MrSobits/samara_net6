Ext.define('B4.catalogs.FinanceSourceSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор разреза финансирования',
    store: 'B4.store.dict.FinanceSource',
    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    editable: false
});