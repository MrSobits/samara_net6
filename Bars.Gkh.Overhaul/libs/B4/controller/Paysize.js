Ext.define('B4.controller.Paysize', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.form.SelectField',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'Ext.ux.data.PagingMemoryProxy',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'Paysize',
        'paysize.Record',
        'paysize.RealEstateType',
        'paysize.RetPreview'
    ],

    models: [
        'Paysize',
        'paysize.Record',
        'paysize.RealEstateType',
        'paysize.RetPreview'
    ],

    views: [
        'paysize.UpdateRetPreviewGrid',
        'paysize.UpdateRetPreviewWindow',
        'paysize.Grid',
        'paysize.EditPanel',
        'paysize.RecordGrid',
        'paysize.RetWindow',
        'paysize.RetGrid'
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'updateTypesPreviewButtonExportAspect',
            gridSelector: 'updateretpreviewgrid',
            buttonSelector: 'updateretpreviewwindow #btnExport',
            controllerName: 'RealEstateType',
            actionName: 'UpdateTypesPreviewExport'
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'updateTypesPreviewPermissionAspect',
            permissions: [
                { name: 'Gkh.UpdateRetPreview_View', applyTo: 'button[action=UpdateRoTypes]', selector: 'paysizegrid' }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'paysizegrid' }
    ],

    init: function() {
        var me = this;

        me.control({
            'paysizegrid': {
                'rowaction': {
                    fn: me.gridRowAction,
                    scope: me
                }
            },
            'paysizegrid b4addbutton': {
                'click': function() {
                    me.editRecord();
                }
            },
            'paysizegrid button[action=UpdateRoTypes]': {
                'click': {
                    fn: me.onClickUpdateRoTypesPreview,
                    scope: me
                }
            },
            'updateretpreviewwindow[location=regop] button[action=UpdateRoTypes]': {
                'click': {
                    fn: me.onClickUpdateRoTypes,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainPanel() || Ext.widget('paysizegrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },
    
    onClickUpdateRoTypesPreview: function () {
        var me = this;

        var window = Ext.create('B4.view.paysize.UpdateRetPreviewWindow');
        window.location = 'regop';
        window.show();

        me.mask('Загрузка...', window);

        B4.Ajax.request(
            {
                url: B4.Url.action('UpdateTypesPreview', 'RealEstateType'),
                params: me.params,
                timeout: 9999999
            }
        ).next(function (response) {
            var data = Ext.JSON.decode(response.responseText),
                proxy = Ext.create('Ext.ux.data.PagingMemoryProxy', {
                    enablePaging: true,
                    data: data
                }),
                grid = window.down('updateretpreviewgrid'),
                store = grid.getStore();

            store.currentPage = 1;
            store.setProxy(proxy);
            store.load();

            me.unmask();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при получении типов');
        });
    },

    onClickUpdateRoTypes: function() {
            var me = this;

            me.mask('Обновление типов домов...', B4.getBody().getActiveTab().getEl());

            B4.Ajax.request(
                {
                    url: B4.Url.action('UpdateTypes', 'RealEstateType'),
                    params: me.params,
                    timeout: 9999999
                }).next(function () {
                    var grid = Ext.ComponentQuery.query('updateretpreviewgrid')[0],
                            store = grid.getStore();
                    store.removeAll();

                    Ext.Msg.alert('Успешно выполнено', 'Типы домов обновлены');

                    me.unmask();
                }).error(function(e) {
                    Ext.Msg.alert('Ошибка', e.message || 'Ошибка при обновлении типов');
                    me.unmask();
                });
    },

    gridRowAction: function(grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editRecord(record);
                break;
            case 'delete':
                this.deletePaysize(record);
                break;
        }
    },

    editRecord: function(record) {
        var id = record ? record.getId() : 0;

        Ext.History.add('paysize_edit/' + id);
    },

    deletePaysize: function (record) {
        var me = this,
            grid = me.getMainPanel();

        me.mask('Удаление', B4.getBody());

        record.destroy()
            .next(function () {
                me.unmask();
                grid.getStore().load();
            })
            .error(function (e) {
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(e.responseData) ? e.responseData : e.responseData.message);
                me.unmask();
            });
    }
});