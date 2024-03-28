Ext.define('B4.controller.manorg.ReformaManOrg', {
    extend: 'B4.controller.MenuItemController',

    views: ['manorg.ReformaPanel'],

    params: {},
    mainView: 'manorg.ReformaPanel',
    mainViewSelector: 'manorgreformapanel',

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    mixins: {
        context: 'B4.mixins.Context'
    },

    loadRequestState: function(panel) {
        var form = panel.getForm();
        panel.mask('Загрузка...');
        B4.Ajax.request({
            url: B4.Url.action('GetDetails', 'Reforma'),
            params: {
                type: 'manorg',
                id: this.params.id
            }
        }).next(function(resp) {
            panel.unmask();
            var response = Ext.decode(resp.responseText);
            if (!response.data) {
                response.data = { RequestStatus: 'Нет данных' };
            }

            form.setValues(response.data);
        }).error(function() {
            panel.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении данных произошла ошибка!');
        });
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgreformapanel');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.loadRequestState(view);
    }
});