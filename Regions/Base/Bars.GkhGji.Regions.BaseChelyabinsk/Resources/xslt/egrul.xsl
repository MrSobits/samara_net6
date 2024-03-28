<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:date="http://exslt.org/dates-and-times" xmlns:gibdd="http://gibdd.ru/rev01" xmlns:ns2="http://orchestra.cos.ru/security/info/" xmlns:ns1="urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.5" xmlns:fl="http://ws.unisoft/EGRNXX/ResponseVIPFL" xmlns:a="http://orchestra.cos.ru/security/info/" xmlns:x="http://ws.unisoft/EGRNXX/ResponseVIPUL" xmlns:sec="http://orchestra.cos.ru/security/info/" xmlns:exslt="http://exslt.org/common" version="2.0">
	<!-- includeXML -->
	<xsl:param name="applicationXML"/>
	<xsl:param name="xmlURL"/>
	<xsl:attribute-set name="table.border">
		<xsl:attribute name="border">solid 1pt black</xsl:attribute>
	</xsl:attribute-set>
	<xsl:attribute-set name="table.cell.padding">
		<xsl:attribute name="padding-left">3pt</xsl:attribute>
		<xsl:attribute name="padding-right">3pt</xsl:attribute>
		<xsl:attribute name="padding-top">1pt</xsl:attribute>
		<xsl:attribute name="padding-bottom">1pt</xsl:attribute>
	</xsl:attribute-set>
	<xsl:param name="x" select="0"/>
	<xsl:variable name="mfcAdress" select="document($xmlURL)/Report4Service/MFC_Address"/>
	<xsl:variable name="MFCFullName" select="$xmlURL/Report4Service/MFCName"/>
	<xsl:variable name="AppInfo" select="."/>
	<xsl:variable name="main">
		<xsl:variable name="data">
			<xsl:apply-templates select="." mode="local"/>
		</xsl:variable>
		<xsl:copy-of select="exslt:node-set($data)"/>
	</xsl:variable>
	<xsl:variable name="founders">
		<xsl:for-each select="exslt:node-set($main)//СвЮЛ/СвУчредит/child::*">
			<item>
				<xsl:copy-of select="."/>
			</item>
		</xsl:for-each>
	</xsl:variable>
	<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя'"/>
	<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ'"/>
	<xsl:variable name="newStructure">
		<xsl:call-template name="transform">
			<xsl:with-param name="mainTitle" select="'Наименование'"/>
			<xsl:with-param name="title" select="'Полное наименование'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвНаимЮЛ/@НаимЮЛПолн"/>
		</xsl:call-template>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Сокращенное наименование'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвНаимЮЛ/@НаимЮЛСокр"/>
		</xsl:call-template>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($main)//СвНаимЮЛ"/>
		</xsl:call-template>
		<xsl:call-template name="transformAddressRF">
			<xsl:with-param name="address" select="exslt:node-set($main)//СвАдресЮЛ/АдресРФ"/>
			<xsl:with-param name="mainTitle" select="'Адрес (место нахождения)'"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвАдресЮЛ/СвНедАдресЮЛ">
			<xsl:for-each select="exslt:node-set($main)//СвАдресЮЛ/СвНедАдресЮЛ">
				<xsl:call-template name="transform">
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Дополнительные сведения'"/>
					<xsl:with-param name="value" select="@ТекстНедАдресЮЛ"/>
				</xsl:call-template>
				<xsl:if test="РешСудНедАдр">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого адрес признан недостоверным'"/>
						<xsl:with-param name="value" select="concat(РешСудНедАдр/@НаимСуда,', №',РешСудНедАдр/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСудНедАдр/@Дата"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="РешСудНедАдр"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвАдресЮЛ/СвРешИзмМН">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дополнительные сведения'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвАдресЮЛ/СвРешИзмМН/@ТекстРешИзмМН"/>
			</xsl:call-template>
			<xsl:call-template name="transformAddressRF">
				<xsl:with-param name="address" select="exslt:node-set($main)//СвАдресЮЛ/СвРешИзмМН"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвАдрЭлПочты">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Адрес электронной почты'"/>
				<xsl:with-param name="title" select="'Адрес электронной почты'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвАдрЭлПочты/@E-mail"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="exslt:node-set($main)//СвОбрЮЛ/СпОбрЮЛ/@КодСпОбрЮЛ!='03'">
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения о регистрации'"/>
					<xsl:with-param name="title" select="'Способ образования'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвОбрЮЛ/СпОбрЮЛ/@НаимСпОбрЮЛ"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвОбрЮЛ/@ОГРН"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения о регистрации'"/>
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвОбрЮЛ/@ОГРН"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Дата присвоения ОГРН'"/>
			<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвОбрЮЛ/@ДатаОГРН"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвОбрЮЛ/@РегНом">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Регистрацонный номер'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвОбрЮЛ/@РегНом"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвОбрЮЛ/@ДатаРег">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата регистрации'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвОбрЮЛ/@ДатаРег"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвОбрЮЛ/@НаимРО">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Орган регистрации'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвОбрЮЛ/@НаимРО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($main)//СвОбрЮЛ"/>
		</xsl:call-template>
		<xsl:call-template name="transform">
			<xsl:with-param name="mainTitle" select="'Сведения о регистрирующем органе по месту нахождения юридического лица'"/>
			<xsl:with-param name="title" select="'Код органа'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвРегОрг/@КодНО"/>
		</xsl:call-template>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвРегОрг/@НаимНО"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвРегОрг/@АдрРО">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Адрес регистрирующего органа'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвРегОрг/@АдрРО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($main)//СвРегОрг"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвСтатус">
			<xsl:for-each select="exslt:node-set($main)//СвСтатус">
				<xsl:choose>
					<xsl:when test="position()=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о состоянии (статусе) '"/>
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Наименование статуса'"/>
							<xsl:with-param name="value" select="@НаимСтатусЮЛ"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Наименование статуса'"/>
							<xsl:with-param name="value" select="@НаимСтатусЮЛ"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="СвРешИсклЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Дата решения о предстоящем исключении недействующего ЮЛ из ЕГРЮЛ'"/>
						<xsl:with-param name="valueDate" select="СвРешИсклЮЛ/@ДатаРеш"/>
					</xsl:call-template>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Номер решения о предстоящем исключении недействующего ЮЛ из ЕГРЮЛ'"/>
						<xsl:with-param name="value" select="СвРешИсклЮЛ/@НомерРеш"/>
					</xsl:call-template>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Номер решения о предстоящем исключении недействующего ЮЛ из ЕГРЮЛ'"/>
						<xsl:with-param name="valueDate" select="СвРешИсклЮЛ/@ДатаПубликации"/>
					</xsl:call-template>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Дата публикации решения о предстоящем исключении недействующего ЮЛ из ЕГРЮЛ'"/>
						<xsl:with-param name="valueDate" select="СвРешИсклЮЛ/@ДатаПубликации"/>
					</xsl:call-template>
					<xsl:if test="СвРешИсклЮЛ/@НомерЖурнала">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Номер журнала, в котором опубликовано решение'"/>
							<xsl:with-param name="value" select="СвРешИсклЮЛ/@НомерЖурнала"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвПрекрЮЛ">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о прекращении юридического лица'"/>
				<xsl:with-param name="title" select="'Дата прекращения юридического лица'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвПрекрЮЛ/@ДатаПрекрЮЛ"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Способ прекращения юридического лица'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвПрекрЮЛ/СпПрекрЮЛ/@НаимСпПрекрЮЛ"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Сведения о регистрирующем органе, внесшем запись о прекращении юридического лица'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвПрекрЮЛ/СвРегОрг/@НаимНО"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвПрекрЮЛ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвУчетНО">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения об учете в налоговом органе'"/>
				<xsl:with-param name="title" select="'ИНН'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвУчетНО/@ИНН"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'КПП'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвУчетНО/@КПП"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата постановки на учет в налоговом органе'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвУчетНО/@ДатаПостУч"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование налогового орага'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвУчетНО/СвНО/@НаимНО"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвУчетНО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвРегПФ">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о регистрации в качестве страхователя в территориальном органе Пенсионного фонда Российской Федерации'"/>
				<xsl:with-param name="title" select="'Регистрационный номер'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвРегПФ/@РегНомПФ"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата регистрации'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвРегПФ/@ДатаРег"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование территориального органа Пенсионного фонда'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвРегПФ/СвОргПФ/@НаимПФ"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвРегПФ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвРегФСС">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о регистрации в качестве страхователя в исполнительном органе Фонда социального страхования Российской Федерации'"/>
				<xsl:with-param name="title" select="'Регистрационный номер'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвРегФСС/@РегНомФСС"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата регистрации'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвРегФСС/@ДатаРег"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование исполнительного органа Фонда социального страхования Российской Федерации'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвРегФСС/СвОргФСС/@НаимФСС"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвРегФСС"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвУстКап">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения об  уставном капитале (складочного капитала, уставного фонда, паевого фонда)'"/>
				<xsl:with-param name="title" select="'Вид'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвУстКап/@НаимВидКап"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Размер (в рублях)'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвУстКап/@СумКап"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвУстКап/ДоляРубля">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Доля рубля в капитале'"/>
					<xsl:with-param name="value" select="concat(exslt:node-set($main)//СвУстКап/@ДоляРубля/@Числит,'/',exslt:node-set($main)//СвУстКап/@ДоляРубля/@Знаменат)"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвУстКап"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвУстКап/СведУмУК">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о нахождении хозяйственного общества в процессе уменьшения уставного капитала'"/>
				<xsl:with-param name="title" select="'Размер (в рублях)'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвУстКап/СведУмУК/@ДатаРеш"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвУстКап/СведУмУК"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвТипУстав">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения об использовании типового устава'"/>
				<xsl:with-param name="title" select="'Наименование государственного органа, утвердившего типовой устав'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@НаимГОУтвТУ"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Вид нормативного правового акта об утверждении типового устава'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@ВидНПА"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование нормативного правового акта об утверждении типового устава'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@НаимНПА"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Номер нормативного правового акта об утверждении типового устава'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@НомерНПА"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата нормативного правового акта об утверждении типового устава'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@ДатаНПА"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@НомерПрил">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Номер приложения'"/>
					<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвТипУстав/СвНПАУтвТУ/@НомерПрил"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвТипУстав"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвУпрОрг">
			<xsl:for-each select="exslt:node-set($main)//СвУпрОр">
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения об управляющей организации'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="empty"/>
					<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
					<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
				<xsl:if test="НаимИННЮЛ/@ОГРН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ОГРН'"/>
						<xsl:with-param name="value" select="НаимИННЮЛ/@ОГРН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="НаимИННЮЛ/@ИНН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ИНН'"/>
						<xsl:with-param name="value" select="НаимИННЮЛ/@ИНН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Полное наименование'"/>
					<xsl:with-param name="value" select="НаимИННЮЛ/@НимЮЛПолн"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="exslt:node-set($main)//НаимИННЮЛ"/>
				</xsl:call-template>
				<xsl:if test="СвРегИн">
					<xsl:call-template name="transform">
						<xsl:with-param name="emptyTitle" select="'Сведения о регистрации в стране происхождения'"/>
						<xsl:with-param name="title" select="'Страна происхождения'"/>
						<xsl:with-param name="value" select="СвРегИн/@НаимСтран"/>
					</xsl:call-template>
					<xsl:if test="СвРегИн/@ДатаРег">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата регистрации'"/>
							<xsl:with-param name="valueDate" select="СвРегИн/@ДатаРег"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвРегИн/@РегНомер">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Регистрационный номер'"/>
							<xsl:with-param name="value" select="СвРегИн/@РегНомер"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвРегИн/@НаимРегОрг">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
							<xsl:with-param name="value" select="СвРегИн/@НаимРегОрг"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвРегИн/@АдрСтр">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
							<xsl:with-param name="value" select="СвРегИн/@АдрСтр"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвРегИн"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвНедДанУпрОрг">
					<xsl:for-each select="СвНедДанУпрОрг">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Сведения о недостоверности данных об управляющей организации'"/>
							<xsl:with-param name="value" select="@ТекстНедДанУпрОрг"/>
						</xsl:call-template>
						<xsl:if test="РешСудНедДанУпрОрг">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
								<xsl:with-param name="value" select="concat(РешСудНедДанУпрОрг/@НаимСуда,', №',РешСудНедДанУпрОрг/@Номер)"/>
								<xsl:with-param name="valueDate" select="РешСудНедДанУпрОрг/@Дата"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="."/>
						</xsl:call-template>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="СвПредЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Полное наименование представительства или филиала в Российской Федерации, через которое иностранное ЮЛ осуществляет полномочия управляющей организации'"/>
						<xsl:with-param name="value" select="СвПредЮЛ/@НаимПредЮЛ"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвПредЮЛ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвАдрРФ">
					<xsl:call-template name="transformAddressRF">
						<xsl:with-param name="emptyTitle" select="'Сведения об адресе управляющей организации в Российской Федерации'"/>
						<xsl:with-param name="address" select="СвАдрРФ"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвАдрРФ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвНомТел">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Контактный телефон'"/>
						<xsl:with-param name="value" select="СвНомТел/@НомТел"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвНомТел"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="ПредИнЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="emptyTitle" select="'Сведения о лице, через которое иностранное юридическое лицо осуществляет полномочия управляющей организации'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="ПредИнЮЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="ПредИнЮЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:call-template name="transformFIO">
						<xsl:with-param name="fio" select="ПредИнЮЛ/СвФЛ"/>
						<xsl:with-param name="SubTitle" select="'Сведения о ФИО и (при наличии) ИНН ФЛ'"/>
					</xsl:call-template>
					<xsl:if test="ПредИнЮЛ/СвНомТел/@НомТел">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Контактный телефон'"/>
							<xsl:with-param name="value" select="ПредИнЮЛ/СвНомТел/@НомТел"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СведДолжнФЛ">
			<xsl:for-each select="exslt:node-set($main)//СведДолжнФЛ">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СведДолжнФЛ)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о лице, имеющем право без доверенности действовать от имени юридического лица'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
									<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
									<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
									<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
									<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о лице, имеющем право без доверенности действовать от имени юридического лица'"/>
							<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
							<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transformFIO">
					<xsl:with-param name="fio" select="СвФЛ"/>
				</xsl:call-template>
				<xsl:if test="СвДолжн/ОГРНИП">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ОГРН ИП - управляющего юридическим лицом'"/>
						<xsl:with-param name="value" select="СвДолжн/@ОГРНИП"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование вида должностного лица'"/>
					<xsl:with-param name="value" select="СвДолжн/@НаимВидДолжн"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование должности'"/>
					<xsl:with-param name="value" select="СвДолжн/@НаимДолжн"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="СвДолжн"/>
				</xsl:call-template>
				<xsl:if test="СвДолжн/СвНомТел">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Контактный телефон'"/>
						<xsl:with-param name="value" select="СвДолжн/СвНомТел/@НомТел"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвДолжн/СвНомТел"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвНедДанДолжнФЛ">
					<xsl:for-each select="СвНедДанДолжнФЛ">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Дополнительные сведения'"/>
							<xsl:with-param name="value" select="@ТекстНедДанДолжнФЛ"/>
						</xsl:call-template>
						<xsl:if test="РешСудНедДанДолжнФЛ">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
								<xsl:with-param name="value" select="concat(РешСудНедДанДолжнФЛ/@НаимСуда,', №',РешСудНедДанДолжнФЛ/@Номер)"/>
								<xsl:with-param name="valueDate" select="РешСудНедДанДолжнФЛ/@Дата"/>
							</xsl:call-template>
							<xsl:call-template name="transformGRNDate">
								<xsl:with-param name="node" select="."/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="СвДискв">
					<xsl:for-each select="СвДискв">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="subTitle" select="'Сведения о дисквалификации'"/>
							<xsl:with-param name="title" select="'Дата начала дисквалификации'"/>
							<xsl:with-param name="valueDate" select="@ДатаНачДискв"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата окончания дисквалификации'"/>
							<xsl:with-param name="valueDate" select="@ДатаОкончДискв"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата вынесения  судебным органом постановления о дисквалификации '"/>
							<xsl:with-param name="valueDate" select="@ДатаРеш"/>
						</xsl:call-template>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="."/>
						</xsl:call-template>
					</xsl:for-each>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвУчредит">
			<xsl:for-each select="exslt:node-set($founders)/item">
				<xsl:variable name="count" select="count(exslt:node-set($founders)/item)"/>
				<xsl:if test="УчрЮЛРос">
					<xsl:call-template name="transformFounderUL">
						<xsl:with-param name="node" select="УчрЮЛРос"/>
						<xsl:with-param name="Pos" select="position()"/>
						<xsl:with-param name="Count" select="$count"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="УчрЮЛИн">
					<xsl:call-template name="transformFounderUL">
						<xsl:with-param name="node" select="УчрЮЛИн"/>
						<xsl:with-param name="Pos" select="position()"/>
						<xsl:with-param name="Count" select="$count"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="УчрФЛ">
					<xsl:call-template name="transformFounderFL">
						<xsl:with-param name="node" select="УчрФЛ"/>
						<xsl:with-param name="Pos" select="position()"/>
						<xsl:with-param name="Count" select="$count"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="УчрРФСубМО">
					<xsl:call-template name="transformFounderSubj">
						<xsl:with-param name="node" select="УчрРФСубМО"/>
						<xsl:with-param name="Pos" select="position()"/>
						<xsl:with-param name="Count" select="$count"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="УчрПИФ">
					<xsl:call-template name="transformFounderPIF">
						<xsl:with-param name="Pos" select="position()"/>
						<xsl:with-param name="node" select="УчрПИФ"/>
						<xsl:with-param name="Count" select="$count"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвДоляООО">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о доле в уставном капитале общества с ограниченной ответственностью, принадлежащей обществу'"/>
				<xsl:with-param name="title" select="'Номинальная стоимость доли в рублях'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвДоляООО/@НоминСтоим"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвДоляООО/РазмерДоли">
				<xsl:if test="exslt:node-set($main)//СвДоляООО/РазмерДоли/Процент">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в процентах'"/>
						<xsl:with-param name="value" select="exslt:node-set($main)//СвДоляООО/РазмерДоли/Процент"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="exslt:node-set($main)//СвДоляООО/РазмерДоли/ДробДесят">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в десятичных дробях'"/>
						<xsl:with-param name="value" select="exslt:node-set($main)//СвДоляООО/РазмерДоли/ДробДесят"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="exslt:node-set($main)//СвДоляООО/РазмерДоли/ДробПрост">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в простых дробях'"/>
						<xsl:with-param name="value" select="concat(exslt:node-set($main)//СвДоляООО/РазмерДоли/ДробПрост/@Числит,'/',exslt:node-set($main)//СвДоляООО/РазмерДоли/ДробПрост/@Знаменат)"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="exslt:node-set($main)//СвДоляООО"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвДоляООО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвДержРеестрАО">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о держателе реестра акционеров акционерного общества'"/>
				<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвДержРеестрАО/ГРНДатаПерв/@ГРН"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвДержРеестрАО/ГРНДатаПерв/@ДатаЗаписи"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО/@ОГРН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО/@ОГРН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО/@ИНН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ИНН'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО/@ИНН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Полное наименование'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО/@НаимЮЛПолн"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвДержРеестрАО/ДержРеестрАО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвОКВЭД">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle">
					<xsl:value-of select="'Сведения о видах экономической деятельности по Общероссийскому классификатору видов экономической деятельности'"/>&#xA0;
								<xsl:call-template name="typeOKVED">
						<xsl:with-param name="type" select="exslt:node-set($main)//СвОКВЭД/СвОКВЭДОсн/@ПрВерсОКВЭД"/>
					</xsl:call-template>
				</xsl:with-param>
				<xsl:with-param name="subTitle" select="'Сведения об основном виде деятельности'"/>
				<xsl:with-param name="title" select="'Код и наименование вида деятельности'"/>
				<xsl:with-param name="value" select="concat(exslt:node-set($main)//СвОКВЭД/СвОКВЭДОсн/@КодОКВЭД,' ',exslt:node-set($main)//СвОКВЭД/СвОКВЭДОсн/@НаимОКВЭД)"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвОКВЭД/СвОКВЭДОсн"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвОКВЭД/СвОКВЭДДоп">
				<xsl:for-each select="exslt:node-set($main)//СвОКВЭД/СвОКВЭДДоп">
					<xsl:choose>
						<xsl:when test="count(exslt:node-set($main)//СвОКВЭД/СвОКВЭДДоп)!=1">
							<xsl:choose>
								<xsl:when test="position()=1">
									<xsl:call-template name="transform">
										<xsl:with-param name="subTitle" select="'Сведения о дополнительных видах деятельности'"/>
										<xsl:with-param name="pos" select="position()"/>
										<xsl:with-param name="title" select="'Код и наименование вида деятельности'"/>
										<xsl:with-param name="value" select="concat(@КодОКВЭД,' ',@НаимОКВЭД)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="transform">
										<xsl:with-param name="pos" select="position()"/>
										<xsl:with-param name="title" select="'Код и наименование вида деятельности'"/>
										<xsl:with-param name="value" select="concat(@КодОКВЭД,' ',@НаимОКВЭД)"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="transform">
								<xsl:with-param name="subTitle" select="'Сведения о дополнительных видах деятельности'"/>
								<xsl:with-param name="title" select="'Код и наименование вида деятельности'"/>
								<xsl:with-param name="value" select="concat(@КодОКВЭД,' ',@НаимОКВЭД)"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="(.)"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:if>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвЛицензия">
			<xsl:for-each select="exslt:node-set($main)//СвЛицензия">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвЛицензия)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="subTitle" select="'Сведения о лицензиях, выданных юридическому лицу'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Номер лицензии'"/>
									<xsl:with-param name="value" select="@НомЛиц"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Номер лицензии'"/>
									<xsl:with-param name="value" select="@НомЛиц"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о лицензиях, выданных юридическому лицу'"/>
							<xsl:with-param name="title" select="'Номер лицензии'"/>
							<xsl:with-param name="value" select="@НомЛиц"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата лицензии'"/>
					<xsl:with-param name="valueDate" select="@ДатаЛиц"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата начала действия лицензии'"/>
					<xsl:with-param name="valueDate" select="@ДатаНачЛиц"/>
				</xsl:call-template>
				<xsl:if test="@ДатаОкончЛиц">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Дата окончания действия лицензии'"/>
						<xsl:with-param name="valueDate" select="@ДатаОкончЛиц"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:for-each select="НаимЛицВидДеят">
					<xsl:choose>
						<xsl:when test="count(НаимЛицВидДеят)!=1">
							<xsl:call-template name="transform">
								<xsl:with-param name="pos" select="position()"/>
								<xsl:with-param name="title" select="'Вид лицензируемой деятельности'"/>
								<xsl:with-param name="value" select="."/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Вид лицензируемой деятельности'"/>
								<xsl:with-param name="value" select="."/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
				<xsl:if test="МестоДейстЛиц">
					<xsl:for-each select="МестоДейстЛиц">
						<xsl:choose>
							<xsl:when test="count(МестоДейстЛиц)!=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Место действия лицензии'"/>
									<xsl:with-param name="value" select="МестоДейстЛиц"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="title" select="'Место действия лицензии'"/>
									<xsl:with-param name="value" select="МестоДейстЛиц"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="ЛицОргВыдЛиц">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Наименование лицензирующего органа, выдавшего или переоформившего лицензию'"/>
						<xsl:with-param name="value" select="ЛицОргВыдЛиц"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
				<xsl:if test="СвПриостЛиц">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Дата приостановления действия лицензии'"/>
						<xsl:with-param name="valueDate" select="СвПриостЛиц/@ДатаПриостЛиц"/>
					</xsl:call-template>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Наименование лицензирующего органа, приостановившего действие лицензии'"/>
						<xsl:with-param name="value" select="СвПриостЛиц/@ЛицОргПриостЛиц"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвПриостЛиц"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвПодразд">
			<xsl:if test="exslt:node-set($main)//СвПодразд/СвФилиал">
				<xsl:call-template name="transformFilial">
					<xsl:with-param name="node" select="exslt:node-set($main)//СвПодразд/СвФилиал"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="exslt:node-set($main)//СвПодразд/СвПредстав">
				<xsl:call-template name="transformFilial">
					<xsl:with-param name="node" select="exslt:node-set($main)//СвПодразд/СвФилиал"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвРеорг">
			<xsl:for-each select="exslt:node-set($main)//СвРеорг">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвРеорг)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения об участии в реорганизации'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Код и наименование формы реорганизации (статуса) юридического лиц'"/>
									<xsl:with-param name="value" select="concat(СвСтатус/@КодСтатусЮЛ,' ',СвСтатус/@НаимСтатусЮЛ)"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Код и наименование формы реорганизации (статуса) юридического лиц'"/>
									<xsl:with-param name="value" select="concat(СвСтатус/@КодСтатусЮЛ,' ',СвСтатус/@НаимСтатусЮЛ)"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения об участии в реорганизации'"/>
							<xsl:with-param name="title" select="'Код и наименование формы реорганизации (статуса) юридического лиц'"/>
							<xsl:with-param name="value" select="concat(СвСтатус/@КодСтатусЮЛ,' ',СвСтатус/@НаимСтатусЮЛ)"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвПредш">
			<xsl:for-each select="exslt:node-set($main)//СвПредш">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвПредш)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'	Сведения о правопредшественнике'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРН'"/>
									<xsl:with-param name="value" select="@ОГРН"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРН'"/>
									<xsl:with-param name="value" select="@ОГРН"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о правопредшественнике'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="@ОГРН"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="@ИНН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ИНН'"/>
						<xsl:with-param name="value" select="@ИНН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
					<xsl:with-param name="value" select="@НаимЮЛПолн"/>
				</xsl:call-template>
				<xsl:if test="СвЮЛсложнРеорг">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о ЮЛ, путем реорганизации которого был создан правопредшественник при реорганизации в форме выделения или разделения с одновременным присоединением или слиянием'"/>
						<xsl:with-param name="title" select="'ОГРН'"/>
						<xsl:with-param name="value" select="СвЮЛсложнРеорг/@ОГРН"/>
					</xsl:call-template>
					<xsl:if test="СвЮЛсложнРеорг/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЮЛсложнРеорг/@ИНН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transform">
						<xsl:with-param name="number" select="98"/>
						<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
						<xsl:with-param name="value" select="СвЮЛсложнРеорг/@НаимЮЛПолн"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвКФХПредш">
			<xsl:for-each select="exslt:node-set($main)//СвКФХПредш">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвКФХПредш)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о крестьянском (фермерском) хозяйстве, на базе имущества которого создано юридическое лицо'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРНИП'"/>
									<xsl:with-param name="value" select="@ОГРНИП"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРНИП'"/>
									<xsl:with-param name="value" select="@ОГРНИП"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о крестьянском (фермерском) хозяйстве, на базе имущества которого создано юридическое лицо'"/>
							<xsl:with-param name="title" select="'ОГРНИП'"/>
							<xsl:with-param name="value" select="@ОГРНИП"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transformFIO">
					<xsl:with-param name="fio" select="СвФЛ"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвПреем">
			<xsl:for-each select="exslt:node-set($main)//СвПреем">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвПреем)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о правопреемнике'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРН'"/>
									<xsl:with-param name="value" select="@ОГРН"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРН'"/>
									<xsl:with-param name="value" select="@ОГРН"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о правопреемнике'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="@ОГРН"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="@ИНН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ИНН'"/>
						<xsl:with-param name="value" select="@ИНН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
					<xsl:with-param name="value" select="@НаимЮЛПолн"/>
				</xsl:call-template>
				<xsl:if test="СвЮЛсложнРеорг">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о ЮЛ, которое было создано в форме слияния с участием правопреемника, или к которому присоединился правопреемник при реорганизации в форме выделения или разделения с одновременным присоединением или слиянием'"/>
						<xsl:with-param name="title" select="'ОГРН'"/>
						<xsl:with-param name="value" select="СвЮЛсложнРеорг/@ОГРН"/>
					</xsl:call-template>
					<xsl:if test="СвЮЛсложнРеорг/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЮЛсложнРеорг/@ИНН"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
							<xsl:with-param name="value" select="СвЮЛсложнРеорг/@НаимЮЛПолн"/>
						</xsl:call-template>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="."/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвКФХПреем">
			<xsl:for-each select="exslt:node-set($main)//СвКФХПреем">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвКФХПреем)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о крестьянском (фермерском) хозяйстве, которые  внесены в ЕГРИП в связи с приведением правового статуса крестьянского (фермерского) хозяйства в соответствие с нормами части первой Гражданского кодекса Российской Федерации'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРНИП'"/>
									<xsl:with-param name="value" select="@ОГРНИП"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ОГРНИП'"/>
									<xsl:with-param name="value" select="@ОГРНИП"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о крестьянском (фермерском) хозяйстве, которые  внесены в ЕГРИП в связи с приведением правового статуса крестьянского (фермерского) хозяйства в соответствие с нормами части первой Гражданского кодекса Российской Федерации'"/>
							<xsl:with-param name="title" select="'ОГРНИП'"/>
							<xsl:with-param name="value" select="@ОГРНИП"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
			<xsl:call-template name="transformFIO">
				<xsl:with-param name="fio" select="СвФЛ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвЗапЕГРЮЛ">
			<xsl:for-each select="exslt:node-set($main)//СвЗапЕГРЮЛ">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвЗапЕГРЮЛ)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о записях, внесенных в ЕГРЮЛ'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ГРН и дата внесения записи в ЕГРЮЛ'"/>
									<xsl:with-param name="value" select="@ГРН"/>
									<xsl:with-param name="valueDate" select="@ДатаЗап"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'ГРН и дата внесения записи в ЕГРЮЛ'"/>
									<xsl:with-param name="value" select="@ГРН"/>
									<xsl:with-param name="valueDate" select="@ДатаЗап"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о записях, внесенных в ЕГРЮЛ'"/>
							<xsl:with-param name="title" select="'ГРН и дата внесения записи в ЕГРЮЛ'"/>
							<xsl:with-param name="value" select="@ГРН"/>
							<xsl:with-param name="valueDate" select="@ДатаЗап"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Причина внесения записи в ЕГРЮЛ'"/>
					<xsl:with-param name="value" select="ВидЗап/@НаимВидЗап"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование регистрирующего (налогового) органа, которым запись внесена в ЕГРЮЛ'"/>
					<xsl:with-param name="value" select="СвРегОрг/@НаимНО"/>
				</xsl:call-template>
				<xsl:if test="СведПредДок">
					<xsl:for-each select="СведПредДок">
						<xsl:variable name="last" select="last()"/>
						<!--	<xsl:choose>
								<xsl:when test="count(СведПредДок)!=1">-->
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="emptyTitle" select="'Сведения о документах, представленных при внесении записи в ЕГРЮЛ'"/>
									<xsl:with-param name="title" select="'Наименование документа'"/>
									<xsl:with-param name="value" select="НаимДок"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="empty" select="'&#xA0;'"/>
									<xsl:with-param name="title" select="'Наименование документа'"/>
									<xsl:with-param name="value" select="НаимДок"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
						<!--</xsl:when>
								<xsl:otherwise>
								<xsl:call-template name="transform">
							<xsl:with-param name="emptyTitle" select="'Сведения о документах, представленных при внесении записи в ЕГРЮЛ'"/>
						-->
						<!--	<xsl:with-param name="empty" select="'&#xA0;'"/>-->
						<!--
								<xsl:with-param name="title" select="'Наименование документа'"/>
									<xsl:with-param name="value" select="НаимДок"/>
						</xsl:call-template>
								</xsl:otherwise>
								</xsl:choose>-->
						<xsl:if test="НомДок">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Номер документа'"/>
								<xsl:with-param name="value" select="НомДок"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="ДатаДок">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Дата документа'"/>
								<xsl:with-param name="valueDate" select="ДатаДок"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="СвСвид">
					<xsl:for-each select="СвСвид">
						<!--<xsl:choose>
								<xsl:when test="count(СвСвид)!=1">-->
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="emptyTitle" select="'Сведения о свидетельстве, подтверждающем факт внесения записи в ЕГРЮЛ'"/>
									<!--<xsl:with-param name="pos" select="position()"/>-->
									<xsl:with-param name="title" select="'Серия, номер и дата выдачи свидетельства'"/>
									<xsl:with-param name="value" select="concat(@Серия,' ',@Номер)"/>
									<xsl:with-param name="valueDate" select="@ДатаВыдСвид"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<!--xsl:with-param name="pos" select="position()"/-->
									<xsl:with-param name="empty" select="'&#xA0;'"/>
									<xsl:with-param name="title" select="'Серия, номер и дата выдачи свидетельства'"/>
									<xsl:with-param name="value" select="concat(@Серия,' ',@Номер)"/>
									<xsl:with-param name="valueDate" select="@ДатаВыдСвид"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
						<!--</xsl:when>
								<xsl:otherwise>
								<xsl:call-template name="transform">
							<xsl:with-param name="emptyTitle" select="'Сведения о свидетельстве, подтверждающем факт внесения записи в ЕГРЮЛ'"/>
								<xsl:with-param name="title" select="'Серия, номер и дата выдачи свидетельства'"/>
									<xsl:with-param name="value" select="concat(@Серия,' ',@Номер)"/>
									<xsl:with-param name="valueDate" select="@ДатаВыдСвид"/>
						</xsl:call-template>
								</xsl:otherwise>
								</xsl:choose>-->
						<xsl:if test="ГРНДатаСвидНед">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ записи, содержащей сведения о признании свидетельства недействительным по решению суда '"/>
								<xsl:with-param name="value" select="ГРНДатаСвидНед/@ГРН"/>
								<xsl:with-param name="valueDate" select="ГРНДатаСвидНед/@ДатаЗаписи"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="ГРНДатаИспрПред">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ГРН и дата записи, в которую внесены исправления'"/>
						<xsl:with-param name="value" select="ГРНДатаИспрПред/@ГРН"/>
						<xsl:with-param name="valueDate" select="ГРНДатаИспрПред/@ДатаЗаписи"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="ГРНДатаНедПред">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ГРН и дата записи, которая признана недействительной'"/>
						<xsl:with-param name="value" select="ГРНДатаНедПред/@ГРН"/>
						<xsl:with-param name="valueDate" select="ГРНДатаНедПред/@ДатаЗаписи"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвСтатусЗап">
					<xsl:if test="ГРНДатаНед">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ГРН и дата внесения записи, которой запись признана недействительной'"/>
							<xsl:with-param name="value" select="СвСтатусЗап/ГРНДатаНед/@ГРН"/>
							<xsl:with-param name="valueDate" select="СвСтатусЗап/ГРНДатаНед/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="ГРНДатаИспр">
						<xsl:for-each select="ГРНДатаИспр">
							<xsl:choose>
								<xsl:when test="count(ГРНДатаИспр)!=1">
									<xsl:call-template name="transform">
										<xsl:with-param name="pos" select="position()"/>
										<xsl:with-param name="title" select="'ГРН и дата записи, которой внесены исправления в связи с технической ошибкой'"/>
										<xsl:with-param name="value" select="@ГРН"/>
										<xsl:with-param name="valueDate" select="@ДатаЗаписи"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="transform">
										<xsl:with-param name="title" select="'ГРН и дата записи, которой внесены исправления в связи с технической ошибкой'"/>
										<xsl:with-param name="value" select="@ГРН"/>
										<xsl:with-param name="valueDate" select="@ДатаЗаписи"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:variable>
	<xsl:template match="/">
		<!-- stdVariable -->
		<!--<xsl:variable name="Applicant" select="$applicationXML/Application/Applicants/Person[RepresentativeID != ' ' or (IsApplicant = 'true' and not (ancestor::Application/Applicants/Personexslt:node-set($main)//RepresentativeID != ' '))]"/>-->
		<xsl:variable name="security" select="exslt:node-set($main)//*[local-name()='securityData']"/>
		<xsl:variable name="smevSign" select="$security/signatureData[1]"/>
		<fo:root font-size="10pt" font-family="Times New Roman">
			<fo:layout-master-set>
				<fo:simple-page-master master-name="A4" page-width="210mm" page-height="297mm" margin-top="2cm" margin-bottom="2cm" margin-left="2cm" margin-right="1.5cm">
					<fo:region-body region-name="xsl-region-body" padding-bottom="0.5cm"/>
					<fo:region-after region-name="xsl-region-after" extent="0.1cm"/>
					<!-- <fo:region-after extent="1cm"/> -->
				</fo:simple-page-master>
			</fo:layout-master-set>
			<fo:page-sequence master-reference="A4">
				<fo:static-content flow-name="xsl-region-after">
					<xsl:choose>
						<xsl:when test="exslt:node-set($main)//СвЮЛ">
							<fo:block text-align="center" font-family="Times New Roman" font-size="11pt">
								<fo:table space-before="5mm">
									<fo:table-column column-width="45%"/>
									<fo:table-column column-width="45%"/>
									<fo:table-column column-width="10%"/>
									<fo:table-body>
										<fo:table-row>
											<fo:table-cell text-align="left">
												<!--fo:block>
													Выписка из ЕГРЮЛ	
										</fo:block-->
												<!--fo:block>
													<xsl:call-template name="toDate">
														<xsl:with-param name="dateTime" select="date:date-time()"/>
													</xsl:call-template>&#xA0;<xsl:value-of select="substring(date:date-time(), 12 ,8)"/>
												</fo:block>
											</fo:table-cell>
											<fo:table-cell text-align="center">
												<fo:block-->
												<!--<xsl:choose>
												<xsl:when test="exslt:node-set($main)//СвИП">
													<xsl:value-of select="concat('ОГРНИП ',exslt:node-set($main)//СвИП/@ОГРНИП)"/>
												</xsl:when>
												<xsl:otherwise>-->
												<!--xsl:value-of select="concat('ОГРН ',exslt:node-set($main)//СвЮЛ/@ОГРН)"/-->
												<!--</xsl:otherwise>
											</xsl:choose>-->
												<!--/fo:block-->
											</fo:table-cell>
											<fo:table-cell text-align="right">
												<fo:block>Страница <fo:page-number/>
													<!--из-->
													<!--fo:page-number-citation ref-id="terminator"/-->
												</fo:block>
											</fo:table-cell>
										</fo:table-row>
									</fo:table-body>
								</fo:table>
							</fo:block>
						</xsl:when>
						<xsl:otherwise/>
					</xsl:choose>
				</fo:static-content>
				<fo:flow flow-name="xsl-region-body" space-before="5mm">
					<xsl:choose>
						<xsl:when test="exslt:node-set($main)//СвЮЛ">
							<xsl:call-template name="UL">
								<xsl:with-param name="security" select="$security"/>
								<xsl:with-param name="smevSign" select="$smevSign"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<fo:block text-align="center" font-family="Times New Roman" font-size="12pt">
						Сведения из Единого государственного реестра юридических лиц
												</fo:block>
							<fo:block text-align="center" font-family="Times New Roman" font-size="14pt" padding-top="10mm" padding-bottom="10mm">
								<xsl:if test="exslt:node-set($main)//КодОбр = '01'">
												Ответ: Запрашиваемые сведения не найдены
												</xsl:if>
								<xsl:if test="exslt:node-set($main)//КодОбр = '53'">
												Ответ: Сведения в отношении юридического лица не могут быть предоставлены в электронном виде
												</xsl:if>
								<xsl:if test="//status">Ответ: 
									<xsl:value-of select="//status/name"/>
								</xsl:if>
							</fo:block>
						</xsl:otherwise>
					</xsl:choose>
					<fo:block padding-top="5mm" text-align="left">
					</fo:block>
					<!--fo:block padding-top="5mm" text-align="left">
						Сведения, содержащиеся  настоящей выписке, получены из:
					</fo:block>
					<fo:block padding-top="5mm" text-align="left" border-bottom="0.2pt solid black">
						ИС "Автоматизированная информационная система "ФЦОД" ФНС"
					</fo:block>
					
					<fo:block>&#xA0;</fo:block-->
					<!--fo:block font-weight="bold">Реквизиты ключа проверки электронной подписи:</fo:block>
					<fo:block>а) серийный номер сертификата ключа проверки электронной подписи: <xsl:value-of select="$smevSign//serialNumber"/>
					</fo:block>
					<fo:block>б) срок действия сертификата ключа проверки электронной подписи: c <xsl:call-template name="toDate">
							<xsl:with-param name="dateTime" select="$smevSign//validFrom"/>
						</xsl:call-template> по <xsl:call-template name="toDate">
							<xsl:with-param name="dateTime" select="$smevSign//validUntil"/>
						</xsl:call-template>
					</fo:block>
					<fo:block>в) кому выдан: <xsl:value-of select="$smevSign//subjectParts/part[@type='CN']"/>, <xsl:value-of select="$smevSign//subjectParts/part[@type='OU']"/>, <xsl:value-of select="$smevSign//subjectParts/part[@type='O']"/>, <xsl:value-of select="$smevSign//subjectParts/part[@type='L']"/>, <xsl:value-of select="$smevSign//subjectParts/part[@type='STREET']"/>.</fo:block>
					<mfcInfo>
					<fo:block page-break-inside="avoid">
						<fo:block font-weight="bold">Полное наименование и местонахождение ОГВ, выдавшего выписку из информационной системы:</fo:block>
						<fo:block>
							<xsl:value-of select="$MFCFullName"/>
							<xsl:if test="string-length($mfcAdress) > 1 ">
								<xsl:text>, </xsl:text>
								<xsl:value-of select="$mfcAdress"/>
							</xsl:if>
						</fo:block>
						<fo:block-container height="3mm">
							<fo:block>
								<xsl:text>&#160;</xsl:text>
							</fo:block>
						</fo:block-container>
						<								<fo:block>
									<xsl:value-of select="$MFCFullName"/>
									<xsl:text>подтверждает неизменность информации, полученной из подсистемы межведомственного взаимодействия автоматизированной информационной системы ФССП России</xsl:text>
								</fo:block>
								<fo:block-container height="3mm">
									<fo:block>
										<xsl:text>&#160;</xsl:text>
									</fo:block>
								</fo:block-container>
