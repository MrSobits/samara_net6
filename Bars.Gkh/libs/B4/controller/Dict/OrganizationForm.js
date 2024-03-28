Ext.define('B4.controller.dict.OrganizationForm', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],

    models: ['dict.OrganizationForm'],
    stores: ['dict.OrganizationForm'],
    views: ['dict.organizationform.Grid'],

    mainView: 'dict.organizationform.Grid',
    mainViewSelector: 'organizationFormGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'organizationFormGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'organizationFormGrid',
            permissionPrefix: 'Gkh.Dictionaries.OrganizationForm'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'organizationFormGridAspect',
            storeName: 'dict.OrganizationForm',
            modelName: 'dict.OrganizationForm',
            gridSelector: 'organizationFormGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('organizationFormGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.OrganizationForm').load();
    },

    init: function () {
        var me = this,
            actions = {
                'organizationFormGrid button[action="import"]': {
                    click: me.onImportBtnClick
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onImportBtnClick: function (btn) {
        var me = this;
        if (!me.importWindow) {
            me.importWindow = Ext.create('Ext.window.Window', {
                title: 'Импорт данных',
                modal: true,
                closeAction: 'hide',
                width: 400,
                items: [
                    {
                        xtype: 'form',
                        border: 0,
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'b4filefield',
                                name: 'File',
                                flex: 1,
                                allowBlank: false,
                                possibleFileExtensions: 'xls',
                                margin: '10 10 10 10'
                            }
                        ],
                        buttons: [{
                            xtype: 'b4savebutton',
                            margin: '5 5 5 5',
                            handler: function () {
                                this.up('form').submit({
                                    url: B4.Url.action('Import', 'OrganizationForm'),
                                    success: function (form) {
                                        B4.QuickMsg.msg('Сохранение записи', 'Успешно сохранено', 'success');
                                        me.getMainView().getStore().load();
                                    },
                                    failure: function (form, action) {
                                        switch (action.failureType) {
                                            case Ext.form.action.Action.CLIENT_INVALID:
                                                B4.QuickMsg.msg('Сохранение записи', 'Форма заполнена неверно.', 'error');
                                                break;
                                            case Ext.form.action.Action.CONNECT_FAILURE:
                                                B4.QuickMsg.msg('Сохранение записи', 'Проблемы с соединением, попробуйте позже.', 'error');
                                                break;
                                            case Ext.form.action.Action.SERVER_INVALID:
                                                B4.QuickMsg.msg('Сохранение записи', action.result.data.message, 'error');
                                        }
                                    }
                                });
                                this.up('window').close();
                            }

                        }]
                    }
                ]
            });
        }

        me.importWindow.show();

    }
});