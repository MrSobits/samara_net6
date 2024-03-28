<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:date="http://exslt.org/dates-and-times" xmlns:gibdd="http://gibdd.ru/rev01" xmlns:ns2="http://orchestra.cos.ru/security/info/" xmlns:ns1="urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.5" xmlns:fl="http://ws.unisoft/EGRNXX/ResponseVIPFL" xmlns:a="http://orchestra.cos.ru/security/info/" xmlns:x="http://ws.unisoft/EGRNXX/ResponseVIPUL" xmlns:sec="http://orchestra.cos.ru/security/info/" xmlns:exslt="http://exslt.org/common" version="2.0">
	<!-- includeXML -->
	<xsl:param name="applicationXML"/>
	<xsl:param name="xmlURL"/>
	<xsl:param name="inventoryURL"/>
	<xsl:param name="xmlInventory" select="document($inventoryURL)"/>
	<xsl:variable name="inn" select="$xmlInventory//*[local-name() = 'ИНН']"/>
	<xsl:variable name="ogrn" select="$xmlInventory//*[local-name() = 'ОГРНИП']"/>
	<xsl:variable name="person" select="$applicationXML//Application/Applicants/Person[INN = $inn or Organization/OGRN = $ogrn]"/>
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
		<xsl:call-template name="transformFIO">
			<xsl:with-param name="fio" select="exslt:node-set($main)//СвФЛ"/>
			<xsl:with-param name="MainTittle" select="'Фамилия, имя, отчество (при наличии) индивидуального предпринимателя'"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвГражд/@ВидГражд='1'">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о гражданстве'"/>
				<xsl:with-param name="title" select="'Гражданство'"/>
				<xsl:with-param name="value" select="'гражданин Российской Федерации'"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвГражд/@ВидГражд='2'">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о гражданстве'"/>
				<xsl:with-param name="title" select="'Гражданство'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвГражд/@НаимСтран"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвГражд/@ВидГражд='3'">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о гражданстве'"/>
				<xsl:with-param name="title" select="'Гражданство'"/>
				<xsl:with-param name="value" select="'Лицо без гражданства'"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($main)//СвГражд"/>
		</xsl:call-template>
		<!--	<xsl:call-template name="transformAddressRF">
			<xsl:with-param name="mainTitle" select="'Сведения о месте жительства'"/>
			<xsl:with-param name="address" select="exslt:node-set($main)//СвИП/СвАдрМЖ"/>
		</xsl:call-template>-->
		<xsl:if test="exslt:node-set($main)//СвАдрЭлПочты">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Адрес электронной почты'"/>
				<xsl:with-param name="title" select="'Адрес электронной почты'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвАдрЭлПочты/@E-mail"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвАдрЭлПочты"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transform">
			<xsl:with-param name="mainTitle" select="'Сведения о регистрации'"/>
			<xsl:with-param name="title" select="'ОГРНИП'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегИП/@ОГРНИП"/>
		</xsl:call-template>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'Дата присвоения ОГРНИП'"/>
			<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегИП/@ДатаОГРНИП"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/@РегНом">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Регистрационный номер, присвоенный 
