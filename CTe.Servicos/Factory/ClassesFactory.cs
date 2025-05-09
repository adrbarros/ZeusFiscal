using System;
using System.Collections.Generic;
using System.Text;
using CTe.Classes;
using CTe.Classes.Servicos.Consulta;
using CTe.Classes.Servicos.Evento;
using CTe.Classes.Servicos.Inutilizacao;
using CTe.Classes.Servicos.Recepcao;
using CTe.Classes.Servicos.Recepcao.Retorno;
using CTe.Classes.Servicos.Status;
using CTe.Servicos.Inutilizacao;
using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using CTeEletronica = CTe.Classes.CTe;

namespace CTe.Servicos.Factory
{
    public class ClassesFactory
    {
        public static consStatServCte CriaConsStatServCte(ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            return new consStatServCte
            {
                versao = configServico.VersaoLayout,
                tpAmb = configServico.tpAmb
            };
        }

        public static consStatServCTe CriaConsStatServCTe(ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            return new consStatServCTe
            {
                versao = configServico.VersaoLayout,
                cUF = configServico.cUF,
                tpAmb = configServico.tpAmb
            };
        }

        public static consSitCTe CriarconsSitCTe(string chave, ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            return new consSitCTe
            {
                tpAmb = configServico.tpAmb,
                versao = configServico.VersaoLayout,
                chCTe = chave
            };
        }

        public static inutCTe CriaInutCTe(ConfigInutiliza configInutiliza, ConfiguracaoServico configuracaoServico = null)
        {
            if (configInutiliza == null) throw new ArgumentNullException("Preciso de uma configuração de inutilização");

            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            var id = new StringBuilder("ID");
            id.Append(configServico.cUF.GetCodigoIbgeEmString());
            id.Append(configInutiliza.Cnpj);
            id.Append((byte) configInutiliza.ModeloDocumento);
            id.Append(configInutiliza.Serie.ToString("D3"));
            id.Append(configInutiliza.NumeroInicial.ToString("D9"));
            id.Append(configInutiliza.NumeroFinal.ToString("D9"));

            return new inutCTe
            {
                versao = configServico.VersaoLayout,
                infInut = new infInutEnv
                {
                    Id = id.ToString(),
                    tpAmb = configServico.tpAmb,
                    cUF = configServico.cUF,
                    CNPJ = configInutiliza.Cnpj,
                    ano = configInutiliza.Ano,
                    nCTIni = configInutiliza.NumeroInicial,
                    nCTFin = configInutiliza.NumeroFinal,
                    mod = configInutiliza.ModeloDocumento,
                    serie = configInutiliza.Serie,
                    xJust = configInutiliza.Justificativa,
                }
            };
        }

        public static consReciCTe CriaConsReciCTe(string recibo, ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            return new consReciCTe
            {
                tpAmb = configServico.tpAmb,
                versao = configServico.VersaoLayout,
                nRec = recibo
            };
        }

        public static evCancCTe CriaEvCancCTe(string justificativa, string numeroProtocolo)
        {
            return new evCancCTe
            {
                nProt = numeroProtocolo,
                xJust = justificativa
            };
        }

        public static evCCeCTe CriaEvCCeCTe(List<infCorrecao> infCorrecaos)
        {
            return new evCCeCTe
            {
                infCorrecao = infCorrecaos
            };
        }

        public static evPrestDesacordo CriaEvPrestDesacordo(string indicadorDesacordo, string observacao, ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            var evPrestDesacordo = new evPrestDesacordo
            {
                indDesacordoOper = indicadorDesacordo,
                xObs = observacao
            };

            if (configServico.cUF == Estado.MT)//sem acentuação issue #1386
            {
                evPrestDesacordo.descEvento = "Prestacao do Servico em Desacordo";
            }

            return evPrestDesacordo;
        }

        public static enviCTe CriaEnviCTe(int lote, List<CTeEletronica> cteEletronicoList, ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;
            
            return new enviCTe(configServico.ObterVersaoLayoutValida(), lote, cteEletronicoList);
        }
    }
}