>
						<fo:block font-weight="bold">Дата и время составления выписки из информационной системы:</fo:block>
						<fo:block>
							<xsl:call-template name="toDateEx">
								<xsl:with-param name="dateTime" select="date:date-time()"/>
							</xsl:call-template>&#xA0;<xsl:value-of select="substring(date:date-time(), 12 ,8)"/>
						</fo:block>
						<fioSign>
						<fo:block padding-bottom="3mm">
								</fo:block>
						<fo:table>
							<fo:table-column column-number="1" column-width="60%"/>
							<fo:table-column column-number="2" column-width="15%"/>
							<fo:table-column column-number="3" column-width="25%"/>
							<fo:table-body>
								<fo:table-row>
									<fo:table-cell padding="2pt" text-align="center">
										<xsl:if test="$applicationXML//TechnicianSignature/WhoRegistered">	
													<fo:block border-bottom="0.2pt solid black"><xsl:value-of select="$applicationXML//TechnicianSignature/WhoRegistered"/></fo:block>
                                                  </xsl:if>
										<fo:block border-bottom="0.2pt solid black">&#xA0;</fo:block>
									</fo:table-cell>
									<fo:table-cell/>
									<fo:table-cell padding="2pt" text-align="center">
										<fo:block border-bottom="0.2pt solid black">&#xA0;</fo:block>
									</fo:table-cell>
								</fo:table-row>
								<fo:table-row>
									<fo:table-cell text-align="center" font-size="9pt">
										<fo:block>(фамилия имя и отчество уполномоченного сотрудника)</fo:block>
									</fo:table-cell>
									<fo:table-cell/>
									<fo:table-cell text-align="center" font-size="9pt">
										<fo:block>(подпись)</fo:block>
									</fo:table-cell>
								</fo:table-row>
								<fo:table-row>
									<fo:table-cell/>
									<fo:table-cell/>
									<fo:table-cell>
										<fo:block>м.п.</fo:block>
									</fo:table-cell>
								</fo:table-row>
							</fo:table-body>
						</fo:table>
					</fo:block -->
					<!--<fo:block padding-top="2mm" text-align="left">
						Реквизиты сертификата ключа проверки электронной подписи:
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						Серийный номер: <xsl:value-of select="$smevSign//serialNumber"/>
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						Cрок действия: <xsl:if test="$smevSign//validUntil[.!='']">
							<xsl:call-template name="toDate">
								<xsl:with-param name="dateTime" select="$smevSign//validUntil"/>
							</xsl:call-template>
						</xsl:if>
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						Владелец электронной подписи: <xsl:value-of select="$smevSign//subjectParts/part[@type='CN']"/>, <xsl:value-of select="$smevSign//subjectParts/part[@type='SURNAME']"/>&#160;<xsl:value-of select="$smevSign//subjectParts/part[@type='GIVENNAME']"/>
					</fo:block>
					<fo:block padding-top="10mm" text-align="left">
						Выписка выдана: <xsl:value-of select="document($xmlURL)//MFCName"/>
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						Местонахождение: <xsl:value-of select="document($xmlURL)//MFC_Address"/>
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						<xsl:value-of select="document($xmlURL)//MFCName"/> подтверждает неизменность информации, полученной из Автоматизированной информационной системы "ФЦОД" ФНС
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
						Уполномоченный сотрудник <xsl:value-of select="document($xmlURL)//MFCNameRod"/>
					</fo:block>
					<fo:table space-before="10mm">
						<fo:table-column column-width="50%"/>
						<fo:table-column column-width="50%"/>
						<fo:table-body>
							<fo:table-row>
								<fo:table-cell>
									<fo:block text-align="left">
										_________________________________
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block text-align="right">
										_____________________________________________
									</fo:block>
								</fo:table-cell>
							</fo:table-row>
							<fo:table-row>
								<fo:table-cell>
									<fo:block text-align="left" font-size="10pt">
										(подпись)
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block text-align="right" font-size="10pt">
										(ФИО сотрудника)
									</fo:block>
								</fo:table-cell>
							</fo:table-row>
							<fo:table-row>
								<fo:table-cell>
									<fo:block text-align="left" font-size="10pt">
										М.П.
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block text-align="right">
										
									</fo:block>
								</fo:table-cell>
							</fo:table-row>
						</fo:table-body>
					</fo:table>
					<fo:block text-align="left" padding-before="5mm">
						Дата и время составления выписки: <xsl:choose>
							<xsl:when test="$security/timestamp[.!='']">
								<xsl:if test="$security/timestamp[.!='']">
									<xsl:call-template name="toDateEx">
										<xsl:with-param name="dateTime" select="$security/timestamp"/>
									</xsl:call-template>
								</xsl:if>&#xA0;<xsl:if test="$security/timestamp[.!='']">
									<xsl:value-of select="substring($security/timestamp, 12 ,8)"/>
								</xsl:if>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="toDateEx">
									<xsl:with-param name="dateTime" select="date:date-time()"/>
								</xsl:call-template>&#xA0;<xsl:value-of select="substring(date:date-time(), 12 ,8)"/>
							</xsl:otherwise>
						</xsl:choose>
					</fo:block>-->
					<fo:block id="terminator"/>
				</fo:flow>
			</fo:page-sequence>
		</fo:root>
	</xsl:template>
	<xsl:template name="transform">
		<xsl:param name="mainTitle"/>
		<xsl:param name="subTitle"/>
		<xsl:param name="empty"/>
		<xsl:param name="emptyTitle"/>
		<xsl:param name="pos"/>
		<xsl:param name="title"/>
		<xsl:param name="value"/>
		<xsl:param name="valueDate"/>
		<elem>
			<xsl:if test="$mainTitle!='' and ($pos=1 or $pos='')">
				<xsl:attribute name="mainTitle"><xsl:value-of select="$mainTitle"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="$subTitle!='' and ($pos=1 or $pos='')">
				<xsl:attribute name="subTitle"><xsl:value-of select="$subTitle"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="$emptyTitle!=''">
				<xsl:attribute name="emptyTitle"><xsl:value-of select="$emptyTitle"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="$empty!=''">
				<xsl:attribute name="empty"><xsl:value-of select="$empty"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="$pos!=''">
				<xsl:attribute name="pos"><xsl:value-of select="$pos"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="title"><xsl:value-of select="$title"/></xsl:attribute>
			<xsl:attribute name="value"><xsl:if test="$value"><xsl:value-of select="$value"/>&#xA0;</xsl:if><xsl:if test="$valueDate"><xsl:call-template name="toDate"><xsl:with-param name="dateTime" select="$valueDate"/></xsl:call-template></xsl:if></xsl:attribute>
		</elem>
	</xsl:template>
	<xsl:template name="transformAddressRF">
		<xsl:param name="address"/>
		<xsl:param name="mainTitle"/>
		<xsl:param name="emptyTitle"/>
		<xsl:choose>
			<xsl:when test="$address/@Индекс">
				<xsl:call-template name="transform">
					<xsl:with-param name="emptyTitle" select="$emptyTitle"/>
					<xsl:with-param name="mainTitle" select="$mainTitle"/>
					<xsl:with-param name="title" select="'Индекс'"/>
					<xsl:with-param name="value" select="$address/@Индекс"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Субъект Российской Федерации'"/>
					<xsl:with-param name="value" select="concat($address/Регион/@ТипРегион,' ',$address/Регион/@НаимРегион)"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="emptyTitle" select="$emptyTitle"/>
					<xsl:with-param name="mainTitle" select="$mainTitle"/>
					<xsl:with-param name="title" select="'Субъект Российской Федерации'"/>
					<xsl:with-param name="value" select="concat($address/Регион/@ТипРегион,' ',$address/Регион/@НаимРегион)"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$address/Район">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Район (улус и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address/Район/@ТипРайон,' ',$address/Район/@НаимРайон)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/Город">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Город (волость и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address/Город/@ТипГород,' ',$address/Город/@НаимГород)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/НаселПункт">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Населенный пункт (село и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address/НаселПункт/@ТипНаселПункт,' ',$address/НаселПункт/@НаимНаселПункт)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/Улица">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Улица (проспект, переулок и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address/Улица/@ТипУлица,' ',$address/Улица/@НаимУлица)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/@Дом">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дом (владение и т.п.)'"/>
				<xsl:with-param name="value" select="$address/@Дом"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/@Корпус">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Корпус (строение и т.п.)'"/>
				<xsl:with-param name="value" select="$address/@Корпус"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address/@Кварт">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Квартира (офис и т.п.)'"/>
				<xsl:with-param name="value" select="$address/@Кварт"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="$address"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="transformAddressIN">
		<xsl:param name="data"/>
		<xsl:call-template name="transform">
			<xsl:with-param name="subTitle" select="'Адрес (место расположения) за пределами территории Российской Федерации'"/>
			<xsl:with-param name="title" select="'Код и наименование страны'"/>
			<xsl:with-param name="value" select="concat(exslt:node-set($data)//@ОКСМ,' ',exslt:node-set($data)//@НаимСтран)"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($data)/АдрИн">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Адрес'"/>
				<xsl:with-param name="value" select="exslt:node-set($data)/АдрИн"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($data)"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="transformGRNDate">
		<xsl:param name="node"/>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
			<xsl:with-param name="value" select="$node//ГРНДата/@ГРН"/>
			<xsl:with-param name="valueDate" select="$node//ГРНДата/@ДатаЗаписи"/>
		</xsl:call-template>
		<xsl:if test="$node//ГРНДатаИспр">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи об исправлении технической ошибки в указанных сведения'"/>
				<xsl:with-param name="value" select="$node//ГРНДатаИспр/@ГРН"/>
				<xsl:with-param name="valueDate" select="$node//ГРНДатаИспр/@ДатаЗаписи"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFIO">
		<xsl:param name="fio"/>
		<xsl:param name="SubTittle"/>
		<xsl:param name="Empty"/>
		<xsl:if test="$fio">
			<xsl:if test="$fio/@Фамилия">
				<xsl:call-template name="transform">
					<xsl:with-param name="subTitle" select="$SubTittle"/>
					<xsl:with-param name="empty" select="$Empty"/>
					<xsl:with-param name="title" select="'Фамилия'"/>
					<xsl:with-param name="value" select="$fio/@Фамилия"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/@Имя">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Имя'"/>
					<xsl:with-param name="value" select="$fio/@Имя"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/@Отчество">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Отчество'"/>
					<xsl:with-param name="value" select="$fio/@Отчество"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/@ИННФЛ">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ИНН'"/>
					<xsl:with-param name="value" select="$fio/@ИННФЛ"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$fio"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFounderUL">
		<xsl:param name="node"/>
		<xsl:param name="Pos"/>
		<xsl:param name="Count"/>
		<xsl:choose>
			<xsl:when test="$Count!=1">
				<xsl:choose>
					<xsl:when test="$Pos=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
					<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
					<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
					<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$node/НаимИННЮЛ/@ОГРН">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'ОГРН'"/>
				<xsl:with-param name="value" select="$node/НаимИННЮЛ/@ОГРН"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/НаимИННЮЛ/@ИНН">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'ИНН'"/>
				<xsl:with-param name="value" select="$node/НаимИННЮЛ/@ИНН"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Полное наименование'"/>
			<xsl:with-param name="value" select="$node/НаимИННЮЛ/@НаимЮЛПолн"/>
		</xsl:call-template>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="$node/НаимИННЮЛ"/>
		</xsl:call-template>
		<xsl:if test="$node/СвРегСтарые">
			<xsl:if test="$node/СвРегСтарые/@РегНом">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="title" select="'Регистрационный номер, присвоенный юридическому лицу до 1 июля 2002 года'"/>
					<xsl:with-param name="value" select="$node/СвРегСтарые/@РегНом"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвРегСтарые/@ДатаРег">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата регистрации юридического лица до 1 июля 2002 года'"/>
					<xsl:with-param name="valueDate" select="$node/СвРегСтарые/@ДатаРег"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвРегСтарые/@НаимРО">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование органа, зарегистрировавшего юридическое лицо до 1 июля 2002 года'"/>
					<xsl:with-param name="value" select="$node/СвРегСтарые/@НаимРО"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/СвРегСтарые"/>
			</xsl:call-template>
		</xsl:if>
		<!--Тлолько для Иностр ЮЛ-->
		<xsl:if test="$node/СвРегИн">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения о регистрации управляющей, организации – иностранного юридического лица в стране происхождения'"/>
				<xsl:with-param name="title" select="'Страна происхождения'"/>
				<xsl:with-param name="value" select="$node/СвРегИн/@НаимСтран"/>
			</xsl:call-template>
			<xsl:if test="$node/СвРегИн/@ДатаРег">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата регистрации'"/>
					<xsl:with-param name="valueDate" select="$node/СвРегИн/@ДатаРег"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвРегИн/@РегНомер">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Регистрационный номер'"/>
					<xsl:with-param name="value" select="$node/СвРегИн/@РегНомер"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвРегИн/@НаимРегОрг">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
					<xsl:with-param name="value" select="$node/СвРегИн/@НаимРегОрг"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвРегИн/@АдрСтр">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
					<xsl:with-param name="value" select="$node/СвРегИн/@АдрСтр"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/СвРегИн"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвНедДанУчр">
			<xsl:for-each select="$node/СвНедДанУчр">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Дополнительные сведения'"/>
					<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
				</xsl:call-template>
				<xsl:if test="РешСудНедДанУчр">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
						<xsl:with-param name="value" select="concat(РешСудНедДанУчр/@НаимСуда,', №',РешСудНедДанУчр/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСудНедДанУчр/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/ДоляУстКап">
			<xsl:call-template name="transform">
				<xsl:with-param name="empty" select="'&#xA0;'"/>
				<xsl:with-param name="title" select="'Номинальная стоимость доли в рублях'"/>
				<xsl:with-param name="value" select="$node/ДоляУстКап/@НоминСтоим"/>
			</xsl:call-template>
			<xsl:if test="$node/ДоляУстКап/РазмерДоли">
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/Процент">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в процентах'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/Процент"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробДесят">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в десятичных дробях'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/ДробДесят"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробПрост">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в простых дробях'"/>
						<xsl:with-param name="value" select="concat($node/ДоляУстКап/РазмерДоли/ДробПрост/@Числит,'/',$node/ДоляУстКап/РазмерДоли/ДробПрост/@Знаменат)"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/ДоляУстКап"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвОбрем">
			<xsl:for-each select="$node/СвОбрем">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Вид обременения'"/>
					<xsl:with-param name="value" select="@ВидОбрем"/>
				</xsl:call-template>
				<xsl:if test="@СрокОбременения">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Срок обременения или порядок определения срока'"/>
						<xsl:with-param name="value" select="@СрокОбременения"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="РешСуд">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении судебного органа, по которому на долю учредителя (участника) наложено обременение'"/>
						<xsl:with-param name="value" select="concat(РешСуд/@НаимСуда,', №',РешСуд/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСуд/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
				<xsl:if test="СвЗалогДержФЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержФЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержФЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:call-template name="transformFIO">
						<xsl:with-param name="Empty" select="'&#xA0;'"/>
						<xsl:with-param name="fio" select="СвЗалогДержФЛ/СвФЛ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвЗалогДержЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН">
						<xsl:call-template name="transform">
							<xsl:with-param name="empty" select="'&#xA0;'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Полное наименование'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@НаимЮЛПолн"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвЗалогДержЮЛ/НаимИННЮЛ"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/СвРегИн">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о регистрации в стране происхождения'"/>
							<xsl:with-param name="title" select="'Наименование страны происхождения'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимСтран"/>
						</xsl:call-template>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@ДатаРег">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Дата регистрации'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@ДатаРег"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@РегНомер">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Регистрационный номер'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@РегНомер"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@АдрСтр">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@АдрСтр"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="СвЗалогДержЮЛ/СвРегИн"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/СвНотУдДогЗал">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о нотариальном удостоверении договора залога'"/>
							<xsl:with-param name="title" select="'Номер договора залога'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Номер"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата договора залог'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Дата"/>
						</xsl:call-template>
						<xsl:call-template name="transformFIO">
							<xsl:with-param name="SubTitle" select="'ФИО и (при наличии) ИНН нотариуса, удостоверившего договор залога'"/>
							<xsl:with-param name="fio" select="СвЗалогДержЮЛ/СвНотУдДогЗал/СвНотариус"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFounderFL">
		<xsl:param name="node"/>
		<xsl:param name="Pos"/>
		<xsl:param name="Count"/>
		<xsl:choose>
			<xsl:when test="$Count!=1">
				<xsl:choose>
					<xsl:when test="$Pos=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
					<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
					<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
					<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:call-template name="transformFIO">
			<xsl:with-param name="fio" select="$node/СвФЛ"/>
			<xsl:with-param name="Empty" select="'&#xA0;'"/>
		</xsl:call-template>
		<xsl:if test="$node/СвНедДанУчр">
			<xsl:for-each select="$node/СвНедДанУчр">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Дополнительные сведения'"/>
					<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
				</xsl:call-template>
				<xsl:if test="РешСудНедДанУчр">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
						<xsl:with-param name="value" select="concat(РешСудНедДанУчр/@НаимСуда,', №',РешСудНедДанУчр/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСудНедДанУчр/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/ДоляУстКап">
			<xsl:call-template name="transform">
				<xsl:with-param name="empty" select="'&#xA0;'"/>
				<xsl:with-param name="title" select="'Номинальная стоимость доли в рублях'"/>
				<xsl:with-param name="value" select="$node/ДоляУстКап/@НоминСтоим"/>
			</xsl:call-template>
			<xsl:if test="$node/ДоляУстКап/РазмерДоли">
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/Процент">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в процентах'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/Процент"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробДесят">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в десятичных дробях'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/ДробДесят"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробПрост">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в простых дробях'"/>
						<xsl:with-param name="value" select="concat($node/ДоляУстКап/РазмерДоли/ДробПрост/@Числит,'/',$node/ДоляУстКап/РазмерДоли/ДробПрост/@Знаменат)"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/ДоляУстКап"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвОбрем">
			<xsl:for-each select="$node/СвОбрем">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Вид обременения'"/>
					<xsl:with-param name="value" select="@ВидОбрем"/>
				</xsl:call-template>
				<xsl:if test="@СрокОбременения">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Срок обременения или порядок определения срока'"/>
						<xsl:with-param name="value" select="@СрокОбременения"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="РешСуд">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении судебного органа, по которому на долю учредителя (участника) наложено обременение'"/>
						<xsl:with-param name="value" select="concat(РешСуд/@НаимСуда,', №',РешСуд/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСуд/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="(.)"/>
				</xsl:call-template>
				<xsl:if test="СвЗалогДержФЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержФЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержФЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:call-template name="transformFIO">
						<xsl:with-param name="Empty" select="'&#xA0;'"/>
						<xsl:with-param name="fio" select="СвЗалогДержФЛ/СвФЛ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвЗалогДержЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН">
						<xsl:call-template name="transform">
							<xsl:with-param name="empty" select="'&#xA0;'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Полное наименование'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@НаимЮЛПолн"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвЗалогДержЮЛ/НаимИННЮЛ"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/СвРегИн">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о регистрации в стране происхождения'"/>
							<xsl:with-param name="title" select="'Наименование страны происхождения'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимСтран"/>
						</xsl:call-template>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@ДатаРег">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Дата регистрации'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@ДатаРег"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@РегНомер">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Регистрационный номер'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@РегНомер"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@АдрСтр">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@АдрСтр"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="СвЗалогДержЮЛ/СвРегИн"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/СвНотУдДогЗал">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о нотариальном удостоверении договора залога'"/>
							<xsl:with-param name="title" select="'Номер договора залога'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Номер"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата договора залог'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Дата"/>
						</xsl:call-template>
						<xsl:call-template name="transformFIO">
							<xsl:with-param name="SubTitle" select="'ФИО и (при наличии) ИНН нотариуса, удостоверившего договор залога'"/>
							<xsl:with-param name="fio" select="СвЗалогДержЮЛ/СвНотУдДогЗал/СвНотариус"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/СвДовУпрЮЛ">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения о доверительном управляющем'"/>
				<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
				<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
				<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
			</xsl:call-template>
			<xsl:if test="$node/НаимИННДовУпр/@ОГРН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="$node/НаимИННДовУпр/@ОГРН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/НаимИННДовУпр/@ИНН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ИНН'"/>
					<xsl:with-param name="value" select="$node/НаимИННДовУпр/@ИНН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Полное наименование'"/>
				<xsl:with-param name="value" select="$node/НаимИННДовУпр/@НаимЮЛПолн"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/НаимИННДовУпр"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвДовУпрФЛ">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения о доверительном управляющем'"/>
				<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
				<xsl:with-param name="value" select="$node/СвДовУпрФЛ/ГРНДатаПерв/@ГРН"/>
				<xsl:with-param name="valueDate" select="$node/СвДовУпрФЛ/ГРНДатаПерв/@ДатаЗаписи"/>
			</xsl:call-template>
			<xsl:call-template name="transformFIO">
				<xsl:with-param name="fio" select="$node/СвДовУпрФЛ/СвФЛ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/ЛицоУпрНасл">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения о лице, осуществляющем управление долей, переходящей в порядке наследования'"/>
				<xsl:with-param name="title" select="'ДатаОткрНасл'"/>
				<xsl:with-param name="valueDate" select="$node/ЛицоУпрНасл/@ДатаОткрНасл"/>
			</xsl:call-template>
			<xsl:if test="$node/ЛицоУпрНасл/ГРНДатаПерв">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
					<xsl:with-param name="value" select="$node/ЛицоУпрНасл/ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="$node/ЛицоУпрНасл/ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformFIO">
				<xsl:with-param name="fio" select="$node/ЛицоУпрНасл/СвФЛ"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFounderSubj">
		<xsl:param name="node"/>
		<xsl:param name="Pos"/>
		<xsl:param name="Count"/>
		<xsl:choose>
			<xsl:when test="$Count!=1">
				<xsl:choose>
					<xsl:when test="$Pos=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
					<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
					<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
					<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$node/ВидНаимУчр/@НаимМО">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование муниципального образования'"/>
				<xsl:with-param name="value" select="$node/ВидНаимУчр/@НаимМО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/ВидНаимУчр/@НаимРегион">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование субъекта Российской Федерации'"/>
				<xsl:with-param name="value" select="$node/ВидНаимУчр/@НаимРегион"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвНедДанУчр">
			<xsl:for-each select="$node/СвНедДанУчр">
				<xsl:choose>
					<xsl:when test="count($node/СвНедДанУчр)!=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Дополнительные сведения'"/>
							<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дополнительные сведения'"/>
							<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="РешСудНедДанУчр">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
						<xsl:with-param name="value" select="concat(РешСудНедДанУчр/@НаимСуда,', №',РешСудНедДанУчр/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСудНедДанУчр/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/ДоляУстКап">
			<xsl:call-template name="transform">
				<xsl:with-param name="empty" select="'&#xA0;'"/>
				<xsl:with-param name="title" select="'Номинальная стоимость доли в рублях'"/>
				<xsl:with-param name="value" select="$node/ДоляУстКап/@НоминСтоим"/>
			</xsl:call-template>
			<xsl:if test="$node/ДоляУстКап/РазмерДоли">
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/Процент">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в процентах'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/Процент"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробДесят">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в десятичных дробях'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/ДробДесят"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробПрост">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в простых дробях'"/>
						<xsl:with-param name="value" select="concat($node/ДоляУстКап/РазмерДоли/ДробПрост/@Числит,'/',$node/ДоляУстКап/РазмерДоли/ДробПрост/@Знаменат)"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/ДоляУстКап"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвОргОсущПр">
			<xsl:for-each select="$node/СвОргОсущПр">
				<xsl:choose>
					<xsl:when test="count($node/СвОргОсущПр)!=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="НаимИННЮЛ/@ОГРН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ОГРН'"/>
						<xsl:with-param name="value" select="НаимИННЮЛ/@ОГРН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="НаимИННЮЛ/@ИНН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ИНН'"/>
						<xsl:with-param name="value" select="НаимИННЮЛ/@ИНН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Полное наименование'"/>
					<xsl:with-param name="value" select="НаимИННЮЛ/@НаимЮЛПолн"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="НаимИННЮЛ"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/СвФЛОсущПр">
			<xsl:for-each select="$node/СвФЛОсущПр">
				<xsl:choose>
					<xsl:when test="count($node/СвФЛОсущПр)!=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transformFIO">
					<xsl:with-param name="fio" select="СвФЛ"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/СвОбрем">
			<xsl:for-each select="$node/СвОбрем">
				<xsl:call-template name="transform">
					<xsl:with-param name="empty" select="'&#xA0;'"/>
					<xsl:with-param name="pos" select="position()"/>
					<xsl:with-param name="title" select="'Вид обременения'"/>
					<xsl:with-param name="value" select="@ВидОбрем"/>
				</xsl:call-template>
				<xsl:if test="@СрокОбременения">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Срок обременения или порядок определения срока'"/>
						<xsl:with-param name="value" select="@СрокОбременения"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="РешСуд">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении судебного органа, по которому на долю учредителя (участника) наложено обременение'"/>
						<xsl:with-param name="value" select="concat(РешСуд/@НаимСуда,', №',РешСуд/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСуд/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="(.)"/>
				</xsl:call-template>
				<xsl:if test="СвЗалогДержФЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержФЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержФЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:call-template name="transformFIO">
						<xsl:with-param name="Empty" select="'&#xA0;'"/>
						<xsl:with-param name="fio" select="СвЗалогДержФЛ/СвФЛ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвЗалогДержЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН">
						<xsl:call-template name="transform">
							<xsl:with-param name="empty" select="'&#xA0;'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Полное наименование'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@НаимЮЛПолн"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвЗалогДержЮЛ/НаимИННЮЛ"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/СвРегИн">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о регистрации в стране происхождения'"/>
							<xsl:with-param name="title" select="'Наименование страны происхождения'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимСтран"/>
						</xsl:call-template>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@ДатаРег">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Дата регистрации'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@ДатаРег"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@РегНомер">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Регистрационный номер'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@РегНомер"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@АдрСтр">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@АдрСтр"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="СвЗалогДержЮЛ/СвРегИн"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/СвНотУдДогЗал">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о нотариальном удостоверении договора залога'"/>
							<xsl:with-param name="title" select="'Номер договора залога'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Номер"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата договора залог'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Дата"/>
						</xsl:call-template>
						<xsl:call-template name="transformFIO">
							<xsl:with-param name="SubTitle" select="'ФИО и (при наличии) ИНН нотариуса, удостоверившего договор залога'"/>
							<xsl:with-param name="fio" select="СвЗалогДержЮЛ/СвНотУдДогЗал/СвНотариус"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFounderPIF">
		<xsl:param name="node"/>
		<xsl:param name="Pos"/>
		<xsl:param name="Count"/>
		<xsl:choose>
			<xsl:when test="$Count!=1">
				<xsl:choose>
					<xsl:when test="$Pos=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="$Pos"/>
							<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
							<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
							<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
							<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="mainTitle" select="'Сведения об учредителях (участниках) юридического лица'"/>
					<!--<xsl:with-param name="empty" select="'&#xA0;'"/>-->
					<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
					<xsl:with-param name="value" select="$node/ГРНДатаПерв/@ГРН"/>
					<xsl:with-param name="valueDate" select="$node/ГРНДатаПерв/@ДатаЗаписи"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Название (индивидуальное обозначение) паевого инвестиционного фонда'"/>
			<xsl:with-param name="value" select="$node/СвНаимПИФ/@НаимПИФ"/>
		</xsl:call-template>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="$node/СвНаимПИФ"/>
		</xsl:call-template>
		<xsl:if test="$node/СвНедДанУчр">
			<xsl:for-each select="$node/СвНедДанУчр">
				<xsl:choose>
					<xsl:when test="count($node/СвНедДанУчр)!=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Дополнительные сведения'"/>
							<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Дополнительные сведения'"/>
							<xsl:with-param name="value" select="@ТекстНедДанУчр"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="РешСудНедДанУчр">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении суда, на основании которого указанные сведения признаны недостоверными'"/>
						<xsl:with-param name="value" select="concat(РешСудНедДанУчр/@НаимСуда,', №',РешСудНедДанУчр/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСудНедДанУчр/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="$node/СвУпрКомпПИФ">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения об управляющей компании паевого инвестиционного фонда'"/>
				<xsl:with-param name="title" select="'ГРН и дата внесения в  ЕГРЮЛ записи, содержащей указаннные сведения'"/>
				<xsl:with-param name="value" select="$node/СвУпрКомпПИФ/ГРНДатаПерв/@ГРН"/>
				<xsl:with-param name="valueDate" select="$node/СвУпрКомпПИФ/ГРНДатаПерв/@ДатаЗаписи"/>
			</xsl:call-template>
			<xsl:if test="$node/СвУпрКомпПИФ/УпрКомпПиф/@ОГРН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="$node/СвУпрКомпПИФ/УпрКомпПиф/@ОГРН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$node/СвУпрКомпПИФ/УпрКомпПиф//@ИНН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ИНН'"/>
					<xsl:with-param name="value" select="$node/СвУпрКомпПИФ/УпрКомпПиф//@ИНН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Полное наименование'"/>
				<xsl:with-param name="value" select="$node/СвУпрКомпПИФ/УпрКомпПиф/@НаимЮЛПолн"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="СвЗалогДержЮЛ/НаимИННЮЛ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/ДоляУстКап">
			<xsl:call-template name="transform">
				<xsl:with-param name="empty" select="'&#xA0;'"/>
				<xsl:with-param name="title" select="'Номинальная стоимость доли в рублях'"/>
				<xsl:with-param name="value" select="$node/ДоляУстКап/@НоминСтоим"/>
			</xsl:call-template>
			<xsl:if test="$node/ДоляУстКап/РазмерДоли">
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/Процент">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в процентах'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/Процент"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробДесят">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в десятичных дробях'"/>
						<xsl:with-param name="value" select="$node/ДоляУстКап/РазмерДоли/ДробДесят"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$node/ДоляУстКап/РазмерДоли/ДробПрост">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Размер доли в простых дробях'"/>
						<xsl:with-param name="value" select="concat($node/ДоляУстКап/РазмерДоли/ДробПрост/@Числит,'/',$node/ДоляУстКап/РазмерДоли/ДробПрост/@Знаменат)"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$node/ДоляУстКап"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$node/СвОбрем">
			<xsl:for-each select="$node/СвОбрем">
				<xsl:choose>
					<xsl:when test="count($node/СвОбрем)!=1">
						<xsl:call-template name="transform">
							<xsl:with-param name="pos" select="position()"/>
							<xsl:with-param name="title" select="'Вид обременения'"/>
							<xsl:with-param name="value" select="@ВидОбрем"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="tableRow">
							<xsl:with-param name="number" select="97"/>
							<xsl:with-param name="title" select="'Вид обременения'"/>
							<xsl:with-param name="value" select="@ВидОбрем"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="@СрокОбременения">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Срок обременения или порядок определения срока'"/>
						<xsl:with-param name="value" select="@СрокОбременения"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="РешСуд">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Сведения о решении судебного органа, по которому на долю учредителя (участника) наложено обременение'"/>
						<xsl:with-param name="value" select="concat(РешСуд/@НаимСуда,', №',РешСуд/@Номер)"/>
						<xsl:with-param name="valueDate" select="РешСуд/@Дата"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="."/>
				</xsl:call-template>
				<xsl:if test="СвЗалогДержФЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержФЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержФЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:call-template name="transformFIO">
						<xsl:with-param name="Empty" select="'&#xA0;'"/>
						<xsl:with-param name="fio" select="СвЗалогДержФЛ/СвФЛ"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвЗалогДержЮЛ">
					<xsl:call-template name="transform">
						<xsl:with-param name="subTitle" select="'Сведения о залогодержателе'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="СвЗалогДержЮЛ/ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН">
						<xsl:call-template name="transform">
							<xsl:with-param name="empty" select="'&#xA0;'"/>
							<xsl:with-param name="title" select="'ОГРН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ОГРН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ИНН'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@ИНН"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'Полное наименование'"/>
						<xsl:with-param name="value" select="СвЗалогДержЮЛ/НаимИННЮЛ/@НаимЮЛПолн"/>
					</xsl:call-template>
					<xsl:call-template name="transformGRNDate">
						<xsl:with-param name="node" select="СвЗалогДержЮЛ/НаимИННЮЛ"/>
					</xsl:call-template>
					<xsl:if test="СвЗалогДержЮЛ/СвРегИн">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о регистрации в стране происхождения'"/>
							<xsl:with-param name="title" select="'Наименование страны происхождения'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимСтран"/>
						</xsl:call-template>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@ДатаРег">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Дата регистрации'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@ДатаРег"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@РегНомер">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Регистрационный номер'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@РегНомер"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Наименование регистрирующего органа'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@НаимРегОрг"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="СвЗалогДержЮЛ/СвРегИн/@АдрСтр">
							<xsl:call-template name="transform">
								<xsl:with-param name="title" select="'Адрес (место нахождения) в стране происхождения'"/>
								<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвРегИн/@АдрСтр"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:call-template name="transformGRNDate">
							<xsl:with-param name="node" select="СвЗалогДержЮЛ/СвРегИн"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="СвЗалогДержЮЛ/СвНотУдДогЗал">
						<xsl:call-template name="transform">
							<xsl:with-param name="subTitle" select="'Сведения о нотариальном удостоверении договора залога'"/>
							<xsl:with-param name="title" select="'Номер договора залога'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Номер"/>
						</xsl:call-template>
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'Дата договора залог'"/>
							<xsl:with-param name="value" select="СвЗалогДержЮЛ/СвНотУдДогЗал/@Дата"/>
						</xsl:call-template>
						<xsl:call-template name="transformFIO">
							<xsl:with-param name="SubTitle" select="'ФИО и (при наличии) ИНН нотариуса, удостоверившего договор залога'"/>
							<xsl:with-param name="fio" select="СвЗалогДержЮЛ/СвНотУдДогЗал/СвНотариус"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFilial">
		<xsl:param name="node"/>
		<xsl:for-each select="$node">
			<xsl:choose>
				<xsl:when test="count($node)!=1">
					<xsl:choose>
						<xsl:when test="position()=1">
							<xsl:call-template name="transform">
								<xsl:with-param name="mainTitle" select="'	Сведения об обособленных подразделениях юридического лица'"/>
								<xsl:with-param name="pos" select="position()"/>
								<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
								<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
								<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="transform">
								<xsl:with-param name="pos" select="position()"/>
								<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
								<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
								<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="transform">
						<xsl:with-param name="mainTitle" select="'	Сведения об обособленных подразделениях юридического лица'"/>
						<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРЮЛ сведений о данном лице'"/>
						<xsl:with-param name="value" select="ГРНДатаПерв/@ГРН"/>
						<xsl:with-param name="valueDate" select="ГРНДатаПерв/@ДатаЗаписи"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="СвНаим">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование'"/>
					<xsl:with-param name="value" select="СвНаим/@НаимПолн"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="СвНаим"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="АдрМНРФ">
				<xsl:call-template name="transformAddressRF">
					<xsl:with-param name="address" select="АдрМНРФ"/>
					<xsl:with-param name="mainTitle" select="'Адрес (место расположения) на территории Российской Федерации'"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="АдрМНРФ"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="АдрМНИн">
				<xsl:call-template name="transformAddressIN">
					<xsl:with-param name="data" select="АдрМНИн"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="СвУчетНОФилиал">
				<xsl:call-template name="transform">
					<xsl:with-param name="subTitle" select="'Сведения об учете в налоговом органе по месту нахождения филиала'"/>
					<xsl:with-param name="title" select="'КПП филиала/представительства'"/>
					<xsl:with-param name="value" select="СвУчетНОФилиал/@КПП"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата постановки на учет в налоговом органе'"/>
					<xsl:with-param name="valueDate" select="СвУчетНОФилиал/@ДатаПостУч"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Код и наименование налогового органа'"/>
					<xsl:with-param name="valueDate" select="concat(СвУчетНОФилиал/СвНО/@КодНО,' ',СвУчетНОФилиал/СвНО/@НаимНО)"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="СвУчетНОФилиал"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="СвУчетНОПредстав">
				<xsl:call-template name="transform">
					<xsl:with-param name="subTitle" select="'Сведения об учете в налоговом органе по месту нахождения представительства'"/>
					<xsl:with-param name="title" select="'КПП филиала/представительства'"/>
					<xsl:with-param name="value" select="СвУчетНОПредстав/@КПП"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Дата постановки на учет в налоговом органе'"/>
					<xsl:with-param name="valueDate" select="СвУчетНОПредстав/@ДатаПостУч"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Код и наименование налогового органа'"/>
					<xsl:with-param name="valueDate" select="concat(СвУчетНОПредстав/СвНО/@КодНО,' ',СвУчетНОПредстав/СвНО/@НаимНО)"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="СвУчетНОПредстав"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="UL">
		<xsl:param name="security"/>
		<xsl:param name="smevSign"/>
		<fo:block padding-top="5mm" font-weight="bold" text-align="center">
			Сведения
		</fo:block>
		<fo:block padding-top="3mm" font-weight="bold" text-align="center">
			из Единого государственного реестра юридических лиц
			<!--<xsl:copy-of select="exslt:node-set($newStructure)"/>-->
		</fo:block>
		<fo:block padding-top="5mm" text-align="left">
					</fo:block>
		<!-- fromInfo -->
