Ext.define('B4.controller.JurJournal', {
    extend: 'B4.base.Controller',
    requires: [],

    models: [],
    stores: [],
    views: ['jurjournal.Panel'],

    mainView: 'jurjournal.Panel',
    mainViewSelector: 'clwjurjournalpanel',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'clwjurjournalpanel'
        }
    ],

    aspects: [],

    init: function() {
        var me = this;

        me.control({
            'clwjurjournalpanel b4combobox[name=TypeBase]': { 'change': me.onChangeTypeBase }
  });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('clwjurjournalpanel');
        
        view.ctxKey = 'jurjournal'; // bindContext заменен на прямое определение ctxKey, 
        me.application.deployView(view);
    },
    
    deployTabs: function (controller, view) {
        var me = this,
            container = me.cmpQueryInContext('container[name=gridcontainer]')[0],
            viewSelector = Ext.String.format('#{0}', view.getItemId());
        if (container && !container.down(viewSelector)) {
            container.add(view);
        }
    },
    
    deployViewKeys: {
        jurjournal: 'deployTabs'
    },

    onChangeTypeBase: function(cmp, newValue) {
        var me = this,
            container = cmp.up('panel').down('container[name=gridcontainer]'),
            typeDocument = me.getMainView().down('b4enumcombo[name = TypeDocument]'),
            isDisabled = newValue === 'jurjournal/buildcontract';

        container.removeAll();

        if (newValue) {
            Ext.History.add(newValue);
        }

        typeDocument.setDisabled(isDisabled);
        typeDocument.setVisible(!isDisabled);
    }
});