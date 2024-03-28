/*
    Данный аспект предназначен для импорта данных в BankAccountStatementImport
    Отдельный аспект создан для поддержания настроек импорта
*/

Ext.define('B4.aspects.GkhButtonImportBankStatementAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhbuttonimportbankstatementaspect',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
        
    buttonSelector: null,
    codeImport: null,
    importStore: null,
    params: {},
    windowImportSelector: '#bankStatementImportWindow',
    windowImportView: 'regop.bankstatement.BankStatementImportWindow',
    serializerCode: 'default',
    multyImport: true,
    /**
    * @cfg {String} ownerWindowSelector
    * Селектор окна в пределах которого надо создать окно импорта
    */
    ownerWindowSelector: null,
    windowImport: null,
    importId: null,
    outcomesOnly: null,
    distributeOnPenalty: null,
    maxFileSize: 2097152000, //в байтах 2000 мб по умолчанию
    timeout: 9999999,

    init: function (controller) {
        var me = this,
            actions = {};
        me.callParent(arguments);

        if (me.multyImport) {
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
        var btn = Ext.ComponentQuery.query(this.buttonSelector)[0];

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
            var ownerWindow = Ext.ComponentQuery.query(me.ownerWindowSelector)[0];
            if (!Ext.isEmpty(ownerWindow)) {
                rndr = ownerWindow.getEl();
            }
        } else {
            rndr = B4.getBody().getActiveTab().getEl();
        }
        

        me.windowImport = Ext.ComponentQuery.query(key)[0];
        if (!me.windowImport) {
            me.windowImport = Ext.create('B4.view.' + (me.getWindowImportView(me.importId) || me.windowImportView), //me.windowImportView,
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
        
        me.getUserParams();
        me.params.importId = me.importId;

        me.params.outcomesOnlyParam = me.windowImport.down('#cbOutcomesOnly').value;
        me.params.withoutRegisterParam = me.windowImport.down('#cbWithoutRegister').value;
        me.params.distributeOnPenaltyParam = me.windowImport.down('#cbDistributeOnPenalty').value;    
 
        if (formImport.isValid()) {
            var fileImport = me.windowImport.down('#fileImport');
            if (!fileImport.isFileExtensionOK()) {
                Ext.Msg.alert('Импорт', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions);
                return;
            }
            
            if (!Ext.isEmpty(me.maxFileSize) && fileImport.isFileLoad() && fileImport.getSize() > me.maxFileSize) {
                Ext.Msg.alert('Импорт', 'Необходимо выбрать файл допустимого размера');
                return;
            }

            me.mask('Загрузка данных', me.windowImport);
            formImport.submit({
                url: me.getUrl(),
                params: me.params,
                timeout: me.timeout,
                success: function (form, action) {
                    me.refreshData(me.params.importId);
                    me.unmask();
                    var message;
                    if (!Ext.isEmpty(action.result.message)) {
                        if (!Ext.isEmpty(action.result.title)) {
                            message = action.result.title + ' <br/>';
                        } else {
                            message = '';
                        }

                         message += action.result.message;
                    } else if (!Ext.isEmpty(action.result.title)) {
                        message = action.result.title;
                    } else {
                        message = '';
                    }

                    message = message + ' <br/>' + 'Закрыть окно загрузки?';

                    if (action.result.status == 40) {
                        Ext.Msg.alert(action.result.title, action.result.message);
                    } else {
                        Ext.Msg.confirm('Успешная загрузка', message, function(confirmationResult) {
                            if (confirmationResult == 'yes') {
                                me.closeWindow();
                            } else {
                                var log = me.windowImport.down('#log');
                                if (log) {
                                    log.setValue(me.createLink(action.result.logFileId));
                                }
                            }
                        }, me);
                    }

                },
                failure: function (form, action) {
                    me.refreshData(me.params.importId);
                    me.unmask();
                    var message = '';
                    
                    if (!Ext.isEmpty(action.result.title)) {
                        message = action.result.title;
                    }
                    
                    if (!Ext.isEmpty(action.result.message)) {
                        
                        if (!Ext.isEmpty(message)) {
                            message = message + ' <br/>';
                        }

                        message = message + action.result.message;
                    }
                    
                    Ext.Msg.alert('Ошибка загрузки', message, function () {
                        if (action.result.logFileId > 0) {
                            var log = me.windowImport.down('#log');
                            if (log) {
                                log.setValue(me.createLink(action.result.logFileId));
                            }
                        }
                    });
                }
            }, this);
        } else {
            //получаем все поля формы
            var fields = formImport.getFields();

            var invalidFields = '';

            //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
            Ext.each(fields.items, function (field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            //выводим сообщение
            Ext.Msg.alert('Ошибка загрузки', 'Не заполнены обязательные поля: ' + invalidFields);
        }
    },

    getUrl: function () {
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