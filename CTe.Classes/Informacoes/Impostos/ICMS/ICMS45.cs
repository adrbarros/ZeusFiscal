using CTe.Classes.Informacoes.Impostos.ICMS.Tipos;
using CTe.Classes.Informacoes.Tipos;
using DFe.Classes;

namespace CTe.Classes.Informacoes.Impostos.ICMS
{
    public class ICMS45 : ICMSBasico
    {
        private decimal? _vIcmsDeson;
        public CST CST { get; set; }

        public decimal? vICMSDeson
        {
            get { return _vIcmsDeson.Arredondar(2); }
            set { _vIcmsDeson = value.Arredondar(2); }
        }

        public string cBenef { get; set; }

        public bool ShouldSerializevICMSDeson()
        {
            return vICMSDeson.HasValue;
        }

        public bool ShouldSerializecBenef()
        {
            return !string.IsNullOrWhiteSpace(cBenef);
        }
    }
}