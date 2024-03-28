Ext.define('B4.controller.GisGmpParams', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.mixins.MaskBody',
        'B4.QuickMsg',
        'B4.Ajax',
        'B4.Url',
        'B4.view.GisGmpPatternEditWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: ['GisGmpParamsPanel'],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'gisGmpPatternGridWindowAspect',
            gridSelector: 'gisgmppatterngrid',
            editFormSelector: 'gisgmppatterneditwindow',
            storeName: 'GisGmpPattern',
            modelName: 'GisGmpPattern',
            controllerEditName: 'B4.controller.GisGmpParams',
            editWindowView: 'B4.view.GisGmpPatternEditWindow',
            listeners: {
                'savesuccess': function (aspect) {
                    aspect.controller.getMainView().down('#gisGmpPatternGrid').getStore().load();
                },

                'deletesuccess': function (aspect) {
                    aspect.controller.getMainView().down('#gisGmpPatternGrid').getStore().load();
                },
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'gisGmpPatternDictGridAspect',
            storeName: 'GisGmpPatternDict',
            modelName: 'GisGmpPatternDict',
            gridSelector: '#gisGmpPatternDictGrid',
            listeners: {
                'beforesave': function(asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        isValid = true;

                    Ext.Array.each(modifRecords, function(rec) {
                        if (!rec.getData().PatternName && !rec.getData().PatternCode) {
                            isValid = false;
                        }
                    });
                    if (!isValid) {
                        Ext.Msg.alert('Ошибка', 'Необходимо заполнить хотя бы одно из полей: Наименование шаблона или Код шаблона');
                    }
                    return isValid;
                }
            }
        }
    ],

    mainView: 'GisGmpParamsPanel',
    mainViewSelector: 'gisgmpparamspanel',

    init: function () {
        var me = this;

        me.control({
            'gisgmpparamspanel': {
                'render': {
                    fn: me.onRenderPanel,
                    scope: me
                }
            },
            'gisgmpparamspanel b4savebutton[name=saveBtn]': {
                'click': {
                    fn: me.onClickSave,
                    scope: me
                }
            },
            '#gisGmpPatternGrid b4updatebutton': {
                click: {
                    fn: me.updateStore,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gisgmpparamspanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

    onRenderPanel: function (pnl) {
        var me = this;

        me.mask('Загрузка параметров...', B4.getBody().getActiveTab());

        B4.Ajax.request(
            B4.Url.action('GetParams', 'GisGmpParams')
        ).next(function(response) {
            try {
                var obj = Ext.decode(response.responseText);

                pnl.getForm().setValues(obj);

                me.unmask();
            } catch (e) {}
        }).error(function () {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', 'При получении параметров произошла ошибка', 'error');
        });
    },

    onClickSave: function(btn) {
        var me = this,
            pnl = btn.up('gisgmpparamspanel'),
            valuesToSave = btn.up('gisgmpparamspanel').getForm().getValues(),
            form = pnl.getForm();
        
        if (form.isValid()) {
            me.mask('Сохранение параметров...', pnl);

            B4.Ajax.request({
                url: B4.Url.action('SaveParams', 'GisGmpParams'),
                params: valuesToSave
            }).next(function() {
                Ext.Msg.alert('Успешно', 'Параметры успешно сохранены и будут применены после перезапуска приложения');
                me.unmask();
            }).error(function() {
                Ext.Msg.alert('Ошибка', 'При сохранении параметров произошла ошибка');
                me.unmask();
            });
        } else {
            //получаем все поля формы
            var fields = form.getFields();

            var invalidFields = '';

            //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
            Ext.each(fields.items, function(field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            //выводим сообщение
            Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
        }
    },

    updateStore: function () {
        this.getMainView().down('#gisGmpPatternGrid').getStore().load();
    }
});