до 1 января 2004 года'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегИП/@РегНом"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/@ДатаРег">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата регистрации до 1 января 2004 года'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегИП/@ДатаРег"/>
			</xsl:call-template>
		</xsl:if>
		<!--	<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/@ДатаОГРНИП">
	<xsl:call-template name="transform">
	<xsl:with-param name="title" select="'Дата регистрации до 1 января 2004 года'"/>
	<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегИП/@ДатаОГРНИП"/>
	</xsl:call-template>
	</xsl:if>-->
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/@НаимРО">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование органа, зарегистрировавшего ИП или КФХ до 1 января 2004 года'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегИП/@НаимРО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/СвКФХ">
			<xsl:call-template name="transform">
				<xsl:with-param name="subTitle" select="'Сведения о крестьянском (фермерском) хозяйстве, созданном до 1 января 1995 года, содержащиеся в ЕГРЮЛ'"/>
				<xsl:with-param name="title" select="'ОГРН юридического лица'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегИП/СвКФХ/@ОГРН"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвИП/СвРегИП/@ИНН">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'ИНН юридического лица'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегИП/СвКФХ/@ИНН"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегИП/СвКФХ/@НаимЮЛПолн"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвРегИП/СвКФХ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transform">
			<xsl:with-param name="mainTitle" select="'Сведения о регистрирующем органе по месту жительства'"/>
			<xsl:with-param name="title" select="'Наименование регистрирующего (налогового) органа'"/>
			<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегОрг/@НаимНО"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегОрг/@АдрРО">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Адрес регистрирующего органа'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегОрг/@АдрРО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвРегОрг"/>
		</xsl:call-template>
		<xsl:if test="exslt:node-set($main)//СвИП/СвСтатус">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о состоянии (статусе)  КФХ'"/>
				<xsl:with-param name="title" select="'Наименование статуса'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвСтатус/СвСтатус/@НаимСтатус"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвСтатус"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвПрекращ">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о прекращении деятельности'"/>
				<xsl:with-param name="title" select="'Наименование способа прекращения'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвПрекращ/СвСтатус/@НаимСтатус"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата прекращения'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвПрекращ/СвСтатус/@ДатаПрекращ"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвПрекращ"/>
			</xsl:call-template>
			<xsl:if test="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ">
				<xsl:call-template name="transform">
					<xsl:with-param name="subTitle" select="'Сведения о юридическом лице, созданном на базе крестьянского (фермерского) хозяйства'"/>
					<xsl:with-param name="title" select="'ОГРН'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ/@ОГРН"/>
				</xsl:call-template>
				<xsl:if test="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ/@ИНН">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ИНН'"/>
						<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ/@ИНН"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Полное наименование юридического лица'"/>
					<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ/@НаимЮЛПолн"/>
				</xsl:call-template>
				<xsl:call-template name="transformGRNDate">
					<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвПрекращ/СвНовЮЛ"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвУчетНО">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения об учете в налоговом органе'"/>
				<xsl:with-param name="title" select="'ИНН'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвУчетНО/@ИННФЛ"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дата постановки на учет в налоговом органе'"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвУчетНО/@ДатаПостУч"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Наименование налогового органа'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвУчетНО/СвНО/@НаимНО"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвУчетНО"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегПФ">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о регистрации в качестве страхователя в территориальном органе 
Пенсионного фонда Российской Федерации'"/>
				<xsl:with-param name="title" select="'Регистрационный номер  и дата регистрации в территориальном органе Пенсионного фонда Российской Федерации'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегПФ/@РегНомПФ"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегПФ/@ДатаРег"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Код и наименование территориального органа Пенсионного фонда'"/>
				<xsl:with-param name="value" select="concat(exslt:node-set($main)//СвИП/СвРегПФ/СвОргПФ/@КодПФ,' ',exslt:node-set($main)//СвИП/СвРегПФ/СвОргПФ/@НаимПФ)"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвРегПФ"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="exslt:node-set($main)//СвИП/СвРегФСС">
			<xsl:call-template name="transform">
				<xsl:with-param name="mainTitle" select="'Сведения о регистрации в качестве страхователя в исполнительном органе 
