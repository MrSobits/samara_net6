Ext.define('B4.view.disposal.SubjectVerificationLicensingGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalsubjectverificationlicensinggrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Предмет проверки',
    store: 'disposal.DisposalVerificationSubjectLicensing',
    itemId: 'subjectVerificationLicensingGrid',

    initComponent: function() {
        var me = this;
        
        var sm = Ext.create('Ext.selection.CheckboxModel', {
            mode: 'MULTI',
            showHeaderCheckbox: false,
            headerWidth: 60,

            getHeaderConfig: function () {
                var me = this;

                return {
                    isCheckerHd: false,
                    text: 'В работе',
                    clickTargetName: 'el',
                    width: me.headerWidth,
                    sortable: false,
                    draggable: false,
                    resizable: false,
                    hideable: false,
                    menuDisabled: true,
                    dataIndex: '',
                    cls: '',
                    renderer: Ext.Function.bind(me.renderer, me),
                    editRenderer: me.editRenderer || me.renderEmpty,
                    locked: me.hasLockedHeader()
                };
            }
        });

        Ext.applyIf(me, {
            columnLines: true,
            selModel: sm,
            columns: [
                {
                    xtype: 'gridcolumn',
                    sortable: false,
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                 {
                                     xtype: 'button',
                                     text: 'Сохранить',
                                     cmd: 'saveValidationSubjectLicensing',
                                     tooltip: 'Сохранить',
                                     iconCls: 'icon-accept'
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