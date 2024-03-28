Ext.define('B4.catalogs.DeliveryAgentSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор агента доставки',
    store: 'B4.store.deliveryagent.DeliveryAgent',
    columns: [
        {
            text: 'Муниципальный район',
            dataIndex: 'Municipality',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Наименование контрагента',
            dataIndex: 'Name',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Статус',
            dataIndex: 'ContragentState',
            flex: 1,
            filter: { xtype: 'textfield' }
        }
    ],
    editable: false
});