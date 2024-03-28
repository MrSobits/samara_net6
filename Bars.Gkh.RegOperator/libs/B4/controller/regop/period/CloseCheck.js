Ext.define('B4.controller.regop.period.CloseCheck',
{
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'regop.period.CloseCheck'
    ],

    stores: [
        'regop.period.CloseCheck'
    ],

    views: [
        'regop.periodclosecheck.Grid',
        'regop.periodclosecheck.HistoryWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'periodclosecheckgrid'
        }
    ],

    mainView: 'regop.periodclosecheck.Grid',
    mainViewSelector: 'periodclosecheckgrid',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.Settings.PeriodChecks.Edit',
                    applyTo: 'button[actionName=add]',
                    selector: 'periodclosecheckgrid'
                },
                {
                    name: 'GkhRegOp.Settings.PeriodChecks.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'periodclosecheckgrid'
                },
                {
                    name: 'GkhRegOp.Settings.PeriodChecks.Edit',
                    applyTo: 'b4deletecolumn',
                    selector: 'periodclosecheckgrid',
                    applyBy: function(c, a) {
                        c.setVisible(a);
                    }
                },
                {
                    name: 'GkhRegOp.Settings.PeriodChecks.Edit',
                    applyTo: '[dataIndex=IsCritical]',
                    selector: 'periodclosecheckgrid',
                    applyBy: function(c, a) {
                        var e = c.getEditor && c.getEditor();
                        if (e) {
                            e.setDisabled(!a);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'editAspect',
            modelName: 'regop.period.CloseCheck',
            gridSelector: 'periodclosecheckgrid',
            deleteRecord: function (record) {
                var me = this,
                    store = me.getStore();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        record.destroy({
                            callback: function() {
                                store.load();
                            }
                        });
                    }
                }, me);
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'addAspect',
            buttonSelector: 'periodclosecheckgrid button[actionName=add]',
            multiSelectWindowSelector: '#periodclosecheckgridAddWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'regop.period.CloseChecker',
            storeSelected: 'regop.period.CloseChecker',
            selModelMode: 'MULTI',
            columnsGridSelect: [
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', width: 50 },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            titleSelectWindow: 'Выбор проверок',
            titleGridSelect: 'Проверки для отбора',
            titleGridSelected: 'Выбранные проверки',
            listeners: {
                getdata: function(asp, records) {
                    var grid = asp.controller.getMainView(),
                        store = grid.getStore();

                    Ext.Array.each(records.items,
                        function(item) {
                            store.add({
                                'Code': item.get('Code'),
                                'Impl': item.get('Impl'),
                                'Name': item.get('Name')
                            });
                        });

                    asp.controller.getAspect('editAspect').save();

                    return true;
                }
            }
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'periodclosecheckgrid': {
                'rowaction': {
                    fn: me.onRowAction,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('periodclosecheckgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onRowAction: function(grid, action, record) {
        var window,
            store;

        if (action === 'edit') {
            window = Ext.widget('periodclosecheckhistorywindow');
            window.loadRecord(record);

            store = window.down('grid').getStore();
            store.clearFilter(true);
            store.filter('checkId', record.getId());

            window.show();
        }
    }
});