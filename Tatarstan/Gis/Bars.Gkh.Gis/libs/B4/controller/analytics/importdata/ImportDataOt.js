Ext.define('B4.controller.analytics.importdata.ImportDataOt', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.Permission'
    ],

    views: [
        'analytics.importdata.importdataot.ImportPanel'
    ],

    mainView: 'analytics.importdata.importdataot.ImportPanel',
    mainViewSelector: 'importdataotpanel',

    params: {},

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView',
        selector: 'importdataotpanel'
    }],

    init: function () {
        var me = this;

        me.control({
            'importdataotpanel button[name="btnLoadFile"]': {
                click: me.loadButtonClick
            },
            
            'importdataotpanel gridpanel': {
                afterrender: {
                    fn: me.onImportGridRender,
                    scope: me
                },
                
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('importdataotpanel');
        this.bindContext(view);
        this.application.deployView(view);
    },

    onImportGridRender: function (gridpanel) {
        var me = this,
            store = gridpanel.getStore();
        
        if (store) {
            store.on('beforeload', me.onBeforeLoadImportDataStore, me);
            
            store.load();
        }
    },

    onBeforeLoadImportDataStore: function (store, operation) {
        operation.params = {};

        operation.params.typeImport = this.typeImport;
    },

    //загрузка файла
    loadButtonClick: function () {
        var me = this,
            view = me.getMainView(),
            fileField = view.down('b4filefield'),
            filesGrid = view.down('gridpanel[name=ImportGrid]'),
            nameField = view.down('textfield[name=name]');

        me.importPanel = me.getMainComponent();

        //проверка файлов
        if (!fileField.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Вы выбрали некорректные файлы для загрузки', 'warning');
            return;
        }

        me.mask('Загрузка данных', me.getMainComponent());

        var formImport = me.importPanel.down('#importForm');
        if (formImport.getForm().isValid()) {
            formImport.submit({
                url: B4.Url.action('/ImportDataOt/ImportIndicators'),
                waitMsg: 'Загрузка файлов...',
                //params: params,
                success: function(form, action) {
                    Ext.Msg.show({
                        title: 'Файл загружен',
                        msg: action.result.message,
                        width: 300,
                        buttons: Ext.Msg.OK,
                        icon: Ext.window.MessageBox.INFO
                    });

                    nameField.setValue(null);
                    filesGrid.getStore().load();
                    me.unmask();

                    //костылек))
                    //после submit свойство сбрасывается в false и не понятно почему
                    fileField.fileInputEl.dom.multiple = true;
                },

                failure: function(form, action) {
                    var message = "",
                        errors = action.result.errors;

                    filesGrid.getStore().load();

                    if (!Ext.isEmpty(action.result.message)) {
                        message = action.result.message;
                    } else if (errors.length > 0) {
                        Ext.each(errors, function(error, index) {
                            message += index > 0
                                ? index > 1
                                ? "<br />" + error
                                : "<br /> <br />" + error
                                : error;
                        });
                    }

                    Ext.Msg.alert('Ошибка загрузки', message);

                    me.unmask();
                }
            }, this);
        } else {
            me.unmask();
            B4.QuickMsg.msg('Внимание!', 'Не все поля заполнены', 'warning');
        }
    },

    rowAction: function (grid, action, record) {
        var me = this;

        if (me.fireEvent('beforerowaction', me, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'delete':
                    me.deleteRecord(grid, record);
                    break;
            }
        }
    },

    deleteRecord: function (grid, record) {
        var me = this;

        Ext.Msg.confirm('Удаление данных!', 'Вы действительно хотите удалить данные?', function (result) {
            if (result === 'yes') {
                me.deleteData(grid, record);
            }
        }, me);
    },

    deleteData: function (grid, record) {
        var me = this;

        me.mask('Удаление...', me.getMainComponent());

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('DeleteOtData', 'ImportDataOt'),
            params: { Id: record.getId() },
            timeout: 999999
        }).next(function (response) {
            var result = Ext.decode(response.responseText);
            me.unmask();
            if (result.success) {
                record.commit();
                B4.QuickMsg.msg('Успешно', 'Данные успешно удалены', 'success');
            } else {
                B4.QuickMsg.msg('Ошибка!', result.message, 'error');
            }
        }).error(function () {
            me.unmask();
            B4.QuickMsg.msg('Ошибка!', 'Не удалось удалить данные', 'error');
        });
    }
});