Фонда социального страхования Российской Федерации'"/>
				<xsl:with-param name="title" select="'Регистрационный номер  и дата регистрации в территориальном органе Пенсионного фонда Российской Федерации'"/>
				<xsl:with-param name="value" select="exslt:node-set($main)//СвИП/СвРегФСС/@РегНомФСС"/>
				<xsl:with-param name="valueDate" select="exslt:node-set($main)//СвИП/СвРегФСС/@ДатаРег"/>
			</xsl:call-template>
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Код и наименование территориального органа Пенсионного фонда'"/>
				<xsl:with-param name="value" select="concat(exslt:node-set($main)//СвИП/СвРегФСС/СвОргФСС/@КодФСС,' ',exslt:node-set($main)//СвИП/СвРегФСС/СвОргФСС/@НаимФСС)"/>
			</xsl:call-template>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="exslt:node-set($main)//СвИП/СвРегФСС"/>
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
		<xsl:if test="exslt:node-set($main)//СвЗапЕГРИП">
			<xsl:for-each select="exslt:node-set($main)//СвЗапЕГРИП">
				<xsl:choose>
					<xsl:when test="count(exslt:node-set($main)//СвЗапЕГРИП)!=1">
						<xsl:choose>
							<xsl:when test="position()=1">
								<xsl:call-template name="transform">
									<xsl:with-param name="mainTitle" select="'Сведения о записях, внесенных в ЕГРИП'"/>
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Дата внесения записи в ЕГРИП'"/>
									<xsl:with-param name="value" select="@ГРНИП"/>
									<xsl:with-param name="valueDate" select="@ДатаЗап"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="transform">
									<xsl:with-param name="pos" select="position()"/>
									<xsl:with-param name="title" select="'Дата внесения записи в ЕГРИП'"/>
									<xsl:with-param name="value" select="@ГРНИП"/>
									<xsl:with-param name="valueDate" select="@ДатаЗап"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="transform">
							<xsl:with-param name="mainTitle" select="'Сведения о записях, внесенных в ЕГРИП'"/>
							<xsl:with-param name="title" select="'Дата внесения записи в ЕГРИП'"/>
							<xsl:with-param name="value" select="@ГРНИП"/>
							<xsl:with-param name="valueDate" select="@ДатаЗап"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Причина внесения записи в ЕГРИП'"/>
					<xsl:with-param name="value" select="ВидЗап/@НаимВидЗап"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Наименование регистрирующего (налогового) органа, которым запись внесена в ЕГРИП'"/>
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
									<xsl:with-param name="emptyTitle" select="'Сведения о документах, представленных при внесении записи в ЕГРИП'"/>
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
									<xsl:with-param name="emptyTitle" select="'Сведения о свидетельстве, подтверждающем факт внесения записи в ЕГРИП'"/>
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
								<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРИП записи, содержащей сведения о признании свидетельства недействительным по решению суда '"/>
								<xsl:with-param name="value" select="ГРНИПДатаСвидНед/@ГРНИП"/>
								<xsl:with-param name="valueDate" select="ГРНИПДатаСвидНед/@ДатаЗаписи"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="ГРНИПДатаИспрПред">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ГРН и дата записи, в которую внесены исправления'"/>
						<xsl:with-param name="value" select="ГРНИПДатаИспрПред/@ГРНИП"/>
						<xsl:with-param name="valueDate" select="ГРНИПДатаИспрПред/@ДатаЗаписи"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="ГРНИПДатаНедПред">
					<xsl:call-template name="transform">
						<xsl:with-param name="title" select="'ГРН и дата записи, которая признана недействительной'"/>
						<xsl:with-param name="value" select="ГРНИПДатаНедПред/@ГРНИП"/>
						<xsl:with-param name="valueDate" select="ГРНИПДатаНедПред/@ДатаЗаписи"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="СвСтатусЗап">
					<xsl:if test="ГРНИПДатаНед">
						<xsl:call-template name="transform">
							<xsl:with-param name="title" select="'ГРН и дата внесения записи, которой запись признана недействительной'"/>
							<xsl:with-param name="value" select="СвСтатусЗап/ГРНИПДатаНед/@ГРНИП"/>
							<xsl:with-param name="valueDate" select="СвСтатусЗап/ГРНИПДатаНед/@ДатаЗаписи"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="ГРНИПДатаИспр">
						<xsl:for-each select="ГРНИПДатаИспр">
							<xsl:choose>
								<xsl:when test="count(ГРНИПДатаИспр)!=1">
									<xsl:call-template name="transform">
										<xsl:with-param name="pos" select="position()"/>
										<xsl:with-param name="title" select="'ГРН и дата записи, которой внесены исправления в связи с технической ошибкой'"/>
										<xsl:with-param name="value" select="@ГРНИП"/>
										<xsl:with-param name="valueDate" select="@ДатаЗаписи"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="transform">
										<xsl:with-param name="title" select="'ГРН и дата записи, которой внесены исправления в связи с технической ошибкой'"/>
										<xsl:with-param name="value" select="@ГРНИП"/>
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
						<xsl:when test="exslt:node-set($main)//СвИП">
							<fo:block text-align="center" font-family="Times New Roman" font-size="11pt">
								<fo:table space-before="5mm">
									<fo:table-column column-width="33%"/>
									<fo:table-column column-width="33%"/>
									<fo:table-column column-width="34%"/>
									<fo:table-body>
										<fo:table-row>
											<fo:table-cell text-align="left">
												<fo:block>
												Выписка из ЕГРИП
										</fo:block>
												<fo:block>
													<xsl:call-template name="toDate">
														<xsl:with-param name="dateTime" select="date:date-time()"/>
													</xsl:call-template>&#xA0;<xsl:value-of select="substring(date:date-time(), 12 ,8)"/>
												</fo:block>
											</fo:table-cell>
											<fo:table-cell text-align="center">
												<fo:block>
													<!--<xsl:choose>
												<xsl:when test="exslt:node-set($main)//СвИП">
													<xsl:value-of select="concat('ОГРНИП ',exslt:node-set($main)//СвИП/@ОГРНИП)"/>
												</xsl:when>
												<xsl:otherwise>-->
													<xsl:value-of select="concat('ОГРНИП ',exslt:node-set($main)//СвИП/@ОГРНИП)"/>
													<!--</xsl:otherwise>
											</xsl:choose>-->
												</fo:block>
											</fo:table-cell>
											<fo:table-cell text-align="right">
												<fo:block>Страница <fo:page-number/> 
						из <fo:page-number-citation ref-id="terminator"/>
												</fo:block>
											</fo:table-cell>
										</fo:table-row>
									</fo:table-body>
								</fo:table>
							</fo:block>
						</xsl:when>
						<xsl:otherwise>
					</xsl:otherwise>
					</xsl:choose>
				</fo:static-content>
				<fo:flow flow-name="xsl-region-body" space-before="5mm">
					<xsl:choose>
						<xsl:when test="exslt:node-set($main)//СвИП">
							<xsl:call-template name="IP">
								<xsl:with-param name="security" select="$security"/>
								<xsl:with-param name="smevSign" select="$smevSign"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<fo:block text-align="center" font-family="Times New Roman" font-size="16pt">
						Справка об отсутствии запрашиваемой информации
												</fo:block>
							<fo:block text-align="left" font-family="Times New Roman" font-size="14pt" padding-top="10mm" padding-bottom="10mm">
								<xsl:choose>
									<xsl:when test="//status/name='Сведения в отношении индивидуального предпринимателя не найдены'">
								Настоящим сообщается, что по сведениям Единого государственного реестра индивидуальных предпринимателей по состоянию на <xsl:call-template name="toDate">
											<xsl:with-param name="dateTime" select="date:date-time()"/>
										</xsl:call-template> указанное в запросе физическое лицо <xsl:value-of select="$person/Identity/FullName"/> c <xsl:if test="$ogrn!=''">ОГРН <xsl:value-of select="$ogrn"/>
										</xsl:if>
										<xsl:if test="$inn!=''">ИНН <xsl:value-of select="$inn"/>
										</xsl:if> не является индивидуальным предпринимателем
						</xsl:when>
									<xsl:when test="//status/name='Сведения в отношении индивидуального предпринимателя не могут быть предоставлены в электронном виде'">
						Сведения не могут быть предоставлены в электронном виде.
						</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="//status/name"/>
									</xsl:otherwise>
								</xsl:choose>
							</fo:block>
						</xsl:otherwise>
					</xsl:choose>
					<fo:block padding-top="5mm" text-align="left">
						Сведения, содержащиеся  настоящей выписке, получены из:
					</fo:block>
					<fo:block padding-top="5mm" text-align="left" border-bottom="0.2pt solid black">
						Автоматизированная информационная система "ФЦОД" ФНС
					</fo:block>
					<fo:block padding-top="2mm" text-align="left">
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
					</fo:block>
					<fo:block id="terminator"/>
				</fo:flow>
			</fo:page-sequence>
		</fo:root>
	</xsl:template>
	<xsl:template name="Sex">
		<xsl:param name="sex"/>
		<xsl:choose>
			<xsl:when test="$sex='2'">
				<xsl:value-of select="'женский'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'мужской'"/>
			</xsl:otherwise>
		</xsl:choose>
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
			<xsl:when test="$address//@Индекс">
				<xsl:call-template name="transform">
					<xsl:with-param name="emptyTitle" select="$emptyTitle"/>
					<xsl:with-param name="mainTitle" select="$mainTitle"/>
					<xsl:with-param name="title" select="'Индекс'"/>
					<xsl:with-param name="value" select="$address//@Индекс"/>
				</xsl:call-template>
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Субъект Российской Федерации'"/>
					<xsl:with-param name="value" select="concat($address//Регион/@ТипРегион,' ',$address//Регион/@НаимРегион)"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="transform">
					<xsl:with-param name="emptyTitle" select="$emptyTitle"/>
					<xsl:with-param name="mainTitle" select="$mainTitle"/>
					<xsl:with-param name="title" select="'Субъект Российской Федерации'"/>
					<xsl:with-param name="value" select="concat($address//Регион/@ТипРегион,' ',$address//Регион/@НаимРегион)"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$address//Район">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Район (улус и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address//Регион/@ТипРайон,' ',$address//Регион/@НаимРайон)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//Город">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Город (волость и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address//Город/@ТипГород,' ',$address//Город/@НаимГород)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//НаселПункт">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Населенный пункт (село и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address//НаселПункт/@ТипНаселПункт,' ',$address//НаселПункт/@НаимНаселПункт)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//Улица">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Улица (проспект, переулок и т.п.)'"/>
				<xsl:with-param name="value" select="concat($address//Улица/@ТипУлица,' ',$address//Улица/@НаимУлица)"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//@Дом">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Дом (владение и т.п.)'"/>
				<xsl:with-param name="value" select="$address//@Дом"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//@Корпус">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Корпус (строение и т.п.)'"/>
				<xsl:with-param name="value" select="$address//@Корпус"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$address//@Кварт">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'Квартира (офис и т.п.)'"/>
				<xsl:with-param name="value" select="$address//@Кварт"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="transformGRNDate">
			<xsl:with-param name="node" select="$address"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="transformGRNDate">
		<xsl:param name="node"/>
		<xsl:call-template name="transform">
			<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРИП записи, содержащей указанные сведения'"/>
			<xsl:with-param name="value" select="$node//ГРНИПДата/@ГРНИП"/>
			<xsl:with-param name="valueDate" select="$node//ГРНИПДата/@ДатаЗаписи"/>
		</xsl:call-template>
		<xsl:if test="$node//ГРНИПДатаИспр">
			<xsl:call-template name="transform">
				<xsl:with-param name="title" select="'ГРН и дата внесения в ЕГРИП записи об исправлении технической ошибки в указанных сведениях'"/>
				<xsl:with-param name="value" select="$node//ГРНИПДатаИспр/@ГРНИП"/>
				<xsl:with-param name="valueDate" select="$node//ГРНИПДатаИспр/@ДатаЗаписи"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="transformFIO">
		<xsl:param name="fio"/>
		<xsl:param name="SubTittle"/>
		<xsl:param name="Empty"/>
		<xsl:param name="MainTittle"/>
		<xsl:if test="$fio">
			<xsl:if test="$fio/ФИОРус/@Фамилия">
				<xsl:call-template name="transform">
					<xsl:with-param name="subTitle" select="$SubTittle"/>
					<xsl:with-param name="mainTitle" select="$MainTittle"/>
					<xsl:with-param name="empty" select="$Empty"/>
					<xsl:with-param name="title" select="'Фамилия'"/>
					<xsl:with-param name="value" select="concat($fio/ФИОРус/@Фамилия,' ',$fio/ФИОЛат/@Фамилия)"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/ФИОРус/@Имя">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Имя'"/>
					<xsl:with-param name="value" select="concat($fio/ФИОРус/@Имя,' ',$fio/ФИОЛат/@Имя)"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/ФИОРус/@Отчество">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Отчество'"/>
					<xsl:with-param name="value" select="concat($fio/ФИОРус/@Отчество,' ',$fio/ФИОЛат/@Отчество)"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$fio/@Пол">
				<xsl:call-template name="transform">
					<xsl:with-param name="title" select="'Пол'"/>
					<xsl:with-param name="value">
						<xsl:call-template name="Sex">
							<xsl:with-param name="sex" select="$fio/@Пол"/>
						</xsl:call-template>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template name="transformGRNDate">
				<xsl:with-param name="node" select="$fio"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="IP">
		<xsl:param name="security"/>
		<xsl:param name="smevSign"/>
		<fo:block padding-top="5mm" font-weight="bold" text-align="center">
			ВЫПИСКА
		</fo:block>
		<fo:block padding-top="3mm" font-weight="bold" text-align="center">
			из Единого государственного реестра индивидуальных предпринимателей
		</fo:block>
		<fo:table space-before="10mm">
			<fo:table-column column-width="50%"/>
			<fo:table-column column-width="50%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center" border-bottom="0.2pt solid black">
							<xsl:call-template name="toDate">
								<!--<xsl:with-param name="dateTime" select="exslt:node-set($main)//СвЮЛ/@ДатаВып"/>-->
								<xsl:with-param name="dateTime" select="date:date-time()"/>
							</xsl:call-template>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center">
										№______________________________
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
										(Дата)
									</fo:block>
					</fo:table-cell>
					<fo:table-cell>
						<fo:block text-align="center" font-size="10pt">
										
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<fo:block padding-top="3mm" text-align="left">
				Настоящая выписка содержит сведения об индивидуальном предпринимателе
		</fo:block>
		<fo:table space-before="5mm">
			<fo:table-column column-width="100%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center" border-bottom="0.5pt solid black">
							<xsl:value-of select="concat(exslt:node-set($main)//СвИП/СвФЛ/ФИОРус/@Фамилия,' ',exslt:node-set($main)//СвИП/СвФЛ/ФИОРус/@Имя,' ',exslt:node-set($main)//СвИП/СвФЛ/ФИОРус/@Отчество)"/>
						</fo:block>
					</fo:table-cell>
				</fo:table-row>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
										(фамилия, имя, отчество)
									</fo:block>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<xsl:variable name="n" select="string-length(exslt:node-set($main)//СвИП/@ОГРНИП)"/>
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
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="{40 div $n}%"/>
			<fo:table-column column-width="30%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell>
						<fo:block text-align="center">
							<xsl:value-of select="'ОГРНИП'"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 1, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 2, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 3, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 4, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 5, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 6, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 7, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 8, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 9, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 10, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 11, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 12, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 13, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 14, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell border="0.5pt solid black">
						<fo:block text-align="center">
							<xsl:value-of select="substring(exslt:node-set($main)//СвИП/@ОГРНИП, 15, 1)"/>
						</fo:block>
					</fo:table-cell>
					<fo:table-cell>
					</fo:table-cell>
				</fo:table-row>
			</fo:table-body>
		</fo:table>
		<fo:block padding-top="3mm" text-align="left">
								включенные в Единый государственный реестр индивидуальных предпринимателей по состоянию на:
					</fo:block>
		<fo:table space-before="5mm">
			<fo:table-column column-width="40%"/>
			<fo:table-column column-width="60%"/>
			<fo:table-body>
				<fo:table-row>
					<fo:table-cell border-bottom="0.5pt solid black" padding-left="1mm">
						<fo:block text-align="center" border-bottom="0.2pt solid black">
							<xsl:call-template name="toDateEx">
								<!--<xsl:with-param name="dateTime" select="exslt:node-set($main)//СвЮЛ/@ДатаВып"/>-->
								<xsl:with-param name="dateTime" select="date:date-time()"/>
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
