Ext.define('B4.controller.import.Embir', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url'
    ],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: ['import.EmbirPanel'],

    mainView: 'import.EmbirPanel',
    mainViewSelector: 'importembirpanel',

    init: function () {
        var me = this;

        me.control({
            'importembirpanel button[action=import]': {
                click: me.onImportClick
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        
        me.bindContext(view);
        me.application.deployView(view);
        
        me.reloadPanel();
    },
    
    onImportClick: function (btn) {
        var me = this,
            importKey = btn.up('form').down('combobox[name=ImportName]').getValue();
        if (importKey) {
            me.mask('Импорт', me.getMainView());
            B4.Ajax.request({
                url: B4.Url.action('Import', 'GkhImport', { importId: importKey, not_from_file: true }),
                timeout: 99999999
            }).next(function (response) {
                var resp = Ext.decode(response.responseText);
                me.getMainView().down('embirimportloggrid').getStore().load();
                me.reloadPanel();
                Ext.Msg.alert('Информация', resp.message || 'Импорт завершен!');

                me.unmask();
            }).error(function (e) {
                Ext.Msg.alert('Информация', e.message || 'Импорт завершен! Ошибки можно посмотреть в логе.');
                me.unmask();
            });
        }
    },
    
    reloadPanel: function () {
        var me = this,
            field,
            form = me.getMainView();
        
        me.mask('Загрузка', me.getMainView());
        
        B4.Ajax.request({
            url: B4.Url.action('GetInfo', 'ImportEmbir')
        }).next(function (response) {
            var resp = Ext.decode(response.responseText);
            
            Ext.iterate(resp, function (key, value) {
                field = form.down(Ext.String.format('datefield[name={0}]', key));
                if (field) {
                    field.setValue(value);
                }
            });
            
            me.unmask();
        }).error(function (e) {
            me.unmask();
        });
    }
});