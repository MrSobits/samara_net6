Ext.define('B4.controller.importexport.ImportData', {
    extend: 'B4.base.Controller',
    views: [
        'importexport.incrementalimport.Panel'
    ],
    mainView: 'importexport.incrementalimport.Panel',
    mainViewSelector: 'incrementalimportpanel',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'incrementalimportpanel'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'incrementalimportpanel filefield': {
                change: {
                    fn: me.fileChange,
                    scope: me
                }
            },
            
            'incrementalimportpanel button[name="btnLoadFile"]': {
                click: me.loadButtonClick
            },
            
            'incrementalimportpanel gridpanel': {
                afterrender: {
                    fn: me.onImportGridRender,
                    scope: me
                },
                
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                }
            },
            
            'incrementalimportpanel combobox[name="Format"]': {
                change: {
                    fn: me.onChangeFormatCbx,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('incrementalimportpanel');
        me.bindContext(view);
        me.application.deployView(view);
        view.down('gridpanel[name=ImportGrid]').getStore().load();
    },

    onImportGridRender: function (gridpanel) {
        var me = this,
            store = gridpanel.getStore();
        
        if (store) {
            store.on('beforeload', me.onBeforeLoadImportDataStore, me);
            
            if (me.typeImport) {
                store.load();
            }
        }
    },
    
    onChangeFormatCbx: function (cbx, newValue) {
        var grid = cbx.up('incrementalimportpanel').down('gridpanel');
        
        this.typeImport = newValue;
        if (grid) {
            grid.getStore().load();
        }
    },

    onBeforeLoadImportDataStore: function (store, operation) {
        operation.params = operation.params || {};

        operation.params.typeImport = this.typeImport;
    },
    
    // выбор файлов
    fileChange: function (fileField) {
        var files = fileField.fileInputEl.dom.files;
        this.fileValidation();

        var rawValue = files.length > 0 ? files[0].name : '';
        for (var i = 1; i < files.length; i++) {
            rawValue += ', ' + files[i].name;
        }
        fileField.setRawValue(rawValue);
    },

    // проверка файлов
    fileValidation: function () {
        var me = this,
            view = me.getMainView(),
            fileField = view.down('filefield'),
            files = fileField.fileInputEl.dom.files;

        if (files.length === 0 || files.length > 10) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать от 1 до 10 файлов', 'error');
            return false;
        }

        Ext.each(files, function (f) {
            // 1Gb
            if (f.size > 1073741824) {
                B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл(ы) допустимого размера (до 1 гб)', 'error');
                return false;
            }
        });

        return true;
    },

    // загрузка файда
    loadButtonClick: function (button) {
        var me = this,
            view = me.getMainView(),
            fileField = view.down('filefield'),
            filesGrid = view.down('gridpanel[name=ImportGrid]'),
            formatField = view.down('combobox[name=Format]'),
            format = formatField.getValue();

        me.importPanel = me.getMainComponent();
        
        // проверка файлов
        if (!fileField.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Вы выбрали некорректные файлы для загрузки', 'warning');
            return;
        }
       
        // проверка файлов
        if (!me.fileValidation()) {
            return;
        }

        if (!format || format == '') {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать формат загрузки', 'warning');
            return;
        }

        me.mask('Загрузка данных', me.getMainComponent());

        var formImport = me.importPanel.down('#importForm');
        if (formImport.getForm().isValid()) {
            formImport.submit({
                url: B4.Url.action('/ImportData/Import'),
                params: {
                    Format: format
                },
                waitMsg: 'Загрузка файлов...',
                success: function (form, action) {
                    Ext.Msg.show({
                        title: 'Файл загружен',
                        msg: action.result.message,
                        width: 300,
                        buttons: Ext.Msg.OK,
                        icon: Ext.window.MessageBox.INFO
                    });

                    filesGrid.getStore().load();
                    me.unmask();

                    // костылек))
                    // после submit свойство сбрасывается в false и не понятно почему
                    fileField.fileInputEl.dom.multiple = true;
                },

                failure: function (form, action) {
                    var message = "",
                        errors = action.result.errors;

                    filesGrid.getStore().load();

                    if (!Ext.isEmpty(action.result.message)) {
                        message = action.result.message;
                    } else if (errors.length > 0) {
                        Ext.each(errors, function (error, index) {
                            message += index > 0
                                ? index > 1
                                    ? "<br />" + error
                                    : "<br /> <br />" + error
                                : error;
                        });
                    }

                    Ext.Msg.alert('Ошибка загрузки', message);

                    me.unmask();
                    
                    // костылек))
                    // после submit свойство сбрасывается в false и не понятно почему
                    fileField.fileInputEl.dom.multiple = true;
                }
            }, this);
        }
    }
});