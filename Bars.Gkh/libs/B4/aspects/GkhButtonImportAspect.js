/*
*Данный аспект предназначен для импорта данных
*/

Ext.define('B4.aspects.GkhButtonImportAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhbuttonimportaspect',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
        
    buttonSelector: null,
    codeImport: null,
    importStore: null,
    params: {},
    windowImportSelector: '#importWindow',
    windowImportView: 'Import.Window',
    serializerCode: 'default',
    loadImportList: true,
    
    /**
    * @cfg {String} ownerWindowSelector
    * Селектор окна в пределах которого надо создать окно импорта
    */
    ownerWindowSelector: null,
    windowImport: null,
    importId: null,
    maxFileSize: 2097152000, //в байтах 2000 мб по умолчанию
    timeout: 9999999,

    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'onforminvalid',
            'onsuccesssave',
            'onsavefauilure'
        );
    },
    
    init: function (controller) {
        var me = this,
            actions = {};
        me.callParent(arguments);

        me.on('onsuccesssave', me.onSuccessSaveHandler, me);
        me.on('onsavefauilure', me.onSaveFailure, me);
        me.on('onforminvalid', me.onFormInvalid, me);

        if (me.loadImportList) {
            actions[me.buttonSelector + ' menuitem'] = { 'click': { fn: me.onMenuItemClick, scope: me } };

            me.importStore = Ext.create('Ext.data.Store', {
                autoLoad: false,
                fields: ['Key', 'Name', 'PossibleFileExtensions'],
                proxy: {
                    autoLoad: false,
                    type: 'ajax',
                    url: B4.Url.action('/GkhImport/GetImportList'),
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            });

            me.importStore.on('beforeload', me.onBeforeLoadImportStore, me);
            me.importStore.on('load', me.onLoadImportStore, me);
        } else {
            actions[me.buttonSelector] = { 'click': { fn: me.onSingleImportButtonClick, scope: me } };
        }

        controller.control(actions);
    },
    
    closeWindow: function () {
        if (this.windowImport) {
            this.windowImport.destroy();
        }
    },

    loadImportStore: function () {
        this.importStore.load();
    },

    onBeforeLoadImportStore: function (store, operation) {
        operation.params = {};
        operation.params.codeImport = this.codeImport;
    },

    onLoadImportStore: function (store) {
        var btn = this.componentQuery(this.buttonSelector);

        if (btn) {
            btn.menu.removeAll();

            store.each(function (rec) {
                btn.menu.add({
                    xtype: 'menuitem',
                    text: rec.data.Name,
                    textAlign: 'left',
                    actionName: rec.data.Key,
                    iconCls: 'icon-build',
                    possibleFileExtensions: rec.data.PossibleFileExtensions
                });
            });
        }
    },

    createWindowImport: function () {
        var me = this,
            key = me.windowImportSelector + '-' + me.importId;
        var rndr = null;
        if (!Ext.isEmpty(this.ownerWindowSelector)) {
            var ownerWindow = me.componentQuery(me.ownerWindowSelector);
            if (!Ext.isEmpty(ownerWindow)) {
                rndr = ownerWindow.getEl();
            }
        } else {
            rndr = B4.getBody().getActiveTab().getEl();
        }
        
        me.windowImport = me.componentQuery(key);
        if (!me.windowImport) {
            me.windowImport = Ext.create('B4.view.' + (me.getWindowImportView(me.importId) || me.windowImportView),
                {
                    itemId: key,
                    constrain: true,
                    renderTo: rndr
                });
        }
        
        me.windowImport.down('b4savebutton').on('click', me.onSaveClick, me);
        me.windowImport.down('b4closebutton').on('click', me.closeWindow, me);

        me.fireEvent('aftercreatewindow', me.windowImport, me.importId);
    },

    //Пустая функция на случай, стандартной реализации и отсутствия переопределения этой функции.
    getWindowImportView: function(importId) {
        return false;
    },

    onSingleImportButtonClick: function (btn) {
        this.importId = btn.importId;
        this.createWindowImport();
        this.windowImport.title = btn.text;
        var fileImport = this.windowImport.down('#fileImport');
        fileImport.possibleFileExtensions = btn.possibleFileExtensions;
        this.windowImport.show();
    },

    onMenuItemClick: function (itemMenu) {
        this.importId = itemMenu.actionName;
        this.createWindowImport();

        this.windowImport.title = itemMenu.text;

        var fileImport = this.windowImport.down('#fileImport');
        fileImport.possibleFileExtensions = itemMenu.possibleFileExtensions;
        this.windowImport.show();
    },

    onSaveClick: function () {
        var me = this,
            formImport = me.windowImport.getForm();
        
        if (!formImport.isValid()) {
            me.fireEvent('onforminvalid', me, formImport);
            return;
        }

        var fileImport = me.windowImport.down('#fileImport');
        if (!Ext.isEmpty(me.maxFileSize) && fileImport.isFileLoad() && fileImport.getSize() > me.maxFileSize) {
            Ext.Msg.alert('Импорт', 'Необходимо выбрать файл допустимого размера');
            return;
        }

        me.mask('Загрузка данных', me.windowImport);

        const request = new XMLHttpRequest();
        request.open('POST', me.getUrl());
        request.timeout = me.timeout;

        request.onreadystatechange = function() {
            me.unmask();

            if (request.readyState === XMLHttpRequest.DONE) {
                try {
                    var response = Ext.decode(request.response);
                    
                    if (request.status === 200) {
                        me.fireEvent('onsuccesssave', me, response);
                    } else {
                        me.fireEvent('onsavefauilure', me, response);
                    }
                } catch {
                    Ext.Msg.alert('Ошибка', 'Произошла ошибка при получении результата импорта');
                }
            }
        };

        request.send(me.getFormData(fileImport));
    },

    onSuccessSaveHandler: function (asp, response) {
        var me = asp,
            message;
        
        if (!Ext.isEmpty(response.result.message)) {
            if (!Ext.isEmpty(response.result.title)) {
                message = response.result.title + ' <br/>';
            } else {
                message = '';
            }

            message += response.result.message;
        } else if (!Ext.isEmpty(response.result.title)) {
            message = response.result.title;
        } else {
            message = '';
        }

        message = message + ' <br/>' + 'Закрыть окно загрузки?';

        if (response.result.status == 40) {
            Ext.Msg.alert(response.result.title, response.result.message);
        } else {
            Ext.Msg.confirm('Успешная загрузка', message, function(confirmationResult) {
                if (confirmationResult === 'yes') {
                    me.closeWindow();
                } else {
                    var log = me.windowImport.down('#log');
                    if (log) {
                        log.setValue(me.createLink(response.result.logFileId));
                    }
                }
            }, me);
        }
    },
    
    onSaveFailure: function (asp, response){
        var me = asp,
            message = '';

        if (!Ext.isEmpty(response.result.title)) {
            message = response.result.title;
        }

        if (!Ext.isEmpty(response.result.message)) {

            if (!Ext.isEmpty(message)) {
                message = message + ' <br/>';
            }

            message = message + response.result.message;
        }

        Ext.Msg.alert('Ошибка загрузки', message, function () {
            if (response.result.logFileId > 0) {
                var log = me.windowImport.down('#log');
                if (log) {
                    log.setValue(me.createLink(response.result.logFileId));
                }
            }
        });
    },

    onFormInvalid: function (asp, form) {
        //получаем все поля формы
        var fields = form.getFields(),
            invalidFields = '';

        //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
        Ext.each(fields.items, function (field) {
            if (!field.isValid()) {
                invalidFields += '<br>' + field.fieldLabel;
            }
        });

        //выводим сообщение
        Ext.Msg.alert('Ошибка загрузки', 'Не заполнены обязательные поля: ' + invalidFields);
    },

    getFormData: function (fileField) {
        var me = this,
            formData = new FormData(),
            files = fileField.fileInputEl.dom.files;

        me.getUserParams();
        me.params.importId = me.importId;
        
        if (files.length === 1) {
            me.params[`FileImport`] = files[0];
        } else {
            Ext.iterate(Array.from(files), function (file, key) {
                me.params[`FileImport_${key}`] = file;
            });
        }

        Ext.each(Object.keys(me.params), function (paramKey) {
            formData.append(paramKey, me.params[paramKey]);
        });

        return formData;
    },

    getUrl: function () {
        if (this.windowImport.supportMultipleImport) {
            return B4.Url.action('/GkhImport/MultiImport');
        }
        
        return B4.Url.action('/GkhImport/Import');
    },

    getUserParams: function () {
        this.params = this.params || {};
    },

    refreshData: function () {
    },
    
    createLink: function (id) {
        if (!id) return '';
        var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', id));
        return '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог</a>';
    }
});