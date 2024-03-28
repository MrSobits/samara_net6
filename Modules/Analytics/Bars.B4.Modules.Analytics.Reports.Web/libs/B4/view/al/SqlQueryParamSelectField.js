Ext.define('B4.view.al.SqlQueryParamSelectField', {
    extend: 'B4.form.SelectField',
    alias: 'widget.sqlqueryselectfield',
    requires: [
        'B4.base.Store',
        'B4.model.al.SqlQueryParam'
    ],
    titleWindow: 'Выберите значение',
    columns: [{
        text: 'Наименование',
        dataIndex: 'Name',
        flex: 1,
        filter: { xtype: 'textfield' }
    }],
    //textProperty: 'FullAddress',
    editable: false,
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.al.SqlQueryParam',
                autoLoad: false
            });

        me.store = store;

        me.callParent(arguments);
    }
});