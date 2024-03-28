Ext.define('B4.view.menu.DesktopDi', {
    extend: 'Ext.Component',
    alias: 'widget.desktopdi',
    itemId: 'desktopDiComponent',
    html: '<div class="wrapper">' +
            '<div class="info-holder">' +
                '<div class="title">' +
                    'Раздел для раскрытия информации организациями,<br> осуществляющими деятельность в сфере управления многоквартирными домами' +
                '</div>' +
                '<div class="help-text">' +
                    '<span>согласно стандартам, утвержденным постановлениями Правительств Российской Федерации</span> <br/><span class="help-text-inner">от 23 сентября 2010 г. № 731 и от 27 сентября 2014 г. № 988</span>' +
                '</div>' +
                '<ul class="newss-wrap">' +
                    '<li class="newss">' +
                        '<div class="text-wrap">' +
                            '<div class="titlee" style="padding-top: 4px; !important;"><i class="nIcon-attention"></i><span style="padding-left: 15px;">Важная информация<span>' +
                            '</div>' +
                            '<div class="textt">' +
                                'Обновлен раздел «Раскрытие информации». <br> <br>Теперь раскрываемость информации можно отслеживать с помощью процентов, указанных в разделах и подразделах, а также на главной странице в поле «Общий процент по раскрытию информации». <br> <br>' +
                                'Для того, чтобы увидеть актуальный процент, необходимо нажать кнопку «Пересчитать проценты».' +
                            '</div>' +
                        '</div>' +
                    '</li>' +
                    '<li class="newss">' +
                        '<div class="text-wrap">' +
                            '<div class="titlee">Новости' +
                            '</div>' +
                            '<div class="newss-block-wrap">' +
                                '<div class="newss-block">' +
                                    '<div class="newss-title">' +
                                        '<span class="newss-title-date">20.05.2013</span>' +
                                        '<span class="newss-title-text">ВАЖНАЯ НОВОСТЬ!</span>' +
                                    '</div>' +
                                    '<div class="newss-text">' +
                                        'Реализованы дополнительные возможности работы с данными, расположенные в разделе «Объекты в управлении» - кнопка «Действия».' +
                                    '</div>' +
                                    '</div>' +
                                '<div class="newss-block-divider"></div>' +
                                '<div class="newss-block">' +
                                    '<div class="newss-title">' +
                                        '<span class="newss-title-date">20.05.2013</span>' +
                                        '<span class="newss-title-text">ВАЖНАЯ НОВОСТЬ!</span>' +
                                    '</div>' +
                                    '<div class="newss-text">' +
                                        'Изменен процесс внесения данных по работам по текущему ремонту. Работы разделены на планово-предупредительные работы и техническое обслуживание.' +
                                    '</div>' +
                                    '</div>' +
                            '</div>' +
                            '</div>' +
                    '</li>' +
                '</ul>' +
            '</div>' +
        '</div>'
});
