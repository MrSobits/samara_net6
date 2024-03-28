Ext.define('B4.controller.Import.Gku', {
    extend: 'B4.base.Controller',

    views: ['Import.GkuPanel'],

    mainView: 'Import.GkuPanel',
    mainViewSelector: 'gkuimportpanel',

    refs: [{
        ref: 'mainView',
        selector: 'gkuimportpanel'
    }],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this;

        this.control({
            'gkuimportpanel button': {
                click: me.loadButtonClick
            }
        });
    },

    loadButtonClick: function () {
        var me = this;

        me.importPanel = me.getMainComponent();

        var fileImport = me.importPanel.down('#fileImport');

        if (!fileImport.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл для импорта!', 'warning');
            return;
        }

        if (!fileImport.isFileExtensionOK()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
            return;
        }

        me.params = {};
        me.params.importId = 'ImportGku';

        var formImport = me.importPanel.down('#importForm');

        me.mask('Загрузка данных', me.getMainComponent());

        formImport.submit({
            url: B4.Url.action('/GkhImport/Import'),
            params: me.params,
            success: function (form, action) {
                me.unmask();
                var message;
                if (!Ext.isEmpty(action.result.message)) {
                    message = action.result.title + ' <br/>' + action.result.message;
                } else {
                    message = action.result.title;
                }

                Ext.Msg.show({
                    title: 'Успешная загрузка',
                    msg: message,
                    width: 300,
                    buttons: Ext.Msg.OK,
                    icon: Ext.window.MessageBox.INFO
                });

                var log = me.importPanel.down('#log');
                if (log) {
                    log.setValue(me.createLink(action.result.logFileId));
                }
            },
            failure: function (form, action) {
                me.unmask();
                var message;
                if (!Ext.isEmpty(action.result.message)) {
                    message = action.result.title + ' <br/>' + action.result.message;
                } else {
                    message = action.result.title;
                }

                Ext.Msg.alert('Ошибка загрузки', message, function () {
                    if (action.result.logFileId > 0) {
                        var log = me.importPanel.down('#log');
                        if (log) {
                            log.setValue(me.createLink(action.result.logFileId));
                        }
                    }
                });
            }
        }, this);
    },

    createLink: function (id) {
        if (!id) return '';
        var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', id));
        return '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог</a>';
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('gkuimportpanel');
        this.bindContext(view);
        this.application.deployView(view);
    }
});