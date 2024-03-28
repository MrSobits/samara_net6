Ext.define('B4.controller.robject.ReformaRobject', {
    extend: 'B4.base.Controller',

    views: ['robject.ReformaPanel'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },


    params: null,
    mainView: 'robject.ReformaPanel',
    mainViewSelector: 'robjectreformapanel',

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('robjectreformapanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.loadRequestState(view);
    },

    loadRequestState: function(panel) {
        var me = this,
            form = panel.getForm();

        me.mask('Загрузка...', panel);
        B4.Ajax.request({
            url: B4.Url.action('GetDetails', 'Reforma'),
            params: {
                type: 'robject',
                id: me.getContextValue(me.getMainView(), 'realityObjectId')
            }
        }).next(function(resp) {
            me.unmask();
            var response = Ext.decode(resp.responseText);
            if (!response.data) {
                response.data = { ExternalId: 'Нет данных' };
            }

            form.setValues(response.data);
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении данных произошла ошибка!');
        });
    }
});