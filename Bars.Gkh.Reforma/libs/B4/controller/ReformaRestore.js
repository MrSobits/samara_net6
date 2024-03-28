Ext.define('B4.controller.ReformaRestore', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: ['reformarestore.Panel'],

    mainView: 'reformarestore.Panel',
    mainViewSelector: 'restorepanel',

    init: function() {
        var me = this;

        this.control({
            'restorepanel button': {
                click: me.loadButtonClick
            }
        });
    },

    loadButtonClick: function() {
        var me = this;

        var importPanel = me.getMainComponent();

        var fileImport = importPanel.down('[name=RestoreFile]');
        if (!fileImport.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл для восстановления!', 'warning');
            return;
        }

        if (!fileImport.isFileExtensionOK()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
            return;
        }

        var formImport = importPanel.down('[name=RestoreForm]');
        me.mask('Запуск восстановления', me.getMainComponent());

        formImport.submit({
            url: B4.Url.action('/ReformaRestore/Execute'),
            timeout: 9999999,
            success: function(form, action) {
                me.unmask();
                B4.QuickMsg.msg('Восстановление', action.result.message, 'success');
            },
            failure: function(form, action) {
                me.unmask();
                B4.QuickMsg.msg('Восстановление', action.result.message, 'error');
            }
        }, this);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});