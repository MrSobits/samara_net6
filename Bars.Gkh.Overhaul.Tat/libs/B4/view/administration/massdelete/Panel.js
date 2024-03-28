Ext.define('B4.view.administration.massdelete.Panel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.store.administration.massdelete.RealityObject',
        'B4.view.administration.massdelete.SelectGrid',
        'B4.form.SelectField'
    ],

    title: 'Массовое удаление КЭ',
    alias: 'widget.massdeleterosepanel',
    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    margins: '5 5 5 5',
                    name: 'RealityObject',
                    store: 'B4.store.administration.massdelete.RealityObject',
                    columns: [
                        { xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'gridcolumn', dataIndex: 'Address', text: 'Адрес', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    textProperty: 'Address',
                    fieldLabel: 'Жилой дом',
                    labelAlign: 'right'
                },
                {
                    xtype: 'massdeleteroseselectgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});