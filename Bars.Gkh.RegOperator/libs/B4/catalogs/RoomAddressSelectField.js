Ext.define('B4.catalogs.RoomAddressSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор адреса помещения',
    store: 'B4.store.RoomAddress',
    textProperty: 'Address',
    columns: [
        {
            text: 'Муниципальный район',
            dataIndex: 'Municipality',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Адрес',
            dataIndex: 'Address',
            flex: 1,
            filter: { xtype: 'textfield' }
        }
    ],
    editable: false
});