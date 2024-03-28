/**
 * Окно предыдущих категорий проверок ГЖИ
 *
 * Все обработчики реализованы в самом компоненте, а не привязывается в контроллерах категорий во имя DRY.
 * Иначе пришлось бы писать повторяющийся код или реализовывать аспект. Не хочется разносить код по файлам,
 * поэтому без аспекта в данном случае. И так проще написать обработчики событий, чтобы ещё и на разных
 * вкладках с разными видами проверок работало.
 */
Ext.define('B4.view.inspectiongji.RiskPrevWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.view.inspectiongji.RiskPrevGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 500,
    minHeight: 300,
    title: 'Предыдущие категории риска',
    closeAction: 'destroy',
    modal: true,
    trackResetOnLoad: true,

    /**
     * Идентификатор проверки ГЖИ
     * Его должен установить контроллер, который создаёт это окно
     */
    inspectionId: 0,

    /**
     * Признак того, что данные успешно сохранились. Внешние контроллеры (аспекты) подписываются на закрытие окна и
     * перед закрытием смотрят на этот флаг. Если он установлен, то им необходимо перезагрузить данные,
     * потому что, что-то актуальная категория могла измениться.
     */
    saved: false,

    initComponent: function () {
        var me = this;

        me.on('afterrender', function(win) {
            win.down('inspectiongjiriskprevgrid').getStore().load();
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'inspectiongjiriskprevgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});