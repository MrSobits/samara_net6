Ext.define('B4.controller.CopyUninhabitablePremisesController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Url',
        'B4.Ajax'
    ],
    views: ['service.CopyUninhabitablePremisesEditPanel'],
    mainView: 'service.CopyUninhabitablePremisesEditPanel',
    mainViewSelector: '#copyUninhabitablePremisesEditPanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var actions = {};
        actions['copyUninhabitablePremisesEditPanel b4savebutton'] = { 'click': { fn: this.btnCopyClick, scope: this } };
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };

        this.control(actions);
    },

    onMainViewAfterRender: function () {
        if (this.params) {
            var me = this,
                copyEditPanel = this.getMainComponent();

            me.mask('Подождите', copyEditPanel);
            B4.Ajax.request(B4.Url.action('GetInfoServPeriod', 'Service', {
                disclosureInfoId: this.params.disclosureInfoId
            })).next(function (response) {
                var obj = Ext.decode(response.responseText);
                copyEditPanel.down('[name=sfManagingOrg]').setValue(obj.ManOrg);
                copyEditPanel.down('[name=sfPeriodDiCurrent]').setValue(obj.Period);
                me.unmask();
                return true;
            }).error(function () {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Не удалось получить сведения о использовании нежилых помещений');
                copyEditPanel.setDisabled(true);
            });
        }
    },

    btnCopyClick: function () {
        var me = this,
            copyEditPanel = me.getMainComponent();

        if (copyEditPanel.getForm().isValid()) {
            Ext.Msg.confirm('Подтверждение', 'Скопировать список сведений в выбранный отчетный период?', function(result) {
                if (result == 'yes') {

                    if (copyEditPanel.down('[name=sfPeriodDiCurrent]').getValue() == copyEditPanel.down('[name=sfPeriodDiFrom]').getValue()) {
                        Ext.Msg.alert('Копирование', 'Периоды копирования не должны совпадать');
                        return;
                    }

                    me.mask('Копирование', me.getMainComponent());
                    Ext.Ajax.request({
                        url: B4.Url.action('/Service/CopyUninhabitablePremisesPeriod'),
                        params: {
                            manorgId: copyEditPanel.down('[name=sfManagingOrg]').getValue(),
                            periodCurrentId: copyEditPanel.down('[name=sfPeriodDiCurrent]').getValue(),
                            periodFromId: copyEditPanel.down('[name=sfPeriodDiFrom]').getValue()
                        },
                        timeout: 1200000,
                        success: function (response) {
                            var resp = Ext.decode(response.responseText),
                                message = resp.message,
                                logFileId = resp.data,
                                url,
                                logLink = '';

                            if (logFileId) {
                                url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', logFileId));
                                logLink = '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать подробный лог</a>';
                            }

                            Ext.Msg.show({
                                title: 'Копирование',
                                msg: Ext.String.format('{0}&nbsp;{1}', message, logLink),
                                width: 300,
                                buttons: Ext.Msg.OK,
                                icon: Ext.window.MessageBox.INFO
                            });

                            me.unmask();
                            return true;
                        },
                        failure: function(response) {
                            var obj = Ext.decode(response.responseText);
                            me.unmask();
                            Ext.Msg.alert('Ошибка', obj.message || 'Не удалось скопировать сведения');
                        }
                    });
                }
            });
        } else {
            Ext.Msg.alert('Ошибка валидации', 'Необходимо заполнить все обязательные поля');
        }
    }
});