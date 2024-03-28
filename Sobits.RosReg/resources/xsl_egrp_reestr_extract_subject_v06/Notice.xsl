<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template name="Notice">
    <xsl:param name="Notice"/>
    <xsl:for-each select="$Notice">
      <table class="t" border="1" cellpadding="2" cellspacing="0" width="100%">
        <tr>
          <td width="1%">1.</td>
          <td width="40%">
            <xsl:call-template name="Panel">
              <xsl:with-param name="Text">
                <xsl:text>Вид запрошенной информации:</xsl:text>
              </xsl:with-param>
            </xsl:call-template>
          </td>
          <td>
            <xsl:call-template name="Panel">
              <xsl:with-param name="Text">
                <xsl:call-template name="Value">
                  <xsl:with-param name="Node" select="TypeInfoText"/>
                </xsl:call-template>
              </xsl:with-param>
            </xsl:call-template>
          </td>
        </tr>
        <tr>
          <td>2.</td>
          <td>
            <xsl:call-template name="Panel">
              <xsl:with-param name="Text">
                <xsl:text>Правообладатель:</xsl:text>
              </xsl:with-param>
            </xsl:call-template>
          </td>
          <td>
            <xsl:call-template name="Panel">
              <xsl:with-param name="Text">
                <xsl:choose>
                  <xsl:when test="Subject">
                    <xsl:choose>
                      <xsl:when test="Subject/Person">
                        <xsl:call-template name="Value">
                          <xsl:with-param name="Node" select="Subject/Person/Content"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test="Subject/Organization">
                        <xsl:call-template name="Value">
                          <xsl:with-param name="Node" select="Subject/Organization/Content"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test="Subject/Governance">
                        <xsl:call-template name="Value">
                          <xsl:with-param name="Node" select="Subject/Governance/Content"/>
                        </xsl:call-template>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="Value">
                      <xsl:with-param name="Node" select="SubjectInfo"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
            </xsl:call-template>
          </td>
        </tr>
      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
