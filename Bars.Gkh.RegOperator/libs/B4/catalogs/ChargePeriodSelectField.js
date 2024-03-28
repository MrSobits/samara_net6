Ext.define('B4.catalogs.ChargePeriodSelectField', {
    extend: 'B4.form.SelectField',
    alias: 'widget.chargeperiodselectfield',
    titleWindow: 'Выбор периода',
    store: 'B4.store.regop.ChargePeriod',

    labelAlign: 'right',
    fieldLabel: 'Период',
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