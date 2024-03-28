/*
Данный аспект предназначен для описание взаимодействия компонентов в карточках Основания проверки
Необходим для сохранения общих сведений и содержит методы для создания дочерних документов
*/

Ext.define('B4.aspects.GjiInspection', {
    extend: 'B4.aspects.GkhEditPanel',
    
    alias: 'widget.gjiinspectionaspect',

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        this.controller = controller;

        actions[this.editPanelSelector + ' #btnCancel'] = { 'click': { fn: this.btnCancelClick, scope: this } };

        actions[this.editPanelSelector] = {
            afterrender: function (panel) {
                var links = panel.query('[cls=alert-hyperlink]');

                // создаём событие у компонентов, которые этого события не имеют
                Ext.Array.forEach(links, function (cmp) {
                    if (cmp && Ext.isFunction(cmp.getEl)) {
                        cmp.getEl()
                            .on('click', function () {
                                cmp.fireEvent('click', cmp);
                            });
                    }
                });
            }
        };

        controller.control(actions);
    },

    //При нажатии на кнопку Оттмена мы просто загружаем существующие данные документа
    btnCancelClick: function () {
        this.setData(this.controller.params.inspectionId);
    }
});