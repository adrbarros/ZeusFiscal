using System;
using System.IO;
using System.Xml;
using CTe.Classes;
using CTe.Classes.Servicos.Recepcao;
using CTe.Classes.Servicos.Tipos;
using CTe.Utils.Validacao;
using DFe.Classes.Entidades;
using DFe.Utils;

namespace CTe.Utils.CTe
{
    public static class ExtEnvCte
    {
        public static void ValidaSchema(this enviCTe enviCTe, ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            var xmlValidacao = enviCTe.ObterXmlString();

            switch (enviCTe.versao)
            {
                case versao.ve200:
                    Validador.Valida(xmlValidacao, "enviCTe_v2.00.xsd", configServico);
                    break;
                case versao.ve400:
                case versao.ve300:
                    Validador.Valida(xmlValidacao, "enviCTe_v3.00.xsd", configServico);
                    break;
                default:
                    throw new InvalidOperationException("Nos achamos um erro na hora de validar o schema, " +
                                                        "a versão está inválida, somente é permitido " +
                                                        "versão 2.00 é 3.00");
            }
        }

        /// <summary>
        ///     Converte o objeto enviCTe para uma string no formato XML
        /// </summary>
        /// <param name="pedEnvio"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto enviCTe</returns>
        public static string ObterXmlString(this enviCTe pedEnvio)
        {
            return FuncoesXml.ClasseParaXmlString(pedEnvio);
        }

        public static void SalvarXmlEmDisco(this enviCTe enviCte, ConfiguracaoServico configuracaoServico = null)
        {
            var instanciaServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            if (instanciaServico.NaoSalvarXml()) return;

            var caminhoXml = instanciaServico.DiretorioSalvarXml;

            var arquivoSalvar = Path.Combine(caminhoXml, enviCte.idLote + "-env-lot.xml");

            FuncoesXml.ClasseParaArquivoXml(enviCte, arquivoSalvar);
        }

        public static XmlDocument CriaRequestWs(this enviCTe enviCTe, ConfiguracaoServico configuracaoServico = null)
        {
            var request = new XmlDocument();

            var xml = enviCTe.ObterXmlString();

            var instanciaServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            if (instanciaServico.cUF == Estado.PR
                || instanciaServico.cUF == Estado.MT)
                //Caso o lote seja enviado para o PR, colocar o namespace nos elementos <CTe> do lote, pois o serviço do PR o exige, conforme https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe/issues/456
                xml = xml.Replace("<CTe>", "<CTe xmlns=\"http://www.portalfiscal.inf.br/cte\">");

            request.LoadXml(xml);

            return request;
        }
    }
}