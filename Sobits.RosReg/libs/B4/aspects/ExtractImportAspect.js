Ext.define('B4.aspects.ExtractImportAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.extractimportaspect',
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
        
    viewSelector: null,
    params: {},
    importId: null,
    maxFileSize: 209715200, //200mb

    buttonSelector: null,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.viewSelector + ' ' + (this.buttonSelector || 'button')] = { 'click': { fn: this.loadButtonClick, scope: this } };
        
        controller.control(actions);
    },
    
    loadButtonClick: function () {
        var me = this;

        me.importPanel = me.controller.getMainComponent();

        var fileImport = me.importPanel.down('#fileImport');

        if (!fileImport.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл для импорта!', 'warning');
            return;
        }

        if (!fileImport.isFileExtensionOK()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
            return;
        }
        
        if (!Ext.isEmpty(me.maxFileSize) && fileImport.isFileLoad() && fileImport.getSize() > me.maxFileSize) {
            Ext.Msg.alert('Импорт', 'Необходимо выбрать файл допустимого размера');
            return;
        }

        me.getUserParams();
        me.params.importId = me.importId;

        var formImport = me.importPanel.down('#importForm');

        me.mask('Загрузка данных', me.controller.getMainComponent());

        formImport.submit({
            // ?
            url: B4.Url.action('/ExtractImport/Import'),
            //
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
                    message = (action.result.title ? (action.result.title + ' <br/>') : '') + action.result.message;
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
    
    getUserParams: function () {
        this.params = this.params || {};
    },
    
    createLink: function (id) {
        if (!id) return '';
        var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', id));
        return '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог</a>';
    }
});