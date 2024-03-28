Ext.define('B4.controller.import.chesimport.Compared', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditForm'
    ],

    models: [
        'AddressMatch',
        'import.chesimport.ChesMatchIndAccountOwner',
        'import.chesimport.ChesMatchLegalAccountOwner'
    ],
    mixins: { 
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody' 
    },

    mainView: 'import.chesimport.ComparedPanel',
    mainViewSelector: 'chesimportcomparedpanel',

    aspects: [
        {
            xtype: 'grideditformaspect',
            gridSelector: 'chesimportcomparedaddressgrid',
            modelName: 'AddressMatch'
        },
        {
            xtype: 'grideditformaspect',
            gridSelector: 'chesimportcomparedindownergrid',
            modelName: 'import.chesimport.ChesMatchIndAccountOwner'
        },
        {
            xtype: 'grideditformaspect',
            gridSelector: 'chesimportcomparedlegalownergrid',
            modelName: 'import.chesimport.ChesMatchLegalAccountOwner'
        }
    ],

    refs: [
        {
            ref: 'comparedAddressGrid',
            selector: 'chesimportcomparedpanel chesimportcomparedaddressgrid'
        },
        {
            ref: 'comparedIndOwnerGrid',
            selector: 'chesimportcomparedpanel chesimportcomparedindownergrid'
        },
        {
            ref: 'comparedLegalOwnerGrid',
            selector: 'chesimportcomparedpanel chesimportcomparedlegalownergrid'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'chesimportcomparedpanel grid' : {
                'viewready': {
                    fn: function(view) { view.getStore().load(); },
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
        me.setContextValue(view, 'periodId', id);
    }
});