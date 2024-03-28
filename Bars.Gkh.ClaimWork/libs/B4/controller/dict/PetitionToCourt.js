Ext.define('B4.controller.dict.PetitionToCourt', {
    extend: 'B4.base.Controller',

    views: [
        'dict.petition.PetitionToCourt'
    ],
    stores: ['dict.PetitionToCourt'],
    models: ['PetitionToCourt'],

    requires: [
        'B4.mixins.MaskBody',
        'B4.mixins.Context',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.InlineGrid',
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        { ref: 'mainView', selector: 'petitiontocourt' }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            mainView = me.getMainView() || Ext.widget('petitiontocourt');

        me.bindContext(mainView);
        me.application.deployView(mainView);

        mainView.getStore().load();
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'petitiontocourt',
            modelName: 'PetitionToCourt',
            listeners: {
                beforesave: function(asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        result = true;
                    Ext.each(modifRecords, function(rec) {
                        return result =
                            !Ext.isEmpty(rec.get('Code'))
                            && !Ext.isEmpty(rec.get('ShortName'))
                            && !Ext.isEmpty(rec.get('FullName'));
                    });

                    if (!result) {
                        Ext.Msg.alert('Предупреждение', 'Необходимо заполнить все поля', 'warning');
                        return false;
                    }
                }
            }
        }
    ]
});