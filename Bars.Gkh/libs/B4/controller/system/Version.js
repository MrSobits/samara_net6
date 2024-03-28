/**
 * Информация о версии сборки
 */
Ext.define('B4.controller.system.Version', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.system.VersionInfoPanel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'versioninfopanel'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('versioninfopanel');

        me.bindContext(view);
        me.application.deployView(view);
        me.update();
    },

    update: function () {
        var me = this,
            json;

        me.mask('Загрузка данных...');

        B4.Ajax.request({
            url: B4.Url.action('GetBuildInfo', 'SystemVersionInfo')
        }).next(function (response) {
            json = Ext.JSON.decode(response.responseText);
            if (json) {
                me.getMainView().down('form').getForm().setValues(json);
            } else {
                me.showErrorMsg();
            }

            me.unmask();
        }).error(function () {
            me.showErrorMsg();
            me.unmask();
        });
    },

    showErrorMsg: function(msg) {
        Ext.Msg.alert('Ошибка!', msg || 'Ошибка при получении данных!');
    }
});
