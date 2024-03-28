Ext.define('B4.controller.YandexMap', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.YandexMapLoader',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse'
    ],

    mixins: {
        map: 'B4.mixins.YandexMapLoader',
        context: 'B4.mixins.Context'
    },
    
    views: [
        'realityobj.MapPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjmappanel'
        }
    ],
    
    index: function (id) {
        var me = this, args = {}, tmp,
            view = me.getMainView() || Ext.widget('realityobjmappanel');

        me.bindContext(view);
        me.application.deployView(view);
        
        B4.Ajax.request({
            url: B4.Url.action('getformap', 'realityobject'),
            method: 'POST',
            params: { id: id }
        }).next(function (response) {
            tmp = Ext.decode(response.responseText);
            args.record = tmp.data;
            me.loadMap(args);
        });
    }
});