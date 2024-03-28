Ext.define('B4.view.confirmcontribution.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.confirmcontribution.ManagOrg',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.ux.grid.Panel',
        'B4.view.confirmcontribution.RecordGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    maximized: true,
    bodyPadding: 5,
    alias: 'widget.confContribEditWindow',
    title: 'Поступление взносов на КР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ManagingOrganization',
                    textProperty: 'ManagingOrganizationName',
                    fieldLabel: 'Управляющая организация',
                    windowContainerSelector: 'confContribEditWindow',
                    store: 'B4.store.confirmcontribution.ManagOrg',
                    padding: '5px 0 5px 0',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование УО',
                            dataIndex: 'ManagingOrganizationName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'confirmContribRecordGrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});