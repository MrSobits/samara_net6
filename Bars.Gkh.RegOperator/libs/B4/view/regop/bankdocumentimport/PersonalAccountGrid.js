Ext.define('B4.view.regop.bankdocumentimport.PersonalAccountGrid', {
    extend: 'B4.form.Window',
    alias: 'widget.bankdocumentimportpagrid',
    itemId: 'bankDocImportPersonalAccountGrid',
    title: 'Реестр лицевых счетов',
    trackResetOnLoad: true,
    modal: true,
    width: 600,
    minWidth: 600,
    minHeight: 500,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    mixins: ['B4.mixins.window.ModalMask'],

    requires: [
        'Ext.ux.CheckColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.regop.personal_account.BasePersonalAccount'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.BasePersonalAccount');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    cls: 'x-large-head',
                    store: store,
                    flex: 1,
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                    selModel: Ext.create('Ext.selection.CheckboxModel', {
                        mode: 'SINGLE'
                    }),
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PersonalAccountNum',
                            text: 'Номер',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RoomAddress',
                            text: 'Адрес',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AccountOwner',
                            text: 'ФИО/Наименование абонента',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'button',
                                            text: 'Сопоставить',
                                            textAlign: 'left',
                                            action: 'Compare'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Сопоставить и подтвердить',
                                            textAlign: 'left',
                                            action: 'CompareAndAccept'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показывать неактивные ЛС',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-4 0 0 10',
                                            name: 'ShowInActivePa'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});