<!--		<fo:block padding-bottom="3mm">-->
<!--			<fo:table>-->
<!--				<fo:table-column column-number="1" column-width="5%"/>-->
<!--				<fo:table-column column-number="2" column-width="95%"/>-->
<!--				<fo:table-body>-->
<!--					<fo:table-row>-->
<!--						<fo:table-cell padding="3mm">-->
<!--							<fo:block font-weight="bold">из</fo:block>-->
<!--						</fo:table-cell>-->
<!--						<fo:table-cell padding="2pt" text-align="center">-->
<!--							<fo:block border-bottom="0.2pt solid black">ИС "Автоматизированная информационная система "ФЦОД" ФНС"</fo:block>-->
<!--						</fo:table-cell>-->
<!--					</fo:table-row>-->
<!--					<fo:table-row>-->
<!--						<fo:table-cell text-align="center" font-size="9pt"/>-->
<!--						<fo:table-cell text-align="center" font-size="9pt">-->
<!--							<fo:block>(наименование информационной системы органа, предоставляющего услугу, из которой получены сведения)</fo:block>-->
<!--						</fo:table-cell>-->
<!--					</fo:table-row>-->
<!--				</fo:table-body>-->
<!--			</fo:table>-->
<!--		</fo:block>-->
		<fo:table space-before="10mm">
			<fo:table-column column-width="25%"/>
			<fo:table-column column-width="50%"/>
			<fo:table-column column-width="25%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center" border-bottom="0.2pt solid black">
							<xsl:call-template name="toDate">
		<xsl:with-param name="dateTime" select="exslt:node-set($main)//СвЮЛ/@ДатаВып"/>
		<xsl:with-param name="dateTime" select="date:date-time()"/>
							</xsl:call-template>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center">
							
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center" border-bottom="0.2pt solid black">
							
							<xsl:value-of select="exslt:node-set($main)//FNSVipULResponse/@ИдДок"/>
							
										<!--№______________________-->
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
										дата формирования выписки
									</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center" font-size="10pt">
										
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center" font-size="10pt">
							номер выписки
						</fo:block>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<fo:block padding-top="3mm" text-align="left">
			Настоящая выписка содержит сведения о юридическом лице
		</fo:block>
		<fo:table space-before="5mm">
			<fo:table-column column-width="100%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center" border-bottom="0.5pt solid black">
							<xsl:value-of select="exslt:node-set($main)//СвНаимЮЛ/@НаимЮЛПолн"/>
						</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
										(полное наименование юридического лица)
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<xsl:variable name="n" select="string-length(exslt:node-set($main)//СвЮЛ/@ОГРН)"/>
		<fo:table space-before="5mm">
			<fo:table-column column-width="30%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="30%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
							<xsl:value-of select="'ОГРН'"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 1, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 2, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 3, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 4, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 5, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 6, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 7, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 8, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 9, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 10, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 11, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 12, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвЮЛ/@ОГРН, 13, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<fo:block padding-top="3mm" text-align="left">
						включенные в Единый государственный реестр юридических лиц по состоянию на:
					</fo:block>
		<fo:table space-before="5mm">
			<fo:table-column column-width="40%"/>
			<fo:table-column column-width="60%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell border-bottom="0.5pt solid black" padding-left="1mm">
						<fo:block text-align="center">
							<xsl:call-template name="toDateEx">
								<xsl:with-param name="dateTime" select="exslt:node-set($main)//СвЮЛ/@ДатаВып"/>
							</xsl:call-template>
						</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell padding-left="1mm">
						<fo:block text-align="center">(число) (месяц прописью) (год)</fo:block>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<fo:table space-before="2mm">
			<fo:table-column column-width="6%"/>
			<fo:table-column column-width="47%"/>
			<fo:table-column column-width="47%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										N п/п
									</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										Наименование показателя
									</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										Значение показателя
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										1
									</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										2
									</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black" padding="1mm">
						<fo:block text-align="center">
										3
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<xsl:for-each select="exslt:node-set($newStructure)/elem">
					<xsl:if test="@mainTitle ">
						<fo:table-row>
							<fo:table-cell border="0.5pt solid black" padding="1mm" number-columns-spanned="3">
								<fo:block text-align="center" font-weight="bold">
									<xsl:value-of select="@mainTitle"/>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
					<xsl:if test="@subTitle">
						<fo:table-row>
							<fo:table-cell border="0.5pt solid black" padding="1mm" number-columns-spanned="3">
								<fo:block text-align="center" font-style="italic">
									<xsl:value-of select="@subTitle"/>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
					<xsl:if test="@emptyTitle">
						<fo:table-row>
							<fo:table-cell border="0.5pt solid black" padding="1mm">
							</fo:table-cell>
							<fo:table-cell border="0.5pt solid black" padding="1mm">
								<fo:block text-align="left">
									<xsl:value-of select="@emptyTitle"/>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="0.5pt solid black" padding="1mm">
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
					<xsl:if test="@empty">
						<fo:table-row>
							<fo:table-cell border="0.5pt solid black" padding="1mm" number-columns-spanned="3">
								<fo:block text-align="center" font-weight="bold">
									<xsl:value-of select="@empty"/>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
					<xsl:if test="@pos">
						<fo:table-row>
							<fo:table-cell border="0.5pt solid black" padding="1mm" number-columns-spanned="3">
								<fo:block text-align="center">
									<xsl:value-of select="@pos"/>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
					<fo:table-row>
						<fo:table-cell border="0.5pt solid black" padding="1mm">
							<fo:block text-align="center">
								<xsl:value-of select="position()"/>
							</fo:block>
						</fo:table-cell>
						<fo:table-cell border="0.5pt solid black" padding="1mm">
							<fo:block text-align="left">
								<xsl:value-of select="@title"/>
							</fo:block>
						</fo:table-cell>
						<fo:table-cell border="0.5pt solid black" padding="1mm">
							<fo:block text-align="left">
								<xsl:value-of select="@value"/>&#xA0;
				<xsl:call-template name="toDate">
									<xsl:with-param name="dateTime" select="@valueDate"/>
								</xsl:call-template>
							</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</xsl:for-each>
			</fo:table-body>
		</fo:table>
	</xsl:template>
	<xsl:template name="typeOKVED">
		<xsl:param name="type"/>
		<xsl:choose>
			<xsl:when test="$type = '2001'">
				<xsl:value-of select="'ОКВЭД ОК 029-2001 (КДЕС Ред. 1)'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'ОКВЭД ОК 029-2014 (КДЕС Ред. 2)'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="addressRFIP">
		<xsl:param name="addr"/>
		<xsl:if test="$addr">
			<fo:inline>
				<xsl:value-of select="concat($addr/Регион/@ТипРегион,' ',$addr/Регион/@НаимРегион)"/>
			</fo:inline>, 
			<fo:inline>
				<xsl:value-of select="concat($addr/Район/@ТипРайон,' ',$addr/Район/@НаимРайон)"/>
			</fo:inline>,  
			<fo:inline>
				<xsl:value-of select="concat($addr/Город/@ТипГород,' ',$addr/Город/@НаимГород)"/>
			</fo:inline>, 
			<fo:inline>
				<xsl:value-of select="concat($addr/НаселПункт/@ТипНаселПункт,' ',$addr/НаселПункт/@НаимНаселПункт)"/>
			</fo:inline>, 
			<fo:inline>
				<xsl:value-of select="concat($addr/Улица/@ТипУлица,' ',$addr/Улица/@НаимУлица)"/>
			</fo:inline>, 
			<xsl:if test="$addr/@Дом">
				<fo:inline>
					<xsl:value-of select="concat('д. ',$addr/@Дом)"/>
				</fo:inline>,
			</xsl:if>
			<xsl:if test="$addr/@Корпус">
				<fo:inline>
					<xsl:value-of select="concat('к. ',$addr/@Корпус)"/>
				</fo:inline>,
			</xsl:if>
			<xsl:if test="$addr/@Кварт">
				<fo:inline>
					<xsl:value-of select="concat('кв. ',$addr/@Кварт)"/>
				</fo:inline>,
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<xsl:template name="identityDoc">
		<xsl:param name="doc"/>
		<xsl:if test="$doc">
			<fo:inline>
				<xsl:value-of select="$doc/@НаимДок"/>
			</fo:inline>, 
			<fo:inline>
				<xsl:value-of select="$doc/@СерНомДок"/>
			</fo:inline>
			<fo:inline>
				<xsl:call-template name="toDate">
					<xsl:with-param name="dateTime" select="$doc/@ДатаДок"/>
				</xsl:call-template>
			</fo:inline>, 
			<fo:inline>
				<xsl:value-of select="$doc/@ВыдДок"/>
			</fo:inline>,
			<fo:inline>
				<xsl:value-of select="$doc/@КодВыдДок"/>
			</fo:inline>
		</xsl:if>
	</xsl:template>
	<xsl:template name="tableRow">
		<xsl:param name="number"/>
		<xsl:param name="title"/>
		<xsl:param name="value"/>
		<xsl:param name="valueDate"/>
		<fo:table-row>
			<fo:table-cell border="0.5pt solid black" padding="1mm">
				<fo:block text-align="center">
					<!--<xsl:number level="any"/>-->
					<xsl:value-of select="$number"/>
					<!--<xsl:call-template name="inc">
					<xsl:with-param name="end" select="500"/>
					</xsl:call-template>-->
				</fo:block>
			</fo:table-cell>
			<fo:table-cell border="0.5pt solid black" padding="1mm">
				<fo:block text-align="left">
					<xsl:value-of select="$title"/>
				</fo:block>
			</fo:table-cell>
			<fo:table-cell border="0.5pt solid black" padding="1mm">
				<fo:block text-align="justify">
					<xsl:value-of select="$value"/>&#xA0;
				<xsl:call-template name="toDate">
						<xsl:with-param name="dateTime" select="$valueDate"/>
						<!--xsl:with-param name="dateTime" select="exslt:node-set($main)//СвЮЛ/@ДатаОГРН"/-->
					</xsl:call-template>
				</fo:block>
			</fo:table-cell>
		</fo:table-row>
	</xsl:template>
	<xsl:template name="toDate">
		<xsl:param name="dateTime"/>
		<xsl:if test="$dateTime and $dateTime !=''">
			<xsl:value-of select="substring($dateTime,9,2)"/>.<xsl:value-of select="substring($dateTime,6,2)"/>.<xsl:value-of select="substring($dateTime,1,4)"/>
		</xsl:if>
	</xsl:template>
	<xsl:template name="toMonthName">
		<xsl:param name="month"/>
		<xsl:choose>
			<xsl:when test="$month = '01'">января</xsl:when>
			<xsl:when test="$month = '02'">февраля</xsl:when>
			<xsl:when test="$month = '03'">марта</xsl:when>
			<xsl:when test="$month = '04'">апреля</xsl:when>
			<xsl:when test="$month = '05'">мая</xsl:when>
			<xsl:when test="$month = '06'">июня</xsl:when>
			<xsl:when test="$month = '07'">июля</xsl:when>
			<xsl:when test="$month = '08'">августа</xsl:when>
			<xsl:when test="$month = '09'">сентября</xsl:when>
			<xsl:when test="$month = '10'">октября</xsl:when>
			<xsl:when test="$month = '11'">ноября</xsl:when>
			<xsl:when test="$month = '12'">декабря</xsl:when>
			<xsl:otherwise>Error</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="toDateEx">
		<xsl:param name="dateTime"/>
    "<xsl:value-of select="substring($dateTime,9,2)"/>"&#xA0;<xsl:call-template name="toMonthName">
			<xsl:with-param name="month" select="substring($dateTime,6,2)"/>
		</xsl:call-template>&#xA0;<xsl:value-of select="substring($dateTime,1,4)"/> года
  </xsl:template>
	<xsl:template name="toDateEx2">
		<xsl:param name="dateTime"/>
		<xsl:value-of select="substring($dateTime,9,2)"/>.<xsl:value-of select="substring($dateTime,6,2)"/>.<xsl:value-of select="substring($dateTime,1,4)"/>г.&#160;<xsl:value-of select="substring($dateTime, 12 ,5)"/>
	</xsl:template>
	<xsl:template name="parseOGRNtoTable">
		<xsl:param name="text"/>
		<fo:table-cell xsl:use-attribute-sets="table.border">
			<fo:block text-align="center">
				<xsl:value-of select="substring($text, 1, 1)"/>
			</fo:block>
		</fo:table-cell>
		<xsl:if test="string-length($text) > 1">
			<xsl:call-template name="parseOGRNtoTable">
				<xsl:with-param name="text" select="substring($text, 2, string-length($text) - 1)"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="*|@*" mode="local">
		<xsl:element name="{local-name()}">
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates mode="local"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
