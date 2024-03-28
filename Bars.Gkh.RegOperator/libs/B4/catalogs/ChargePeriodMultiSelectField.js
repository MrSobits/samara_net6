Ext.define('B4.catalogs.ChargePeriodMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор периода',
    store: 'B4.store.regop.ChargePeriod',
    selectionMode: 'MULTI',
    columns: [
        {
            text: 'Наименование',
            dataIndex: 'Name',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Дата открытия',
            xtype: 'datecolumn',
            format: 'd.m.Y',
            dataIndex: 'StartDate',
            flex: 1,
            filter: { xtype: 'datefield' }
        },
        {
            text: 'Дата закрытия',
            xtype: 'datecolumn',
            format: 'd.m.Y',
            dataIndex: 'EndDate',
            flex: 1,
            filter: { xtype: 'datefield' }
        },
        {
            text: 'Состояние',
            dataIndex: 'IsClosed',
            flex: 1,
            renderer: function (value) {
                return value ? 'Закрыт' : 'Открыт';
            }
        }],
    editable: false
});