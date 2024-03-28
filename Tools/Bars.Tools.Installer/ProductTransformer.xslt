<?xml version="1.0" encoding="windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
  xmlns:iis="http://schemas.microsoft.com/wix/IIsExtension"
  xmlns="http://schemas.microsoft.com/wix/2006/wi"
  exclude-result-prefixes="wix">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="wix:Wix">
    <Wix>
      <xsl:text>&#xa;</xsl:text>
      <xsl:text>&#x9;</xsl:text>

      <xsl:text disable-output-escaping="yes"><![CDATA[<?include Variables.wxi ?>]]></xsl:text>

      <xsl:text>&#xa;</xsl:text>
      <xsl:text>&#xa;</xsl:text>

      <xsl:text>&#x9;</xsl:text>
      <xsl:text>&#x9;</xsl:text>
      <xsl:copy-of select="wix:Fragment" />

      <xsl:text>&#xa;</xsl:text>
    </Wix>
  </xsl:template>
</xsl:stylesheet>