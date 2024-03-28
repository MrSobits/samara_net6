Ext.define('B4.view.claimwork.LawsuitOwnerRepresentativeGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox'
    ],

    header: false,
    title: 'Законные представители',
    store: 'claimwork.LawsuitOwnerRepresentative',
    alias: 'widget.lawsuitownerrepgrid',
    enableColumnHide: true,
    height: 150,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn' },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Rloi',
                    flex: 1,
                    text: 'rloi',
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Surname',
                    flex: 1,
                    text: 'Фамилия',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstName',
                    flex: 1,
                    text: 'Имя',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Patronymic',
                    flex: 1,
                    text: 'Отчество',
                    hidden: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BirthDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата рождения',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BirthPlace',
                    flex: 1,
                    text: 'Место рождения',
                    hidden: false
                },
                {   xtype: 'b4deletecolumn' }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'b4addbutton' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});