Ext.define('B4.controller.Import.loadidis.LoadIdIs', {
    extend: 'B4.base.Controller',
    views: ['Import.loadidis.Panel'],
    
    mainView: 'Import.loadidis.Panel',
    mainViewSelector: 'loadIdIsPanel',
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView',
        selector: 'loadIdIsPanel'
    }],

    init: function () {

        function loadButtonClick() {
            var me = this;
            me.mask('Загрузка', me.getMainComponent());
            
            function run() {
                me.unmask();
                Ext.Msg.alert('Импорт', 'Неверный формат файла');
            }

            setTimeout(run, 2000);

        }

        this.control({
            'loadIdIsPanel button': {
                click: loadButtonClick
            }
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('loadIdIsPanel');
        this.bindContext(view);
        this.application.deployView(view);
    }
});